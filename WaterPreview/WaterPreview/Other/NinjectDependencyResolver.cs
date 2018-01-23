using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using WaterPreview.Service.Interface;
using WaterPreview.Service.Service;
using Ninject;

namespace WaterPreview.Other
{
    public class NinjectDependencyResolver:IDependencyResolver
    //public class NinjectDependencyResolver:NinjectDependencyScope,IDependencyResolver
    {

        private IKernel kernel {get; set;}

        private List<IDisposable> disposableServices = new List<IDisposable>();

        public NinjectDependencyResolver(NinjectDependencyResolver parent)
        {
            this.kernel = parent.kernel;
        }

        public NinjectDependencyResolver()
        {
            this.kernel = new Ninject.StandardKernel();
            this.kernel.Settings.InjectNonPublic = true;
            //this.AddBinds();
        }


        public void Register<TFrom, TTo>() where TTo : TFrom
        {
            this.kernel.Bind<TFrom>().To<TTo>();
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
            return new NinjectDependencyResolver(this);
        }

        public object GetService(Type serviceType)
        {
            return this.kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            foreach (var service in this.kernel.GetAll(serviceType))
            {
                this.AddDisposableService(service);
                yield return service;
            }
            //return this.kernel.GetAll(serviceType);
        }

        public void Dispose()
        {
            foreach(IDisposable disposable in disposableServices){
                disposable.Dispose();
            }
        }

        private void AddDisposableService(object service)
        {
            IDisposable disposable = service as IDisposable;
            if (null != disposable && !disposableServices.Contains(disposable))
            {
                disposableServices.Add(disposable);
            }
        }
    }
}