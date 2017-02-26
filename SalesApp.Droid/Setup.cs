using Android.Content;
using MvvmCross.Droid.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using SalesApp.Core.Interfaces;
using SalesApp.Droid.Services;
using SalesApp.Core.Database;
using SalesApp.Droid.Database;

namespace SalesApp.Droid
{

    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new SalesApp.Core.App();
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }
        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            return new CustomPresenter();
        }
        public class CustomPresenter : MvxAndroidViewPresenter
        {
            protected override void Show(Intent intent)
            {
                Activity.StartActivity(intent);
                Activity.OverridePendingTransition(0, 0);
            }
        }
        protected override void InitializeFirstChance()
        {
            Mvx.LazyConstructAndRegisterSingleton<IDialogService, DialogService>();
            Mvx.LazyConstructAndRegisterSingleton<ICustomerDatabase, CustomerDatabase>();
            Mvx.LazyConstructAndRegisterSingleton<IGeoCoder, GeoCoder>();
            Mvx.LazyConstructAndRegisterSingleton<ISalesDatabase, SalesDatabase>();
            Mvx.LazyConstructAndRegisterSingleton<ISalesLineDatabase, SalesLineDatabase>();
            Mvx.LazyConstructAndRegisterSingleton<IItemDatabase, ItemDatabase>();
            Mvx.LazyConstructAndRegisterSingleton<ISQLite, SqliteDroid>();
            base.InitializeFirstChance();
        }
    }
}
