using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalBase.Enums;
using TotalModel.Models;
using TotalCore.Repositories.Accounts;


namespace TotalDAL.Repositories.Accounts
{
    public class CreditNoteRepository : GenericWithDetailRepository<CreditNote, CreditNoteDetail>, ICreditNoteRepository
    {
        public CreditNoteRepository(TotalSalesPortalEntities totalSalesPortalEntities)
            : base(totalSalesPortalEntities, "CreditNoteEditable", "CreditNoteApproved")
        {
        }
    }








    public class CreditNoteAPIRepository : GenericAPIRepository, ICreditNoteAPIRepository
    {
        public CreditNoteAPIRepository(TotalSalesPortalEntities totalSalesPortalEntities)
            : base(totalSalesPortalEntities, "GetCreditNoteIndexes")
        {
        }
    }


}
