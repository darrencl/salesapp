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
using Android.Gms.Common;
using Firebase.Messaging;
using Firebase.Iid;
using Android.Util;
using SalesApp.Core.Services;

namespace SalesApp.Droid.Views
{ 
    [Activity(Label = "Promotion List")]
    public class PromotionView: MvxActivity
    {
        const string TAG = "MainActivity";
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.PromotionView);
            ActionBar.Hide();

            Log.Debug(TAG, "InstanceID token: " + FirebaseInstanceId.Instance.Token);
            if (Intent.Extras != null)
            {
                foreach (var key in Intent.Extras.KeySet())
                {
                    var value = Intent.Extras.GetString(key);
                    Log.Debug(TAG, "Key: {0} Value: {1}", key, value);
                }
            }
            Log.Debug(TAG, "google app id: " + Resource.String.google_app_id);
            FirebaseMessaging.Instance.SubscribeToTopic("promotion");
            Log.Debug(TAG, "Subscribed to remote notifications");
        }
        protected override void OnStop()
        {
            base.OnStop();
            if (GlobalVars.isLoggedOut)
            {
                FirebaseMessaging.Instance.UnsubscribeFromTopic("promotion");
                Log.Debug(TAG, "Unsubscribed from topic");
            }
        }
    }
}