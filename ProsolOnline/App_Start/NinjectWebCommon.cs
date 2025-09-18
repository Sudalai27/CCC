[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(ProsolOnline.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(ProsolOnline.App_Start.NinjectWebCommon), "Stop")]

namespace ProsolOnline.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Prosol.Core.Interface;
    using Prosol.Data;
    using Prosol.Core;

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
            //kernel.Bind<IDBConStr>().To<DBConStr>().InRequestScope();
            kernel.Bind(typeof(IRepository<>)).To(typeof(BaseRepository<>)).InRequestScope();
            kernel.Bind<INounModifier>().To<NounModifierService>();
            kernel.Bind<IUserCreate>().To<UserCreateService>();
            kernel.Bind<IUserAccess>().To<UserAccessService>();
            kernel.Bind<ICharateristics>().To<CharateristicsService>();
            kernel.Bind<I_ItemRequest>().To<ItemRequestService>();
            kernel.Bind<I_login>().To<loginservice>();
            kernel.Bind<I_pwRecovery>().To<passwordrecovery_service>();
            kernel.Bind<I_Import>().To<ImportService>();           
            kernel.Bind<IGeneralSettings>().To<GeneralSettingService>();
            kernel.Bind<ICatalogue>().To<CatalogueService>();
            kernel.Bind<ISequence>().To<SequenceService>();
            kernel.Bind<IItemRequestLog>().To<ItemRequestLogService>();
            kernel.Bind<IItemApprove>().To<ItemApproveService>();          
            kernel.Bind<I_Assignwork>().To<AssignWorkService>();
            kernel.Bind<I_Report>().To<ReportService>();
            kernel.Bind<IValuestandardization>().To<ValuestandardisationService>();
            kernel.Bind<ISearchByReference>().To<SearchByReferenceService>();
            kernel.Bind<IMaster>().To<MasterService>();
            kernel.Bind<ISearch>().To<SearchService>();
            kernel.Bind<IDashboard>().To<DashboardService>();
            kernel.Bind<IServiceMaster>().To<ServiceMasterService>();
            kernel.Bind<IServiceCreation>().To<ServiceCreationService>();
            kernel.Bind<IServiceSearch>().To<ServiceSearchService>();
            kernel.Bind<IServiceReport>().To<ServiceReportService>();
            kernel.Bind<ILogic>().To<LogicService>();
            kernel.Bind<IEmailSettings>().To<EmailSettingService>();
            kernel.Bind<I_Bom>().To<BomService>();
            kernel.Bind<IBusinessPartner>().To<BusinessPartnerService>();
            kernel.Bind<IEquipment>().To<EquipmentService>();
            kernel.Bind<I_Asset>().To<AssetService>();
            //kernel.Bind<IEmailservice>().To<emailservice>();
        }
    }        
    
}
