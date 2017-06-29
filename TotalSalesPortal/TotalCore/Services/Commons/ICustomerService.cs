using TotalDTO.Commons;
using TotalModel.Models;

namespace TotalCore.Services.Commons
{
    public interface ICustomerService : IGenericService<Customer, CustomerDTO, CustomerPrimitiveDTO>
    {
        bool GetShowDiscount(int customerID);
    }
}
