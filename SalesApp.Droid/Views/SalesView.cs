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
using Android.Util;
using SalesApp.Core.Services;
using Firebase.Messaging;
//using Firebase.Iid;
//using Firebase.Messaging;

namespace SalesApp.Droid.Views
{
    [Activity(Label = "Sales List")]
    public class SalesView :MvxActivity
    {
        SalesViewModel vm;
        private bool isResuming = false;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SalesView);
            vm = ViewModel as SalesViewModel;
            ActionBar.Hide();

            //GCM TEST
            /*Log.Debug("SalesView", "InstanceID token: " + FirebaseInstanceId.Instance.Token);
            if (Intent.Extras != null)
            {
                foreach (var key in Intent.Extras.KeySet())
                {
                    var value = Intent.Extras.GetString(key);
                    Log.Debug("SalesView", "Key: {0} Value: {1}", key, value);
                    Log.Debug("SalesView", "google app id: " + Resource.String.google_app_id);
                }
            }
            FirebaseMessaging.Instance.SubscribeToTopic("promotion");
            Log.Debug("SalesView", "Subscribed to remote notification");*/
            //GCM TEST END
            ImageView syncButton = FindViewById<ImageView>(Resource.Id.btnSync);
            syncButton.Click += delegate
            {
                var progressDialog = ProgressDialog.Show(this, "Please wait...", "Sync is in progress...", true);
                new Thread(new ThreadStart(async delegate
               {
                   await vm.CmdSync();
                   vm.refresh();
                   RunOnUiThread(() => progressDialog.Dismiss());
               })).Start();
            };
        }
        protected override async void OnResume()
        {
            base.OnResume();
            if (!isResuming)
            {
                isResuming = true;
                await vm.loadSales();
                vm.refresh();
                isResuming = false;
            }
        }
        protected override void OnStop()
        {
            base.OnStop();
            if (GlobalVars.isLoggedOut)
                FirebaseMessaging.Instance.UnsubscribeFromTopic("promotion");
        }
    }
}