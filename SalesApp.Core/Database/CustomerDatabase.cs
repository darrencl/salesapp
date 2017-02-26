using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalesApp.Core.Models;
using SalesApp.Core.Interfaces;
using SQLite.Net;
using MvvmCross.Platform;
using System.Collections.ObjectModel;

namespace SalesApp.Core.Database
{
    public class CustomerDatabase : ICustomerDatabase
    {
        private readonly SQLiteConnection database;
        public CustomerDatabase()
        {
            var sqlite = Mvx.Resolve<ISQLite>();
            database = sqlite.GetConnection();
            database.CreateTable<Customer>();
        }
        public Task<IEnumerable<Customer>> GetAllCustomers()
        {
            return Task.FromResult<IEnumerable<Customer>>(database.Table<Customer>().ToList());
        }

        public Task<Customer> GetCustomerWhere(string customerid)
        {
            return Task.FromResult<Customer>((database.Table<Customer>().Where(x=> x.CustomerId == customerid).ToList()).FirstOrDefault());
        }

        public Task<int> InsertCustomer(Customer customer)
        {
            var num = database.Insert(customer);
            database.Commit();
            return Task.FromResult(num);
        }
        
        public Task<int> DeleteCustomer(Customer customer)
        {
            var num = database.Delete(customer);
            database.Commit();
            return Task.FromResult(num);
        }

        public Task<int> InsertAllCustomers(ObservableCollection<Customer> customers)
        {
            var num = database.InsertAll(customers);
            database.Commit();
            return Task.FromResult(num);
        }

        public Task<int> Count()
        {
            return Task.FromResult(database.Table<Customer>().ToList().Count());
        }
    }
}
