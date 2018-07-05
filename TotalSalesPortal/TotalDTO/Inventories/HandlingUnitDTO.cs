using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalBase.Enums;
using TotalDTO.Helpers;
using TotalDTO.Commons;

namespace TotalDTO.Inventories
{
    public class HandlingUnitPrimitiveDTO : QuantityDTO<HandlingUnitDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.HandlingUnit; } }

        public int GetID() { return this.HandlingUnitID; }
        public void SetID(int id) { this.HandlingUnitID = id; }

        public int HandlingUnitID { get; set; }

        public virtual int CustomerID { get; set; }
        public virtual int ReceiverID { get; set; }

        [Display(Name = "Địa chỉ giao hàng")]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng")]
        [UIHint("AutoCompletes/ShippingAddress")]
        public string ShippingAddress { get; set; }

        [Display(Name = "Tên người nhận hàng")]
        [UIHint("AutoCompletes/Addressee")]
        public string Addressee { get; set; }

        public virtual Nullable<int> GoodsIssueID { get; set; }
        public string GoodsIssueReferences { get; set; }
        [Display(Name = "Số đơn hàng")]
        [UIHint("Commons/SOCode")]
        public string Code { get; set; }

        public virtual int PackagingStaffID { get; set; }

        [Display(Name = "Số thứ tự lô hàng")]
        public int LotNo { get; set; }
        [Display(Name = "Số thứ tự thùng, bao")]
        public int Identification { get; set; }
        [Display(Name = "Tổng số thùng, bao trong lô")]
        public int CountIdentification { get; set; }

        [Display(Name = "Số thứ tự thùng, bao")]
        public string IdentificationPerLotNo { get { return this.Identification.ToString("0") + "/" + this.CountIdentification.ToString("0"); } }

        [Display(Name = "Loại thùng, bao")]
        public int PackingMaterialID { get; set; }
        [Display(Name = "Quy cách, kích thước")]
        public string Dimension { get; set; }

        [Display(Name = "Trọng lượng thực tế")]
        public decimal RealWeight { get; set; }
        [Display(Name = "Chênh lệch trọng lượng")]
        public decimal WeightDifference { get { return this.RealWeight - this.TotalWeight; } }

        [Display(Name = "Tổng trọng lượng")]
        public decimal TotalWeight { get; set; }
        protected virtual decimal GetTotalWeight() { return this.DtoDetails().Select(o => o.Weight).Sum(); }


        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if (this.TotalWeight != this.GetTotalWeight()) yield return new ValidationResult("Lỗi tổng trọng lượng", new[] { "TotalWeight" });
            if (Math.Abs(this.RealWeight - this.TotalWeight) > (this.TotalWeight * (decimal)0.1 + (decimal)2.5)) yield return new ValidationResult("Chênh lệch không vượt quá: +/-" + (this.TotalWeight * (decimal)0.1 + (decimal)2.5).ToString("0.00"), new[] { "WeightDifference" });
        }


        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();

            if (this.Addressee == null) { this.Addressee = ""; } this.Addressee = this.Addressee.Trim();

            string goodsIssueReferences = ""; string goodsIssueCodes = "";
            this.DtoDetails().ToList().ForEach(e => { e.CustomerID = this.CustomerID; e.ReceiverID = this.ReceiverID; if (goodsIssueReferences.IndexOf(e.GoodsIssueReference) < 0) goodsIssueReferences = goodsIssueReferences + (goodsIssueReferences != "" ? ", " : "") + e.GoodsIssueReference; if (e.Quantity > 0 && e.GoodsIssueCode != null && goodsIssueCodes.IndexOf(e.GoodsIssueCode) < 0) goodsIssueCodes = goodsIssueCodes + (goodsIssueCodes != "" ? ", " : "") + e.GoodsIssueCode; });
            this.GoodsIssueReferences = goodsIssueReferences; this.Code = goodsIssueCodes != "" ? goodsIssueCodes : null;
        }
    }

    public class HandlingUnitDTO : HandlingUnitPrimitiveDTO, IBaseDetailEntity<HandlingUnitDetailDTO>
    {
        public HandlingUnitDTO()
        {
            this.HandlingUnitViewDetails = new List<HandlingUnitDetailDTO>();
        }

        public override int CustomerID { get { return (this.Customer != null ? this.Customer.CustomerID : 0); } }
        [UIHint("Commons/CustomerBase")]
        public CustomerBaseDTO Customer { get; set; }

        public override int ReceiverID { get { return (this.Receiver != null ? this.Receiver.CustomerID : 0); } }
        [UIHint("Commons/CustomerBase")]
        public CustomerBaseDTO Receiver { get; set; }

        public override Nullable<int> GoodsIssueID { get { return (this.GoodsIssue != null ? (Nullable<int>)this.GoodsIssue.GoodsIssueID : null); } }
        [UIHint("Commons/GoodsIssueBox")]
        public GoodsIssueBoxDTO GoodsIssue { get; set; }

        public override int PackagingStaffID { get { return (this.PackagingStaff != null ? this.PackagingStaff.EmployeeID : 0); } }
        [Display(Name = "Nhân viên đóng hàng")]
        [UIHint("AutoCompletes/EmployeeBase")]
        public EmployeeBaseDTO PackagingStaff { get; set; }

        public List<HandlingUnitDetailDTO> HandlingUnitViewDetails { get; set; }
        public List<HandlingUnitDetailDTO> ViewDetails { get { return this.HandlingUnitViewDetails; } set { this.HandlingUnitViewDetails = value; } }

        public ICollection<HandlingUnitDetailDTO> GetDetails() { return this.HandlingUnitViewDetails; }

        protected override IEnumerable<HandlingUnitDetailDTO> DtoDetails() { return this.HandlingUnitViewDetails; }
    }

}
