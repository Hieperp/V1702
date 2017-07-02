using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalDAL.Repositories.Commons
{
    public class PromotionRepository : GenericRepository<Promotion>, IPromotionRepository
    {
        public PromotionRepository(TotalSalesPortalEntities totalSalesPortalEntities)
            : base(totalSalesPortalEntities)
        { }

        public IList<Promotion> GetPromotionByCustomers(int? customerID)
        {
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = false;
            List<Promotion> promotions = this.TotalSalesPortalEntities.GetPromotionByCustomers(customerID).ToList();
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = true;

            return promotions;
        }

        public void AddPromotionCustomers(int? promotionID, int? customerID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("PromotionID", promotionID), new ObjectParameter("CustomerID", customerID) };
            this.ExecuteFunction("AddPromotionCustomers", parameters);
        }

        public void RemovePromotionCustomers(int? promotionID, int? customerID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("PromotionID", promotionID), new ObjectParameter("CustomerID", customerID) };
            this.ExecuteFunction("RemovePromotionCustomers", parameters);
        }

    }
}
