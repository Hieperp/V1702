using TotalModel.Models;

namespace TotalCore.Repositories.Sales
{
    public interface ISalesOrderRepository : IGenericWithDetailRepository<SalesOrder, SalesOrderDetail>
    {
    }

    public interface ISalesOrderAPIRepository : IGenericAPIRepository
    {
    }

}
