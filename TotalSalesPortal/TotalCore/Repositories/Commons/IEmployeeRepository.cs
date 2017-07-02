using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Commons
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        IList<Employee> SearchEmployees(string searchText);
    }

    public interface IEmployeeAPIRepository : IGenericAPIRepository
    {
    }

}

