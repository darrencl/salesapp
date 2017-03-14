using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApp.Core.Services
{
    public class ServerDatabaseApi
    {
        public static string myip;
        public static string ipAddress { get { return myip; } }
        public static string port { get { return "3000"; } }
        public static string WebServiceAddress {get { return "http://" + ipAddress + ":" + port + "/"; } }

        public static string loginEndpoint { get { return WebServiceAddress + "login/"; } }
        public static string getDetailEndpoint { get { return WebServiceAddress + "mydetail/"; } }

        public static string getAllSalesEndpoint { get { return WebServiceAddress + "sales"; } }
        public static string getAllSalesLinesEndpoint { get { return WebServiceAddress + "saleslines"; } }

        public static string insertCustomerEndpoint { get { return WebServiceAddress + "insertcustomer/"; } }
        public static string getAllCustomersEndpoint { get { return WebServiceAddress + "customers"; } }

        public static string getAllItemsEndpoint { get { return WebServiceAddress + "items"; } }

        public static string getMySalesEndpoint { get { return WebServiceAddress + "sales/"; } }
        public static string insertSalesEndpoint { get { return WebServiceAddress + "insertsales"; } }

        public static string getSalesLinesEndpoint { get { return WebServiceAddress + "saleslines/"; } }
        public static string insertSalesLinesEndpoint { get { return WebServiceAddress + "insertsalesline"; } }

        public static string getPromotionEndpoint { get { return WebServiceAddress + "promotion/"; } }
        public static string getImageEndpoint { get { return WebServiceAddress + "images/"; } }

        public static string getNextShipmentIdEndpoint { get { return WebServiceAddress + "nextshipmentid/"; } }
    }
}
