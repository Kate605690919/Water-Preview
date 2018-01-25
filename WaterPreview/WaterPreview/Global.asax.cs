using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
//using WaterPreview.App_Start;
//using WaterPreview.Other;
using WaterPreview.Service.Interface;
using WaterPreview.Service.Service;
using WaterPreview.Util.Infrastructure;

namespace WaterPreview
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());

           // GlobalConfiguration.Configure(WebApiConfig.Register);

            //var dependencyResolver = new NinjectDependencyResolver(new Ninject.StandardKernel());
            //DependencyResolver.SetResolver(dependencyResolver);
            //DependencyResolver.SetResolver(new NinjectDependencyResolver());

            NinjectDependencyResolver dependencyResolver = new NinjectDependencyResolver();
            dependencyResolver.Register<IAccountService, AccountService>();
            dependencyResolver.Register<IAreaDeviceService, AreaDeviceService>();
            dependencyResolver.Register<IAreaService, AreaService>();
            dependencyResolver.Register<IAreaUserService, AreaUserService>();
            dependencyResolver.Register<IFlowService, FlowService>();
            dependencyResolver.Register<IFlowDayService, FlowDayService>();
            dependencyResolver.Register<IFlowHourService, FlowHourService>();
            dependencyResolver.Register<IFlowMeterService, FlowMeterService>();
            dependencyResolver.Register<IFlowMeterStatusService, FlowMeterStatusService>();
            dependencyResolver.Register<IFlowMonthService, FlowMonthService>();
            dependencyResolver.Register<IPressureService, PressureService>();
            dependencyResolver.Register<IPressureDayService, PressureDayService>();
            dependencyResolver.Register<IPressureHourService, PressureHourService>();
            dependencyResolver.Register<IPressureMeterService, PressureMeterService>();
            dependencyResolver.Register<IPressureMeterStatusService, PressureMeterStatusService>();
            dependencyResolver.Register<IPressureMonthService, PressureMonthService>();
            dependencyResolver.Register<IQualityMeterService, QualityMeterService>();
            dependencyResolver.Register<IQualityMeterStatusService, QualityMeterStatusService>();
            dependencyResolver.Register<IExceptionService, ExceptionService>();

            GlobalConfiguration.Configuration.DependencyResolver = dependencyResolver;
        }


    }
}
