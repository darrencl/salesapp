using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalesApp.Core.Models;
using System.Collections.ObjectModel;

namespace SalesApp.Core.Interfaces
{
    public interface ICustomerDatabase
    {
        Task<IEnumerable<Customer>> GetAllCustomers();

        Task<Customer> GetCustomerWhere(string customerid);

        Task<int> InsertCustomer(Customer customer);

        Task<int> InsertAllCustomers(ObservableCollection<Customer> customers);

        Task<int> DeleteCustomer(Customer customer);

        Task<int> DeleteAll();

        Task<int> Count();
    }
}
