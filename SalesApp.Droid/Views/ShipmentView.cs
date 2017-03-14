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
using Android.Telephony;
using ZXing.Mobile;
using SalesApp.Core.ViewModels;

namespace SalesApp.Droid.Views
{
    [Activity(Label = "Shipment View")]
    public class ShipmentView: MvxActivity
    {
        ShipmentViewModel vm;
        private SmsManager _smsManager;
        private BroadcastReceiver _smsSentBroadcastReceiver, _smsDeliveredBroadcastReceiver;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            MobileBarcodeScanner.Initialize(Application);
            SetContentView(Resource.Layout.ShipmentView);
            vm = ViewModel as ShipmentViewModel;
            ActionBar.Hide();
            this.Window.SetSoftInputMode(Android.Views.SoftInput.AdjustPan);
            _smsManager = SmsManager.Default;
            
            

            Button postButton = FindViewById<Button>(Resource.Id.btnPost);
            postButton.Click += async (s, e) =>
            {
                //add scanned item to server database using method in viewmodel
                bool sent = await vm.postShipment();
                //var email = new Intent(Android.Content.Intent.ActionSend);
                //email.PutExtra(Android.Content.Intent.ExtraEmail, new string[] { "darrenc2995@gmail.com" });

                //email.PutExtra(Android.Content.Intent.ExtraSubject, "Item Shipment");

                //email.PutExtra(Android.Content.Intent.ExtraText, "Your items are on their way.\nShipment number " + vm.ShipmentNumber);
                //email.SetType("message/rfc822");
                if (sent)
                {
                    var phone = vm._customerDetail.Phone;
                    var message = "Your items are on their way.\nShipment number " + vm.ShipmentNumber;
                    var piSent = PendingIntent.GetBroadcast(this, 0, new Intent("SMS_SENT"), 0);
                    var piDelivered = PendingIntent.GetBroadcast(this, 0, new Intent("SMS_DELIVERED"), 0);

                    _smsManager.SendTextMessage(phone, null, message, piSent, piDelivered);
                    //StartActivity(email);
                    vm.resetForm();
                }
            };
        }
        protected override void OnResume()
        {
            base.OnResume();
            _smsSentBroadcastReceiver = new SMSSentReceiver();
            _smsDeliveredBroadcastReceiver = new SMSDeliveredReceiver();

            RegisterReceiver(_smsSentBroadcastReceiver, new IntentFilter("SMS_SENT"));
            RegisterReceiver(_smsDeliveredBroadcastReceiver, new IntentFilter("SMS_DELIVERED"));
        }
        protected override void OnStart()
        {
            base.OnStart();
            _smsSentBroadcastReceiver = new SMSSentReceiver();
            _smsDeliveredBroadcastReceiver = new SMSDeliveredReceiver();

            RegisterReceiver(_smsSentBroadcastReceiver, new IntentFilter("SMS_SENT"));
            RegisterReceiver(_smsDeliveredBroadcastReceiver, new IntentFilter("SMS_DELIVERED"));
        }
        protected override void OnPause()
        {
            base.OnPause();

            UnregisterReceiver(_smsSentBroadcastReceiver);
            UnregisterReceiver(_smsDeliveredBroadcastReceiver);
        }
    }
    [BroadcastReceiver(Exported = true)]
    public class SMSSentReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            switch ((int)ResultCode)
            {
                case (int)Result.Ok:
                    Toast.MakeText(Application.Context, "SMS has been sent", ToastLength.Short).Show();
                    break;
                case (int)SmsResultError.GenericFailure:
                    Toast.MakeText(Application.Context, "Generic Failure", ToastLength.Short).Show();
                    break;
                case (int)SmsResultError.NoService:
                    Toast.MakeText(Application.Context, "No Service", ToastLength.Short).Show();
                    break;
                case (int)SmsResultError.NullPdu:
                    Toast.MakeText(Application.Context, "Null PDU", ToastLength.Short).Show();
                    break;
                case (int)SmsResultError.RadioOff:
                    Toast.MakeText(Application.Context, "Radio Off", ToastLength.Short).Show();
                    break;
            }
        }
    }

    [BroadcastReceiver(Exported = true)]
    public class SMSDeliveredReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            switch ((int)ResultCode)
            {
                case (int)Result.Ok:
                    Toast.MakeText(Application.Context, "SMS Delivered", ToastLength.Short).Show();
                    break;
                case (int)Result.Canceled:
                    Toast.MakeText(Application.Context, "SMS not delivered", ToastLength.Short).Show();
                    break;
            }
        }
    }
}