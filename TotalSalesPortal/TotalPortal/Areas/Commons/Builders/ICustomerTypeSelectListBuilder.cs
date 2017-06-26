using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalPortal.Areas.Commons.Builders
{
    public interface ICustomerTypeSelectListBuilder
    {
        IEnumerable<SelectListItem> BuildSelectListItemsForCustomerTypes(IEnumerable<CustomerType> CustomerTypes);
    }

    public class CustomerTypeSelectListBuilder : ICustomerTypeSelectListBuilder
    {
        public IEnumerable<SelectListItem> BuildSelectListItemsForCustomerTypes(IEnumerable<CustomerType> CustomerTypes)
        {
            return CustomerTypes.Select(pt => new SelectListItem { Text = pt.Name, Value = pt.CustomerTypeID.ToString() }).ToList();
        }
    }
}
