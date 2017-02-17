namespace TotalPortal.Areas.Commons.ViewModels.Helpers
{
    public interface IWarehouseAutoCompleteViewModel
    {
        int WarehouseID { get; set; }
        string WarehouseName { get; set; }
        string WarehouseLocationTelephone { get; set; }
        string WarehouseLocationFacsimile { get; set; }
        string WarehouseLocationName { get; set; }
        string WarehouseLocationAddress { get; set; }         
    }
}
