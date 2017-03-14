using MvvmCross.Core.ViewModels;
using SalesApp.Core.Interfaces;
using SalesApp.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApp.Core.ViewModels
{
    public class SalesUpdateViewModel: MvxViewModel
    {
        private readonly IDialogService dialog;
        private ISalesDatabase salesDb;
        private ICustomerDatabase customerDb;
        private ISalesLineDatabase salesLineDb;
        public string ThisDocumentNumber
        {
            get { return GlobalVars.selectedSales.DocumentNo; }
            set { GlobalVars.selectedSales.DocumentNo = value; RaisePropertyChanged(() => ThisDocumentNumber); }
        }
        public string ThisDateCreated
        {
            get { return GlobalVars.selectedSales.DateString; }
        }
        public string CustomerName
        {
            get { return GlobalVars.selectedSales.CustomerName; }
        }
        public string CustomerAddress
        {
            get { return GlobalVars.selectedSales.CustomerAddress; }
        }
        public ObservableCollection<SalesItem> UpdateSalesItemsList
        {
            get { return GlobalVars.updateSalesItemsList; }
            set { GlobalVars.updateSalesItemsList = value; RaisePropertyChanged(() => UpdateSalesItemsList); }
        }
        private double totalDiscountAmount
        {
            get { return UpdateSalesItemsList.Sum(x => x.DiscountAmount); }
        }
        public string TotalDiscountAmount
        {
            get { return string.Format("{0:n0}", UpdateSalesItemsList.Sum(x => x.DiscountAmount)); }
        }
        private double total
        {
            get { return (UpdateSalesItemsList.Sum(x => (x.Quantity * x.ActualPrice)) - UpdateSalesItemsList.Sum(x => x.DiscountAmount)); }
        }
        public string Total
        {
            get { return string.Format("{0:n0}", (UpdateSalesItemsList.Sum(x => (x.Quantity * x.ActualPrice)) - UpdateSalesItemsList.Sum(x => x.DiscountAmount))); }
        }
        public IMvxCommand AddItem
        {
            get { return new MvxCommand(() => ShowViewModel<SalesUpdateAddItemViewModel>()); }
        }
        public IMvxCommand UpdateItem
        {
            get { return new MvxCommand<SalesItem>((selected) =>
          {
              GlobalVars.selectedSalesItem = selected;
              ShowViewModel<SalesUpdateAddItemViewModel>();
          }); }
        }
        public IMvxCommand RemoveItem
        {
            get
            {
                return new MvxCommand<SalesItem>(async (itemToBeRemoved) =>
                {
                    if (await dialog.Show("Are you sure you want to remove " + itemToBeRemoved.Quantity.ToString() + " " + itemToBeRemoved.UnitMeasurement + " of " + itemToBeRemoved.ItemName + "?", "Remove Item", "Yes", "No"))
                    {
                        UpdateSalesItemsList.Remove(itemToBeRemoved);
                        RaisePropertyChanged(() => UpdateSalesItemsList);
                        RaisePropertyChanged(() => TotalDiscountAmount);
                        RaisePropertyChanged(() => Total);
                    }
                });
            }
        }
        public IMvxCommand Back
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    if (await dialog.Show("Are you sure you want to close this page and discard changes?", "Discard changes?", "OK", "Cancel"))
                    {
                        UpdateSalesItemsList.Clear();
                        GlobalVars.selectedSales = null;
                        Close(this);
                    }
                });
            }

        }
        public async Task ProceedUpdateCommand()
        {
            //update items to local db
            try
            {
                await salesLineDb.DeleteSalesLineWhere(GlobalVars.selectedSales.DocumentNo);
                ObservableCollection<Models.SalesLineTable> updatedSalesLines = new ObservableCollection<Models.SalesLineTable>();
                int lineNoCounter = 1;
                foreach (SalesItem data in UpdateSalesItemsList)
                {
                    updatedSalesLines.Add(new Models.SalesLineTable(lineNoCounter, data.DocumentNo, data.ItemId, data.ItemName,
                        data.ActualPrice, data.Quantity, data.UnitMeasurement, data.DiscountAmount, data.DiscountPercentage));
                    lineNoCounter++;
                }
                await salesLineDb.InsertSalesLines(updatedSalesLines);
                await salesDb.Update(new Models.SalesTable(GlobalVars.selectedSales.DocumentNo, GlobalVars.selectedSales.DateCreated, GlobalVars.selectedSales.Location, GlobalVars.selectedSales.Latitude,
                    GlobalVars.selectedSales.Longitude, totalDiscountAmount, total, GlobalVars.myDetail.SalesmanId, GlobalVars.selectedSales.CustomerId, GlobalVars.selectedSales.isGPSEnabled));
                UpdateSalesItemsList.Clear();
                Close(this);
            }
            catch (Exception ex)
            {
                await dialog.Show(ex.Message, "Update Failed");
            }
        }


        public SalesUpdateViewModel(IDialogService ids, ISalesDatabase isd, ISalesLineDatabase isld, ICustomerDatabase icd)
        {
            dialog = ids;
            salesDb = isd;
            salesLineDb = isld;
            customerDb = icd;

            Task.Run(async () =>
            {
                var temp = await salesLineDb.GetAllSalesLinesWhere(GlobalVars.selectedSales.DocumentNo);
                UpdateSalesItemsList = new ObservableCollection<SalesItem>();
                foreach (Models.SalesLineTable data in temp)
                {
                    UpdateSalesItemsList.Add(new SalesItem(data.LineNumber, data.DocumentNo, data.ItemId, data.ItemName, data.ActualPrice, data.Quantity, 
                        data.UnitMeasurement, data.DiscountAmount, data.DiscountPercentage));
                }
                RaisePropertyChanged(() => UpdateSalesItemsList);
                RaisePropertyChanged(() => TotalDiscountAmount);
                RaisePropertyChanged(() => Total);
            });
        }

        public void refreshData()
        {
            RaisePropertyChanged(() => UpdateSalesItemsList);
            RaisePropertyChanged(() => TotalDiscountAmount);
            RaisePropertyChanged(() => Total);
        }
    }
}
