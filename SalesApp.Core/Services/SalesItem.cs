using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApp.Core.Services
{
    public class SalesItem:MvxViewModel
    {
        public int LineNumber { get; set; }
        public string DocumentNo { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public double ActualPrice { get; set; }
        public string ActualPriceString { get { return string.Format("{0:n0}", ActualPrice); } }
        public double Quantity { get; set; }
        public string UnitMeasurement { get; set; }
        public double DiscountAmount { get; set; }
        public double DiscountPercentage { get; set; }
        public double DiscountPercentageDisplay { get { return Math.Round(DiscountPercentage, 2); } }
        public string ItemPriceTotalString { get { return string.Format("{0:n0}", (ActualPrice*Quantity-DiscountAmount)); } }
        public string DiscountAmountString { get { return string.Format("{0:n0}", (DiscountAmount)); } }

        public SalesItem() { }
        public SalesItem(int lineno, string docno, int itemid, string itemname, double actualprice, double quantity, string unitmeasurement, double discamount, double discpercent)
        {
            LineNumber = lineno;
            DocumentNo = docno;
            ItemId = itemid;
            ItemName = itemname;
            ActualPrice = actualprice;
            Quantity = quantity;
            UnitMeasurement = unitmeasurement;
            DiscountAmount = discamount;
            DiscountPercentage = discpercent;
        }
        public void refresh()
        {
            RaisePropertyChanged(() => Quantity);
            RaisePropertyChanged(() => DiscountAmount);
            RaisePropertyChanged(() => DiscountPercentage);
            RaisePropertyChanged(() => DiscountAmountString);
            RaisePropertyChanged(() => DiscountPercentageDisplay);
            RaisePropertyChanged(() => ItemPriceTotalString);
        }
    }
}
