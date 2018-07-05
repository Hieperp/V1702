using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

using TotalCore.Repositories.Commons;
using TotalModel.Models;
using TotalDTO.Commons;
using TotalPortal.APIs.Sessions;
//using TotalPortal.ViewModels.Commons;

namespace TotalPortal.Areas.Commons.APIs
{
    //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class CustomerAPIsController : Controller
    {
        private readonly ICustomerRepository customerRepository;
        private readonly ICustomerAPIRepository customerAPIRepository;

        public CustomerAPIsController(ICustomerRepository customerRepository, ICustomerAPIRepository customerAPIRepository)
        {
            this.customerRepository = customerRepository;
            this.customerAPIRepository = customerAPIRepository;
        }

        public JsonResult GetCustomerIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<CustomerIndex> CustomerIndexes = this.customerAPIRepository.GetEntityIndexes<CustomerIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = CustomerIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchSuppliers(string searchText)
        {
            var result = customerRepository.SearchSuppliers(searchText).Select(s => new { s.CustomerID, s.Name, s.AttentionName, s.Birthday, s.VATCode, s.Telephone, s.BillingAddress, EntireTerritoryEntireName = s.EntireTerritory.EntireName });

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetCustomerBases(string searchText, int warehouseTaskID)
        {
            var result = customerRepository.GetCustomerBases(searchText, warehouseTaskID);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustomers([DataSourceRequest] DataSourceRequest request, int customerCategoryID, int customerTypeID, int territoryID)
        {
            var customers = this.customerRepository.SearchCustomersByIndex(customerCategoryID, customerTypeID, territoryID);

            DataSourceResult response = customers.ToDataSourceResult(request, o => new CustomerPrimitiveDTO
            {
                CustomerID = o.CustomerID,
                Code = o.Code,
                Name = o.Name,
                OfficialName = o.OfficialName,
                AttentionName = o.AttentionName,
                AttentionTitle = o.AttentionTitle,
                Birthday = o.Birthday,
                BillingAddress = o.BillingAddress,
                Telephone = o.Telephone,
                Facsimile = o.Facsimile,
                Remarks = o.Remarks
            });
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustomers2()
        {
            var customers = this.customerRepository.GetAllCustomers().AsQueryable();

            return Json(customers, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustomers(string text, [DataSourceRequest] DataSourceRequest request)
        {
            //var customers = customerRepository.SearchCustomers(text).Select(s => new { s.CustomerID, s.Name, s.AttentionName, s.OfficialName, s.Birthday, s.Telephone, s.BillingAddress, EntireTerritoryEntireName = s.EntireTerritory.EntireName });

            //DataSourceResult response = customers.ToDataSourceResult(request, o => new CustomerViewModel
            //{
            //    CustomerID = o.CustomerID,
            //    Name = o.Name,
            //    AttentionName = o.AttentionName,
            //    Birthday = o.Birthday,
            //    BillingAddress = o.BillingAddress,
            //    Telephone = o.Telephone,
            //    OfficialName = o.OfficialName,
            //    EntireTerritoryEntireName = o.EntireTerritoryEntireName
            //});
            //return Json(response, JsonRequestBehavior.AllowGet);

            return Json(new JsonResult(), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetShippingAddress(int? customerID, string searchText)
        {
            var result = customerRepository.GetShippingAddress(customerID, searchText);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAddressees(int? customerID, string searchText)
        {
            var result = customerRepository.GetAddressees(customerID, searchText);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}