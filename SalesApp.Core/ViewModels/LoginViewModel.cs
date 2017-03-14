using MvvmCross.Core.ViewModels;
using SalesApp.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using SalesApp.Core.Services;
using System.Collections.ObjectModel;
using Android.Content;
using Android.App;

namespace SalesApp.Core.ViewModels
{
    public class LoginViewModel :MvxViewModel
    {
        private readonly IDialogService dialog;
        private ISalesDatabase salesDb;
        private ISalesLineDatabase salesLineDb;
        private ICustomerDatabase customerDb;
        private IItemDatabase itemDb;
        private string username;
        public string Username
        {
            get { return username; }
            set { username = value; RaisePropertyChanged(() => Username); }
        }
        private string password;
        public string Password
        {
            get { return password; }
            set { password = value; RaisePropertyChanged(() => Password); }
        }
        public string IpAddress
        {
            get { return ServerDatabaseApi.myip; }
            set { ServerDatabaseApi.myip = value; RaisePropertyChanged(() => IpAddress); }
        }
        public IMvxCommand Login
        {
            get
            {
                return new MvxCommand<LoginViewModel>((selected) =>
                {
                    if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(IpAddress))
                        funcLogin(Username, Password);
                    else
                        dialog.Show("Please fill both username and password", "Login failed");
                });
            }
        }
        
        public LoginViewModel(IDialogService ids, ISalesDatabase isd, ISalesLineDatabase isld, ICustomerDatabase icd, IItemDatabase iid)
        {
            this.dialog = ids;
            salesDb = isd;
            salesLineDb = isld;
            customerDb = icd;
            itemDb = iid;
        }

        /* Function: funcLogin
         * input: username and password
         * This function performs validation of user and password with the database
         * if the login credential correct, this method leads to showing sales screen
         * otherwise, it is still in login page
         */
        private async void funcLogin(string username, string password)
        {
            if (username == "warehouse" && password == "warehouse")
            {
                var serverDb = new ServerDatabaseService();
                try
                {
                    var deleteLocalSalesLines = await salesLineDb.DeleteAll();
                    var deleteLocalSales = await salesDb.DeleteAll();
                    var deleteLocalCustomer = await customerDb.DeleteAll();
                    var deleteLocalItem = await itemDb.DeleteAll();

                    var allSales = await serverDb.getAllSales();
                    var allSalesLines = await serverDb.getAllSalesLines();
                    var allCustomer = await serverDb.getAllCustomers();
                    var allItem = await serverDb.getAllItems();

                    await salesDb.InsertAllSales(allSales);
                    await salesLineDb.InsertSalesLines(allSalesLines);
                    await customerDb.InsertAllCustomers(allCustomer);
                    await itemDb.InsertItems(allItem);
                    //go to shipment view
                    ShowViewModel<ShipmentViewModel>();
                    Close(this);
                }
                catch (Exception e)
                {
                    await dialog.Show(e.Message, "Error");
                }
            }
            else
            {
                //integrate with server database to check username and password
                var serverDb = new ServerDatabaseService();
                int isLogged = await serverDb.checkLogin(username, password);
                if (isLogged == 1)
                {
                    loggingIn();
                }
                else if (isLogged == 0)
                {
                    //login fail, send notification to user
                    await dialog.Show("Username or password is incorrect. Please try again.", "Login Failed");
                }
                else if (isLogged == 3)
                {
                    //network error
                    await dialog.Show("Please check your network and try again later", "Network Error");
                }
            }
        }
        public async void loggingIn()
        {
            //set globalvars, request detail from database, load previous transactions (sales and salesline) done by salesman
            var serverDb = new ServerDatabaseService();
            try
            {
                Models.Salesman tmp = await serverDb.getDetail(Username);
                if (tmp != null)
                {
                    var mySales = await serverDb.getMySales(tmp.SalesmanId);
                    ISharedPreferences mysettings = Application.Context.GetSharedPreferences("mysetting", FileCreationMode.Private);
                    var myExistingSales = await salesDb.GetAllSalesWhere(tmp.SalesmanId);
                    List<Models.SalesTable> mySalesToBeAdded;
                    if (myExistingSales != null)
                        mySalesToBeAdded = mySales.Except(myExistingSales.ToList()).ToList();
                    else
                        mySalesToBeAdded = new List<Models.SalesTable>();
                    if (mySalesToBeAdded != null)
                    {
                        await salesDb.InsertAllSales(new ObservableCollection<Models.SalesTable>((mySalesToBeAdded.Select(x => { x.Transferred(); return x; })).ToList()));
                    }
                    var listOfDocNo = mySalesToBeAdded.Select(x => x.DocumentNo).ToList();
                    var mySalesLines = await serverDb.getMySalesLines(tmp.SalesmanId);
                    var mySalesLinesToBeAdded = mySalesLines.Where(x => listOfDocNo.Contains(x.DocumentNo)).ToList();
                    if (mySalesLinesToBeAdded != null)
                        await salesLineDb.InsertSalesLines(new ObservableCollection<Models.SalesLineTable>(mySalesLinesToBeAdded));
                    GlobalVars.myDetail = new Models.Salesman(tmp.SalesmanId, tmp.Name, tmp.Address, tmp.Phone, tmp.Username);
                    ShowViewModel<PromotionViewModel>();
                    Close(this);
                }
            }
            catch (Exception e)
            {
                await dialog.Show(e.Message, "Error");
            }
        }
        public async void goToSalesView()
        {
            await dialog.Show("Logged in as " + GlobalVars.myDetail.Name, "Login Succeess");
            ShowViewModel<PromotionViewModel>();
            Close(this);
        }
    }
}
