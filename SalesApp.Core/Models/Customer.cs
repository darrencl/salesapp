using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace SalesApp.Core.Models
{
    public class Customer : IEquatable<Customer>
    {
        [PrimaryKey, MaxLength(10)]
        public string CustomerId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public Customer() { }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Customer o = obj as Customer;
            if (o == null) return false;
            return Equals(o);
        }
        public override int GetHashCode()
        {
            int hashCustId = CustomerId == null ? 0 : CustomerId.GetHashCode();

            int hashName = Name == null ? 0 : Name.GetHashCode();

            return hashCustId ^ hashName;
        }
        public bool Equals(Customer other)
        {
            if (Object.ReferenceEquals(other, null)) return false;
            if (Object.ReferenceEquals(this, other)) return true;
            return CustomerId == other.CustomerId && Name == other.Name;
        }
    }
}
