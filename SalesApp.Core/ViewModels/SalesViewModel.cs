using Android.App;
using Android.Content;
using Android.Locations;
using Java.Lang;
using MvvmCross.Core.ViewModels;
using Plugin.Connectivity;
using SalesApp.Core.Interfaces;
using SalesApp.Core.Models;
using SalesApp.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApp.Core.ViewModels
{
    public class SalesViewModel :MvxViewModel
    {
        
        private readonly IDialogService dialog;
        private ISalesDatabase salesDb;
        private ICustomerDatabase customerDb;
        private ISalesLineDatabase salesLineDb;
        public IMvxCommand AddSales
        {
            get { return new MvxCommand(() => funcAddSales()); }
        }
        public IMvxCommand navCustomers
        {
            get { return new MvxCommand(() => { ShowViewModel<CustomerViewModel>(); Close(this); }); }
        }
        public IMvxCommand navPromo
        {
            get { return new MvxCommand(() => { ShowViewModel<PromotionViewModel>(); Close(this); }); }
        }
        public IMvxCommand ItemDetailCommand
        {
            get { return new MvxCommand<Sales>(async (selectedSales) =>
            {
                if (selectedSales.isTransferred)
                    funcShowDetail(selectedSales);
                else
                {
                    if (await dialog.Show("Do you want to update or view the Sales with document number " + selectedSales.DocumentNo + "?", "Update or View", "Update", "View"))
                    {
                        //go to update view
                        funcUpdateSales(selectedSales);
                    }
                    else
                    {
                        funcShowDetail(selectedSales);
                    }
                }
            }); }
        }
        public IMvxCommand DeleteCommand
        {
            get { return new MvxCommand<Sales>(async (selected) => 
            {
                if (!selected.isTransferred)
                {
                    //able to delete
                    if (await dialog.Show("Are you sure you want to delete the sales under the name of " + selected.CustomerName + " on " + selected.DateCreated + "?", "Delete Sales", "Yes", "No"))
                    {
                        await salesLineDb.DeleteSalesLineWhere(selected.DocumentNo);
                        await salesDb.DeleteSales(new SalesTable(selected.DocumentNo,selected.DateCreated, selected.Location, selected.Latitude, selected.Longitude, selected.TotalDiscountAmount, selected.Total, GlobalVars.myDetail.SalesmanId, selected.CustomerId,selected.isGPSEnabled));
                        await loadSales();
                    }
                }
            }); }
        }

        public async Task CmdSync()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (await CrossConnectivity.Current.IsRemoteReachable(ServerDatabaseApi.ipAddress, int.Parse(ServerDatabaseApi.port)))
                {
                    await syncSales();
                }
                else
                    await dialog.Show("Server is not reachable", "Connection Error");
            }
            else
                await dialog.Show("No internet connection", "Connection Error");
        }
        public IMvxCommand Logout
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    if (await dialog.Show("Are you sure you want to logout?", "Logout", "Yes", "No"))
                    {
                        GlobalVars.isLoggedOut = true;
                        var mySales = await salesDb.GetAllSalesWhere(GlobalVars.myDetail.SalesmanId);
                        DateTime timeNow = DateTime.Now;
                        ISharedPreferences mysettings = Application.Context.GetSharedPreferences("mysetting", FileCreationMode.Private);
                        List<SalesTable> mySalesToList = mySales.ToList().Where(x => ((timeNow.Year - x.DateCreated.Year) * 12 + timeNow.Month - x.DateCreated.Month) > mysettings.GetInt("deleteOffset", 6)).ToList();
                        ISharedPreferencesEditor editor = mysettings.Edit();
                        editor.PutBoolean("isLoggedIn", false);
                        editor.Remove("id");
                        editor.Remove("name");
                        editor.Remove("address");
                        editor.Remove("phone");
                        editor.Remove("username");
                        editor.Apply();
                        foreach (SalesTable data in mySalesToList)
                        {
                            await salesLineDb.DeleteSalesLineWhere(data.DocumentNo);
                            await salesDb.DeleteSales(data);
                        }
                        //await salesLineDb.DeleteAll();
                        //await salesDb.DeleteAll();
                        GlobalVars.myDetail = null;
                        ShowViewModel<LoginViewModel>();
                        Close(this);
                    }
                });
            }
        }
        public ObservableCollection<Sales> SalesList
        {
            get { return GlobalVars.salesList; }
            set { GlobalVars.salesList = value; RaisePropertyChanged(() => SalesList); }
        }
        public SalesViewModel(IDialogService ids, ISalesDatabase isd, ICustomerDatabase icd, ISalesLineDatabase isld)
        {
            dialog = ids;
            salesDb = isd;
            customerDb = icd;
            salesLineDb = isld;
        }

        /* Function: loadSales
         * This function load sales from local database to the local variable (i.e. SalesList) that is binded to the MvxListView
         */
        public async Task loadSales()
        {
            ObservableCollection<Sales> salestmp = new ObservableCollection<Sales>();
            if (!string.IsNullOrEmpty(GlobalVars.myDetail.SalesmanId))
            {
                var mySales = await salesDb.GetAllSalesWhere(GlobalVars.myDetail.SalesmanId);
                var tmp = mySales.OrderByDescending(s => s.DateCreated).ToList();
                var customertmp = new Models.Customer();
                //var countCustomer = await customerDb.Count();
                //if (countCustomer == 0)
                if (CrossConnectivity.Current.IsConnected)
                {
                    if (await CrossConnectivity.Current.IsRemoteReachable(ServerDatabaseApi.ipAddress, int.Parse(ServerDatabaseApi.port)))
                    {
                        await downloadCustomers();
                        
                    }
                }
                foreach (SalesTable s in tmp)
                {
                    customertmp = await customerDb.GetCustomerWhere(s.CustomerId);
                    salestmp.Add(new Sales(s.DocumentNo, s.DateCreated, s.Location, s.TotalDiscountAmount, s.Total, s.CustomerId, customertmp.Name, customertmp.Address, s.isTransferred, s.isGPSEnabled));
                }
                SalesList = salestmp;
                RaisePropertyChanged(() => SalesList);
                GlobalVars.salesListIsLoaded = true;
            }
        }

        public async Task downloadCustomers()
        {
            var serverDb = new ServerDatabaseService();
            var fetchedCustomers = await serverDb.getAllCustomers();
            if (fetchedCustomers != null)
            {
                var countCustomer = await customerDb.Count();
                int insertCustomers;
                if (countCustomer == 0)
                    insertCustomers = await customerDb.InsertAllCustomers(fetchedCustomers);
                else
                {
                    //compare against local
                    var localCustomers = await customerDb.GetAllCustomers();
                    List<Models.Customer> fetchedData = new List<Models.Customer>(fetchedCustomers);
                    List<Models.Customer> newData = fetchedData.Except<Models.Customer>(localCustomers).ToList();
                    List<Models.Customer> deletedData = localCustomers.Except<Models.Customer>(fetchedData).ToList();
                    if (newData != null)
                    {
                        //insert new data to local db
                        insertCustomers = await customerDb.InsertAllCustomers(new ObservableCollection<Models.Customer>(newData));
                    }
                    if (deletedData != null)
                    {
                        //delete deleted data in local db
                        foreach (Models.Customer data in deletedData)
                        {
                            await customerDb.DeleteCustomer(data);
                        }
                    }
                }
            }
        }
        public async Task syncSales()
        {
            var serverDb = new ServerDatabaseService();
            var salesToBeTransferred = await salesDb.GetNotTransferredSales();
            if (salesToBeTransferred.Count() > 0)
            {
                var result = await serverDb.insertSales(salesToBeTransferred);
                if (result == 1)
                {
                    //continue by inserting sales lines
                    foreach (SalesTable x in salesToBeTransferred)
                    {
                        var saleslines = await salesLineDb.GetAllSalesLinesWhere(x.DocumentNo);
                        var sltresult = await serverDb.insertSalesLines(saleslines);
                        if (sltresult == 1)
                        {
                            var updateresult = await salesDb.UpdateTransferred(x.DocumentNo);
                            x.Transferred();
                        }

                        else if (sltresult == 0)
                        {
                            await dialog.Show("Insert Sales Lines to server database failed", "Insert to Server Failed");
                        }
                    }
                }
                else
                {
                    await dialog.Show("There is something wrong with uploading sales to the server", "Upload Failed");
                }
            }
            //get deleted data from server
            var fetchedData = await serverDb.getMySales(GlobalVars.myDetail.SalesmanId);

            if (fetchedData != null)
            {
                var mysales = await salesDb.GetAllSalesWhere(GlobalVars.myDetail.SalesmanId);
                List<SalesTable> mySalesList = new List<SalesTable>(mysales);
                var filteredData = mySalesList.Where(x => x.isTransferred == true).ToList();
                var deletedData = filteredData.Except(fetchedData.ToList()).ToList();
                if (deletedData != null)
                {
                    if (deletedData.Count() > 0)
                    {
                        try
                        {
                            foreach (SalesTable deletedSales in deletedData)
                            {
                                await salesLineDb.DeleteSalesLineWhere(deletedSales.DocumentNo);
                                await salesDb.DeleteSales(deletedSales);
                            }
                        }
                        catch (System.Exception e)
                        {
                            await dialog.Show(e.Message, "Delete Failed");
                        }
                    }
                }
            }
            await loadSales();
        }
        /* Function: funcAddSales
         * This function calls the add sales screen
         */
        public void funcAddSales()
        {
            ShowViewModel<SalesAddViewModel>();
        }
        /* Function: funcShowDetail
         * This function calls the sales detail screen
         */
        public void funcShowDetail(Sales selected)
        {
            GlobalVars.selectedSales = selected;
            ShowViewModel<SalesDetailViewModel>();
        }
        public void funcUpdateSales(Sales selected)
        {
            GlobalVars.selectedSales = selected;
            ShowViewModel<SalesUpdateViewModel>();
        }

        public void refresh()
        {
            RaisePropertyChanged(() => SalesList);
        }
    }
}
