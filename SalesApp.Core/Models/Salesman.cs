using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApp.Core.Models
{
    public class Salesman
    {
        [PrimaryKey, MaxLength(9)]
        public string SalesmanId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Username { get; set; }

        public Salesman()
        {

        }
        public Salesman(string id, string name, string address, string phone, string username)
        {
            SalesmanId = id;
            Name = name;
            Address = address;
            Phone = phone;
            Username = username;
        }
    }
}
