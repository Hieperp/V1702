using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalPortal.Areas.Commons.Builders
{
    public interface ICustomerCategorySelectListBuilder
    {
        IEnumerable<SelectListItem> BuildSelectListItemsForCustomerCategorys(IEnumerable<CustomerCategory> CustomerCategorys);
    }

    public class CustomerCategorySelectListBuilder : ICustomerCategorySelectListBuilder
    {
        public IEnumerable<SelectListItem> BuildSelectListItemsForCustomerCategorys(IEnumerable<CustomerCategory> CustomerCategorys)
        {
            return CustomerCategorys.Select(pt => new SelectListItem { Text = pt.Name, Value = pt.CustomerCategoryID.ToString() }).ToList();
        }
    }
}
