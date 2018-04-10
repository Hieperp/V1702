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

        string Code { get; set; }
        string Name { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên khách hàng")]
        [Display(Name = "Khách hàng")]
        string CodeAndName { get; }
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
        string PaymentTermCode { get; set; }
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

        [Display(Name = "Mã khách hàng")]
        [Required(ErrorMessage = "Vui lòng nhập mã khách hàng")]
        public string Code { get; set; }

        [Display(Name = "Tên khách hàng")]
        [Required(ErrorMessage = "Vui lòng nhập tên khách hàng")]
        public string Name { get; set; }

        public string CodeAndName { get { return this.Code + (this.Code != null && this.Code != "" && this.Name != null && this.Name != "" ? "  -  " : "") + this.Name; } }

        [Display(Name = "Tên xuất hóa đơn")]
        public virtual string OfficialName { get; set; }

        [Display(Name = "Ngày sinh nhật")]
        public Nullable<System.DateTime> Birthday { get; set; }

        [Display(Name = "Mã số thuế")]
        public string VATCode { get; set; }

        [Display(Name = "Điện thoại")]
        public virtual string Telephone { get; set; }

        [Display(Name = "Địa chỉ xuất hóa đơn")]
        public virtual string BillingAddress { get; set; }
        [Display(Name = "Địa chỉ giao hàng")]
        public virtual string ShippingAddress { get; set; }


        [Display(Name = "Địa bàn")]
        [Required(ErrorMessage = "Vui lòng chọn địa bàn")]
        public int TerritoryID { get; set; }
        [Display(Name = "Địa bàn")]
        public virtual string EntireTerritoryEntireName { get; set; }

        [Required]
        public virtual int SalespersonID { get; set; }
        [Display(Name = "Nhân viên tiếp thị")]
        public virtual string SalespersonName { get; set; }
               
        [Display(Name = "Phương thức thanh toán")]
        [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán")]
        public int PaymentTermID { get; set; }
        [Display(Name = "Phương thức thanh toán")]
        public string PaymentTermCode { get; set; }


        [Display(Name = "Bảng giá")]
        [Required(ErrorMessage = "Vui lòng chọn bảng giá")]
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

        [Required(ErrorMessage = "Vui lòng nhập tên xuất hóa đơn")]
        public override string OfficialName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ xuất hóa đơn")]
        public override string BillingAddress { get; set; }

        [Display(Name = "Mã nhà cung cấp [siêu thị]")]
        public string VendorCode { get; set; }
        [Display(Name = "Mã ngành NCC [siêu thị]")]
        public string VendorCategory { get; set; }

        [Display(Name = "Tài khoản thanh toán")]
        public Nullable<int> MonetaryAccountID { get; set; }

        [Display(Name = "Kênh khách hàng")]
        [Required(ErrorMessage = "Vui lòng nhập kênh khách hàng")]
        public Nullable<int> CustomerCategoryID { get; set; }

        [Display(Name = "Phân loại khách hàng")]
        [Required(ErrorMessage = "Vui lòng nhập loại khách hàng")]
        public Nullable<int> CustomerTypeID { get; set; }

        [Display(Name = "Số fax")]
        public string Facsimile { get; set; }
        [Display(Name = "Người liên hệ")]
        public string AttentionName { get; set; }
        [Display(Name = "Chức danh")]
        public string AttentionTitle { get; set; }

        [Display(Name = "Hạn mức tín dụng")]
        public Nullable<double> LimitAmount { get; set; }

        [Display(Name = "Khách hàng")]
        public bool IsCustomer { get { return true; } }
        [Display(Name = "Nhà cung cấp")]
        public bool IsSupplier { get { return false; } }
        [Display(Name = "Giới tính nữ")]
        public bool IsFemale { get; set; }

        public override int PreparedPersonID { get { return 1; } }
    }

    public class CustomerDTO : CustomerPrimitiveDTO
    {
        public CustomerDTO() { this.Salesperson = new EmployeeBaseDTO(); }

        public override int SalespersonID { get { return (this.Salesperson != null ? this.Salesperson.EmployeeID : 0); } }
        [Display(Name = "Nhân viên tiếp thị")]
        [UIHint("AutoCompletes/EmployeeBase")]
        public EmployeeBaseDTO Salesperson { get; set; }
    }
}
