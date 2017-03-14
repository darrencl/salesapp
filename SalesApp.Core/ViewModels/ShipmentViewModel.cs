using MvvmCross.Core.ViewModels;
using SalesApp.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing.Mobile;

namespace SalesApp.Core.ViewModels
{
    public class ShipmentViewModel : MvxViewModel
    {
        private readonly IDialogService dialog;
        private ISalesDatabase salesDb;
        private ICustomerDatabase customerDb;
        private ISalesLineDatabase salesLineDb;
        private IItemDatabase itemDb;

        public string ShipmentNumber { get; set; }

        private ObservableCollection<Models.SalesTable> _salesList;
        public ObservableCollection<Models.SalesTable> SalesList
        {
            get { return _salesList; }
            set { _salesList = value; RaisePropertyChanged(() => SalesList); }
        }
        private ObservableCollection<Models.SalesLineTable> _salesItemList;
        public ObservableCollection<Models.SalesLineTable> SalesItemList
        {
            get { return _salesItemList; }
            set { _salesItemList = value; RaisePropertyChanged(() => SalesItemList); }
        }
        private ObservableCollection<Models.SalesTable> AutoCompleteList;
        private void LoadAutoCompleteList()
        {
            AutoCompleteList = new ObservableCollection<Models.SalesTable>();
            foreach (Models.SalesTable sales in SalesList)
            {
                AutoCompleteList.Add(sales);
            }
            RaisePropertyChanged(() => AutoCompleteList);
        }
        private ObservableCollection<Models.SalesTable> _autoCompleteSuggestions;
        public ObservableCollection<Models.SalesTable> AutoCompleteSuggestions
        {
            get
            {
                if (_autoCompleteSuggestions == null)
                    _autoCompleteSuggestions = new ObservableCollection<Models.SalesTable>();
                return _autoCompleteSuggestions;
            }
            set { _autoCompleteSuggestions = value; RaisePropertyChanged(() => AutoCompleteSuggestions); }
        }
        private string _salesSearchTerm;
        public string SalesSearchTerm
        {
            get { return _salesSearchTerm; }
            set
            {
                if (value == "")
                {
                    _salesSearchTerm = null;
                    SetSuggestionEmpty();
                }
                else
                {
                    _salesSearchTerm = value;
                    RaisePropertyChanged(() => SalesSearchTerm);
                    /*if (value == "")
                    {
                        _salesSearchTerm = null;
                        SetSuggestionEmpty();
                        return;
                    }
                    else
                    {
                        _salesSearchTerm = value;
                    }*/
                    if (_salesSearchTerm.Trim().Length < 2)
                    {
                        SetSuggestionEmpty();
                    }
                    else
                    {
                        var list = AutoCompleteList.Where(i => (i.DocumentNo ?? "").Contains(_salesSearchTerm));
                        if (list.Count() > 0)
                        {
                            AutoCompleteSuggestions = new ObservableCollection<Models.SalesTable>(list.ToList());
                        }
                        else
                            SetSuggestionEmpty();
                    }
                }
            }
        }
        private object _selectedObj;
        public object SelectedObj
        {
            get { return (_selectedObj as Models.SalesTable).DocumentNo; }
            set
            {
                _selectedObj = value; RaisePropertyChanged(() => SelectedObj);
                SelectedSales = SalesList.Where(x => x.DocumentNo == (_selectedObj as Models.SalesTable).DocumentNo).FirstOrDefault();
            }
        }
        private Models.SalesTable _selectedSales;
        public Models.SalesTable SelectedSales
        {
            get { return _selectedSales; }
            set
            {
                _selectedSales = value;
                RaisePropertyChanged(() => SelectedSales);
                //load saleslines
                Task.Run(async () =>
                {
                    var salesitem = await salesLineDb.GetAllSalesLinesWhere(_selectedSales.DocumentNo);
                    SalesItemList = new ObservableCollection<Models.SalesLineTable>(salesitem.ToList());
                    RaisePropertyChanged(() => SalesItemList);
                    var customer = await customerDb.GetCustomerWhere(_selectedSales.CustomerId);
                    _customerDetail = customer;
                    RaisePropertyChanged(() => ThisDocumentNumber);
                    RaisePropertyChanged(() => CustomerName);
                    RaisePropertyChanged(() => CustomerAddress);
                    if (ScannedItems != null)
                    {
                        ScannedItems.Clear();
                        RaisePropertyChanged(() => ScannedItems);
                    }
                });
            }
        }

        private ObservableCollection<Services.ShipmentScannedItem> _scannedItems;
        public ObservableCollection<Services.ShipmentScannedItem> ScannedItems
        {
            get { return _scannedItems; }
            set { _scannedItems = value; RaisePropertyChanged(() => ScannedItems); }
        }

        public Models.Customer _customerDetail;

        //Text data
        public string ThisDocumentNumber { get { return SelectedSales == null ? "" : SelectedSales.DocumentNo; } }
        public string CustomerName { get { return SelectedSales == null ? "Customer Name" : _customerDetail.Name; } }
        public string CustomerAddress { get { return SelectedSales == null ? "Customer Address" : _customerDetail.Address; } }


        public async Task<bool> postShipment()
        {
            if (ScannedItems == null)
                ScannedItems = new ObservableCollection<Services.ShipmentScannedItem>();
            if (ScannedItems.Count > 0)
            {
                Services.ServerDatabaseService serverDb = new Services.ServerDatabaseService();
                //get next id from server
                var shipmentid = await serverDb.getNextShipmentId(SelectedSales.DocumentNo);
                this.ShipmentNumber = shipmentid;
                //use as shipment id then insert to server
                ObservableCollection<Models.ShipmentLog> shipmentLogData = new ObservableCollection<Models.ShipmentLog>();
                shipmentLogData.Add(new Models.ShipmentLog(shipmentid, DateTime.Now, _customerDetail.CustomerId, ThisDocumentNumber));
                var insertShipmentLog = await serverDb.insertShipment(shipmentLogData);
                if (insertShipmentLog == 1)
                {
                    //insert shipment lines
                    ObservableCollection<Models.ShipmentLine> ShipmentLines = new ObservableCollection<Models.ShipmentLine>();
                    int lineNumberCounter = 1;
                    foreach (Services.ShipmentScannedItem item in ScannedItems)
                    {
                        ShipmentLines.Add(new Models.ShipmentLine(lineNumberCounter, shipmentid, item.ItemId, item.ItemName, double.Parse(item.Quantity), item.UnitMeasurement));
                        lineNumberCounter++;
                    }
                    var insertShipmentLines = await serverDb.insertShipmentLines(ShipmentLines);
                    if (insertShipmentLines == 1)
                    {
                        await dialog.Show("Data are inserted to the server successfully", "Insert Success");
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            else
            {
                await dialog.Show("Please scan items to be delivered", "Scanned Items are Empty");
                return false;
            }
            
        }
        public IMvxCommand BarcodeScan
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    var Items = (await itemDb.GetAllItems()).ToList();
                    var scanner = new MobileBarcodeScanner();
                    var result = await scanner.Scan();
                    if (ScannedItems == null)
                        ScannedItems = new ObservableCollection<Services.ShipmentScannedItem>();
                    if (result != null)
                    {
                        var selectedItem = SalesItemList.Where(x => (Items.Where(y => y.ItemId == x.ItemId).ToList().FirstOrDefault()).Barcode == result.Text).FirstOrDefault();
                        if (selectedItem != null)
                        {
                            if (ScannedItems.Where(x => x.ItemId == selectedItem.ItemId).ToList().Count > 0)
                                await dialog.Show("Scanned item is already there in the scanned items list", "Same Item Detected");
                            else
                            {
                                ScannedItems.Add(new Services.ShipmentScannedItem(selectedItem.ItemId, selectedItem.ItemName, selectedItem.Quantity,
                                    selectedItem.UnitMeasurement));
                                RaisePropertyChanged(() => ScannedItems);
                            }
                        }
                        else
                        {
                            await dialog.Show("There is no matched item with scanned barcode in this Sales Order(result: " + result.Text + ").", "Barcode not matched");
                        }
                    }
                });
            }
        }
        public IMvxCommand RemoveItemCommand
        {
            get
            {
                return new MvxCommand<Services.ShipmentScannedItem>(async (toberemoved) =>
                {
                    if (await dialog.Show("Are you sure you want to remove " + toberemoved.ItemName + " from scanned item list?", "Remove Item Confirmation", "Yes", "No"))
                    {
                        ScannedItems.Remove(toberemoved);
                        RaisePropertyChanged(() => ScannedItems);
                    }
                });
            }
        }
        public IMvxCommand Logout
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    if (await dialog.Show("Are you sure you want to logout?", "Logout", "Yes", "Cancel"))
                    {
                        await salesLineDb.DeleteAll();
                        await salesDb.DeleteAll();
                        await customerDb.DeleteAll();
                        await itemDb.DeleteAll();
                        ShowViewModel<LoginViewModel>();
                        Close(this);
                    }
                });
            }
        }
        public ShipmentViewModel(IDialogService ids, ISalesDatabase isd, ICustomerDatabase icd, ISalesLineDatabase isld, IItemDatabase iid)
        {
            dialog = ids;
            salesDb = isd;
            customerDb = icd;
            salesLineDb = isld;
            itemDb = iid;

            //Load sales list
            Task.Run(async () =>
            {
                SalesList = await salesDb.GetAllSales();
                RaisePropertyChanged(() => SalesList);
                LoadAutoCompleteList();
            });
        }
        public void SetSuggestionEmpty()
        {
            AutoCompleteSuggestions = new ObservableCollection<Models.SalesTable>();
        }
        public void resetForm()
        {
            SelectedSales = null;
            ScannedItems.Clear();
            ShipmentNumber = "";
        }
    }
}
