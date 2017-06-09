using System;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalModel.Helpers;
using TotalDTO.Helpers;
using System.Collections.Generic;

namespace TotalDTO.Accounts
{
    public class CreditNoteDetailDTO : VATAmountDetailDTO, IPrimitiveEntity, IHelperCommodityID, IHelperCommodityTypeID
    {
        public int GetID() { return this.CreditNoteDetailID; }

        public int CreditNoteDetailID { get; set; }
        public int CreditNoteID { get; set; }

        public int CustomerID { get; set; }
        public int SalespersonID { get; set; }

        public Nullable<int> PromotionID { get; set; }

      
        [Display(Name = "Mã CK")]
        [UIHint("AutoCompletes/CommodityBase")]
        public override string CommodityCode { get; set; }

        [Display(Name = "Tên chiết khấu")]
        public override string CommodityName { get; set; }

        [UIHint("Decimal")]
        public override decimal Quantity { get; set; }
        [UIHint("Decimal")]
        [Display(Name = "Đơn giá ck")]
        public override decimal UnitPrice { get; set; }

        [Display(Name = "Diễn giải")]
        public override string Remarks { get; set; }
    }
}
