using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalPortal.Areas.Commons.Builders
{
    public interface ITerritorySelectListBuilder
    {
        IEnumerable<SelectListItem> BuildSelectListItemsForTerritorys(IEnumerable<Territory> Territorys);
    }

    public class TerritorySelectListBuilder : ITerritorySelectListBuilder
    {
        public IEnumerable<SelectListItem> BuildSelectListItemsForTerritorys(IEnumerable<Territory> Territorys)
        {
            return Territorys.Select(pt => new SelectListItem { Text = pt.Name, Value = pt.TerritoryID.ToString() }).ToList();
        }
    }
}
