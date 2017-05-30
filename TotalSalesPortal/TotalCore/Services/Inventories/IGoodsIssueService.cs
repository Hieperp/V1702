using System.Collections.Generic;

using TotalModel.Models;
using TotalDTO.Inventories;

namespace TotalCore.Services.Inventories
{
    public interface IGoodsIssueService : IGenericWithViewDetailService<GoodsIssue, GoodsIssueDetail, GoodsIssueViewDetail, GoodsIssueDTO, GoodsIssuePrimitiveDTO, GoodsIssueDetailDTO>
    {
        ICollection<GoodsIssueViewDetail> GetGoodsIssueViewDetails(int goodsIssueID, int locationID, int deliveryAdviceID, int customerID, int receiverID, int warehouseID, string shippingAddress, decimal? tradeDiscountRate, decimal? vatPercent, bool isReadOnly);

        List<PendingDeliveryAdviceDescription> GetDescriptions(int locationID, int customerID, int receiverID, int warehouseID, string shippingAddress, decimal? tradeDiscountRate, decimal? vatPercent);
    }
}
