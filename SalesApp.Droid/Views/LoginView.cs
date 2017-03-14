using Android.App;
using Android.OS;
using MvvmCross.Droid.Views;
using Android.Content;
using SalesApp.Core.ViewModels;
using SalesApp.Core.Models;
using SalesApp.Core.Services;

namespace SalesApp.Droid.Views
{
    [Activity(Label = "Login")]
    public class LoginView : MvxActivity
    {
        ISharedPreferences mysetting = Application.Context.GetSharedPreferences("mysetting", FileCreationMode.Private);
        LoginViewModel vm;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            ActionBar.Hide();
            vm = ViewModel as LoginViewModel;
            //skipping Login screen when user has Logged in
            if (mysetting.GetBoolean("isLoggedIn", false) == true)
            {
                GlobalVars.myDetail = new Salesman();
                GlobalVars.myDetail.SalesmanId = mysetting.GetString("id", "");
                GlobalVars.myDetail.Name = mysetting.GetString("name", "");
                GlobalVars.myDetail.Phone = mysetting.GetString("phone", "");
                GlobalVars.myDetail.Address = mysetting.GetString("address", "");
                vm.goToSalesView();
            }
            else
            {
                //login screen
                SetContentView(Resource.Layout.LoginView);
            }
        }
        protected override void OnUserLeaveHint()
        {
            base.OnUserLeaveHint();
            if (GlobalVars.myDetail != null)
            {
                if (!string.IsNullOrEmpty(GlobalVars.myDetail.SalesmanId))
                {
                    saveDetails(GlobalVars.myDetail);
                }
                else
                {
                    if (mysetting.Contains("id"))
                    {
                        GlobalVars.myDetail = new Salesman(mysetting.GetString("id", ""), mysetting.GetString("name", ""), mysetting.GetString("address", ""), mysetting.GetString("phone", ""), mysetting.GetString("username", ""));
                        
                    }
                }
            }
            else
            {
                if (mysetting.Contains("id"))
                {
                    GlobalVars.myDetail = new Salesman(mysetting.GetString("id", ""), mysetting.GetString("name", ""), mysetting.GetString("address", ""), mysetting.GetString("phone", ""), mysetting.GetString("username", ""));
                    
                }
            }
        }
        private void saveDetails(Salesman mydetail)
        {
            ISharedPreferencesEditor editor = mysetting.Edit();
            editor.PutString("id", mydetail.SalesmanId);
            editor.PutString("name", mydetail.Name);
            editor.PutString("address",mydetail.Address);
            editor.PutString("phone", mydetail.Phone);
            editor.PutString("username", mydetail.Username);
            editor.PutBoolean("isLoggedIn", true);
            editor.PutInt("deleteOffset", 6);
            editor.Apply();
        }
    }
}