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

namespace SalesApp.Core.ViewModels
{
    public class LoginViewModel :MvxViewModel
    {
        private readonly IDialogService dialog;
        private ISalesDatabase salesDb;
        private ISalesLineDatabase salesLineDb;
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
        public IMvxCommand Login
        {
            get
            {
                return new MvxCommand<LoginViewModel>((selected) =>
                {
                    if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
                        funcLogin(Username, Password);
                    else
                        dialog.Show("Please fill both username and password", "Login failed");
                });
            }
        }
        
        public LoginViewModel(IDialogService ids, ISalesDatabase isd, ISalesLineDatabase isld)
        {
            this.dialog = ids;
            salesDb = isd;
            salesLineDb = isld;
        }

        /* Function: funcLogin
         * input: username and password
         * This function performs validation of user and password with the database
         * if the login credential correct, this method leads to showing sales screen
         * otherwise, it is still in login page
         */
        private async void funcLogin(string username, string password)
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
                    if (mySales != null)
                    {
                        await salesDb.InsertAllSales(mySales);
                        await salesDb.UpdateAllTransferred();
                    }
                    var mySalesLines = await serverDb.getMySalesLines(tmp.SalesmanId);
                    if (mySalesLines != null)
                        await salesLineDb.InsertSalesLines(mySalesLines);
                    GlobalVars.myDetail = new Models.Salesman(tmp.SalesmanId, tmp.Name, tmp.Address, tmp.Phone, tmp.Username);
                    ShowViewModel<SalesViewModel>();
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
            ShowViewModel<SalesViewModel>();
            Close(this);
        }
    }
}
