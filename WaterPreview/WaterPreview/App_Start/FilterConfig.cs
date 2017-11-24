using System.Web;
using System.Web.Mvc;
using WaterPreview.Other.Attribute;

namespace WaterPreview
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new LoginAttribute());

        }
    }
}
