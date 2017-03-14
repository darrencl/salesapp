using Android.App;
using Android.Content;
using MvvmCross.Core.ViewModels;
using SalesApp.Core.Interfaces;
using SalesApp.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Connectivity;

namespace SalesApp.Core.ViewModels
{
    public class CustomerViewModel: MvxViewModel
    {
        private ICustomerDatabase customerDb;
        private ISalesDatabase salesDb;
        private ISalesLineDatabase salesLineDb;
        private readonly IDialogService dialog;

        public ObservableCollection<Customer> CustomerList
        {
            get { return GlobalVars.customerList; }
            set { GlobalVars.customerList = value; RaisePropertyChanged(() => CustomerList); }
        }
        public CustomerViewModel(ICustomerDatabase icd, IDialogService ids, ISalesDatabase isd, ISalesLineDatabase isld)
        {
            customerDb = icd;
            salesDb = isd;
            salesLineDb = isld;
            dialog = ids;
            if(!GlobalVars.customerListIsLoaded)
                loadCustomers();
            syncCustomer();
            OnNetworkChange();
        }

        public async void loadCustomers()
        {
            ObservableCollection<Services.Customer> customerTmp = new ObservableCollection<Customer>();
            var myCust = await customerDb.GetAllCustomers();
            var tmp = myCust.OrderBy(x => x.Name);
            foreach (Models.Customer aCustomer in tmp)
            {
                customerTmp.Add(new Customer(aCustomer.CustomerId, aCustomer.Name, aCustomer.Address, aCustomer.Phone));
            }
            CustomerList = customerTmp;
            GlobalVars.customerListIsLoaded = true;
            RaisePropertyChanged(() => CustomerList);
        }
        public IMvxCommand CustomerDetail
        {
            get
            {
                return new MvxCommand<Customer>((selectedCustomer) =>
                {
                    GlobalVars.selectedCustomer = selectedCustomer;
                    ShowViewModel<CustomerDetailViewModel>();
                });
            }
        }
        public IMvxCommand navSales
        {
            get { return new MvxCommand(() => { ShowViewModel<SalesViewModel>(); Close(this); }); }
        }
        public IMvxCommand navPromo
        {
            get { return new MvxCommand(() => { ShowViewModel<PromotionViewModel>(); Close(this); }); }
        }
        public IMvxCommand AddCustomer
        {
            get { return new MvxCommand(() => ShowViewModel<CustomerAddViewModel>()); }
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
                        ISharedPreferences mysettings = Application.Context.GetSharedPreferences("mysetting", FileCreationMode.Private); List<Models.SalesTable> mySalesToList = mySales.ToList().Where(x => ((timeNow.Year - x.DateCreated.Year) * 12 + timeNow.Month - x.DateCreated.Month) > mysettings.GetInt("deleteOffset", 6)).ToList();
                        ISharedPreferencesEditor editor = mysettings.Edit();
                        editor.PutBoolean("isLoggedIn", false);
                        editor.Remove("id");
                        editor.Remove("name");
                        editor.Remove("address");
                        editor.Remove("phone");
                        editor.Remove("username");
                        editor.Apply();
                        foreach (Models.SalesTable data in mySalesToList)
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
        public async Task SyncCommand()
        {

            if (CrossConnectivity.Current.IsConnected)
            {
                if (await CrossConnectivity.Current.IsRemoteReachable(ServerDatabaseApi.ipAddress, int.Parse(ServerDatabaseApi.port)))
                {
                    syncCustomer();
                }
                else await dialog.Show("Server is not reachable", "Connection Error");
            }
            else
                await dialog.Show("No internet connection", "Connection Error");
        }
        private async void syncCustomer()
        {
            var serverDb = new ServerDatabaseService();
            //get new customers from server, then compares to the local list later on
            List<Models.Customer> CustomerListToList = new List<Models.Customer>();
            Models.Customer temp;
            foreach (Services.Customer customerItem in CustomerList)
            {
                temp = new Models.Customer();
                temp.CustomerId = customerItem.CustomerId;
                temp.Name = customerItem.Name;
                temp.Address = customerItem.Address;
                temp.Phone = customerItem.Phone;
                CustomerListToList.Add(temp);
            }

            var fetchedCustomers = await serverDb.getAllCustomers();
            if (fetchedCustomers != null)
            {
                //compare with local
                List<Models.Customer> fetchedData = new List<Models.Customer>(fetchedCustomers);
                List<Models.Customer> newData = fetchedData.Except<Models.Customer>(CustomerListToList).ToList();
                List<Models.Customer> deletedData = CustomerListToList.Except<Models.Customer>(fetchedData).ToList();
                if (newData != null)
                {
                    //insert new data to local db
                    await customerDb.InsertAllCustomers(new ObservableCollection<Models.Customer>(newData));
                }
                if (deletedData != null)
                {
                    //delete deleted data in local db
                    foreach (Models.Customer data in deletedData)
                    {
                        await customerDb.DeleteCustomer(data);
                    }
                }
                loadCustomers();
            }
        }

        private void OnNetworkChange()
        {
            CrossConnectivity.Current.ConnectivityChanged += (sender, args) =>
            {
                if (args.IsConnected)
                {
                    syncCustomer();
                }
            };
        }


        private class ConnectivityChangedEventArgs : EventArgs
        {
            public bool IsConnected { get; set; }
        }

        private delegate void ConnectivityChangedEventHandler(object sender, ConnectivityChangedEventArgs e);
    }
}
