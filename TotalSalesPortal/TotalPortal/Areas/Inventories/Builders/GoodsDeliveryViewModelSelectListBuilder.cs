using TotalCore.Repositories.Commons;
using TotalCore.Repositories.Inventories;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Inventories.ViewModels;

namespace TotalPortal.Areas.Inventories.Builders
{
    public interface IGoodsDeliveryViewModelSelectListBuilder : IViewModelSelectListBuilder<GoodsDeliveryViewModel>
    {
    }

    public class GoodsDeliveryViewModelSelectListBuilder : IGoodsDeliveryViewModelSelectListBuilder
    {
        private readonly IAspNetUserRepository aspNetUserRepository;
        private readonly IAspNetUserSelectListBuilder aspNetUserSelectListBuilder;

        private readonly IVehicleSelectListBuilder vehicleSelectListBuilder;
        private readonly IVehicleRepository vehicleRepository;

        public GoodsDeliveryViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IVehicleSelectListBuilder vehicleSelectListBuilder, IVehicleRepository vehicleRepository)
        {
            this.aspNetUserRepository = aspNetUserRepository;
            this.aspNetUserSelectListBuilder = aspNetUserSelectListBuilder;

            this.vehicleSelectListBuilder = vehicleSelectListBuilder;
            this.vehicleRepository = vehicleRepository;
        }

        public void BuildSelectLists(GoodsDeliveryViewModel goodsDeliveryViewModel)
        {
            goodsDeliveryViewModel.AspNetUserSelectList = aspNetUserSelectListBuilder.BuildSelectListItemsForAspNetUsers(aspNetUserRepository.GetAllAspNetUsers(), goodsDeliveryViewModel.UserID);
            goodsDeliveryViewModel.VehicleSelectList = vehicleSelectListBuilder.BuildSelectListItemsForVehicles(vehicleRepository.GetAllVehicles());
        }

    }

}