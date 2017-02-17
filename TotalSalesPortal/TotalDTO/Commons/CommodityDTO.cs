using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalBase.Enums;

namespace TotalDTO.Commons
{
    public class CommodityPrimitiveDTO : BaseDTO, IPrimitiveEntity, IPrimitiveDTO
    {
        public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.Commodity; } }

        public int GetID() { return this.CommodityID; }
        public void SetID(int id) { this.CommodityID = id; }

        public int CommodityID { get; set; }
        public string Code { get; set; }
        public string OfficialCode { get { return TotalBase.CommonExpressions.AlphaNumericString(this.Code); } }
        public string Name { get; set; }
        public string OfficialName { get; set; }
        public string OriginalName { get; set; }

        public Nullable<int> PreviousCommodityID { get; set; }

        public int CommodityCategoryID { get; set; }
        public string CommodityCategoryName { get; set; }

        public int CommodityTypeID { get; set; }
        public string CommodityTypeName { get; set; }

        [Display(Name = "Nhà cung cấp")]
        [Required(ErrorMessage = "Vui lòng nhập nhà cung cấp")]
        public Nullable<int> SupplierID { get; set; }

        public Nullable<int> PiecePerPack { get; set; }
        public Nullable<int> QuantityAlert { get; set; }
        public decimal ListedPrice { get; set; }
        public decimal GrossPrice { get; set; }
        public string PurchaseUnit { get; set; }
        public string SalesUnit { get; set; }
        public string Packing { get; set; }
        public string Origin { get; set; }

        public Nullable<double> Weight { get; set; }
        public Nullable<double> LeadTime { get; set; }

        public bool IsRegularCheckUps { get; set; }
        public Nullable<bool> Discontinue { get; set; }

        public string HSCode { get; set; }
        public string Specifycation { get; set; }

        public override int PreparedPersonID { get { return 1; } }
    }

    public class CommodityDTO : CommodityPrimitiveDTO
    {
    }
}
