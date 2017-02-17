using TotalModel.Models;
using TotalDTO.Sales;
using TotalCore.Services.Helpers;

namespace TotalCore.Services.Sales
{    
    public interface IDeliveryAdviceService : IGenericWithViewDetailService<DeliveryAdvice, DeliveryAdviceDetail, DeliveryAdviceViewDetail, DeliveryAdviceDTO, DeliveryAdvicePrimitiveDTO, DeliveryAdviceDetailDTO>
    {
    }

    public interface IDeliveryAdviceHelperService : IHelperService<DeliveryAdvice, DeliveryAdviceDetail, DeliveryAdviceDTO, DeliveryAdviceDetailDTO>
    {
    }
}

