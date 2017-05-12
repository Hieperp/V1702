using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Commons
{
    public interface IWarehouseRepository
    {
        IList<Warehouse> GetWarehouses(int customerID, string searchText, string warehouseTaskIDList);
    }
}
