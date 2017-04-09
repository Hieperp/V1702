using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalPortal.Areas.Commons.Builders
{
    public interface IMonetaryAccountSelectListBuilder
    {
        IEnumerable<SelectListItem> BuildSelectListItemsForMonetaryAccounts(IEnumerable<MonetaryAccount> MonetaryAccounts);
    }

    public class MonetaryAccountSelectListBuilder : IMonetaryAccountSelectListBuilder
    {
        public IEnumerable<SelectListItem> BuildSelectListItemsForMonetaryAccounts(IEnumerable<MonetaryAccount> MonetaryAccounts)
        {
            return MonetaryAccounts.Select(pt => new SelectListItem { Text = pt.Name, Value = pt.MonetaryAccountID.ToString() }).ToList();
        }
    }
}
