using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApp.Core.Services
{
    public class Sales:MvxViewModel
    {
        public string DocumentNo { get; set; }
        public string DocumentNumber { get { return "#" + DocumentNo; } }
        public DateTime DateCreated { get; set; }
        public string DateString { get { return DateCreated.Day.ToString() + "/" + DateCreated.Month.ToString() + "/" + DateCreated.Year.ToString(); } }
        public string Location { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double TotalDiscountAmount { get; set; }
        public string TotalDiscountString { get { return string.Format("{0:n0}", TotalDiscountAmount); } }
        public double Total { get; set; }
        public string TotalString { get { return string.Format("{0:n0}", Total); } }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public bool isTransferred { get; set; }
        public int isGPSEnabled { get; set; }

        public Sales(string docno, DateTime datecreated, string location, double totaldisc, double total, string customerid, string customername, string customeraddress, bool istransferred, int isgpsenabled)
        {
            DocumentNo = docno;
            DateCreated = datecreated;
            Location = location;
            Total = total;
            TotalDiscountAmount = totaldisc;
            CustomerId = customerid;
            CustomerName = customername;
            CustomerAddress = customeraddress;
            isTransferred = istransferred;
            isGPSEnabled = isgpsenabled;
        }
    }
}
