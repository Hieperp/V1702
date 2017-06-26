using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Commons
{
    public interface ICustomerCategoryRepository
    {
        IList<CustomerCategory> GetAllCustomerCategories();
    }
}