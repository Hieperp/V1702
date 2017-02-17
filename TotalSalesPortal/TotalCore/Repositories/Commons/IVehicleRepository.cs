using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Commons
{
    public interface IVehicleRepository
    {
        IList<Vehicle> GetAllVehicles();
    }
}
