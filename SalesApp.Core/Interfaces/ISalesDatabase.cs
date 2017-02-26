using SalesApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApp.Core.Interfaces
{
    public interface ISalesDatabase
    {
        Task<IEnumerable<SalesTable>> GetAllSalesWhere(string salesmanId);

        Task<IEnumerable<SalesTable>> GetNotTransferredSales();

        Task<int> InsertSales(SalesTable salesData);

        Task<int> InsertAllSales(ObservableCollection<SalesTable> newData);

        Task<int> DeleteSales(SalesTable salesData);

        Task<string> GetNextId(string formula);

        Task<int> UpdateTransferred(string docno);

        Task<int> UpdateAllTransferred();

        Task<int> DeleteAll();
    }
}
