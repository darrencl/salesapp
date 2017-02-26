using Android.App;
using Android.OS;
using MvvmCross.Droid.Views;
using Android.Content;
using Android.Widget;
using Android.Locations;
using Android.Runtime;
using System;
using SalesApp.Core.ViewModels;
using System.Threading;

namespace SalesApp.Droid.Views
{
    [Activity(Label = "New Sales")]
    public class SalesAddView : MvxActivity
    {
        SalesAddViewModel vm;

        protected override void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);
            SetContentView(Resource.Layout.SalesAddView);
            vm = ViewModel as SalesAddViewModel;
            ActionBar.Hide();
            this.Window.SetSoftInputMode(Android.Views.SoftInput.AdjustPan);

            Button proceedButton = FindViewById<Button>(Resource.Id.btnProceed);
            proceedButton.Click += delegate
            {
                var progressDialog = ProgressDialog.Show(this, "Please wait...", "Transaction is being processed", true);
                new Thread(new ThreadStart(async delegate
                {
                    await vm.ProceedCommand();
                    RunOnUiThread(() => progressDialog.Dismiss());
                })).Start();
            };

        }
        protected override void OnResume()
        {
            base.OnResume();
            vm.refreshData();
        }
    }
}