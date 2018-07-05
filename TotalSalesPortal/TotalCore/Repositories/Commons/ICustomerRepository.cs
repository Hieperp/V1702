using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Commons
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        IList<CustomerBase> GetCustomerBases(string searchText, int warehouseTaskID);
        IList<string> GetShippingAddress(int? customerID, string searchText);
        IList<string> GetAddressees(int? customerID, string searchText);

        IList<Customer> SearchSuppliers(string searchText);
        IList<Customer> SearchCustomersByIndex(int customerCategoryID, int customerTypeID, int territoryID);

        IList<Customer> GetAllCustomers();
        IList<Customer> GetAllSuppliers();

        bool GetShowDiscount(int customerID);
    }

    public interface ICustomerAPIRepository : IGenericAPIRepository
    {
    }

}
