using TotalCore.Repositories.Commons;
using TotalCore.Repositories.Inventories;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Inventories.ViewModels;

namespace TotalPortal.Areas.Inventories.Builders
{
    public interface IHandlingUnitViewModelSelectListBuilder : IViewModelSelectListBuilder<HandlingUnitViewModel>
    {
    }

    public class HandlingUnitViewModelSelectListBuilder : IHandlingUnitViewModelSelectListBuilder
    {
        private readonly IPackingMaterialSelectListBuilder packingMaterialSelectListBuilder;
        private readonly IPackingMaterialRepository packingMaterialRepository;
        private readonly IAspNetUserRepository aspNetUserRepository;
        private readonly IAspNetUserSelectListBuilder aspNetUserSelectListBuilder;

        public HandlingUnitViewModelSelectListBuilder(IPackingMaterialSelectListBuilder packingMaterialSelectListBuilder, IPackingMaterialRepository packingMaterialRepository, IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
        {
            this.packingMaterialSelectListBuilder = packingMaterialSelectListBuilder;
            this.packingMaterialRepository = packingMaterialRepository;
            this.aspNetUserRepository = aspNetUserRepository;
            this.aspNetUserSelectListBuilder = aspNetUserSelectListBuilder;
        }

        public void BuildSelectLists(HandlingUnitViewModel handlingUnitViewModel)
        {
            handlingUnitViewModel.PackingMaterialSelectList = packingMaterialSelectListBuilder.BuildSelectListItemsForPackingMaterials(packingMaterialRepository.GetAllPackingMaterials());
            handlingUnitViewModel.AspNetUserSelectList = aspNetUserSelectListBuilder.BuildSelectListItemsForAspNetUsers(aspNetUserRepository.GetAllAspNetUsers(), handlingUnitViewModel.UserID);
        }

    }

}