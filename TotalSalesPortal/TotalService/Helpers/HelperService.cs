using System;
using System.Linq;

using AutoMapper;

using TotalBase;
using TotalBase.Enums;
using TotalDTO;
using TotalCore.Services.Helpers;
using TotalModel;
using TotalModel.Helpers;


namespace TotalService.Helpers
{

    public class HelperService<TEntity, TEntityDetail, TDto, TDtoDetail> : IHelperService<TEntity, TEntityDetail, TDto, TDtoDetail>

        where TEntity : IBaseDetailEntity<TEntityDetail>
        where TEntityDetail : class, IHelperEntryDate, IHelperWarehouseID, IHelperCommodityID, IHelperCommodityTypeID
        where TDto : BaseDTO, IBaseDetailEntity<TDtoDetail>
        where TDtoDetail : class, IHelperWarehouseID, IHelperCommodityID, IHelperCommodityTypeID
    {
        /// <summary>
        /// Get checkedDate, warehouseIDList/ commodityIDList Collection for CheckOverStock
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="entity"></param>
        /// <param name="checkedDate"></param>
        /// <param name="warehouseIDList"></param>
        /// <param name="commodityIDList"></param>
        public void GetWCParameters(TDto dto, TEntity entity, ref DateTime? checkedDate, ref string warehouseIDList, ref string commodityIDList)
        {
            #region Get checkedDate, warehouseIDList/ commodityIDList Collection for CheckOverStock

            string warehouseIDListLocal = warehouseIDList; //Cannot use ref or out parameter 'warehouseIDList' inside an anonymous method, lambda expression, or query expression
            string commodityIDListLocal = commodityIDList;

            if (dto != null)
            {
                if (checkedDate == null || checkedDate > dto.EntryDate) checkedDate = dto.EntryDate;

                if (dto.GetDetails() != null && dto.GetDetails().Count > 0)
                    dto.GetDetails().Where(w => (new int[] { (int)GlobalEnums.CommodityTypeID.Parts, (int)GlobalEnums.CommodityTypeID.Consumables }).Contains(w.CommodityTypeID)).Each(detailDTO =>
                    {
                        this.addIdToStringList(ref warehouseIDListLocal, detailDTO.GetWarehouseID());
                        this.addIdToStringList(ref commodityIDListLocal, detailDTO.CommodityID);
                    });
            }

            if (entity != null)
            {
                if (entity.GetDetails() != null && entity.GetDetails().Count > 0)
                {
                    if (checkedDate == null || checkedDate > entity.GetDetails().First().EntryDate) checkedDate = entity.GetDetails().First().EntryDate;

                    entity.GetDetails().Where(w => (new int[] { (int)GlobalEnums.CommodityTypeID.Parts, (int)GlobalEnums.CommodityTypeID.Consumables }).Contains(w.CommodityTypeID)).Each(entityDetail =>
                    {
                        this.addIdToStringList(ref warehouseIDListLocal, (int)entityDetail.GetWarehouseID());
                        this.addIdToStringList(ref commodityIDListLocal, entityDetail.CommodityID);
                    });
                }
            }

            warehouseIDList = warehouseIDListLocal;
            commodityIDList = commodityIDListLocal;

            #endregion
        }

        private void addIdToStringList(ref string stringList, int id)
        {
            if ((stringList + ",").IndexOf(id + ",") == -1) stringList = stringList + (stringList != null && stringList.Length > 0 ? "," : "") + id;
        }

    }

}
