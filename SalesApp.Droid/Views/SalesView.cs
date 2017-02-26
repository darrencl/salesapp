using Android.App;
using Android.OS;
using MvvmCross.Droid.Views;
using Android.Content;
using Android.Runtime;
using System;
using SalesApp.Core.ViewModels;
using Android.Widget;
using System.Threading;
using System.Threading.Tasks;

namespace SalesApp.Droid.Views
{
    [Activity(Label = "Sales List")]
    public class SalesView :MvxActivity
    {
        SalesViewModel vm;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SalesView);
            vm = ViewModel as SalesViewModel;
            ActionBar.Hide();

            ImageView syncButton = FindViewById<ImageView>(Resource.Id.btnSync);
            syncButton.Click += delegate
            {
                var progressDialog = ProgressDialog.Show(this, "Please wait...", "Sync is in progress...", true);
                new Thread(new ThreadStart(async delegate
               {
                   await vm.CmdSync();
                   RunOnUiThread(() => progressDialog.Dismiss());
               })).Start();
            };
        }
        protected override void OnResume()
        {
            base.OnResume();
            vm.loadSales();
            vm.refresh();
        }
    }
}