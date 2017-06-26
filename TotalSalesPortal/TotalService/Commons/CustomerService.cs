using TotalModel.Models;
using TotalDTO.Commons;
using TotalCore.Repositories.Commons;
using TotalCore.Services.Commons;

namespace TotalService.Commons
{
    public class CustomerService : GenericService<Customer, CustomerDTO, CustomerPrimitiveDTO>, ICustomerService
    {
        public CustomerService(ICustomerRepository customerRepository)
            : base(customerRepository)
        {
        }

    }
}
