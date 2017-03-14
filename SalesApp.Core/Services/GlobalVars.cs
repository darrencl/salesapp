using SalesApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SalesApp.Core.Services
{
    public static class GlobalVars
    {
        public static Salesman myDetail;

        public static ObservableCollection<Sales> salesList;
        public static bool salesListIsLoaded = false;

        public static Sales selectedSales;

        public static ObservableCollection<SalesItem> salesItemsList;

        public static ObservableCollection<SalesItem> insertSalesItemsList = new ObservableCollection<SalesItem>();
        public static Customer insertSalesSelectedCustomer;
        
        public static ObservableCollection<SalesItem> updateSalesItemsList = new ObservableCollection<SalesItem>();
        public static SalesItem selectedSalesItem;
        //public static SalesItemSelection updateSelectedItem;

        public static ObservableCollection<Services.Customer> customerList;
        public static bool customerListIsLoaded = false;

        public static ObservableCollection<SalesItemSelection> itemCatalogue;

        public static Customer selectedCustomer;

        public static PromotionItem selectedPromotion;

        public static bool isLoggedOut = false;
    }
}
