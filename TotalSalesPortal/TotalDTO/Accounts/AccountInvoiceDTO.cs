﻿using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalBase.Enums;
using TotalDTO.Helpers;
using TotalDTO.Commons;
using TotalDTO.Inventories;

namespace TotalDTO.Accounts
{
    public class AccountInvoicePrimitiveDTO : FreeQuantityDiscountVATAmountDTO<AccountInvoiceDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.AccountInvoice; } }

        public int GetID() { return this.AccountInvoiceID; }
        public void SetID(int id) { this.AccountInvoiceID = id; }

        public int AccountInvoiceID { get; set; }

        public virtual int CustomerID { get; set; }
        public virtual Nullable<int> ConsumerID { get; set; }
        public virtual Nullable<int> ReceiverID { get; set; }
        public virtual Nullable<int> GoodsIssueID { get; set; }

        public int GoodsIssueFirstID { get; set; }
        public string GoodsIssueReferences { get; set; }

        [Display(Name = "Phương thức TT")]
        public int PaymentTermID { get; set; }

        [Display(Name = "Số hóa đơn")]
        [Required(ErrorMessage = "Vui lòng nhập số hóa đơn")]
        public string VATInvoiceNo { get; set; }
        [Display(Name = "Số seri")]
        [Required(ErrorMessage = "Vui lòng nhập số seri")]
        public string VATInvoiceSeries { get; set; }
        [UIHint("Date")]
        [Display(Name = "Ngày hóa đơn")]
        [Required(ErrorMessage = "Vui lòng nhập ngày hóa đơn")]
        public Nullable<System.DateTime> VATInvoiceDate { get; set; }

        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();

            int goodsIssueFirstID = 0; string goodsIssueReferences = ""; int i = 0;
            this.DtoDetails().ToList().ForEach(e => { e.CustomerID = this.CustomerID; if ((e.Quantity != 0 || e.FreeQuantity != 0) && (goodsIssueFirstID == 0 || goodsIssueFirstID > e.GoodsIssueID)) goodsIssueFirstID = e.GoodsIssueID; if ((e.Quantity != 0 || e.FreeQuantity != 0) && i <= 6 && goodsIssueReferences.IndexOf(e.GoodsIssueReference) < 0) goodsIssueReferences = goodsIssueReferences + (goodsIssueReferences != "" ? ", " : "") + (i++ < 6 ? e.GoodsIssueReference : "..."); });
            this.GoodsIssueFirstID = goodsIssueFirstID; this.GoodsIssueReferences = goodsIssueReferences;
        }
    }

    public class AccountInvoiceDTO : AccountInvoicePrimitiveDTO, IBaseDetailEntity<AccountInvoiceDetailDTO>
    {
        public AccountInvoiceDTO()
        {
            this.AccountInvoiceViewDetails = new List<AccountInvoiceDetailDTO>();
        }

        public override int CustomerID { get { return (this.Customer != null ? this.Customer.CustomerID : 0); } }
        [UIHint("AutoCompletes/CustomerBase")]
        public CustomerBaseDTO Customer { get; set; }

        public override Nullable<int> ConsumerID { get { return (this.Consumer != null ? (this.Consumer.CustomerID > 0 ? (Nullable<int>)this.Consumer.CustomerID : null) : null); } }
        [UIHint("Commons/CustomerBase")]
        public CustomerBaseDTO Consumer { get; set; }

        public override Nullable<int> ReceiverID { get { return (this.Receiver != null ? (this.Receiver.CustomerID > 0 ? (Nullable<int>)this.Receiver.CustomerID : null) : null); } }
        [UIHint("Commons/CustomerBase")]
        public CustomerBaseDTO Receiver { get; set; }

        public override Nullable<int> GoodsIssueID { get { return (this.GoodsIssue != null ? this.GoodsIssue.GoodsIssueID : null); } }
        [UIHint("Commons/GoodsIssueBox")]
        public GoodsIssueBoxDTO GoodsIssue { get; set; }

        public List<AccountInvoiceDetailDTO> AccountInvoiceViewDetails { get; set; }
        public List<AccountInvoiceDetailDTO> ViewDetails { get { return this.AccountInvoiceViewDetails; } set { this.AccountInvoiceViewDetails = value; } }

        public ICollection<AccountInvoiceDetailDTO> GetDetails() { return this.AccountInvoiceViewDetails; }

        protected override IEnumerable<AccountInvoiceDetailDTO> DtoDetails() { return this.AccountInvoiceViewDetails; }
    }

}
