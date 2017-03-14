using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApp.Core.Models
{
    public class ShipmentLog
    {
        public string ShipmentId { get; set; }
        public DateTime DateCreated { get; set; }
        public string CustomerId { get; set; }
        public string DocumentNo { get; set; }

        public ShipmentLog() { }
        public ShipmentLog(string shipmentId, DateTime dateCreated, string customerId, string docno)
        {
            ShipmentId = shipmentId;
            DateCreated = dateCreated;
            CustomerId = customerId;
            DocumentNo = docno;
        }
    }
}
