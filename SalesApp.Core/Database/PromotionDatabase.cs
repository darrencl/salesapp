using SalesApp.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalesApp.Core.Models;
using System.Collections.ObjectModel;
using SQLite.Net;
using MvvmCross.Platform;

namespace SalesApp.Core.Database
{
    public class PromotionDatabase : IPromotionDatabase
    {
        private readonly SQLiteConnection database;
        public PromotionDatabase()
        {
            var sqlite = Mvx.Resolve<ISQLite>();
            database = sqlite.GetConnection();
            database.CreateTable<Promotion>();
        }
        public Task<int> DeleteAll()
        {
            var num = database.DeleteAll<Promotion>();
            database.Commit();
            return Task.FromResult(num);
        }

        public Task<int> InsertAll(ObservableCollection<Promotion> Promotions)
        {
            var num = database.InsertAll(Promotions);
            database.Commit();
            return Task.FromResult(num);
        }
        public Task<ObservableCollection<Promotion>> GetAllPromotions()
        {
            return Task.FromResult(new ObservableCollection<Promotion>(database.Table<Promotion>().ToList()));
        }
    }
}
