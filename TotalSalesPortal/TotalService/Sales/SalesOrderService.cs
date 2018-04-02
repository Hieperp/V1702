using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using AutoMapper;

using TotalBase.Enums;
using TotalModel.Models;
using TotalDTO.Sales;
using TotalCore.Repositories.Sales;
using TotalCore.Repositories.Commons;
using TotalCore.Services.Sales;
using TotalCore.Services.Helpers;
using TotalService.Helpers;

namespace TotalService.Sales
{
    public class SalesOrderService : GenericWithViewDetailService<SalesOrder, SalesOrderDetail, SalesOrderViewDetail, SalesOrderDTO, SalesOrderPrimitiveDTO, SalesOrderDetailDTO>, ISalesOrderService
    {
        private DateTime? checkedDate; //For check over stock
        private string warehouseIDList = "";
        private string commodityIDList = "";

        private readonly IInventoryRepository inventoryRepository;
        private readonly ISalesOrderHelperService salesOrderHelperService;


        public SalesOrderService(ISalesOrderRepository salesOrderRepository, IInventoryRepository inventoryRepository, ISalesOrderHelperService salesOrderHelperService)
            : base(salesOrderRepository, "SalesOrderPostSaveValidate", "SalesOrderSaveRelative", "SalesOrderToggleApproved", "SalesOrderToggleVoid", "SalesOrderToggleVoidDetail", "GetSalesOrderViewDetails")
        {
            this.inventoryRepository = inventoryRepository;
            this.salesOrderHelperService = salesOrderHelperService;
        }

        public override ICollection<SalesOrderViewDetail> GetViewDetails(int salesOrderID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("SalesOrderID", salesOrderID) };
            return this.GetViewDetails(parameters);
        }

        public override bool Save(SalesOrderDTO salesOrderDTO)
        {
            salesOrderDTO.SalesOrderViewDetails.RemoveAll(x => x.Quantity == 0 && x.FreeQuantity == 0);
            return base.Save(salesOrderDTO);
        }

        protected override void UpdateDetail(SalesOrderDTO dto, SalesOrder entity)
        {
            this.salesOrderHelperService.GetWCParameters(dto, null, ref this.checkedDate, ref this.warehouseIDList, ref this.commodityIDList);

            base.UpdateDetail(dto, entity);
        }

        protected override void UndoDetail(SalesOrderDTO dto, SalesOrder entity, bool isDelete)
        {
            this.salesOrderHelperService.GetWCParameters(null, entity, ref this.checkedDate, ref this.warehouseIDList, ref this.commodityIDList);

            base.UndoDetail(dto, entity, isDelete);
        }



        protected override void PostSaveValidate(SalesOrder entity)
        {
            //this.inventoryRepository.CheckOverStock(this.checkedDate, this.warehouseIDList, this.commodityIDList);
            base.PostSaveValidate(entity);
        }


    }


    public class SalesOrderHelperService : HelperService<SalesOrder, SalesOrderDetail, SalesOrderDTO, SalesOrderDetailDTO>, ISalesOrderHelperService
    {
    }


}
