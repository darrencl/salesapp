using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApp.Core.Models
{
    public class SalesLineTable
    {
        [PrimaryKey]
        public string ToBeKey { get { return DocumentNo + LineNumber.ToString(); } }
        public int LineNumber { get; set; }
        public string DocumentNo { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public double ActualPrice { get; set; }
        public double Quantity { get; set; }
        public string UnitMeasurement { get; set; }
        public double DiscountAmount { get; set; }
        public double DiscountPercentage { get; set; }

        public SalesLineTable() { }
        public SalesLineTable(int lineno, string docno, int itemid, string itemname, double actualPrice, double quantity, string unitmeasurement, double discAmount, double discPercent)
        {
            LineNumber = lineno;
            DocumentNo = docno;
            ItemId = itemid;
            ItemName = itemname;
            ActualPrice = actualPrice;
            Quantity = quantity;
            UnitMeasurement = unitmeasurement;
            DiscountAmount = discAmount;
            DiscountPercentage = discPercent;
        }
        
    }
}
