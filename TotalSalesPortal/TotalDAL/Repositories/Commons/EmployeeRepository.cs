using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalDAL.Repositories.Commons
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(TotalSalesPortalEntities totalSalesPortalEntities)
            : base(totalSalesPortalEntities)
        {
        }

        public IList<Employee> SearchEmployees(string searchText)
        {
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = false;
            List<Employee> employees = this.TotalSalesPortalEntities.Employees.Where(w => (w.Code.Contains(searchText) || w.Name.Contains(searchText))).OrderByDescending(or => or.Name).Take(20).ToList();
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = true;

            return employees;
        }
    }
}