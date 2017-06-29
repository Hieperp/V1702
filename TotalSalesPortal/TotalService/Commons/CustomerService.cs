using TotalModel.Models;
using TotalDTO.Commons;
using TotalCore.Repositories.Commons;
using TotalCore.Services.Commons;

namespace TotalService.Commons
{
    public class CustomerService : GenericService<Customer, CustomerDTO, CustomerPrimitiveDTO>, ICustomerService
    {
        private ICustomerRepository customerRepository;
        public CustomerService(ICustomerRepository customerRepository)
            : base(customerRepository, "", "CustomerSaveRelative")
        {
            this.customerRepository = customerRepository;
        }

        public bool GetShowDiscount(int customerID) { return this.customerRepository.GetShowDiscount(customerID); }
    }
}
