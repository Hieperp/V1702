using TotalModel.Models;

using TotalDTO.Commons;
using TotalCore.Services.Commons;

using TotalPortal.Controllers;
using TotalPortal.Areas.Commons.ViewModels;
using TotalPortal.Areas.Commons.Builders;


namespace TotalPortal.Areas.Commons.Controllers
{
    public class CustomersController : GenericSimpleController<Customer, CustomerDTO, CustomerPrimitiveDTO, CustomerViewModel>
    {
        public CustomersController(ICustomerService customerService, ICustomerSelectListBuilder customerViewModelSelectListBuilder)
            : base(customerService, customerViewModelSelectListBuilder)
        {
        }
    }
}

