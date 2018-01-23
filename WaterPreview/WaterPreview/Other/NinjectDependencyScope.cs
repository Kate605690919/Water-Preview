using Ninject.Activation;
using Ninject.Parameters;
using Ninject.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;

namespace WaterPreview.Other
{
    public class NinjectDependencyScope:IDependencyScope
    {
        protected IResolutionRoot resolutionRoot;
        //public NinjectDependencyScope()
        //{

        //}
        public NinjectDependencyScope(IResolutionRoot resolutionRoot)
        {
            this.resolutionRoot = resolutionRoot;
        }
        public object GetService(Type serviceType)
        {
            if (this.resolutionRoot == null)
            {
                throw new ObjectDisposedException("this", "This scope has already been disposed");
            }
            return resolutionRoot.Resolve(this.CreateRequest(serviceType)).SingleOrDefault();
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (this.resolutionRoot == null)
            {
                throw new ObjectDisposedException("this", "This scope has already been disposed");
            }
            return this.resolutionRoot.Resolve(this.CreateRequest(serviceType));
        }
        private IRequest CreateRequest(Type serviceType)
        {
            return resolutionRoot.CreateRequest(serviceType, null, new Parameter[0], true, true);
        }
        public void Dispose()
        {
            var disposable = this.resolutionRoot as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
            this.resolutionRoot = null;
        }
    }
}