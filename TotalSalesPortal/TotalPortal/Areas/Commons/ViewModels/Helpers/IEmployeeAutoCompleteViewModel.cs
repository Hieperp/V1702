using System.ComponentModel.DataAnnotations;

namespace TotalPortal.Areas.Commons.ViewModels.Helpers
{
    public interface IEmployeeAutoCompleteViewModel
    {
        int EmployeeID { get; set; }
        [Display(Name = "Nhân viên thực hiện")]
        string EmployeeName { get; set; }
    }

    public interface IReceptionistAutoCompleteViewModel
    {
        int ReceptionistID { get; set; }
        [Display(Name = "Nhân viên tiếp nhận")]
        string ReceptionistName { get; set; }
    }
}
