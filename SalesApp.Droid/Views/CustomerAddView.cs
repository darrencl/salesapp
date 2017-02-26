using Android.App;
using Android.OS;
using MvvmCross.Droid.Views;
using Android.Content;

namespace SalesApp.Droid.Views
{
    [Activity(Label = "Add New Customer")]
    public class CustomerAddView : MvxActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.CustomerAddView);
            ActionBar.Hide();
            this.Window.SetSoftInputMode(Android.Views.SoftInput.AdjustPan);
        }
    }
}