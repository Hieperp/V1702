[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(TotalPortal.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(TotalPortal.App_Start.NinjectWebCommon), "Stop")]

namespace TotalPortal.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;


    using TotalModel.Models;

    using TotalCore.Repositories;
    using TotalCore.Repositories.Commons;
    using TotalCore.Repositories.Sales;
    using TotalCore.Repositories.Inventories;
    using TotalCore.Repositories.Accounts;
    using TotalCore.Repositories.Sessions;

    using TotalCore.Services.Sales;
    using TotalCore.Services.Inventories;
    using TotalCore.Services.Accounts;


    using TotalDAL.Repositories;
    using TotalDAL.Repositories.Commons;
    using TotalDAL.Repositories.Sales;
    using TotalDAL.Repositories.Inventories;
    using TotalDAL.Repositories.Accounts;
    using TotalDAL.Repositories.Sessions;


    using TotalService.Sales;
    using TotalService.Inventories;
    using TotalService.Accounts;


    using TotalPortal.Areas.Sales.Builders;
    using TotalPortal.Areas.Inventories.Builders;
    using TotalPortal.Areas.Commons.Builders;
    using TotalPortal.Areas.Accounts.Builders;



    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();


                kernel.Bind<TotalSalesPortalEntities>().ToSelf().InRequestScope();

                kernel.Bind<IBaseRepository>().To<BaseRepository>();

                kernel.Bind<IModuleRepository>().To<ModuleRepository>();
                kernel.Bind<IModuleDetailRepository>().To<ModuleDetailRepository>();


                kernel.Bind<IDeliveryAdviceService>().To<DeliveryAdviceService>();
                kernel.Bind<IDeliveryAdviceRepository>().To<DeliveryAdviceRepository>();
                kernel.Bind<IDeliveryAdviceAPIRepository>().To<DeliveryAdviceAPIRepository>();
                kernel.Bind<IDeliveryAdviceHelperService>().To<DeliveryAdviceHelperService>();
                kernel.Bind<IDeliveryAdviceViewModelSelectListBuilder>().To<DeliveryAdviceViewModelSelectListBuilder>();


                kernel.Bind<IGoodsIssueService>().To<GoodsIssueService>();
                kernel.Bind<IGoodsIssueRepository>().To<GoodsIssueRepository>();
                kernel.Bind<IGoodsIssueAPIRepository>().To<GoodsIssueAPIRepository>();
                kernel.Bind<IGoodsIssueViewModelSelectListBuilder>().To<GoodsIssueViewModelSelectListBuilder>();

                kernel.Bind<IHandlingUnitService>().To<HandlingUnitService>();
                kernel.Bind<IHandlingUnitRepository>().To<HandlingUnitRepository>();
                kernel.Bind<IHandlingUnitAPIRepository>().To<HandlingUnitAPIRepository>();
                kernel.Bind<IHandlingUnitViewModelSelectListBuilder>().To<HandlingUnitViewModelSelectListBuilder>();

                kernel.Bind<IGoodsDeliveryService>().To<GoodsDeliveryService>();
                kernel.Bind<IGoodsDeliveryRepository>().To<GoodsDeliveryRepository>();
                kernel.Bind<IGoodsDeliveryAPIRepository>().To<GoodsDeliveryAPIRepository>();
                kernel.Bind<IGoodsDeliveryViewModelSelectListBuilder>().To<GoodsDeliveryViewModelSelectListBuilder>();

                kernel.Bind<IAccountInvoiceService>().To<AccountInvoiceService>();
                kernel.Bind<IAccountInvoiceRepository>().To<AccountInvoiceRepository>();
                kernel.Bind<IAccountInvoiceAPIRepository>().To<AccountInvoiceAPIRepository>();
                kernel.Bind<IAccountInvoiceViewModelSelectListBuilder>().To<AccountInvoiceViewModelSelectListBuilder>();

                kernel.Bind<IReceiptService>().To<ReceiptService>();
                kernel.Bind<IReceiptRepository>().To<ReceiptRepository>();
                kernel.Bind<IReceiptAPIRepository>().To<ReceiptAPIRepository>();
                kernel.Bind<IReceiptViewModelSelectListBuilder>().To<ReceiptViewModelSelectListBuilder>();


                kernel.Bind<IInventoryRepository>().To<InventoryRepository>();

                kernel.Bind<IAspNetUserSelectListBuilder>().To<AspNetUserSelectListBuilder>();
                kernel.Bind<IPaymentTermSelectListBuilder>().To<PaymentTermSelectListBuilder>();
                kernel.Bind<IPackingMaterialSelectListBuilder>().To<PackingMaterialSelectListBuilder>();
                kernel.Bind<IVehicleSelectListBuilder>().To<VehicleSelectListBuilder>();



                kernel.Bind<IAspNetUserRepository>().To<AspNetUserRepository>();
                kernel.Bind<ICommodityRepository>().To<CommodityRepository>();
                kernel.Bind<ICustomerRepository>().To<CustomerRepository>();
                kernel.Bind<IEmployeeRepository>().To<EmployeeRepository>();
                kernel.Bind<IVoidTypeRepository>().To<VoidTypeRepository>();
                kernel.Bind<IPromotionRepository>().To<PromotionRepository>();
                kernel.Bind<IPaymentTermRepository>().To<PaymentTermRepository>();
                kernel.Bind<IPackingMaterialRepository>().To<PackingMaterialRepository>();
                kernel.Bind<IVehicleRepository>().To<VehicleRepository>();


                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
        }
    }
}
