using Android.Graphics;
using Newtonsoft.Json;
using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApp.Core.Models
{
    public class Promotion:IEquatable<Promotion>
    {
        [PrimaryKey]
        public string PromotionId { get; set; }
        public string PromotionTitle { get; set; }
        public string PromotionDetail { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public string ImageName { get; set; }

        public Promotion() { }
        public Promotion(string promotionId, string promotionTitle, string promotionDetail, DateTime periodStart, DateTime periodEnd, string imageName)
        {
            PromotionId = promotionId;
            PromotionTitle = promotionTitle;
            PromotionDetail = promotionDetail;
            PeriodStart = periodStart;
            PeriodEnd = periodEnd;
            ImageName = imageName;
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Promotion o = obj as Promotion;
            if (o == null) return false;
            return Equals(o);
        }
        public override int GetHashCode()
        {
            int hashPromoId = PromotionId == null ? 0 : PromotionId.GetHashCode();

            return hashPromoId;
        }

        public bool Equals(Promotion other)
        {
            if (Object.ReferenceEquals(other, null)) return false;
            if (Object.ReferenceEquals(this, other)) return true;
            return PromotionId == other.PromotionId;
        }
    }
}
