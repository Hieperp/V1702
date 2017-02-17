using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using TotalModel.Models;

namespace TotalPortal.Areas.Commons.Builders
{
    public interface IPackingMaterialSelectListBuilder
    {
        IEnumerable<SelectListItem> BuildSelectListItemsForPackingMaterials(IEnumerable<PackingMaterial> packingMaterials);
    }

    public class PackingMaterialSelectListBuilder : IPackingMaterialSelectListBuilder
    {
        public IEnumerable<SelectListItem> BuildSelectListItemsForPackingMaterials(IEnumerable<PackingMaterial> packingMaterials)
        {
            return packingMaterials.Select(pt => new SelectListItem { Text = pt.Name, Value = pt.PackingMaterialID.ToString() }).ToList();
        }
    }
}
