using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApp.Core.Interfaces
{
    public interface IPromotionDatabase
    {
        Task<int> InsertAll(ObservableCollection<Models.Promotion> Promotions);

        Task<int> DeleteAll();

        Task<ObservableCollection<Models.Promotion>> GetAllPromotions();
    }
}
