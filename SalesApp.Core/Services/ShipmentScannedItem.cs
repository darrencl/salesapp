using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApp.Core.Services
{
    public class ShipmentScannedItem : MvxViewModel
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        private string _quantity;
        public string Quantity
        {
            get { return _quantity.ToString(); }
            set
            {
                if (double.Parse(value) <= maxQty)
                {
                    _quantity = value;
                    RaisePropertyChanged(() => Quantity);
                }
                else
                {
                    _quantity = maxQty.ToString();
                    RaisePropertyChanged(() => Quantity);
                }
            }
        }
        public double maxQty { get; set; }
        public string UnitMeasurement { get; set; }

        public ShipmentScannedItem() { }
        public ShipmentScannedItem(int itemId, string itemName, double qty, string unitMeasurement)
        {
            ItemId = itemId;
            ItemName = itemName;
            maxQty = qty;
            _quantity = qty.ToString();RaisePropertyChanged(() => Quantity);
            UnitMeasurement = unitMeasurement;
        }
    }
}
