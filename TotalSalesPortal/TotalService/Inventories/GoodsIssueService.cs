using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalDTO.Inventories;
using TotalCore.Repositories.Inventories;
using TotalCore.Services.Inventories;

namespace TotalService.Inventories
{
    public class GoodsIssueService : GenericWithViewDetailService<GoodsIssue, GoodsIssueDetail, GoodsIssueViewDetail, GoodsIssueDTO, GoodsIssuePrimitiveDTO, GoodsIssueDetailDTO>, IGoodsIssueService
    {
        private readonly IGoodsIssueRepository goodsIssueRepository;

        public GoodsIssueService(IGoodsIssueRepository goodsIssueRepository)
            : base(goodsIssueRepository, "GoodsIssuePostSaveValidate", "GoodsIssueSaveRelative", "GoodsIssueToggleApproved", null, null, "GetGoodsIssueViewDetails")
        {
            this.goodsIssueRepository = goodsIssueRepository;
        }

        public override ICollection<GoodsIssueViewDetail> GetViewDetails(int goodsIssueID)
        {
            throw new System.ArgumentException("Invalid call GetViewDetails(id). Use GetGoodsIssueViewDetails instead.", "Purchase Invoice Service");
        }

        public ICollection<GoodsIssueViewDetail> GetGoodsIssueViewDetails(int goodsIssueID, int locationID, int deliveryAdviceID, int customerID, int receiverID, int warehouseID, string shippingAddress, decimal? tradeDiscountRate, decimal? vatPercent, bool isReadOnly)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("GoodsIssueID", goodsIssueID), new ObjectParameter("LocationID", locationID), new ObjectParameter("DeliveryAdviceID", deliveryAdviceID), new ObjectParameter("CustomerID", customerID), new ObjectParameter("ReceiverID", receiverID), new ObjectParameter("WarehouseID", warehouseID), new ObjectParameter("ShippingAddress", shippingAddress), new ObjectParameter("TradeDiscountRate", tradeDiscountRate), new ObjectParameter("VATPercent", vatPercent), new ObjectParameter("IsReadOnly", isReadOnly) };
            return this.GetViewDetails(parameters);
        }

        public List<PendingDeliveryAdviceDescription> GetDescriptions(int locationID, int customerID, int receiverID, int warehouseID, string shippingAddress, decimal? tradeDiscountRate, decimal? vatPercent)
        {
            return this.goodsIssueRepository.GetDescriptions(locationID, customerID, receiverID, warehouseID, shippingAddress, tradeDiscountRate, vatPercent);
        }

    }
}
