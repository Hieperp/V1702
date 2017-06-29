using System;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Commons
{
    public interface IPromotionRepository : IGenericRepository<Promotion>
    {
        IList<Promotion> GetPromotionByCustomers(int? customerID);
    }
}
