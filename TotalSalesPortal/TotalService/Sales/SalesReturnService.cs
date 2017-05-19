using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;


using TotalModel.Models;
using TotalDTO.Sales;
using TotalCore.Repositories.Sales;
using TotalCore.Services.Sales;

namespace TotalService.Sales
{
    public class SalesReturnService : GenericWithViewDetailService<SalesReturn, SalesReturnDetail, SalesReturnViewDetail, SalesReturnDTO, SalesReturnPrimitiveDTO, SalesReturnDetailDTO>, ISalesReturnService
    {
        public SalesReturnService(ISalesReturnRepository salesReturnRepository)
            : base(salesReturnRepository, "SalesReturnPostSaveValidate", "SalesReturnSaveRelative", "SalesReturnToggleApproved", null, null, "GetSalesReturnViewDetails")
        {
        }

        public override ICollection<SalesReturnViewDetail> GetViewDetails(int salesReturnID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("SalesReturnID", salesReturnID) };
            return this.GetViewDetails(parameters);
        }

        public override bool Save(SalesReturnDTO salesReturnDTO)
        {
            salesReturnDTO.SalesReturnViewDetails.RemoveAll(x => x.Quantity == 0 && x.FreeQuantity == 0);
            return base.Save(salesReturnDTO);
        }
    }
}