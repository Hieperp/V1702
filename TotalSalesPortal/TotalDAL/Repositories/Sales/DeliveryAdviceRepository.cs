using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalBase.Enums;
using TotalModel.Models;
using TotalCore.Repositories.Sales;


namespace TotalDAL.Repositories.Sales
{
    public class DeliveryAdviceRepository : GenericWithDetailRepository<DeliveryAdvice, DeliveryAdviceDetail>, IDeliveryAdviceRepository
    {
        //1-Set Balance Date to 23.59.59
        //2-Copy 3 table (D.A/ GoodsIssue)
        //3-Add two Store procedure (Update balance/ WH journal)
        //4-Modify to VB Project, verify report 1280.rpt (-> create new report in SSRS -> publish to server)

        public DeliveryAdviceRepository(TotalSalesPortalEntities totalSalesPortalEntities)
            : base(totalSalesPortalEntities, "DeliveryAdviceEditable", "DeliveryAdviceApproved", null, "DeliveryAdviceVoidable")
        {

            //return;


            Helpers.SqlProgrammability.Sales.DeliveryAdvice deliveryAdvice = new Helpers.SqlProgrammability.Sales.DeliveryAdvice(totalSalesPortalEntities);
            deliveryAdvice.RestoreProcedure();

            //return;

            Helpers.SqlProgrammability.Accounts.Receipt receipt = new Helpers.SqlProgrammability.Accounts.Receipt(totalSalesPortalEntities);
            receipt.RestoreProcedure();


            //return;

            Helpers.SqlProgrammability.Sales.SalesReturn salesReturn = new Helpers.SqlProgrammability.Sales.SalesReturn(totalSalesPortalEntities);
            salesReturn.RestoreProcedure();

            //return;


            ////return;

            //AccountInvoice: NOT CHECK FOR Approved COMMPLETELY, PLS CHECK IT CAREFULLY LATER. (SaveRelative, GetPendingGoodsIssueDetails, ...). ALSO DO THE SAME CHECK FOR ALL OTHER MODULES
            //AccountInvoice: SHOULD SAVE BillingAddress

            Helpers.SqlProgrammability.Accounts.AccountInvoice accountInvoice = new Helpers.SqlProgrammability.Accounts.AccountInvoice(totalSalesPortalEntities);
            accountInvoice.RestoreProcedure();

            //return;

            Helpers.SqlProgrammability.Inventories.GoodsIssue goodsIssue = new Helpers.SqlProgrammability.Inventories.GoodsIssue(totalSalesPortalEntities);
            goodsIssue.RestoreProcedure();


            //return;

            Helpers.SqlProgrammability.Sales.SalesOrder salesOrder = new Helpers.SqlProgrammability.Sales.SalesOrder(totalSalesPortalEntities);
            salesOrder.RestoreProcedure();


            //return;

            Helpers.SqlProgrammability.Reports.SaleReports saleReports = new Helpers.SqlProgrammability.Reports.SaleReports(totalSalesPortalEntities);
            saleReports.RestoreProcedure();


            
            

            


            //return;

            Helpers.SqlProgrammability.Inventories.HandlingUnit handlingUnit = new Helpers.SqlProgrammability.Inventories.HandlingUnit(totalSalesPortalEntities);
            handlingUnit.RestoreProcedure();



            //return;
            Helpers.SqlProgrammability.Inventories.Inventories inventories = new Helpers.SqlProgrammability.Inventories.Inventories(totalSalesPortalEntities);
            inventories.RestoreProcedure();




            //return;


            Helpers.SqlProgrammability.Commons.Commons commons = new Helpers.SqlProgrammability.Commons.Commons(totalSalesPortalEntities);
            commons.RestoreProcedure();











            













            Helpers.SqlProgrammability.Commons.AccessControl accessControl = new Helpers.SqlProgrammability.Commons.AccessControl(totalSalesPortalEntities);
            accessControl.RestoreProcedure();











            Helpers.SqlProgrammability.Inventories.GoodsDelivery goodsDelivery = new Helpers.SqlProgrammability.Inventories.GoodsDelivery(totalSalesPortalEntities);
            goodsDelivery.RestoreProcedure();









        }
    }








    public class DeliveryAdviceAPIRepository : GenericAPIRepository, IDeliveryAdviceAPIRepository
    {
        public DeliveryAdviceAPIRepository(TotalSalesPortalEntities totalSalesPortalEntities)
            : base(totalSalesPortalEntities, "GetDeliveryAdviceIndexes")
        {
        }

        public IEnumerable<DeliveryAdvicePendingCustomer> GetCustomers(int? locationID)
        {
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<DeliveryAdvicePendingCustomer> pendingSalesOrderCustomers = base.TotalSalesPortalEntities.GetDeliveryAdvicePendingCustomers(locationID).ToList();
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingSalesOrderCustomers;
        }

        public IEnumerable<DeliveryAdvicePendingSalesOrder> GetSalesOrders(int? locationID)
        {
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<DeliveryAdvicePendingSalesOrder> pendingSalesOrders = base.TotalSalesPortalEntities.GetDeliveryAdvicePendingSalesOrders(locationID).ToList();
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingSalesOrders;
        }

        public IEnumerable<DeliveryAdvicePendingSalesOrderDetail> GetPendingSalesOrderDetails(int? locationID, int? deliveryAdviceID, int? salesOrderID, int? customerID, int? receiverID, int? priceCategoryID, int? warehouseID, string shippingAddress, decimal? tradeDiscountRate, decimal? vatPercent, DateTime? entryDate, string salesOrderDetailIDs, bool isReadonly)
        {
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<DeliveryAdvicePendingSalesOrderDetail> pendingSalesOrderDetails = base.TotalSalesPortalEntities.GetDeliveryAdvicePendingSalesOrderDetails(locationID, deliveryAdviceID, salesOrderID, customerID, receiverID, priceCategoryID, warehouseID, shippingAddress, tradeDiscountRate, vatPercent, entryDate, salesOrderDetailIDs, isReadonly).ToList();
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingSalesOrderDetails;
        }

    }


}
