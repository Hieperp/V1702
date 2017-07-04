using System;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Commons
{
    public interface IPromotionRepository : IGenericWithDetailRepository<Promotion, PromotionCommodityCodePart> 
    {
    }

    public interface IPromotionAPIRepository : IGenericAPIRepository
    {
        IList<Promotion> GetPromotionByCustomers(int? customerID);
        void AddPromotionCustomers(int? promotionID, int? customerID);
        void RemovePromotionCustomers(int? promotionID, int? customerID);
    }

}
