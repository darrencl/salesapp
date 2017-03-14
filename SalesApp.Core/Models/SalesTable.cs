using Newtonsoft.Json;
using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApp.Core.Models
{
    public class SalesTable:IEquatable<SalesTable>
    {
        [PrimaryKey, JsonProperty("DocumentNo")]
        public string DocumentNo { get; set; }
        [JsonProperty("DateCreated")]
        public DateTime DateCreated { get; set; }
        [JsonProperty("Location")]
        public string Location { get; set; }
        [JsonProperty("Latitude")]
        public double Latitude { get; set; }
        [JsonProperty("Longitude")]
        public double Longitude { get; set; }
        [JsonProperty("TotalDiscountAmount")]
        public double TotalDiscountAmount { get; set; }
        [JsonProperty("Total")]
        public double Total { get; set; }
        [JsonProperty("SalesmanId")]
        public string SalesmanId { get; set; }
        [JsonProperty("CustomerId")]
        public string CustomerId { get; set; }
        [JsonProperty("isGPSEnabled")]
        public int isGPSEnabled { get; set; }
        [JsonIgnore]
        public bool isTransferred { get; set; }
        

        public SalesTable() { isTransferred = false; }
        public SalesTable(string DocNo, DateTime dateCreated, string location, double latitude, double longitude, double totaldiscamount, double total, string salesmanid, string customerid, int isgpsenabled)
        {
            DocumentNo = DocNo;
            DateCreated = dateCreated;
            Location = location;
            Latitude = latitude;
            Longitude = longitude;
            TotalDiscountAmount = totaldiscamount;
            Total = total;
            SalesmanId = salesmanid;
            CustomerId = customerid;
            isTransferred = false;
            isGPSEnabled = isgpsenabled;
        }
        public void Transferred()
        {
            isTransferred = true;
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            SalesTable o = obj as SalesTable;
            if (o == null) return false;
            return Equals(o);
        }
        public override int GetHashCode()
        {
            int hashDocNo = DocumentNo == null ? 0 : DocumentNo.GetHashCode();
            int hashCustomerId = CustomerId == null ? 0 : CustomerId.GetHashCode();
            int hashSalesmanId = SalesmanId == null ? 0 : SalesmanId.GetHashCode();

            return hashDocNo * hashCustomerId * hashSalesmanId;
        }
        public bool Equals(SalesTable other)
        {
            if (Object.ReferenceEquals(other, null)) return false;
            if (Object.ReferenceEquals(this, other)) return true;
            return DocumentNo == other.DocumentNo && SalesmanId == other.SalesmanId && CustomerId == other.CustomerId;
        }

        public override string ToString()
        {
            return DocumentNo;
        }
    }
}
