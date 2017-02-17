using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalPortal.Areas.Commons.Builders
{
    public interface IVehicleSelectListBuilder
    {
        IEnumerable<SelectListItem> BuildSelectListItemsForVehicles(IEnumerable<Vehicle> vehicles);
    }

    public class VehicleSelectListBuilder : IVehicleSelectListBuilder
    {
        public IEnumerable<SelectListItem> BuildSelectListItemsForVehicles(IEnumerable<Vehicle> vehicles)
        {
            return vehicles.Select(pt => new SelectListItem { Text = pt.Name, Value = pt.VehicleID.ToString() }).ToList();
        }
    }
}
