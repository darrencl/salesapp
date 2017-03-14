using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalesApp.Core.Services;

namespace SalesApp.Core.ViewModels
{
    public class PromotionDetailViewModel: MvxViewModel
    {
        public string PromotionTitle
        {
            get { return GlobalVars.selectedPromotion.PromotionTitle; }
        }
        public string Period
        {
            get { return GlobalVars.selectedPromotion.PeriodDisplay; }
        }
        public string PromotionDetailText
        {
            get { return GlobalVars.selectedPromotion.PromotionDetail; }
        }
        public string ImageURL
        {
            get { return GlobalVars.selectedPromotion.ImageURL; }
        }
        public IMvxCommand Back
        {
            get { return new MvxCommand(() => 
            {
                GlobalVars.selectedPromotion = null;
                Close(this);
            }); }
        }
        public PromotionDetailViewModel()
        {

        }
    }
}
