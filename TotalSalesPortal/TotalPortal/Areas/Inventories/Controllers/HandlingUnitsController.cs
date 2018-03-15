using System.Web.Mvc;

using TotalModel.Models;

using TotalDTO.Inventories;
using TotalCore.Services.Inventories;

using TotalPortal.Controllers;

using TotalPortal.APIs.Sessions;

using TotalPortal.Areas.Inventories.ViewModels;
using TotalPortal.Areas.Inventories.Builders;
using TotalPortal.Areas.Inventories.Controllers.Sessions;


namespace TotalPortal.Areas.Inventories.Controllers
{
    public class HandlingUnitsController : GenericViewDetailController<HandlingUnit, HandlingUnitDetail, HandlingUnitViewDetail, HandlingUnitDTO, HandlingUnitPrimitiveDTO, HandlingUnitDetailDTO, HandlingUnitViewModel>
    {
        public HandlingUnitsController(IHandlingUnitService handlingUnitService, IHandlingUnitViewModelSelectListBuilder handlingUnitViewModelSelectListBuilder)
            : base(handlingUnitService, handlingUnitViewModelSelectListBuilder, true)
        {
        }

        protected override HandlingUnitViewModel InitViewModelByCopy(HandlingUnitViewModel simpleViewModel)
        {
            return new HandlingUnitViewModel() { Customer = simpleViewModel.Customer, Receiver = simpleViewModel.Receiver, ShippingAddress = simpleViewModel.ShippingAddress, Addressee = simpleViewModel.Addressee, GoodsIssue = simpleViewModel.GoodsIssue, PackagingStaff = simpleViewModel.PackagingStaff, PackingMaterialID = simpleViewModel.PackingMaterialID, LotNo = simpleViewModel.LotNo };
        }
        
        protected override HandlingUnitViewModel InitViewModelByDefault(HandlingUnitViewModel simpleViewModel)
        {
            simpleViewModel = base.InitViewModelByDefault(simpleViewModel);

            if (simpleViewModel.PackagingStaff == null)
            {
                string packagingStaffSession = HandlingUnitSession.GetPackagingStaff(this.HttpContext);

                if (HomeSession.TryParseID(packagingStaffSession) > 0)
                {
                    simpleViewModel.PackagingStaff = new TotalDTO.Commons.EmployeeBaseDTO();
                    simpleViewModel.PackagingStaff.EmployeeID = (int)HomeSession.TryParseID(packagingStaffSession);
                    simpleViewModel.PackagingStaff.Name = HomeSession.TryParseName(packagingStaffSession);
                }
            }

            return simpleViewModel;
        }

        protected override void BackupViewModelToSession(HandlingUnitViewModel simpleViewModel)
        {
            base.BackupViewModelToSession(simpleViewModel);
            HandlingUnitSession.SetPackagingStaff(this.HttpContext, simpleViewModel.PackagingStaff.EmployeeID, simpleViewModel.PackagingStaff.Name);
        }




        public virtual ActionResult GetPendingGoodsIssueDetails()
        {
            return View();
        }



        public ActionResult PrintDetail(int? id)
        {
            return View(InitPrintViewModel(id));
        }
    }
}