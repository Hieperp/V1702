using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Inventories.ViewModels;

namespace TotalPortal.Areas.Inventories.Builders
{
    public interface IHandlingUnitViewModelSelectListBuilder : IViewModelSelectListBuilder<HandlingUnitViewModel>
    {
    }

    public class HandlingUnitViewModelSelectListBuilder : A01ViewModelSelectListBuilder<HandlingUnitViewModel>, IHandlingUnitViewModelSelectListBuilder
    {
        private readonly IPackingMaterialSelectListBuilder packingMaterialSelectListBuilder;
        private readonly IPackingMaterialRepository packingMaterialRepository;

        public HandlingUnitViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IPackingMaterialSelectListBuilder packingMaterialSelectListBuilder, IPackingMaterialRepository packingMaterialRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {
            this.packingMaterialSelectListBuilder = packingMaterialSelectListBuilder;
            this.packingMaterialRepository = packingMaterialRepository;
        }

        public override void BuildSelectLists(HandlingUnitViewModel handlingUnitViewModel)
        {
            base.BuildSelectLists(handlingUnitViewModel);
            handlingUnitViewModel.PackingMaterialSelectList = this.packingMaterialSelectListBuilder.BuildSelectListItemsForPackingMaterials(this.packingMaterialRepository.GetAllPackingMaterials());
        }

    }

}