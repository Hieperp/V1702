using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalDTO.Accounts;
using TotalCore.Repositories.Accounts;
using TotalCore.Services.Accounts;

namespace TotalService.Accounts
{
    public class ReceiptService : GenericWithViewDetailService<Receipt, ReceiptDetail, ReceiptViewDetail, ReceiptDTO, ReceiptPrimitiveDTO, ReceiptDetailDTO>, IReceiptService
    {
        private readonly IReceiptRepository receiptRepository;

        public ReceiptService(IReceiptRepository receiptRepository)
            : base(receiptRepository, "ReceiptPostSaveValidate", "ReceiptSaveRelative", null, null, null, "GetReceiptViewDetails")
        {
            this.receiptRepository = receiptRepository;
        }

        public override ICollection<ReceiptViewDetail> GetViewDetails(int receiptID)
        {
            throw new System.ArgumentException("Invalid call GetViewDetails(id). Use GetReceiptViewDetails instead.", "Purchase Invoice Service");
        }

        public ICollection<ReceiptViewDetail> GetReceiptViewDetails(int receiptID, int locationID, int goodsIssueID, int customerID, bool isReadOnly)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("ReceiptID", receiptID), new ObjectParameter("LocationID", locationID), new ObjectParameter("GoodsIssueID", goodsIssueID), new ObjectParameter("CustomerID", customerID), new ObjectParameter("IsReadOnly", isReadOnly) };
            return this.GetViewDetails(parameters);
        }

        public override bool Save(ReceiptDTO receiptDTO)
        {
            receiptDTO.ReceiptViewDetails.RemoveAll(x => (x.ReceiptAmount == 0 && x.CashDiscount == 0 && x.OtherDiscount == 0 && x.FluctuationAmount == 0));
            return base.Save(receiptDTO);
        }        
    }
}
