using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalBase.Enums;
using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalDAL.Repositories.Commons
{
    class SearchRepository
    {
        public class SearchAPIRepository : GenericAPIRepository, ISearchAPIRepository
    {
        public SearchAPIRepository(TotalSalesPortalEntities totalSalesPortalEntities)
            : base(totalSalesPortalEntities, "GetDeliveryAdviceIndexes")
        {
        }

        protected override ObjectParameter[] GetEntityIndexParameters(string aspUserID, DateTime fromDate, DateTime toDate)
        {

            ObjectParameter[] baseParameters = base.GetEntityIndexParameters(aspUserID, fromDate, toDate);
            ObjectParameter[] objectParameters = new ObjectParameter[] { baseParameters[0], baseParameters[1], baseParameters[2], new ObjectParameter("PendingOnly", this.RepositoryBag.ContainsKey("PendingOnly") && this.RepositoryBag["PendingOnly"] != null ? this.RepositoryBag["PendingOnly"] : false) };

            this.RepositoryBag.Remove("PendingOnly");

            return objectParameters;


            
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

        public IEnumerable<DeliveryAdvicePendingSalesOrderDetail> GetPendingSalesOrderDetails(int? locationID, int? deliveryAdviceID, int? salesOrderID, int? customerID, int? receiverID, int? priceCategoryID, int? warehouseID, string shippingAddress, string addressee, int? tradePromotionID, decimal? vatPercent, DateTime? entryDate, string salesOrderDetailIDs, bool isReadonly)
        {
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<DeliveryAdvicePendingSalesOrderDetail> pendingSalesOrderDetails = base.TotalSalesPortalEntities.GetDeliveryAdvicePendingSalesOrderDetails(locationID, deliveryAdviceID, salesOrderID, customerID, receiverID, priceCategoryID, warehouseID, shippingAddress, addressee, tradePromotionID, vatPercent, entryDate, salesOrderDetailIDs, isReadonly).ToList();
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingSalesOrderDetails;
        }

    }

}
