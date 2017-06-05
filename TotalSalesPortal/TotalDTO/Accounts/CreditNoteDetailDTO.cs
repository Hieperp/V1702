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

        [UIHint("AutoCompletes/CommodityAvailable")]
        public override string CommodityCode { get; set; }

        [UIHint("Decimal")]
        public override decimal Quantity { get; set; }

        public Nullable<int> PromotionID { get; set; }
    }
}
