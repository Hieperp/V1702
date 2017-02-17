using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalDAL.Repositories.Commons
{
    public class VoidTypeRepository : GenericRepository<VoidType>, IVoidTypeRepository
    {
        public VoidTypeRepository(TotalSalesPortalEntities totalSalesPortalEntities)
            : base(totalSalesPortalEntities)
        {
        }

        public IList<VoidType> SearchVoidTypes(string searchText)
        {
            
                this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = false;
                List<VoidType> goodsIssueTypes = this.TotalSalesPortalEntities.VoidTypes.Where(w => (w.Code.Contains(searchText) || w.Name.Contains(searchText))).OrderByDescending(or => or.Name).Take(3).ToList();
                this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = true;

                return goodsIssueTypes;
            
        }
    }
}