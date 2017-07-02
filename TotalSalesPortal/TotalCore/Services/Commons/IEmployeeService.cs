using TotalDTO.Commons;
using TotalModel.Models;

namespace TotalCore.Services.Commons
{
    public interface IEmployeeService : IGenericService<Employee, EmployeeDTO, EmployeePrimitiveDTO>
    {
    }
}
