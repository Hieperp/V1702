using System.Collections.Generic;

namespace TotalModel
{
    public interface IBaseDetailEntity<TEntityDetail> where TEntityDetail : class
    {
        ICollection<TEntityDetail> GetDetails();
    }
}
