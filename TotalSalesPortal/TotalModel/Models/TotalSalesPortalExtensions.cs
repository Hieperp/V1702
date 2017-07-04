using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TotalModel.Helpers;

namespace TotalModel.Models
{

    public partial class SalesOrder : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<SalesOrderDetail>
    {
        public int GetID() { return this.SalesOrderID; }

        public virtual Employee Salesperson { get { return this.Employee; } }
        public virtual Customer Receiver { get { return this.Customer1; } }

        public ICollection<SalesOrderDetail> GetDetails() { return this.SalesOrderDetails; }
    }


    public partial class SalesOrderDetail : IPrimitiveEntity, IHelperEntryDate, IHelperWarehouseID, IHelperCommodityID, IHelperCommodityTypeID
    {
        public int GetID() { return this.SalesOrderDetailID; }
        public int GetWarehouseID() { return (int)this.WarehouseID; }
    }


    public partial class SalesOrderIndex
    {
        public decimal GrandTotalQuantity { get { return this.TotalQuantity + this.TotalFreeQuantity; } }
        public decimal GrandTotalQuantityAdvice { get { return this.TotalQuantityAdvice + this.TotalFreeQuantityAdvice; } }
    }



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


    public partial class DeliveryAdviceIndex
    {
        public decimal GrandTotalQuantity { get { return this.TotalQuantity + this.TotalFreeQuantity; } }
        public decimal GrandTotalQuantityIssue { get { return this.TotalQuantityIssue + this.TotalFreeQuantityIssue; } }
    }






    public partial class SalesReturn : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<SalesReturnDetail>
    {
        public int GetID() { return this.SalesReturnID; }

        public virtual Employee Salesperson { get { return this.Employee; } }
        public virtual Customer Receiver { get { return this.Customer1; } }

        public ICollection<SalesReturnDetail> GetDetails() { return this.SalesReturnDetails; }
    }


    public partial class SalesReturnDetail : IPrimitiveEntity, IHelperEntryDate, IHelperWarehouseID, IHelperCommodityID, IHelperCommodityTypeID
    {
        public int GetID() { return this.SalesReturnDetailID; }
        public int GetWarehouseID() { return (int)this.WarehouseID; }
    }


    public partial class SalesReturnIndex
    {
        public decimal GrandTotalQuantity { get { return this.TotalQuantity + this.TotalFreeQuantity; } }
        public decimal GrandTotalQuantityReceived { get { return this.TotalQuantityReceived + this.TotalFreeQuantityReceived; } }
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

    public partial class DeliveryAdvicePendingCustomer
    {
        public string ReceiverDescription { get { return (this.CustomerID != this.ReceiverID ? this.ReceiverName + ", " : "") + this.ShippingAddress; } }
    }

    public partial class DeliveryAdvicePendingSalesOrder
    {
        public string ReceiverDescription { get { return (this.CustomerID != this.ReceiverID ? this.ReceiverName + ", " : "") + this.ShippingAddress; } }
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

    public partial class ReceiptIndex
    {
        public string DebitAccountType { get { return (this.MonetaryAccountCode != null ? this.MonetaryAccountCode : (this.AdvanceReceiptReference != null ? "CT TT" : (this.SalesReturnReference != null ? "CT TH" : "CT CK"))); } }
        public string DebitAccountCode { get { return (this.MonetaryAccountCode != null ? null : (this.AdvanceReceiptReference != null ? this.AdvanceReceiptReference : (this.SalesReturnReference != null ? this.SalesReturnReference : this.CreditNoteReference))); } }
        public Nullable<System.DateTime> DebitAccountDate { get { return (this.MonetaryAccountCode != null ? null : (this.AdvanceReceiptDate != null ? this.AdvanceReceiptDate : (this.SalesReturnDate != null ? this.SalesReturnDate : this.CreditNoteDate))); } }
    }


    public partial class ReceiptViewDetail
    {
        public string ReceiverDescription { get { return (this.CustomerID != this.ReceiverID ? this.ReceiverName + ", " : "") + this.Description; } }
    }


    public partial class Receipt : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<ReceiptDetail>
    {
        public int GetID() { return this.ReceiptID; }

        public virtual Receipt AdvanceReceipt { get { return this.Receipt1; } }
        public virtual Employee Cashier { get { return this.Employee; } }

        public decimal TotalReceiptAmountSaved { get { return this.TotalReceiptAmount; } }
        public decimal TotalFluctuationAmountSaved { get { return this.TotalFluctuationAmount; } }

        public ICollection<ReceiptDetail> GetDetails() { return this.ReceiptDetails; }
    }


    public partial class ReceiptDetail : IPrimitiveEntity
    {
        public int GetID() { return this.ReceiptDetailID; }
    }





    public partial class CreditNote : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<CreditNoteDetail>
    {
        public int GetID() { return this.CreditNoteID; }

        public virtual Employee Salesperson { get { return this.Employee; } }

        public ICollection<CreditNoteDetail> GetDetails() { return this.CreditNoteDetails; }
    }


    public partial class CreditNoteDetail : IPrimitiveEntity, IHelperEntryDate, IHelperCommodityID, IHelperCommodityTypeID
    {
        public int GetID() { return this.CreditNoteDetailID; }
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

    public partial class Warehouse : IPrimitiveEntity, IBaseEntity
    {
        public int GetID() { return this.WarehouseID; }

        public int UserID { get; set; }
        public int PreparedPersonID { get; set; }
        public int OrganizationalUnitID { get; set; }

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

    public partial class CommodityPrice : IPrimitiveEntity, IBaseEntity
    {
        public int GetID() { return this.CommodityPriceID; }

        public int UserID { get; set; }
        public int PreparedPersonID { get; set; }
        public int OrganizationalUnitID { get; set; }
        public int LocationID { get; set; }

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }


        public virtual CodePartDTO CodePartDTOA { get { return new CodePartDTO() { CodePart = this.CodePartA }; } }
        public virtual CodePartDTO CodePartDTOB { get { return new CodePartDTO() { CodePart = this.CodePartB }; } }
        public virtual CodePartDTO CodePartDTOC { get { return new CodePartDTO() { CodePart = this.CodePartC }; } }
    }

    public partial class Customer : IPrimitiveEntity, IBaseEntity
    {
        public int GetID() { return this.CustomerID; }

        public virtual Employee Salesperson { get { return this.Employee; } }

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

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }
    }





}
