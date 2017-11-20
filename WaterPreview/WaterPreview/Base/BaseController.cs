using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WaterPreview.Base
{
    public class BaseController : Controller
    {
  

        //要清除的对象 可为*Service
        protected IList<IDisposable> DisposableObjects { get; private set; }

        public BaseController()
        {
            this.DisposableObjects = new List<IDisposable>();
        }

        public void AddDisposableObject(object obj)
        {
            IDisposable disposable = obj as IDisposable;
            if (disposable != null)
            {
                this.DisposableObjects.Add(disposable);
            }
        }

    } 
}