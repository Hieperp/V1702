using System;

namespace TotalCore.Repositories.Commons
{
    public interface IInventoryRepository
    {
        bool CheckOverStock(DateTime? checkedDate, string warehouseIDList, string commodityIDList);
    }
}
