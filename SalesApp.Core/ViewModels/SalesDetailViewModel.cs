using MvvmCross.Core.ViewModels;
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
    public class SalesDetailViewModel:MvxViewModel
    {
        private ISalesLineDatabase saleslinedb;

        public ObservableCollection<SalesItem> SalesItemsList
        {
            get { return GlobalVars.salesItemsList; }
            set { GlobalVars.salesItemsList = value; RaisePropertyChanged(() => SalesItemsList); }
        }
        public string ThisDocumentNumber
        {
            get { return GlobalVars.selectedSales.DocumentNo; }
            set { GlobalVars.selectedSales.DocumentNo = value; RaisePropertyChanged(() => ThisDocumentNumber); }
        }
        public string Total
        {
            get { return GlobalVars.selectedSales.TotalString; }
        }
        public string TotalDiscount
        {
            get { return GlobalVars.selectedSales.TotalDiscountString; }
        }
        public SalesDetailViewModel(ISalesLineDatabase isld)
        {
            saleslinedb = isld;

            loadSalesLines();
        }

        public async void loadSalesLines()
        {
            ObservableCollection<SalesItem> itemstmp = new ObservableCollection<SalesItem>();
            var myitems = await saleslinedb.GetAllSalesLinesWhere(GlobalVars.selectedSales.DocumentNo);
            var tmp = myitems.OrderBy(x => x.LineNumber);
            foreach (SalesLineTable item in tmp)
            {
                itemstmp.Add(new SalesItem(item.LineNumber, item.DocumentNo,item.ItemId,item.ItemName,item.ActualPrice,item.Quantity,item.UnitMeasurement,item.DiscountAmount,item.DiscountPercentage));
            }
            SalesItemsList = itemstmp;
            RaisePropertyChanged(() => SalesItemsList);
            RaisePropertyChanged(() => TotalDiscount);
            RaisePropertyChanged(() => Total);
        }
        /*Function: Back
         *Close the view model and going back to the previous
         */
        public IMvxCommand Back
        {
            get
            {
                return new MvxCommand(() => Close(this));
            }
        }
    }
}
