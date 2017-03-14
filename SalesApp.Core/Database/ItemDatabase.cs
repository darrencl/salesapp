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
    public class ItemDatabase : IItemDatabase
    {
        private readonly SQLiteConnection database;
        
        public ItemDatabase()
        {
            var sqlite = Mvx.Resolve<ISQLite>();
            database = sqlite.GetConnection();
            database.CreateTable<Item>();
        }

        public Task<int> DeleteItem(Item deletedItems)
        {
            var num = database.Delete(deletedItems);
            database.Commit();
            return Task.FromResult(num);
        }

        public Task<IEnumerable<Item>> GetAllItems()
        {
            return Task.FromResult<IEnumerable<Item>>(database.Table<Item>().ToList());
        }

        public Task<Item> GetItemDetail(int itemId)
        {
            return Task.FromResult<Item>(database.Table<Item>().Where(x => x.ItemId == itemId).FirstOrDefault());
        }

        public Task<int> InsertItems(ObservableCollection<Item> newItems)
        {
            var num = database.InsertAll(newItems);
            database.Commit();
            return Task.FromResult(num);
        }
        public Task<int> DeleteAll()
        {
            var num = database.DeleteAll<Item>();
            database.Commit();
            return Task.FromResult(num);
        }
    }
}
