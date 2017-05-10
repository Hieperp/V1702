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
    public class DeliveryAdviceService : GenericWithViewDetailService<DeliveryAdvice, DeliveryAdviceDetail, DeliveryAdviceViewDetail, DeliveryAdviceDTO, DeliveryAdvicePrimitiveDTO, DeliveryAdviceDetailDTO>, IDeliveryAdviceService
    {
        private DateTime? checkedDate; //For check over stock
        private string warehouseIDList = "";
        private string commodityIDList = "";

        private readonly IInventoryRepository inventoryRepository;
        private readonly IDeliveryAdviceHelperService deliveryAdviceHelperService;


        public DeliveryAdviceService(IDeliveryAdviceRepository deliveryAdviceRepository, IInventoryRepository inventoryRepository, IDeliveryAdviceHelperService deliveryAdviceHelperService)
            : base(deliveryAdviceRepository, "DeliveryAdvicePostSaveValidate", "DeliveryAdviceSaveRelative", "DeliveryAdviceToggleApproved", "DeliveryAdviceToggleVoid", "DeliveryAdviceToggleVoidDetail", "GetDeliveryAdviceViewDetails")
        {
            this.inventoryRepository = inventoryRepository;
            this.deliveryAdviceHelperService = deliveryAdviceHelperService;
        }

        public override ICollection<DeliveryAdviceViewDetail> GetViewDetails(int deliveryAdviceID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("DeliveryAdviceID", deliveryAdviceID) };
            return this.GetViewDetails(parameters);
        }

        public override bool Save(DeliveryAdviceDTO deliveryAdviceDTO)
        {
            deliveryAdviceDTO.DeliveryAdviceViewDetails.RemoveAll(x => x.Quantity == 0 && x.FreeQuantity == 0);
            return base.Save(deliveryAdviceDTO);
        }

        protected override void UpdateDetail(DeliveryAdviceDTO dto, DeliveryAdvice entity)
        {
            this.deliveryAdviceHelperService.GetWCParameters(dto, null, ref this.checkedDate, ref this.warehouseIDList, ref this.commodityIDList);

            base.UpdateDetail(dto, entity);
        }

        protected override void UndoDetail(DeliveryAdviceDTO dto, DeliveryAdvice entity, bool isDelete)
        {
            this.deliveryAdviceHelperService.GetWCParameters(null, entity, ref this.checkedDate, ref this.warehouseIDList, ref this.commodityIDList);

            base.UndoDetail(dto, entity, isDelete);
        }



        protected override void PostSaveValidate(DeliveryAdvice entity)
        {
            this.inventoryRepository.CheckOverStock(this.checkedDate, this.warehouseIDList, this.commodityIDList);
            base.PostSaveValidate(entity);
        }


    }


    public class DeliveryAdviceHelperService : HelperService<DeliveryAdvice, DeliveryAdviceDetail, DeliveryAdviceDTO, DeliveryAdviceDetailDTO>, IDeliveryAdviceHelperService
    {
    }


}
