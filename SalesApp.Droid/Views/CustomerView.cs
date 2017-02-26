using Android.App;
using Android.OS;
using MvvmCross.Droid.Views;
using Android.Content;
using Android.Support.V7.App;
using Android.Widget;
using SalesApp.Core.ViewModels;
using System.Threading;

namespace SalesApp.Droid.Views
{
    [Activity(Label = "Customer List")]
    public class CustomerView: MvxActivity
    {
        CustomerViewModel vm;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.CustomerView);
            vm = ViewModel as CustomerViewModel;
            ActionBar.Hide();

            ImageView syncButton = FindViewById<ImageView>(Resource.Id.btnSyncCustomer);
            syncButton.Click += delegate
            {
                var progressDialog = ProgressDialog.Show(this, "Please wait...", "Sync is in progress...", true);
                new Thread(new ThreadStart(async delegate
                {
                    await vm.SyncCommand();
                    RunOnUiThread(() => progressDialog.Dismiss());
                })).Start();
            };
        }
    }
}