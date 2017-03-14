using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApp.Core.Services
{
    public class PromotionItem:MvxViewModel
    {
        public string PromotionId { get; set; }
        public string PromotionTitle { get; set; }
        public string PromotionDetail { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public string PeriodDisplay
        {
            get
            {
                return PeriodStart.Day.ToString() + "/" + PeriodStart.Month.ToString() + "/" + PeriodStart.Year.ToString() +
                    " - " + PeriodEnd.Day.ToString() + "/" + PeriodEnd.Month.ToString() + "/" + PeriodEnd.Year.ToString();
            }
        }
        public string ImageName { get; set; }
        public string ImageURL { get { return ServerDatabaseApi.getImageEndpoint + ImageName; } }

        public PromotionItem() { }
        public PromotionItem(string promotionId, string promotionTitle, string promotionDetail, DateTime periodStart, DateTime periodEnd, string imageName)
        {
            PromotionId = promotionId;
            PromotionTitle = promotionTitle;
            PromotionDetail = promotionDetail;
            PeriodStart = periodStart;
            PeriodEnd = periodEnd;
            ImageName = imageName;
        }

        public IMvxCommand ReadMore
        {
            get { return new MvxCommand(() => { }); }
        }
    }
}
