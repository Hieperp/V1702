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
        IList<Promotion> GetPromotionByCustomers(int? customerID, int? applyToSalesVersusReturns, int? filterApplyToTradeDiscount);

        IList<CustomerCategory> GetPromotionCustomerCategories(int? promotionID);

        void AddPromotionCustomerCategories(int? promotionID, int? customerCategoryID);
        void RemovePromotionCustomerCategories(int? promotionID, int? customerCategoryID);

        void AddPromotionCustomers(int? promotionID, int? customerID);
        void RemovePromotionCustomers(int? promotionID, int? customerID);
    }

}
