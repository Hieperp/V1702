using TotalModel.Models;
using TotalDTO.Sales;
using TotalCore.Services.Helpers;

namespace TotalCore.Services.Sales
{
    public interface ISalesOrderService : IGenericWithViewDetailService<SalesOrder, SalesOrderDetail, SalesOrderViewDetail, SalesOrderDTO, SalesOrderPrimitiveDTO, SalesOrderDetailDTO>
    {
    }

    public interface ISalesOrderHelperService : IHelperService<SalesOrder, SalesOrderDetail, SalesOrderDTO, SalesOrderDetailDTO>
    {
    }
}

