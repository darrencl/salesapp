using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Views;
using System.Threading;
using SalesApp.Core.ViewModels;

namespace SalesApp.Droid.Views
{
    [Activity(Label = "Sales Update")]
    public class SalesUpdateView : MvxActivity
    {
        SalesUpdateViewModel vm;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.SalesUpdateView);
            vm = ViewModel as SalesUpdateViewModel;
            ActionBar.Hide();
            this.Window.SetSoftInputMode(Android.Views.SoftInput.AdjustPan);

            Button updateButton = FindViewById<Button>(Resource.Id.btnUpdate);
            updateButton.Click += delegate
            {
                var progressDialog = ProgressDialog.Show(this, "Please wait...", "Transaction is being processed", true);
                new Thread(new ThreadStart(async delegate
                {
                    await vm.ProceedUpdateCommand();
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