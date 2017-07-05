using System;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;


using TotalModel.Models;

using TotalDTO.Sales;
using TotalDTO.Inventories;
using TotalDTO.Commons;
using TotalDTO.Accounts;

using TotalPortal.Areas.Sales.ViewModels;
using TotalPortal.Areas.Inventories.ViewModels;
using TotalPortal.Areas.Accounts.ViewModels;
using TotalPortal.Areas.Commons.ViewModels;


[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(TotalPortal.App_Start.AutoMapperConfig), "SetupMappings")]
namespace TotalPortal.App_Start
{
    public static class AutoMapperConfig
    {
        public static void SetupMappings()
        {
        ////////https://github.com/AutoMapper/AutoMapper/wiki/Static-and-Instance-API

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<SalesOrder, SalesOrderViewModel>();
                cfg.CreateMap<SalesOrder, SalesOrderDTO>();
                cfg.CreateMap<SalesOrderPrimitiveDTO, SalesOrder>();
                cfg.CreateMap<SalesOrderViewDetail, SalesOrderDetailDTO>();
                cfg.CreateMap<SalesOrderDetailDTO, SalesOrderDetail>();

                cfg.CreateMap<SalesOrder, SalesOrderBoxDTO>();


                cfg.CreateMap<DeliveryAdvice, DeliveryAdviceViewModel>();
                cfg.CreateMap<DeliveryAdvice, DeliveryAdviceDTO>();
                cfg.CreateMap<DeliveryAdvicePrimitiveDTO, DeliveryAdvice>();
                cfg.CreateMap<DeliveryAdviceViewDetail, DeliveryAdviceDetailDTO>();
                cfg.CreateMap<DeliveryAdviceDetailDTO, DeliveryAdviceDetail>();

                cfg.CreateMap<DeliveryAdvice, DeliveryAdviceBoxDTO>();


                cfg.CreateMap<SalesReturn, SalesReturnViewModel>();
                cfg.CreateMap<SalesReturn, SalesReturnDTO>();
                cfg.CreateMap<SalesReturnPrimitiveDTO, SalesReturn>();
                cfg.CreateMap<SalesReturnViewDetail, SalesReturnDetailDTO>();
                cfg.CreateMap<SalesReturnDetailDTO, SalesReturnDetail>();

                cfg.CreateMap<SalesReturn, SalesReturnBoxDTO>();                

                cfg.CreateMap<GoodsIssue, GoodsIssueViewModel>();
                cfg.CreateMap<GoodsIssue, GoodsIssueDTO>();
                cfg.CreateMap<GoodsIssuePrimitiveDTO, GoodsIssue>();
                cfg.CreateMap<GoodsIssueViewDetail, GoodsIssueDetailDTO>();
                cfg.CreateMap<GoodsIssueDetailDTO, GoodsIssueDetail>();

                cfg.CreateMap<GoodsIssue, GoodsIssueBoxDTO>();

                cfg.CreateMap<HandlingUnit, HandlingUnitViewModel>();
                cfg.CreateMap<HandlingUnit, HandlingUnitDTO>();
                cfg.CreateMap<HandlingUnitPrimitiveDTO, HandlingUnit>();
                cfg.CreateMap<HandlingUnitViewDetail, HandlingUnitDetailDTO>();
                cfg.CreateMap<HandlingUnitDetailDTO, HandlingUnitDetail>();

                cfg.CreateMap<GoodsDelivery, GoodsDeliveryViewModel>();
                cfg.CreateMap<GoodsDelivery, GoodsDeliveryDTO>();
                cfg.CreateMap<GoodsDeliveryPrimitiveDTO, GoodsDelivery>();
                cfg.CreateMap<GoodsDeliveryViewDetail, GoodsDeliveryDetailDTO>();
                cfg.CreateMap<GoodsDeliveryDetailDTO, GoodsDeliveryDetail>();

                cfg.CreateMap<AccountInvoice, AccountInvoiceViewModel>();
                cfg.CreateMap<AccountInvoice, AccountInvoiceDTO>();
                cfg.CreateMap<AccountInvoicePrimitiveDTO, AccountInvoice>();
                cfg.CreateMap<AccountInvoiceViewDetail, AccountInvoiceDetailDTO>();
                cfg.CreateMap<AccountInvoiceDetailDTO, AccountInvoiceDetail>();

                cfg.CreateMap<Receipt, ReceiptViewModel>();
                cfg.CreateMap<Receipt, ReceiptDTO>();
                cfg.CreateMap<ReceiptPrimitiveDTO, Receipt>();
                cfg.CreateMap<ReceiptViewDetail, ReceiptDetailDTO>();
                cfg.CreateMap<ReceiptDetailDTO, ReceiptDetail>();

                cfg.CreateMap<Receipt, ReceiptBoxDTO>();                
                
                cfg.CreateMap<CreditNote, CreditNoteViewModel>();
                cfg.CreateMap<CreditNote, CreditNoteDTO>();
                cfg.CreateMap<CreditNotePrimitiveDTO, CreditNote>();
                cfg.CreateMap<CreditNoteDetail, CreditNoteDetailDTO>();
                cfg.CreateMap<CreditNoteDetailDTO, CreditNoteDetail>();

                cfg.CreateMap<CreditNote, CreditNoteBoxDTO>();

                cfg.CreateMap<Customer, CustomerViewModel>();
                cfg.CreateMap<Customer, CustomerDTO>();
                cfg.CreateMap<Customer, CustomerBaseDTO>();
                cfg.CreateMap<CustomerPrimitiveDTO, Customer>();

                cfg.CreateMap<Customer, CustomerBaseDTO>();

                cfg.CreateMap<Employee, EmployeeViewModel>();
                cfg.CreateMap<Employee, EmployeeDTO>();
                cfg.CreateMap<Employee, EmployeeBaseDTO>();
                cfg.CreateMap<EmployeePrimitiveDTO, Employee>();

                cfg.CreateMap<Employee, EmployeeBaseDTO>();

                cfg.CreateMap<CommodityPrice, CommodityPriceViewModel>();
                cfg.CreateMap<CommodityPrice, CommodityPriceDTO>();
                cfg.CreateMap<CommodityPricePrimitiveDTO, CommodityPrice>();



                cfg.CreateMap<Promotion, PromotionViewModel>();
                cfg.CreateMap<Promotion, PromotionDTO>();
                cfg.CreateMap<PromotionPrimitiveDTO, Promotion>();
                cfg.CreateMap<PromotionCommodityCodePart, PromotionCommodityCodePartDTO>();
                cfg.CreateMap<PromotionCommodityCodePartDTO, PromotionCommodityCodePart>();


                cfg.CreateMap<Promotion, PromotionBaseDTO>();

                cfg.CreateMap<Warehouse, WarehouseBaseDTO>();
                cfg.CreateMap<VoidType, VoidTypeBaseDTO>();

                //cfg.CreateMap<Module, ModuleViewModel>();
                //cfg.CreateMap<ModuleDetail, ModuleDetailViewModel>();
            });



            //Mapper.CreateMap<Module, ModuleViewModel>();
            //Mapper.CreateMap<ModuleDetail, ModuleDetailViewModel>();
        }
    }
}