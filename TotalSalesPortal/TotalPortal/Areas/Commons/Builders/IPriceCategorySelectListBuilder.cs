using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalPortal.Areas.Commons.Builders
{
    public interface IPriceCategorySelectListBuilder
    {
        IEnumerable<SelectListItem> BuildSelectListItemsForPriceCategorys(IEnumerable<PriceCategory> PriceCategorys);
    }

    public class PriceCategorySelectListBuilder : IPriceCategorySelectListBuilder
    {
        public IEnumerable<SelectListItem> BuildSelectListItemsForPriceCategorys(IEnumerable<PriceCategory> PriceCategorys)
        {
            return PriceCategorys.Select(pt => new SelectListItem { Text = pt.Name, Value = pt.PriceCategoryID.ToString() }).ToList();
        }
    }
}
