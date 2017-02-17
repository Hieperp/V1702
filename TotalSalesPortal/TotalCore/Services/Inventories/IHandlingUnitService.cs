using System.Collections.Generic;

using TotalModel.Models;
using TotalDTO.Inventories;

namespace TotalCore.Services.Inventories
{
    public interface IHandlingUnitService : IGenericWithViewDetailService<HandlingUnit, HandlingUnitDetail, HandlingUnitViewDetail, HandlingUnitDTO, HandlingUnitPrimitiveDTO, HandlingUnitDetailDTO>
    {
    }
}
