using TotalModel.Models;

namespace TotalCore.Repositories.Sales
{
    public interface IDeliveryAdviceRepository : IGenericWithDetailRepository<DeliveryAdvice, DeliveryAdviceDetail>
    {
    }

    public interface IDeliveryAdviceAPIRepository : IGenericAPIRepository
    {
    }

}
