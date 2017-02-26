using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApp.Core.Services
{
    public class DiscTypes
    {
        public string DiscountType { get; set; }
        public DiscTypes() { }
        public DiscTypes(string disctype)
        {
            DiscountType = disctype;
        }
    }
}
