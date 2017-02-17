﻿using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalDAL.Repositories.Commons
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(TotalSalesPortalEntities totalSalesPortalEntities)
            : base(totalSalesPortalEntities)
        {
        }

        public IList<Customer> SearchSuppliers(string searchText)
        {
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = false;
            List<Customer> suppliers = this.TotalSalesPortalEntities.Customers.Include("EntireTerritory").Where(w => w.IsSupplier && (w.Name.Contains(searchText) || w.VATCode.Contains(searchText))).ToList();
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = true;

            return suppliers;
        }

        public IList<Customer> SearchCustomers(string searchText)
        {
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = false;
            List<Customer> customers = this.TotalSalesPortalEntities.Customers.Include(e => e.EntireTerritory).Include(pr => pr.PriceCategory).Include(ep => ep.Employee).Where(w => w.IsCustomer && (w.Code.Contains(searchText) || w.Name.Contains(searchText) || w.OfficialName.Contains(searchText))).OrderByDescending(or => or.CustomerID).Take(20).ToList();
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = true;

            return customers;
        }

        public IList<Customer> SearchCustomersByIndex(int customerCategoryID, int customerTypeID, int territoryID)
        {
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = false;
            List<Customer> customers = this.TotalSalesPortalEntities.Customers.Where(w => w.CustomerCategoryID == customerCategoryID || w.CustomerTypeID == customerTypeID || w.TerritoryID == territoryID).ToList();
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = true;

            return customers;
        }

        public IList<Customer> GetAllCustomers()
        {
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = false;
            List<Customer> customers = this.TotalSalesPortalEntities.Customers.Where(w => w.IsCustomer).ToList();
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = true;

            return customers;
        }

        public IList<Customer> GetAllSuppliers()
        {
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = false;
            List<Customer> suppliers = this.TotalSalesPortalEntities.Customers.Where(w => w.IsSupplier).ToList();
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = true;

            return suppliers;
        }
    }
}

