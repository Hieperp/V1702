using System;

namespace TotalModel.Helpers
{
    public interface IHelperEntryDate
    {
        DateTime EntryDate { get; }
    }

    public interface IHelperWarehouseID
    {
        int GetWarehouseID();
    }
    public interface IHelperCommodityID
    {
        int CommodityID { get; }
    }

    public interface IHelperCommodityTypeID
    {
        int CommodityTypeID { get; }
    }
}
