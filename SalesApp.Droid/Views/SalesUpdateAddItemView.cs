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
using ZXing.Mobile;
using System.Threading;
using SalesApp.Core.ViewModels;

namespace SalesApp.Droid.Views
{
    [Activity(Label = "Add/Update Item")]
    public class SalesUpdateAddItemView : MvxActivity
    {
        SalesUpdateAddItemViewModel vm;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            MobileBarcodeScanner.Initialize(Application);
            SetContentView(Resource.Layout.SalesUpdateAddItemView);
            vm = ViewModel as SalesUpdateAddItemViewModel;
            ActionBar.Hide();
            this.Window.SetSoftInputMode(Android.Views.SoftInput.AdjustPan);


            ImageView syncButton = FindViewById<ImageView>(Resource.Id.btnSyncItemUpdate);
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