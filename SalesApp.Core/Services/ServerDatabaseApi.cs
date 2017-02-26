using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApp.Core.Services
{
    public class ServerDatabaseApi
    {
        public static string ipAddress = "192.168.0.102";
        public static string port = "3000";
        public static string WebServiceAddress = "http://" + ipAddress + ":" + port + "/";

        public static string loginEndpoint = WebServiceAddress + "login/";
        public static string getDetailEndpoint = WebServiceAddress + "mydetail/";

        public static string insertCustomerEndpoint = WebServiceAddress + "insertcustomer/";
        public static string getAllCustomersEndpoint = WebServiceAddress + "customers";

        public static string getAllItemsEndpoint = WebServiceAddress + "items";

        public static string getMySalesEndpoint = WebServiceAddress + "sales/";
        public static string insertSalesEndpoint = WebServiceAddress + "insertsales";

        public static string getSalesLinesEndpoint = WebServiceAddress + "saleslines/";
        public static string insertSalesLinesEndpoint = WebServiceAddress + "insertsalesline";
    }
}
