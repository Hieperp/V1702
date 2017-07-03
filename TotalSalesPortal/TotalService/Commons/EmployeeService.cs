using TotalModel.Models;
using TotalDTO.Commons;
using TotalCore.Repositories.Commons;
using TotalCore.Services.Commons;

namespace TotalService.Commons
{
    public class EmployeeService : GenericService<Employee, EmployeeDTO, EmployeePrimitiveDTO>, IEmployeeService
    {
        public EmployeeService(IEmployeeRepository employeeRepository)
            : base(employeeRepository)
        {
        }

    }
}
