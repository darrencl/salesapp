using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApp.Core.Services
{
    public class SalesItemSelection:MvxViewModel
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string UnitMeasurement { get; set; }
        public string PriceString { get { return string.Format("{0:n0}", Price); } }
        public bool isSelected { get; set; }

        public SalesItemSelection()
        { }

        public SalesItemSelection(int itemid, string name, double price, string um)
        {
            ItemId = itemid;
            Name = name;
            Price = price;
            UnitMeasurement = um;
            isSelected = false;
        }
        public void Selected()
        {
            isSelected = true;
            RaisePropertyChanged(() => isSelected);
        }
        public void NotSelected()
        {
            isSelected = false;
            RaisePropertyChanged(() => isSelected);
        }
    }
}
