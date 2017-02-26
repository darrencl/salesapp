using Android.Locations;
using MvvmCross.Core.ViewModels;
using Plugin.Connectivity;
using Plugin.Geolocator;
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
    public class SalesAddViewModel: MvxViewModel
    {
        private readonly IDialogService dialog;
        private ISalesDatabase salesDb;
        private ICustomerDatabase customerDb;
        private ISalesLineDatabase salesLineDb;
        private IGeoCoder geocoder;
        public ObservableCollection<Services.Customer> CustomerSelection
        {
            get { return GlobalVars.customerList; }
            set { GlobalVars.customerList = value; RaisePropertyChanged(() => CustomerSelection); }
        }
        public Services.Customer SelectedCustomer
        {
            get { return GlobalVars.insertSalesSelectedCustomer; }
            set { GlobalVars.insertSalesSelectedCustomer = value; RaisePropertyChanged(() => SelectedCustomer); }
        }

        public ObservableCollection<SalesItem> InsertSalesItemsList
        {
            get { return GlobalVars.insertSalesItemsList; }
            set { GlobalVars.insertSalesItemsList = value; RaisePropertyChanged(() => InsertSalesItemsList); }
        }
        private double totalDiscountAmount
        {
            get { return InsertSalesItemsList.Sum(x => x.DiscountAmount); }
        }
        public string TotalDiscountAmount
        {
            get { return string.Format("{0:n0}", InsertSalesItemsList.Sum(x => x.DiscountAmount)); }
        }
        private double total
        {
            get { return (InsertSalesItemsList.Sum(x => (x.Quantity*x.ActualPrice)) - InsertSalesItemsList.Sum(x => x.DiscountAmount)); }
        }
        public string Total
        {
            get { return string.Format("{0:n0}", (InsertSalesItemsList.Sum(x => (x.Quantity*x.ActualPrice)) - InsertSalesItemsList.Sum(x => x.DiscountAmount))); }
        }
        /*Function: AddItem
         *Close the view model and going back to the previous
         */
        public IMvxCommand AddItem
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<SalesAddItemViewModel>());
            }
        }
        /*Function: Remove Item
         *Remove selected item from the list
         */
        public IMvxCommand RemoveItem
        {
            get { return new MvxCommand<SalesItem>(async (itemToBeRemoved) =>
            {
                if (await dialog.Show("Are you sure you want to remove " + itemToBeRemoved.Quantity.ToString() + " " + itemToBeRemoved.UnitMeasurement + " of " + itemToBeRemoved.ItemName + "?", "Remove Item", "Yes", "No"))
                {
                    InsertSalesItemsList.Remove(itemToBeRemoved);
                    RaisePropertyChanged(() => InsertSalesItemsList);
                    RaisePropertyChanged(() => TotalDiscountAmount);
                    RaisePropertyChanged(() => Total);
                }
            }); }
        }
        /*Function: Back
         *Close the view model and going back to the previous
         */
        public IMvxCommand Back
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    if (await dialog.Show("Are you sure you want to close this page and discard changes?", "Discard changes?", "OK", "Cancel"))
                    {
                        SelectedCustomer = null;
                        InsertSalesItemsList.Clear();
                        Close(this);
                    }
                });
            }
            
        }
        public async Task ProceedCommand()
        {
                //check: if location is avalable, then proceed, if cannot get device location, can't proceed
                try
                {
                    var locator = CrossGeolocator.Current;
                    locator.DesiredAccuracy = 50;

                    var position = await locator.GetPositionAsync(10000);
                    var address = await geocoder.GetCityFromLocation(new GeoLocation(position.Latitude, position.Longitude));
                    //insert sales here
                    insertSales(position,address);
                    SelectedCustomer = null;
                    InsertSalesItemsList.Clear();
                    Close(this);
                }
                catch (Exception ex)
                {
                    await dialog.Show("Cannot get location at this moment. Please ensure the GPS is turned on and try again.", "Location not available");
                }
        }

        public SalesAddViewModel(ISalesDatabase isd, ICustomerDatabase icd, ISalesLineDatabase isld, IDialogService ids, IGeoCoder geocoder)
        {
            salesDb = isd;
            customerDb = icd;
            salesLineDb = isld;
            dialog = ids;
            this.geocoder = geocoder;

            if (!GlobalVars.customerListIsLoaded)
                loadCustomer();
            RaisePropertyChanged(() => CustomerSelection);
            if (SelectedCustomer != null) RaisePropertyChanged(() => SelectedCustomer);
            if (InsertSalesItemsList != null)
            {
                if (InsertSalesItemsList.Count() > 0)
                {
                    RaisePropertyChanged(() => InsertSalesItemsList);
                    RaisePropertyChanged(() => TotalDiscountAmount);
                    RaisePropertyChanged(() => Total);
                }
            }
        }

        public async void loadCustomer()
        {
            ObservableCollection<Services.Customer> customerTmp = new ObservableCollection<Services.Customer>();
            var myCust = await customerDb.GetAllCustomers();
            var tmp = myCust.OrderBy(x => x.Name);
            foreach (Models.Customer aCustomer in tmp)
            {
                customerTmp.Add(new Services.Customer(aCustomer.CustomerId, aCustomer.Name, aCustomer.Address, aCustomer.Phone));
            }
            CustomerSelection = customerTmp;
            GlobalVars.customerListIsLoaded = true;
            RaisePropertyChanged(() => CustomerSelection);
        }
        public async void insertSales(Plugin.Geolocator.Abstractions.Position position, string address)
        {
            string date = (DateTime.Now.AddHours(7).Day.ToString().Length == 1? "0" + DateTime.Now.AddHours(7).Day.ToString() : DateTime.Now.AddHours(7).Day.ToString()) + (DateTime.Now.AddHours(7).Month.ToString().Length == 1 ? "0"+ DateTime.Now.AddHours(7).Month.ToString() : DateTime.Now.AddHours(7).Month.ToString()) + DateTime.Now.AddHours(7).Year.ToString();
            var newid = await salesDb.GetNextId(GlobalVars.myDetail.SalesmanId+date);
            var result = await salesDb.InsertSales(new SalesTable(newid, DateTime.Now.AddHours(7), address, position.Latitude, position.Longitude, totalDiscountAmount, total, GlobalVars.myDetail.SalesmanId, SelectedCustomer.CustomerId));
            if (result == 1)
            {
                //insert saleslines to local
                ObservableCollection<SalesLineTable> SalesLineTableInsert = new ObservableCollection<SalesLineTable>();
                foreach (SalesItem itemdata in InsertSalesItemsList)
                {
                    SalesLineTableInsert.Add(new SalesLineTable(itemdata.LineNumber, newid, itemdata.ItemId, itemdata.ItemName, itemdata.ActualPrice, itemdata.Quantity, itemdata.UnitMeasurement, itemdata.DiscountAmount, itemdata.DiscountPercentage));
                }
                var sltresult = await salesLineDb.InsertSalesLines(SalesLineTableInsert);
                if (result != 1)
                {
                    await dialog.Show("Insert Sales Lines to local database failed.", "Insert Failed");
                }
                else
                {
                    if (CrossConnectivity.Current.IsConnected)
                    {
                        if (await CrossConnectivity.Current.IsRemoteReachable(ServerDatabaseApi.ipAddress, int.Parse(ServerDatabaseApi.port)))
                            syncSales(newid, SalesLineTableInsert);
                        else
                            await dialog.Show("Server is not reachable", "Connection Error");
                    }
                    else
                        await dialog.Show("No internet connection", "Connection Error");
                }
            }
            else
                await dialog.Show("Insert to Sales local database failed.", "Insert Failed");
        }
        public async void syncSales(string docno, ObservableCollection<SalesLineTable> SalesLineTableInsert)
        {
            var salesToBeTransferred = await salesDb.GetNotTransferredSales();
            var serverDb = new ServerDatabaseService();
            var result = await serverDb.insertSales(salesToBeTransferred);
            if (result == 1)
            {
                //continue by inserting sales lines
                var sltresult = await serverDb.insertSalesLines(SalesLineTableInsert);
                if (sltresult == 1)
                {
                    var updateresult = await salesDb.UpdateTransferred(docno);
                }
                else if (sltresult == 0)
                {
                    await dialog.Show("Insert Sales Lines to server database failed", "Insert to Server Failed");
                }
            }                
            else
            {
                await dialog.Show("Insert Sales to server database failed", "Insert to Server Failed");
            }
        }
        public void refreshData()
        {
            RaisePropertyChanged(() => SelectedCustomer);
            RaisePropertyChanged(() => InsertSalesItemsList);
            RaisePropertyChanged(() => TotalDiscountAmount);
            RaisePropertyChanged(() => Total);
        }
    }
}
