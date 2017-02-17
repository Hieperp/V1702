using System.Collections.Generic;

using TotalModel.Models;
using TotalDTO.Accounts;

namespace TotalCore.Services.Accounts
{
    public interface IReceiptService : IGenericWithViewDetailService<Receipt, ReceiptDetail, ReceiptViewDetail, ReceiptDTO, ReceiptPrimitiveDTO, ReceiptDetailDTO>
    {
        ICollection<ReceiptViewDetail> GetReceiptViewDetails(int receiptID, int purchaseOrderID, int supplierID, bool isReadOnly);
    }
}
