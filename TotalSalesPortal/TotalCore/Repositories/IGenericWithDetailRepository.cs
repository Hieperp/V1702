using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace TotalCore.Repositories
{
    public interface IGenericWithDetailRepository<TEntity, TEntityDetail> : IGenericRepository<TEntity>
        where TEntity : class
        where TEntityDetail : class
    {
        TEntityDetail RemoveDetail(TEntityDetail entityDetail);
        IEnumerable<TEntityDetail> RemoveRangeDetail(IEnumerable<TEntityDetail> entityDetails);
    }
}
