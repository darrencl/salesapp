using MvvmCross.Core.ViewModels;
using Plugin.Connectivity;
using SalesApp.Core.Interfaces;
using SalesApp.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing.Mobile;

namespace SalesApp.Core.ViewModels
{
    public class SalesAddItemViewModel:MvxViewModel
    {
        private readonly IDialogService dialog;
        private IItemDatabase itemDb;

        //ListView of items
        public ObservableCollection<SalesItemSelection> ItemCatalogue
        {
            get { return GlobalVars.itemCatalogue; }
            set { GlobalVars.itemCatalogue = value; RaisePropertyChanged(() => ItemCatalogue); }
        }
        private SalesItemSelection SelectedItem = new SalesItemSelection();

        //MvxSpinner
        private ObservableCollection<DiscTypes> discountTypes = new ObservableCollection<DiscTypes>()
        {new DiscTypes ("IDR"), new DiscTypes("%")};
        public ObservableCollection<DiscTypes> DiscountTypes
        {
            get { return discountTypes; }
            set { discountTypes = value; RaisePropertyChanged(() => DiscountTypes); }
        }
        private DiscTypes selectedDiscountType = new DiscTypes("IDR");
        public DiscTypes SelectedDiscountType
        {
            get { return selectedDiscountType; }
            set
            {
                selectedDiscountType = value; RaisePropertyChanged(() => SelectedDiscountType);
                if (selectedDiscountType.DiscountType == "%")
                {
                    IsDiscAmount = false; IsDiscPercent = true;
                }
                else if (IsTextboxesEnabled == true && selectedDiscountType.DiscountType == "IDR")
                {
                    IsDiscAmount = true;
                    IsDiscPercent = false;
                }
            }
        }


        private bool isTextboxesEnabled = false;
        public bool IsTextboxesEnabled
        {
            get { return isTextboxesEnabled; }
            set { isTextboxesEnabled = value; RaisePropertyChanged(() => IsTextboxesEnabled); }
        }
        private double totalItemCost;

        public string TotalItemCostDisplay
        {
            get { return string.Format("{0:n0}", totalItemCost); }
        }
        public double TotalItemCost
        {
            get
            {
                return totalItemCost;
            }
            set
            {
                totalItemCost = value; RaisePropertyChanged(() => TotalItemCost); RaisePropertyChanged(() => TotalItemCostDisplay);
            }
        }
        private double quantity;
        public double Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                DiscAmount = "0";
                DiscPercent = "0";
                RaisePropertyChanged(() => Quantity);
                RaisePropertyChanged(() => DiscAmount);
                RaisePropertyChanged(() => DiscPercent);
                refreshTotal();
            }
        }

        //Discount Textboxes
        private bool isDiscAmount;
        public bool IsDiscAmount
        {
            get { return isDiscAmount; }
            set {
                if (IsTextboxesEnabled)
                {
                    isDiscAmount = value; RaisePropertyChanged(() => IsDiscAmount);
                }
            }
        }
        private bool isDiscPercent;
        public bool IsDiscPercent
        {
            get { return isDiscPercent; }
            set { isDiscPercent = value; RaisePropertyChanged(() => IsDiscPercent); }
        }
        private string discAmount;
        public string DiscAmount
        {
            get { return discAmount; }
            
            set
            {
                discAmount = value; RaisePropertyChanged(() => DiscAmount);
                if (!string.IsNullOrEmpty(discAmount))
                {
                    if (IsDiscAmount)
                    {
                        DiscPercent = (double.Parse(discAmount) * 100.0 / (SelectedItem.Price * Quantity)).ToString();
                        refreshTotal();
                    }
                }
            }
        }
        private string discPercent;
        public string DiscPercent
        {
            get
            {
                return discPercent;
            }
            set
            {
                discPercent = value; RaisePropertyChanged(() => DiscPercent);
                if (IsDiscPercent)
                {
                    DiscAmount = (double.Parse(discPercent) * Quantity * SelectedItem.Price / 100.0).ToString();
                    RaisePropertyChanged(() => DiscAmount);
                    refreshTotal();
                }
            }
        }

        public IMvxCommand Back
        {
            get {
                return new MvxCommand(async () =>
          {
              if (await dialog.Show("Are you sure you want to close this page and discard changes?", "Discard changes?", "OK", "Cancel"))
                  Close(this);
          }); }
        }
        public IMvxCommand AddItem
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    //add item function here
                    //check total < 0
                    if (TotalItemCost >= 0 && !string.IsNullOrEmpty(DiscAmount) && !string.IsNullOrEmpty(DiscPercent) && Quantity > 0)
                    {
                        GlobalVars.insertSalesItemsList.Add(new SalesItem(GlobalVars.insertSalesItemsList.Count() + 1, "", SelectedItem.ItemId, 
                            SelectedItem.Name, SelectedItem.Price, Quantity, SelectedItem.UnitMeasurement, double.Parse(DiscAmount), double.Parse(discPercent)));
                        Close(this);
                    }
                    else if (TotalItemCost < 0)
                        await dialog.Show("Item cost cannot be less than 0. Please double check the discount again.", "Warning");
                    else if (Quantity <= 0)
                        await dialog.Show("Please double check item quantity and try again.", "Warning");
                    else if (string.IsNullOrEmpty(DiscAmount) || string.IsNullOrEmpty(DiscPercent))
                        await dialog.Show("Please check all the textboxes", "Warning");
                });
            }
        }
        public IMvxCommand SelectItem
        {
            get
            {
                return new MvxCommand<SalesItemSelection>((selectedItem) =>
                {
                    selectedItem.Selected();
                    foreach(SalesItemSelection s in ItemCatalogue)
                    {
                        if (s.ItemId != selectedItem.ItemId)
                            s.NotSelected();
                    }
                    RaisePropertyChanged(() => ItemCatalogue);
                    SelectedItem = selectedItem;
                    Quantity = 1;
                    DiscAmount = "0";
                    DiscPercent = "0";
                    IsTextboxesEnabled = true;
                    IsDiscAmount = true;
                    RaisePropertyChanged(() => Quantity);
                    RaisePropertyChanged(() => DiscAmount);
                    RaisePropertyChanged(() => DiscPercent);
                    RaisePropertyChanged(() => IsDiscAmount);
                    RaisePropertyChanged(() => IsTextboxesEnabled);
                    refreshTotal();
                });
            }
        }
        public IMvxCommand BarcodeScan
        {
            get { return new MvxCommand(async () =>
            {
                var scanner = new MobileBarcodeScanner();
                var result = await scanner.Scan();

                if (result != null)
                {
                    var selectedItem = ItemCatalogue.Where(x => x.ItemId.ToString() == result.Text).FirstOrDefault();
                    if (selectedItem != null)
                    {
                        selectedItem.Selected();
                        foreach (SalesItemSelection s in ItemCatalogue)
                        {
                            if (s.ItemId != selectedItem.ItemId)
                                s.NotSelected();
                        }
                        RaisePropertyChanged(() => ItemCatalogue);
                        SelectedItem = selectedItem;
                        Quantity = 1;
                        DiscAmount = "0";
                        DiscPercent = "0";
                        IsTextboxesEnabled = true;
                        IsDiscAmount = true;
                        RaisePropertyChanged(() => Quantity);
                        RaisePropertyChanged(() => DiscAmount);
                        RaisePropertyChanged(() => DiscPercent);
                        RaisePropertyChanged(() => IsDiscAmount);
                        RaisePropertyChanged(() => IsTextboxesEnabled);
                        refreshTotal();
                    }
                    else
                    {
                        await dialog.Show("There is no matched item with scanned barcode (result: " + result.Text + ").", "Barcode not matched");
                    }
                }
            }); }
        }
        public async Task SyncCommand()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (await CrossConnectivity.Current.IsRemoteReachable(ServerDatabaseApi.ipAddress, int.Parse(ServerDatabaseApi.port)))
                {
                    downloadItemList();
                }
                else await dialog.Show("Server is not reachable", "Connection Error");
            }
            else
                await dialog.Show("No internet connection", "Connection Error");
        }
        public SalesAddItemViewModel(IDialogService ids, IItemDatabase iid)
        {
            dialog = ids;
            itemDb = iid;

            loadItemCatalogue();
            IsTextboxesEnabled = false;
            IsDiscPercent = false;
            IsDiscAmount = false;
            RaisePropertyChanged(() => IsTextboxesEnabled);
            RaisePropertyChanged(() => IsDiscAmount);
            RaisePropertyChanged(() => IsDiscPercent);
        }

        public async void loadItemCatalogue()
        {
            var myItems = await itemDb.GetAllItems();
            var temp = myItems.OrderBy(x => x.Name);
            if (temp != null)
            {
                ObservableCollection<SalesItemSelection> ItemCatalogueTemp = new ObservableCollection<SalesItemSelection>();
                if (temp.Count() > 0)
                {
                    
                    foreach (Models.Item data in temp)
                    {
                        ItemCatalogueTemp.Add(new SalesItemSelection(data.ItemId, data.Name, data.Price, data.UnitMeasurement));
                    }
                    ItemCatalogue = ItemCatalogueTemp;
                }
            }
            RaisePropertyChanged(() => ItemCatalogue);
        }
        private async void downloadItemList()
        {
            var serverDb = new ServerDatabaseService();
            List<Models.Item> itemList = new List<Models.Item>();
            if (ItemCatalogue == null)
                ItemCatalogue = new ObservableCollection<SalesItemSelection>();
            Models.Item temp;
            foreach (SalesItemSelection item in ItemCatalogue)
            {
                temp = new Models.Item(item.ItemId, item.Name, item.Price, item.UnitMeasurement);
                itemList.Add(temp);
            }
            var fetchedItem = await serverDb.getAllItems();
            if (fetchedItem != null)
            {
                //compare with local
                List<Models.Item> fetchedData = new List<Models.Item>(fetchedItem);
                List<Models.Item> newData = fetchedData.Except(itemList).ToList();
                List<Models.Item> deletedData = itemList.Except(fetchedData).ToList();
                if (newData != null)
                {
                    //insert new data to local db
                    await itemDb.InsertItems(new ObservableCollection<Models.Item>(newData));
                    
                }
                if (deletedData != null)
                {
                    //delete deleted data in local db
                    foreach (Models.Item data in deletedData)
                    {
                        await itemDb.DeleteItem(data);
                    }
                }
                loadItemCatalogue();
            }
        }
        public void refreshTotal()
        {
            TotalItemCost = Quantity * SelectedItem.Price - double.Parse(DiscAmount);
            RaisePropertyChanged(() => TotalItemCost);
            RaisePropertyChanged(() => TotalItemCostDisplay);
        }
    }
}
