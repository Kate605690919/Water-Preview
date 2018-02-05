using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using System.Web.Mvc;
using System.Web.Http.Dependencies;

namespace WaterPreview.Other
{
    public class NinjectDependencyResolver:System.Web.Http.Dependencies.IDependencyResolver
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