using Android.App;
using Android.Content;
using MvvmCross.Core.ViewModels;
using SalesApp.Core.Interfaces;
using SalesApp.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Plugin.Connectivity;
using PCLStorage;
using System.Net.Http;

namespace SalesApp.Core.ViewModels
{
    public class PromotionViewModel: MvxViewModel
    {
        private readonly IDialogService dialog;
        private IPromotionDatabase promotionDb;
        private ISalesDatabase salesDb;
        private ISalesLineDatabase salesLineDb;
        private ObservableCollection<PromotionItem> _promotionList;
        public ObservableCollection<PromotionItem> PromotionList
        {
            get { return _promotionList; }
            set { _promotionList = value; RaisePropertyChanged(() => PromotionList); }
        }
        public IMvxCommand navCustomers
        {
            get { return new MvxCommand(() => { ShowViewModel<CustomerViewModel>(); Close(this); }); }
        }
        public IMvxCommand navSales
        {
            get { return new MvxCommand(() => { ShowViewModel<SalesViewModel>(); Close(this); }); }
        }
        public IMvxCommand ReadMore
        {
            get {
                return new MvxCommand<PromotionItem>((selected) =>
          {
              GlobalVars.selectedPromotion = selected;
              ShowViewModel<PromotionDetailViewModel>();
          });
            }
        }
        public IMvxCommand Logout
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    if (await dialog.Show("Are you sure you want to logout?", "Logout", "Yes", "No"))
                    {
                        GlobalVars.isLoggedOut = true;
                        var mySales = await salesDb.GetAllSalesWhere(GlobalVars.myDetail.SalesmanId);
                        DateTime timeNow = DateTime.Now;
                        ISharedPreferences mysettings = Application.Context.GetSharedPreferences("mysetting", FileCreationMode.Private); List<Models.SalesTable> mySalesToList = mySales.ToList().Where(x => ((timeNow.Year - x.DateCreated.Year) * 12 + timeNow.Month - x.DateCreated.Month) > mysettings.GetInt("deleteOffset", 6)).ToList();
                        ISharedPreferencesEditor editor = mysettings.Edit();
                        editor.PutBoolean("isLoggedIn", false);
                        editor.Remove("id");
                        editor.Remove("name");
                        editor.Remove("address");
                        editor.Remove("phone");
                        editor.Remove("username");
                        editor.Apply();
                        foreach (Models.SalesTable data in mySalesToList)
                        {
                            await salesLineDb.DeleteSalesLineWhere(data.DocumentNo);
                            await salesDb.DeleteSales(data);
                        }
                        //await salesLineDb.DeleteAll();
                        //await salesDb.DeleteAll();
                        GlobalVars.myDetail = null;
                        ShowViewModel<LoginViewModel>();
                        Close(this);
                    }
                });
            }
        }
        public PromotionViewModel(IPromotionDatabase ipd, ISalesDatabase isd, ISalesLineDatabase isld, IDialogService ids)
        {
            promotionDb = ipd;
            salesDb = isd;
            salesLineDb = isld;
            dialog = ids;

            Task.Run(async () =>
            {
                var initTemp = await promotionDb.GetAllPromotions();
                PromotionList = new ObservableCollection<PromotionItem>();
                foreach (Models.Promotion promotion in initTemp)
                {
                    PromotionList.Add(new PromotionItem(promotion.PromotionId, promotion.PromotionTitle, promotion.PromotionDetail,
                        promotion.PeriodStart, promotion.PeriodEnd, promotion.ImageName));
                }
                RaisePropertyChanged(() => PromotionList);
                if (CrossConnectivity.Current.IsConnected)
                {
                    if (await CrossConnectivity.Current.IsRemoteReachable(Services.ServerDatabaseApi.ipAddress, int.Parse(ServerDatabaseApi.port)))
                    {
                        var serverDb = new ServerDatabaseService();
                        var deleteLocal = await promotionDb.DeleteAll();
                        var downloadFromServer = await serverDb.getAllPromotions();
                        var insertToLocal = promotionDb.InsertAll(downloadFromServer);
                        var PromotionTemp = downloadFromServer;
                        PromotionList = new ObservableCollection<PromotionItem>();
                        foreach (Models.Promotion promotion in PromotionTemp)
                        {
                            //var x = new PromotionItem(promotion.PromotionId, promotion.PromotionTitle, promotion.PromotionDetail,
                            //    promotion.PeriodStart, promotion.PeriodEnd, promotion.ImageName);
                            PromotionList.Add(new PromotionItem(promotion.PromotionId, promotion.PromotionTitle, promotion.PromotionDetail,
                                promotion.PeriodStart, promotion.PeriodEnd, promotion.ImageName));
                            
                            /*if (await rootFolder.CheckExistsAsync(promotion.ImageName) == ExistenceCheckResult.NotFound)
                            {
                                //download
                                byte[] buffer = await client.GetByteArrayAsync(x.ImageURL);
                                temp = await rootFolder.CreateFileAsync(x.ImageName, CreationCollisionOption.FailIfExists);
                            }*/
                        }
                        RaisePropertyChanged(() => PromotionList);

                        //download and save
                        /*HttpClient client = new HttpClient();
                        IFolder rootFolder = FileSystem.Current.LocalStorage;
                        IFile temp;
                        foreach (Models.Promotion promotion in PromotionTemp)
                        {
                            if (await rootFolder.CheckExistsAsync(promotion.ImageName) == ExistenceCheckResult.NotFound)
                            {
                                var x = new PromotionItem(promotion.PromotionId, promotion.PromotionTitle, promotion.PromotionDetail,
                                    promotion.PeriodStart, promotion.PeriodEnd, promotion.ImageName);
                                //download
                                byte[] buffer = await client.GetByteArrayAsync(x.ImageURL);
                                temp = await rootFolder.CreateFileAsync(promotion.ImageName, CreationCollisionOption.FailIfExists);
                            }
                        }*/
                    }
                }

            });
        }
    }
}
