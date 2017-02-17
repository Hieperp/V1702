using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TotalPortal.Areas.Commons.ViewModels.Helpers
{
    public interface IContractibleInvoiceViewModel : ICustomerAutoCompleteViewModel
    {
        Nullable<int> ServiceContractID { get; set; }
        string ServiceContractReference { get; set; }

        string ServiceContractCommodityID { get; set; }
        string ServiceContractCommodityCode { get; set; }
        string ServiceContractCommodityName { get; set; }

        Nullable<System.DateTime> ServiceContractPurchaseDate { get; set; }
        string ServiceContractLicensePlate { get; set; }
        string ServiceContractChassisCode { get; set; }
        string ServiceContractEngineCode { get; set; }
        string ServiceContractColorCode { get; set; }
        string ServiceContractAgentName { get; set; }
    }
}