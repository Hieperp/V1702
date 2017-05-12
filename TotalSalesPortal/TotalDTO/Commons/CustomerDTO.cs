using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalBase.Enums;

namespace TotalDTO.Commons
{
    public interface ICustomerBaseDTO
    {
        int CustomerID { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên khách hàng")]
        [Display(Name = "Khách hàng")]
        string CodeAndName { get; }
        string Code { get; set; }
        string Name { get; set; }
        string OfficialName { get; set; }
        Nullable<System.DateTime> Birthday { get; set; }
        string VATCode { get; set; }
        string Telephone { get; set; }
        string BillingAddress { get; set; }
        string ShippingAddress { get; set; }
        int TerritoryID { get; set; }
        string EntireTerritoryEntireName { get; set; }
        int SalespersonID { get; set; }
        string SalespersonName { get; set; }
        int PaymentTermID { get; set; }
        int PriceCategoryID { get; set; }
        string PriceCategoryCode { get; set; }
        bool ShowDiscount { get; set; }

        int WarehouseID { get; set; }
        string WarehouseCode { get; set; }
        string WarehouseName { get; set; }
    }

    public class CustomerBaseDTO : BaseDTO, ICustomerBaseDTO
    {
        public int CustomerID { get; set; }

        public string CodeAndName { get { return this.Code + (this.Code != null && this.Code != "" && this.Name != null && this.Name != "" ? "  -  " : "") + this.Name; } }

        [Display(Name = "Mã khách hàng")]
        public string Code { get; set; }

        [Display(Name = "Khách hàng")]
        public string Name { get; set; }

        [Display(Name = "Tên đầy đủ")]
        public string OfficialName { get; set; }

        [Display(Name = "Ngày sinh")]
        public Nullable<System.DateTime> Birthday { get; set; }

        [Display(Name = "Mã số thuế")]
        public string VATCode { get; set; }

        [Display(Name = "Điện thoại")]
        public string Telephone { get; set; }

        [Display(Name = "Địa chỉ xuất hóa đơn")]
        public virtual string BillingAddress { get; set; }
        [Display(Name = "Địa chỉ giao hàng")]
        public virtual string ShippingAddress { get; set; }


        [Required]
        [Display(Name = "Khu vực")]
        public int TerritoryID { get; set; }
        [Display(Name = "Khu vực")]
        public virtual string EntireTerritoryEntireName { get; set; }

        [Required]
        public int SalespersonID { get; set; }
        [Display(Name = "Tên nhân viên")]
        public virtual string SalespersonName { get; set; }

        [Display(Name = "Phương thức thanh toán")]
        public int PaymentTermID { get; set; }

        [Required]
        [Display(Name = "Bảng giá")]
        public int PriceCategoryID { get; set; }
        [Display(Name = "Bảng giá")]
        public string PriceCategoryCode { get; set; }

        [Display(Name = "Mã kho")]
        public int WarehouseID { get; set; }
        [Display(Name = "Mã kho")]
        public string WarehouseCode { get; set; }
        [Display(Name = "Kho hàng")]
        public string WarehouseName { get; set; }
    }


    public class CustomerPrimitiveDTO : CustomerBaseDTO, IPrimitiveEntity, IPrimitiveDTO
    {
        public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.Customer; } }

        public int GetID() { return this.CustomerID; }
        public void SetID(int id) { this.CustomerID = id; }

        //public int CustomerID { get; set; }
        //[Required]
        //[Display(Name = "Tên khách")]
        //public string Name { get; set; }
        //[Display(Name = "Tên đầy đủ")]
        //public string OfficialName { get; set; }
        [Display(Name = "Phân khúc khách hàng")]
        [DefaultValue(1)]
        public int CustomerCategoryID { get; set; }
        [Display(Name = "Phân loại khách hàng")]
        [DefaultValue(1)]
        public int CustomerTypeID { get; set; }
        //[Display(Name = "Khu vực")]
        //public int TerritoryID { get; set; }
        //[Display(Name = "Khu vực")]
        //[Required]
        //public string EntireTerritoryEntireName { get; set; }

        //[Display(Name = "Mã số thuế")]
        //public string VATCode { get; set; }
        //[Display(Name = "Điện thoại")]
        //[Required]
        //public string Telephone { get; set; }
        public string Facsimile { get; set; }
        [Display(Name = "Người liên hệ")]
        public string AttentionName { get; set; }
        [Display(Name = "Chức danh")]
        public string AttentionTitle { get; set; }
        //[Required]
        //[Display(Name = "Ngày sinh")]
        //public Nullable<System.DateTime> Birthday { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ xuất hóa đơn")]
        public override string BillingAddress { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập khu vực")]
        public override string EntireTerritoryEntireName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên nhân viên")]
        public override string SalespersonName { get; set; }

        [Display(Name = "Hạn mức tín dụng")]
        public Nullable<double> LimitAmount { get; set; }

        [Display(Name = "Khách hàng")]
        public bool IsCustomer { get; set; }
        [Display(Name = "Nhà cung cấp")]
        public bool IsSupplier { get; set; }
        [Display(Name = "Giới tính nữ")]
        public bool IsFemale { get; set; }

        public override int PreparedPersonID { get { return 1; } }
    }

    public class CustomerDTO : CustomerPrimitiveDTO
    {
    }
}
