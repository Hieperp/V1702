using TotalModel.Models;
using TotalDTO.Accounts;
using TotalCore.Services.Helpers;

namespace TotalCore.Services.Accounts
{
    public interface ICreditNoteService : IGenericWithDetailService<CreditNote, CreditNoteDetail, CreditNoteDTO, CreditNotePrimitiveDTO, CreditNoteDetailDTO>
    {
    }
}

