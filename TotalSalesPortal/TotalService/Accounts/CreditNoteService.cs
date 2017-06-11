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
    }
}
