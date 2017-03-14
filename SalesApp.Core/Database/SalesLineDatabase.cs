using SalesApp.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalesApp.Core.Models;
using SQLite.Net;
using MvvmCross.Platform;
using System.Collections.ObjectModel;

namespace SalesApp.Core.Database
{
    public class SalesLineDatabase : ISalesLineDatabase
    {
        private readonly SQLiteConnection database;

        public SalesLineDatabase()
        {
            var sqlite = Mvx.Resolve<ISQLite>();
            database = sqlite.GetConnection();
            database.CreateTable<SalesLineTable>();
        }

        public Task<int> DeleteAll()
        {
            var num = database.DeleteAll<SalesLineTable>();
            database.Commit();
            return Task.FromResult(num);
        }

        public Task<int> DeleteSalesLineWhere(string docNo)
        {
            string query = "DELETE FROM SalesLineTable WHERE DocumentNo = '" + docNo+"'";
            var result = database.Execute(query);
            database.Commit();
            return Task.FromResult(result);
        }

        public Task<IEnumerable<SalesLineTable>> GetAllSalesLinesWhere(string docNo)
        {
            return Task.FromResult<IEnumerable<SalesLineTable>>(database.Table<SalesLineTable>().Where(x => x.DocumentNo == docNo).ToList());
        }

        public Task<int> InsertSalesLines(ObservableCollection<SalesLineTable> newData)
        {
            var num = database.InsertAll(newData);
            database.Commit();
            return Task.FromResult(num);
        }
    }
}
