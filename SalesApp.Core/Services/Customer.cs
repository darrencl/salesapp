using MvvmCross.Core.ViewModels;
using SalesApp.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApp.Core.Services
{
    public class Customer:MvxViewModel
    {
        public string CustomerId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        public Customer()
        { }
        public Customer(string id, string name, string address, string phone)
        {
            CustomerId = id;
            Name = name;
            Address = address;
            Phone = phone;
        }
        
    }
}
