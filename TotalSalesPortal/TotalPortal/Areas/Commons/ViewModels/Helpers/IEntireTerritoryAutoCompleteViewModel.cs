using System;
using System.ComponentModel.DataAnnotations;

namespace TotalPortal.Areas.Commons.ViewModels.Helpers
{
    public interface IEntireTerritoryAutoCompleteViewModel
    {
        [Display(Name = "Khu vực")]
        int TerritoryID { get; set; }
        string EntireTerritoryEntireName { get; set; }       
    }
}
