﻿using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using WaterPreview.Service.Interface;
using WaterPreview.Service.Service;

namespace WaterPreview.Other
{
    public class NinjectDependencyResolver:NinjectDependencyScope,IDependencyResolver
    {
        [Ninject.Inject]
        private IKernel kernel;
        public NinjectDependencyResolver()
        {

        }

        public NinjectDependencyResolver(IKernel kernel)
        {
            this.kernel = kernel;
            this.kernel.Settings.InjectNonPublic = true;
            this.AddBinds();
        }

        private void AddBinds()
        {

            this.kernel.Bind<IAccountService>().To<AccountService>();
            this.kernel.Bind<IAreaDeviceService>().To<AreaDeviceService>();
            this.kernel.Bind<IAreaService>().To<AreaService>();
            this.kernel.Bind<IAreaUserService>().To<AreaUserService>();
            this.kernel.Bind<IFlowService>().To<FlowService>();
            this.kernel.Bind<IFlowDayService>().To<FlowDayService>();
            this.kernel.Bind<IFlowHourService>().To<FlowHourService>();
            this.kernel.Bind<IFlowMeterService>().To<FlowMeterService>();
            this.kernel.Bind<IFlowMeterStatusService>().To<FlowMeterStatusService>();
            this.kernel.Bind<IFlowMonthService>().To<FlowMonthService>();
            this.kernel.Bind<IPressureService>().To<PressureService>();
            this.kernel.Bind<IPressureDayService>().To<PressureDayService>();
            this.kernel.Bind<IPressureHourService>().To<PressureHourService>();
            this.kernel.Bind<IPressureMeterService>().To<PressureMeterService>();
            this.kernel.Bind<IPressureMeterStatusService>().To<PressureMeterStatusService>();
            this.kernel.Bind<IPressureMonthService>().To<PressureMonthService>();
            this.kernel.Bind<IQualityMeterService>().To<QualityMeterService>();
            this.kernel.Bind<IQualityMeterStatusService>().To<QualityMeterStatusService>();
            this.kernel.Bind<IExceptionService>().To<ExceptionService>();
        }

        public IDependencyScope BeginScope()
        {
            return new NinjectDependencyScope(this.kernel.BeginBlock());
        }
    }
}