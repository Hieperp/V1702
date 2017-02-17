using System;
namespace TotalPortal.Areas.Commons.ViewModels.Helpers
{
    public interface ICustomerAutoCompleteViewModel
    {
        int CustomerID { get; set; }
        string CustomerName { get; set; }
        Nullable<System.DateTime> CustomerBirthday { get; set; }
        string CustomerVATCode { get; set; }
        string CustomerTelephone { get; set; }
        string CustomerBillingAddress { get; set; }
        string CustomerEntireTerritoryEntireName { get; set; }
    }
}
