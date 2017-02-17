using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Commons
{
    public interface IPaymentTermRepository
    {
        IList<PaymentTerm> GetAllPaymentTerms();
    }
}
