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
        public DeliveryAdviceRepository(TotalSalesPortalEntities totalSalesPortalEntities)
            : base(totalSalesPortalEntities, "DeliveryAdviceEditable", "DeliveryAdviceApproved", null, "DeliveryAdviceVoidable")
        {

            Helpers.SqlProgrammability.Accounts.AccountInvoice accountInvoice = new Helpers.SqlProgrammability.Accounts.AccountInvoice(totalSalesPortalEntities);
            accountInvoice.RestoreProcedure();

            return;

            Helpers.SqlProgrammability.Inventories.HandlingUnit handlingUnit = new Helpers.SqlProgrammability.Inventories.HandlingUnit(totalSalesPortalEntities);
            handlingUnit.RestoreProcedure();


            Helpers.SqlProgrammability.Inventories.GoodsIssue goodsIssue = new Helpers.SqlProgrammability.Inventories.GoodsIssue(totalSalesPortalEntities);
            goodsIssue.RestoreProcedure();

            Helpers.SqlProgrammability.Reports.SaleReports saleReports = new Helpers.SqlProgrammability.Reports.SaleReports(totalSalesPortalEntities);
            saleReports.RestoreProcedure();



            

            Helpers.SqlProgrammability.Commons.AccessControl accessControl = new Helpers.SqlProgrammability.Commons.AccessControl(totalSalesPortalEntities);
            accessControl.RestoreProcedure();

            


            

            Helpers.SqlProgrammability.Commons.Commons commons = new Helpers.SqlProgrammability.Commons.Commons(totalSalesPortalEntities);
            commons.RestoreProcedure();



            Helpers.SqlProgrammability.Sales.DeliveryAdvice deliveryAdvice = new Helpers.SqlProgrammability.Sales.DeliveryAdvice(totalSalesPortalEntities);
            deliveryAdvice.RestoreProcedure();





            Helpers.SqlProgrammability.Inventories.GoodsDelivery goodsDelivery = new Helpers.SqlProgrammability.Inventories.GoodsDelivery(totalSalesPortalEntities);
            goodsDelivery.RestoreProcedure();



            Helpers.SqlProgrammability.Accounts.Receipt receipt = new Helpers.SqlProgrammability.Accounts.Receipt(totalSalesPortalEntities);
            receipt.RestoreProcedure();




            Helpers.SqlProgrammability.Inventories.Inventories inventories = new Helpers.SqlProgrammability.Inventories.Inventories(totalSalesPortalEntities);
            inventories.RestoreProcedure();

            

            

        }
    }








    public class DeliveryAdviceAPIRepository : GenericAPIRepository, IDeliveryAdviceAPIRepository
    {
        public DeliveryAdviceAPIRepository(TotalSalesPortalEntities totalSalesPortalEntities)
            : base(totalSalesPortalEntities, "GetDeliveryAdviceIndexes")
        {
        }
    }


}
