using System;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Commons
{
    public interface ICommodityBrandRepository
    {
        IList<CommodityBrand> GetAllCommodityBrands();
    }

}
