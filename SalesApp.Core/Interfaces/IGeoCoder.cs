using SalesApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApp.Core.Interfaces
{
    public interface IGeoCoder
    {
        Task<string> GetCityFromLocation(GeoLocation location);
    }
}
