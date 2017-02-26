using SalesApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApp.Core.Interfaces
{
    public interface ISalesLineDatabase
    {
        Task<IEnumerable<SalesLineTable>> GetAllSalesLinesWhere(string docNo);

        Task<int> InsertSalesLines(ObservableCollection<SalesLineTable> newData);

        Task<int> DeleteAll();

        Task<int> DeleteSalesLineWhere(string docNo);
    }
}
