using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Inventories.ViewModels;

namespace TotalPortal.Areas.Inventories.Builders
{
    public interface IGoodsDeliveryViewModelSelectListBuilder : IViewModelSelectListBuilder<GoodsDeliveryViewModel>
    {
    }

    public class GoodsDeliveryViewModelSelectListBuilder : A01ViewModelSelectListBuilder<GoodsDeliveryViewModel>, IGoodsDeliveryViewModelSelectListBuilder
    {
        private readonly IVehicleSelectListBuilder vehicleSelectListBuilder;
        private readonly IVehicleRepository vehicleRepository;

        public GoodsDeliveryViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IVehicleSelectListBuilder vehicleSelectListBuilder, IVehicleRepository vehicleRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {
            this.vehicleSelectListBuilder = vehicleSelectListBuilder;
            this.vehicleRepository = vehicleRepository;
        }

        public override void BuildSelectLists(GoodsDeliveryViewModel goodsDeliveryViewModel)
        {
            base.BuildSelectLists(goodsDeliveryViewModel);
            goodsDeliveryViewModel.VehicleSelectList = this.vehicleSelectListBuilder.BuildSelectListItemsForVehicles(this.vehicleRepository.GetAllVehicles());
        }

    }

}