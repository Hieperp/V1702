using System;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Commons
{
    public interface ICommodityBrandAPIRepository : IGenericAPIRepository
    {
        IList<CommodityBrand> GetAllCommodityBrands();
    }

}
