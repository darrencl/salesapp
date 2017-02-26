using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
using System.IO;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http;

namespace SalesApp.Core.Services
{
    public class ServerDatabaseService
    {
        /*Function to check login into the server database
         * Return false if login failed, true if success
         */
        public async Task<int> checkLogin(string username, string password)
        {
            int result = 0;
            WebRequest request = WebRequest.CreateHttp(ServerDatabaseApi.loginEndpoint + username + "/" + password);
            string responseValue = null;
            try
            {
                using (var response = await request.GetResponseAsync())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream != null)
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                responseValue = await reader.ReadToEndAsync();
                            }
                        }
                    }
                }
                var sresponse = JsonConvert.DeserializeObject<Models.loginTemp>(responseValue);
                if (sresponse != null)
                {
                    if (sresponse.counter == 1)
                        result = 1;
                    else
                        result = 0;
                }
                else
                    result = 0;
                return result;
            }
            catch (WebException we)
            {
                var response = we.Response as HttpWebResponse;
                if (response != null)
                {
                    if ((int)response.StatusCode == 404)
                        return 0;
                    else return 3;
                }
                else return 3;
            }
        }

        public async Task<Models.Salesman> getDetail(string username)
        {
            WebRequest request = WebRequest.CreateHttp(ServerDatabaseApi.getDetailEndpoint + username);
            string responseValue = null;
            try
            {
                using (var response = await request.GetResponseAsync())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream != null)
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                responseValue = await reader.ReadToEndAsync();
                            }
                        }
                    }
                }
                var sresponse = JsonConvert.DeserializeObject<Models.Salesman>(responseValue);
                if (sresponse != null)
                {
                    return sresponse;
                }
                else
                    return null;
            }
            catch (WebException we)
            {
                return null;
            }
        }

        public async Task<int> insertCustomers(Customer newcustomer)
        {
            WebRequest request = WebRequest.CreateHttp(ServerDatabaseApi.insertCustomerEndpoint);
            request.Method = "POST";
            request.ContentType = "application/json";
            try
            {
                using (var streamwriter = new StreamWriter(await request.GetRequestStreamAsync()))
                {
                    string json = JsonConvert.SerializeObject(newcustomer);
                    streamwriter.Write(json);
                    streamwriter.Flush();
                }
                return 1;
            }
            catch (WebException we)
            {
                return 0;
            }
        }

        public async Task<ObservableCollection<Models.Customer>> getAllCustomers()
        {
            WebRequest request = WebRequest.CreateHttp(ServerDatabaseApi.getAllCustomersEndpoint);
            string responseValue = null;
            try
            {
                using (var response = await request.GetResponseAsync())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream != null)
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                responseValue = await reader.ReadToEndAsync();
                            }
                        }
                    }
                }
                var sresponse = JsonConvert.DeserializeObject<ObservableCollection<Models.Customer>>(responseValue);
                if (sresponse != null)
                {
                    return sresponse;
                }
                else
                    return null;
            }
            catch (WebException we)
            {
                return null;
            }
        }

        public async Task<ObservableCollection<Models.SalesTable>> getMySales(string salesmanId)
        {
            WebRequest request = WebRequest.CreateHttp(ServerDatabaseApi.getMySalesEndpoint + salesmanId);
            string responseValue = null;
            try
            {
                using (var response = await request.GetResponseAsync())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream != null)
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                responseValue = await reader.ReadToEndAsync();
                            }
                        }
                    }
                }
                var sresponse = JsonConvert.DeserializeObject<ObservableCollection<Models.SalesTable>>(responseValue);
                if (sresponse != null)
                {
                    return sresponse;
                }
                else return null; 
            }
            catch (WebException we)
            {
                return null;
            }
        }
        public async Task<int> insertSales(IEnumerable<Models.SalesTable> newsales)
        {

            var ipAddress = ServerDatabaseApi.ipAddress;// your IP address here
            var port = ServerDatabaseApi.port; // your port here
            var endpoint = $"http://{ipAddress}:{port}/insertsales";
            var requestString = JsonConvert.SerializeObject(newsales, Formatting.Indented);
            var content = new StringContent(requestString, Encoding.UTF8, "application/json");
            try
            {
                using (var client = new HttpClient())
                {
                    var reponse = await client.PostAsync(endpoint, content);
                    if (reponse.IsSuccessStatusCode)
                        return 1;
                    else
                        return 0;
                }
            }
            catch (WebException we)
            {
                return 0;
            }
        }
        public async Task<int> insertSalesLines(IEnumerable<Models.SalesLineTable> newsalesitems)
        {
            var ipAddress = ServerDatabaseApi.ipAddress;// your IP address here
            var port = ServerDatabaseApi.port; // your port here
            var endpoint = $"http://{ipAddress}:{port}/insertsalesline";
            var requestString = JsonConvert.SerializeObject(newsalesitems, Formatting.Indented);
            var content = new StringContent(requestString, Encoding.UTF8, "application/json");
            try
            {
                using (var client = new HttpClient())
                {
                    var reponse = await client.PostAsync(endpoint, content);
                    if (reponse.IsSuccessStatusCode)
                        return 1;
                    else
                        return 0;
                }
            }
            catch (WebException we)
            {
                return 0;
            }
        }
        public async Task<ObservableCollection<Models.Item>> getAllItems()
        {
            WebRequest request = WebRequest.CreateHttp(ServerDatabaseApi.getAllItemsEndpoint);
            string responseValue = null;
            try
            {
                using (var response = await request.GetResponseAsync())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream != null)
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                responseValue = await reader.ReadToEndAsync();
                            }
                        }
                    }
                }
                var sresponse = JsonConvert.DeserializeObject<ObservableCollection<Models.Item>>(responseValue);
                if (sresponse != null)
                {
                    return sresponse;
                }
                else
                    return null;
            }
            catch (WebException we)
            {
                return null;
            }
        }

        public async Task<ObservableCollection<Models.SalesLineTable>> getMySalesLines(string salesmanId)
        {
            WebRequest request = WebRequest.CreateHttp(ServerDatabaseApi.getSalesLinesEndpoint + salesmanId);
            string responseValue = null;
            try
            {
                using (var response = await request.GetResponseAsync())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream != null)
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                responseValue = await reader.ReadToEndAsync();
                            }
                        }
                    }
                }
                var sresponse = JsonConvert.DeserializeObject<ObservableCollection<Models.SalesLineTable>>(responseValue);
                if (sresponse != null)
                {
                    return sresponse;
                }
                else
                    return null;
            }
            catch (WebException we)
            {
                return null;
            }
        }
    }
}
