using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using WaterPreview.Service.Interface;
using WaterPreview.Service.Service;

namespace WaterPreview.Util.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;

        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)ninjectKernel.Get(controllerType);
        }

        private void AddBindings()
        {
            // todo:后面再来添加绑定

            ninjectKernel.Bind<IAccountService>().To<AccountService>();
            ninjectKernel.Bind<IAreaDeviceService>().To<AreaDeviceService>();
            ninjectKernel.Bind<IAreaService>().To<AreaService>();
            ninjectKernel.Bind<IAreaUserService>().To<AreaUserService>();
            ninjectKernel.Bind<IFlowDayService>().To<FlowDayService>();
            ninjectKernel.Bind<IFlowMeterService>().To<FlowMeterService>();
            ninjectKernel.Bind<IFlowMeterStatusService>().To<FlowMeterStatusService>();
            ninjectKernel.Bind<IFlowMonthService>().To<FlowMonthService>();
            ninjectKernel.Bind<IPressureHourService>().To<PressureHourService>();
            ninjectKernel.Bind<IPressureMeterService>().To<PressureMeterService>();
            ninjectKernel.Bind<IPressureMeterStatusService>().To<PressureMeterStatusService>();
            ninjectKernel.Bind<IPressureMonthService>().To<PressureMonthService>();
            ninjectKernel.Bind<IQualityMeterService>().To<QualityMeterService>();
            ninjectKernel.Bind<IQualityMeterStatusService>().To<IQualityMeterStatusService>();



        }
    }
}
