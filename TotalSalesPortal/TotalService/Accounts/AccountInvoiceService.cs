using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalDTO.Accounts;
using TotalCore.Repositories.Accounts;
using TotalCore.Services.Accounts;


namespace TotalService.Accounts
{
    public class AccountInvoiceService: GenericWithViewDetailService<AccountInvoice, AccountInvoiceDetail, AccountInvoiceViewDetail, AccountInvoiceDTO, AccountInvoicePrimitiveDTO, AccountInvoiceDetailDTO>, IAccountInvoiceService
    {
        public AccountInvoiceService(IAccountInvoiceRepository accountInvoiceRepository)
            : base(accountInvoiceRepository, "AccountInvoicePostSaveValidate", "AccountInvoiceSaveRelative", null, null, null, "GetAccountInvoiceViewDetails")
        {
        }

        public override bool Save(AccountInvoiceDTO dto)
        {
            dto.AccountInvoiceViewDetails.RemoveAll(x => x.Quantity == 0 && x.FreeQuantity == 0);
            return base.Save(dto);
        }

        public new bool Save(AccountInvoiceDTO dto, bool useExistingTransaction)
        {
            return base.Save(dto, true);
        }

        public override ICollection<AccountInvoiceViewDetail> GetViewDetails(int accountInvoiceID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("AccountInvoiceID", accountInvoiceID) };
            return this.GetViewDetails(parameters);
        }

    }
}
