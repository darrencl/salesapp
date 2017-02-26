using Android.App;
using Android.OS;
using MvvmCross.Droid.Views;
using Android.Content;
using Android.Widget;
using System.Threading;
using SalesApp.Core.ViewModels;
using ZXing.Mobile;

namespace SalesApp.Droid.Views
{
    [Activity(Label = "Add Item")]
    public class SalesAddItemView : MvxActivity
    {
        SalesAddItemViewModel vm;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            MobileBarcodeScanner.Initialize(Application);
            SetContentView(Resource.Layout.SalesAddItemView);
            vm = ViewModel as SalesAddItemViewModel;
            ActionBar.Hide();
            this.Window.SetSoftInputMode(Android.Views.SoftInput.AdjustPan);


            ImageView syncButton = FindViewById<ImageView>(Resource.Id.btnSyncItem);
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