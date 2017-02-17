using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Commons
{
    public interface IVoidTypeRepository : IGenericRepository<VoidType>
    {
        IList<VoidType> SearchVoidTypes(string searchText);
    }
}

