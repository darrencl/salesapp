using SalesApp.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalesApp.Core.Models;
using SQLite.Net;
using MvvmCross.Platform;
using SalesApp.Core.Services;
using System.Collections.ObjectModel;

namespace SalesApp.Core.Database
{
    public class SalesDatabase : ISalesDatabase
    {
        private readonly SQLiteConnection database;
        public SalesDatabase()
        {
            var sqlite = Mvx.Resolve<ISQLite>();
            database = sqlite.GetConnection();
            database.CreateTable<SalesTable>();
        }

        public Task<int> DeleteSales(SalesTable salesData)
        {
            var num = database.Delete(salesData);
            database.Commit();
            return Task.FromResult(num);
        }

        public Task<ObservableCollection<SalesTable>> GetAllSales()
        {
            return Task.FromResult<ObservableCollection<SalesTable>>(new ObservableCollection<SalesTable>(database.Table<SalesTable>().OrderByDescending(x => x.DateCreated).ToList()));
        }
        public Task<IEnumerable<SalesTable>> GetAllSalesWhere(string salesmanId)
        {
            return Task.FromResult<IEnumerable<SalesTable>>(database.Table<SalesTable>().Where(x => x.SalesmanId == salesmanId).OrderByDescending(x => x.DateCreated).ToList());
        }

        public Task<string> GetNextId(string formula)
        {
            List<SalesTable> temp = (database.Table<SalesTable>().Where(x => x.DocumentNo.StartsWith(formula)).OrderBy(x => x.DocumentNo)).ToList();
            string result;
            if (temp != null)
            {
                if (temp.Count() > 0)
                {
                    int idCounter = int.Parse(temp.Last().DocumentNo.Substring(11)) + 1;
                    string idCounterResult = idCounter.ToString();
                    if (idCounterResult.Length < 4)
                    {
                        for (int i = 0; i < 4- idCounter.ToString().Length; i++)
                        {
                            idCounterResult = "0" + idCounterResult;
                        }
                    }
                    result = temp.Last().DocumentNo.Substring(0, 11) + idCounterResult;
                }
                else 
                    result = formula+ "0001";
            }
            else
                result = formula + "0001";
            return Task.FromResult(result);
        }

        public Task<IEnumerable<SalesTable>> GetNotTransferredSales()
        {
            return Task.FromResult<IEnumerable<SalesTable>>(database.Table<SalesTable>().Where(x => x.isTransferred == false).ToList());
        }

        public Task<int> InsertAllSales(ObservableCollection<SalesTable> newData)
        {
            var num = database.InsertAll(newData);
            database.Commit();
            return Task.FromResult(num);
        }

        public Task<int> InsertSales(SalesTable salesData)
        {
            var num = database.Insert(salesData);
            database.Commit();
            return Task.FromResult(num);
        }

        public Task<int> UpdateTransferred(string docno)
        {
            var existingSales = database.Table<SalesTable>().Where(x => x.DocumentNo == docno).FirstOrDefault();
            int num;
            if (existingSales != null)
            {
                existingSales.isTransferred = true;
                database.Update(existingSales);
                num = 1;
            }
            else
                num = 0;
            database.Commit();
            return Task.FromResult(num);
        }
        public Task<int> DeleteAll()
        {
            var num = database.DeleteAll<SalesTable>();
            database.Commit();
            return Task.FromResult(num);
        }

        public Task<int> UpdateAllTransferred()
        {
            var newData = database.Table<SalesTable>().Select(x => { x.isTransferred = true; return x; }).ToList();
            var result = database.UpdateAll(newData);
            database.Commit();
            return Task.FromResult(result);
        }

        public Task<int> Update(SalesTable salesData)
        {
            var existingSales = database.Table<SalesTable>().Where(x => x.DocumentNo == salesData.DocumentNo).FirstOrDefault();
            int num;
            if (existingSales != null)
            {
                existingSales.Total = salesData.Total;
                existingSales.TotalDiscountAmount = salesData.TotalDiscountAmount;
                database.Update(existingSales);
                num = 1;
            }
            else
                num = 0;
            database.Commit();
            return Task.FromResult(num);
        }

    }
}
