using System;
using System.ComponentModel.DataAnnotations;

namespace TotalDTO.Commons
{
    /// <summary>
    /// NOTES: THIS IWarehouseBaseDTO AND ITS CLASS WarehouseBaseDTO IS USED TO REPRESENT OF FOREIGN RELATIONSHIP OF SalesOrders/ DeliveryAdvices/ GoodsIssues/ SalesReturn TO Wareshouses
    /// AT CURRENT: IN THE DATABASE DESIGN: THE RELATIONSHIP IS MANDATORY (MEANS: THE FOREIGN KEY WarehouseID IN THESES TABLE SalesOrders/ DeliveryAdvices/ GoodsIssues/ SalesReturn ARE REQUIRED - NOT ALLOW NULL)
    /// BUT AT DTO LAYER: WE STILL KEEP:     Nullable<int> WarehouseID
    /// THE TRUST IS THAT: THE WarehouseID IN THESE TABLES SalesOrders/ DeliveryAdvices/ GoodsIssues/ SalesReturn IS NOT VERY IMPORTANT. WE CAN ALLOW WarehouseID NULLABLE
    /// THIS WarehouseID DOES NOT AFFECT THE WAREHOUSE OUTPUT. IT JUST CONTROL ALL WarehouseID IN THE DETAIL TABLES AS THE SAME AS THE MASTER TABLES
    /// ------->IN BRIEF: THE WAREHOUSE OUTPUT IS BASED ON THE DETAIL TABLES. THIS ALLOWS MLUTIPLE WarehouseID ON THE DETAIL TABLES. THE WarehouseID IN THE MASTER TABLES JUST CONTROL: ONLY 1 WarehouseID FOR EVERY DETAIL(S) RECORDS PER EACH MASTER RECORD
    /// LASTER: IF WE ALLOW MULTIPLE WarehouseID IN DETAIL TABLES: JUST MODIFY THE DATABASE, THEN MODIFY APPRORIATE CODING!!! GENERALLY, THIS IS ACCEPTABLE!!! BUT NEED TO MODIFY :) !!!
    /// </summary>
    public interface IWarehouseBaseDTO
    {
        Nullable<int> WarehouseID { get; set; }
        string Code { get; set; }
        [Display(Name = "Kho hàng")]
        [UIHint("AutoCompletes/WarehouseBase")]
        [Required(ErrorMessage = "Vui lòng nhập kho hàng")]
        string Name { get; set; }
    }

    public class WarehouseBaseDTO : BaseDTO, IWarehouseBaseDTO
    {
        public Nullable<int> WarehouseID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class WarehouseDTO
    {
    }
}
