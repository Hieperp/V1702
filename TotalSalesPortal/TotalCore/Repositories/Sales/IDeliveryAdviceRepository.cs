using System;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Sales
{
    public interface IDeliveryAdviceRepository : IGenericWithDetailRepository<DeliveryAdvice, DeliveryAdviceDetail>
    {
    }

    public interface IDeliveryAdviceAPIRepository : IGenericAPIRepository
    {
        IEnumerable<DeliveryAdvicePendingCustomer> GetCustomers(int? locationID);
        IEnumerable<DeliveryAdvicePendingSalesOrder> GetSalesOrders(int? locationID);

        IEnumerable<DeliveryAdvicePendingSalesOrderDetail> GetPendingSalesOrderDetails(int? locationID, int? deliveryAdviceID, int? salesOrderID, int? customerID, int? receiverID, int? priceCategoryID, int? warehouseID, string shippingAddress, string addressee, int? tradePromotionID, decimal? vatPercent, DateTime? entryDate, string salesOrderDetailIDs, bool isReadonly);
    }

}
