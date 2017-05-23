using TotalModel.Models;
using TotalDTO.Sales;
using TotalCore.Services.Helpers;

namespace TotalCore.Services.Sales
{
    public interface ISalesReturnService : IGenericWithViewDetailService<SalesReturn, SalesReturnDetail, SalesReturnViewDetail, SalesReturnDTO, SalesReturnPrimitiveDTO, SalesReturnDetailDTO>
    {
    }
}

