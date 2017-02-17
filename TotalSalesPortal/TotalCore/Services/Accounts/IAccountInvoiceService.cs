using System.Collections.Generic;

using TotalModel.Models;
using TotalDTO.Accounts;

namespace TotalCore.Services.Accounts
{
    public interface IAccountInvoiceService : IGenericWithViewDetailService<AccountInvoice, AccountInvoiceDetail, AccountInvoiceViewDetail, AccountInvoiceDTO, AccountInvoicePrimitiveDTO, AccountInvoiceDetailDTO>
    {
        bool Save(AccountInvoiceDTO dto, bool useExistingTransaction);
    }
}
