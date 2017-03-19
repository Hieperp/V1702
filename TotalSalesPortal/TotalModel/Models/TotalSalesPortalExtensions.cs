using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using TotalModel.Helpers;

namespace TotalModel.Models
{
    public partial class DeliveryAdvice : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<DeliveryAdviceDetail>
    {
        public int GetID() { return this.DeliveryAdviceID; }

        public virtual Employee Salesperson { get { return this.Employee; } }
        public virtual Customer Receiver { get { return this.Customer1; } }

        public ICollection<DeliveryAdviceDetail> GetDetails() { return this.DeliveryAdviceDetails; }
    }


    public partial class DeliveryAdviceDetail : IPrimitiveEntity, IHelperEntryDate, IHelperWarehouseID, IHelperCommodityID, IHelperCommodityTypeID
    {
        public int GetID() { return this.DeliveryAdviceDetailID; }
        public int GetWarehouseID() { return (int)this.WarehouseID; }
    }


    public partial class GoodsIssue : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<GoodsIssueDetail>
    {
        public int GetID() { return this.GoodsIssueID; }

        public virtual Employee Storekeeper { get { return this.Employee; } }
        public virtual Customer Receiver { get { return this.Customer1; } }

        public ICollection<GoodsIssueDetail> GetDetails() { return this.GoodsIssueDetails; }
    }


    public partial class GoodsIssueDetail : IPrimitiveEntity, IHelperEntryDate, IHelperWarehouseID, IHelperCommodityID, IHelperCommodityTypeID
    {
        public int GetID() { return this.GoodsIssueDetailID; }
        public int GetWarehouseID() { return (int)this.WarehouseID; }
    }








    public partial class PendingDeliveryAdvice
    {
        public string ReceiverDescription { get { return (this.CustomerID != this.ReceiverID ? this.ReceiverName + ", " : "") + this.ShippingAddress; } }
    }

    public partial class PendingDeliveryAdviceCustomer
    {
        public string ReceiverDescription { get { return (this.CustomerID != this.ReceiverID ? this.ReceiverName + ", " : "") + this.ShippingAddress; } }
    }
    
    public partial class HandlingUnitIndex
    {
        public string CustomerDescription { get { return this.CustomerName + (this.CustomerName != this.ReceiverName ? ", Người nhận: " + this.ReceiverName : "") + ", Giao hàng: " + this.ShippingAddress; } }
    }

    public partial class HandlingUnit : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<HandlingUnitDetail>
    {
        public int GetID() { return this.HandlingUnitID; }

        public virtual Employee PackagingStaff { get { return this.Employee; } }
        public virtual Customer Receiver { get { return this.Customer1; } }

        public ICollection<HandlingUnitDetail> GetDetails() { return this.HandlingUnitDetails; }
    }


    public partial class HandlingUnitDetail : IPrimitiveEntity, IHelperEntryDate
    {
        public int GetID() { return this.HandlingUnitDetailID; }
    }





    public partial class GoodsDelivery : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<GoodsDeliveryDetail>
    {
        public int GetID() { return this.GoodsDeliveryID; }

        public virtual Employee Driver { get { return this.Employee; } }
        public virtual Employee Collector { get { return this.Employee1; } }
        public virtual Customer Receiver { get { return this.Customer; } }

        public ICollection<GoodsDeliveryDetail> GetDetails() { return this.GoodsDeliveryDetails; }
    }


    public partial class GoodsDeliveryDetail : IPrimitiveEntity, IHelperEntryDate
    {
        public int GetID() { return this.GoodsDeliveryDetailID; }
    }



    public partial class PendingHandlingUnit
    {
        public string ReceiverDescription { get { return (this.CustomerID == this.ReceiverID ? "" : this.ReceiverName + ", ") + this.ShippingAddress; } }
    }



    public partial class AccountInvoice : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<AccountInvoiceDetail>
    {
        public int GetID() { return this.AccountInvoiceID; }

        public virtual Customer Consumer { get { return this.Customer1; } }
        public virtual Customer Receiver { get { return this.Customer2; } }

        public ICollection<AccountInvoiceDetail> GetDetails() { return this.AccountInvoiceDetails; }
    }


    public partial class AccountInvoiceDetail : IPrimitiveEntity, IHelperEntryDate
    {
        public int GetID() { return this.AccountInvoiceDetailID; }
    }


    public partial class Receipt : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<ReceiptDetail>
    {
        public int GetID() { return this.ReceiptID; }

        public virtual Employee Cashier { get { return this.Employee; } }

        public ICollection<ReceiptDetail> GetDetails() { return this.ReceiptDetails; }
    }


    public partial class ReceiptDetail : IPrimitiveEntity
    {
        public int GetID() { return this.ReceiptDetailID; }
    }




    public partial class VoidType : IPrimitiveEntity, IBaseEntity
    {
        public int GetID() { return this.VoidTypeID; }

        public int UserID { get; set; }
        public int PreparedPersonID { get; set; }
        public int OrganizationalUnitID { get; set; }
        public int LocationID { get; set; }

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }
    }

    public partial class Employee : IPrimitiveEntity, IBaseEntity
    {
        public int GetID() { return this.EmployeeID; }

        public int UserID { get; set; }
        public int PreparedPersonID { get; set; }
        public int OrganizationalUnitID { get; set; }

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }
    }

    public partial class Commodity : IPrimitiveEntity, IBaseEntity
    {
        public int GetID() { return this.CommodityID; }

        public int UserID { get; set; }
        public int PreparedPersonID { get; set; }
        public int OrganizationalUnitID { get; set; }
        public int LocationID { get; set; }

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }
    }

    public partial class Customer : IPrimitiveEntity, IBaseEntity
    {
        public int GetID() { return this.CustomerID; }

        public int UserID { get; set; }
        public int PreparedPersonID { get; set; }
        public int OrganizationalUnitID { get; set; }
        public int LocationID { get; set; }

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }
    }

    public partial class Promotion : IPrimitiveEntity, IBaseEntity
    {
        public int GetID() { return this.PromotionID; }

        public int UserID { get; set; }
        public int PreparedPersonID { get; set; }
        public int OrganizationalUnitID { get; set; }
        public int LocationID { get; set; }

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }
    }




    public partial class DeliveryAdviceIndex
    {
        public decimal GrandTotalQuantity { get { return this.TotalQuantity + this.TotalFreeQuantity; } }
        public decimal GrandTotalQuantityIssue { get { return this.TotalQuantityIssue + this.TotalFreeQuantityIssue; } }
    }

}
