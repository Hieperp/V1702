using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Commons
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        IList<Customer> SearchSuppliers(string searchText);
        IList<Customer> SearchCustomers(string searchText);
        IList<Customer> SearchCustomersByIndex(int customerCategoryID, int customerTypeID, int territoryID);

        IList<Customer> GetAllCustomers();
        IList<Customer> GetAllSuppliers();

        bool GetShowDiscount(int customerID);
    }
}
