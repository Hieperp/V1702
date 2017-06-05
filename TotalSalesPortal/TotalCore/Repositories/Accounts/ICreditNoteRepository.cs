using TotalModel.Models;

namespace TotalCore.Repositories.Accounts
{
    public interface ICreditNoteRepository : IGenericWithDetailRepository<CreditNote, CreditNoteDetail>
    {
    }

    public interface ICreditNoteAPIRepository : IGenericAPIRepository
    {
    }

}
