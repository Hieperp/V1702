using TotalModel.Models;
using TotalDTO.Accounts;
using TotalCore.Repositories.Accounts;
using TotalCore.Services.Accounts;

namespace TotalService.Accounts
{
    public class CreditNoteService : GenericWithDetailService<CreditNote, CreditNoteDetail, CreditNoteDTO, CreditNotePrimitiveDTO, CreditNoteDetailDTO>, ICreditNoteService
    {
        public CreditNoteService(ICreditNoteRepository creditNoteRepository)
            : base(creditNoteRepository, "CreditNotePostSaveValidate", "CreditNoteSaveRelative", "CreditNoteToggleApproved")
        {
        }

        public override bool Save(CreditNoteDTO creditNoteDTO)
        {
            creditNoteDTO.CreditNoteDetails.RemoveAll(x => x.Quantity == 0 || x.UnitPrice == 0 || x.Amount == 0);
            return base.Save(creditNoteDTO);
        }
    }
}
