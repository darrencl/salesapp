using SalesApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApp.Core.Interfaces
{
    public interface IItemDatabase
    {
        Task<IEnumerable<Item>> GetAllItems();

        Task<Item> GetItemDetail(int itemId);

        Task<int> InsertItems(ObservableCollection<Item> newItems);

        Task<int> DeleteItem(Item deletedItems);

        Task<int> DeleteAll();
    }
}
