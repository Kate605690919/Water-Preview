using System.Web;
using System.Web.Optimization;

namespace WaterPreview
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/sources/jquery.min.js",
                        "~/Scripts/sources/datatables.min.js",
                        "~/Scripts/sources/jquery.duallistbox.js",
                        "~/Scripts/sources/jstree.min.js"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/sources/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/sources/bootstrap.min.js",
                      "~/Scripts/sources/respond.min.js",
                      "~/Scripts/sources/inspinia/inspinia.js",
                      "~/Scripts/sources/inspinia/jquery.metisMenu.js",
                      "~/Scripts/sources/inspinia/jquery.slimscroll.min.js",
                      "~/Scripts/sources/inspinia/pace.min.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/react").Include(
                      "~/Scripts/sources/react/react.min.js",
                      "~/Scripts/sources/react/react-dom.min.js",
                      "~/Scripts/sources/react/redux.min.js",
                      "~/Scripts/sources/react/react-redux.min.js",
                      "~/Scripts/sources/react/babel.min.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/datepicker3.css",
                      "~/Content/jquery.duallistbox.min.css",
                      "~/Content/datatables.min.css",
                      "~/Content/site.css",
                      "~/Content/inspinia/animate.css",
                      "~/Content/inspinia/style.css",
                      "~/Content/jstree/style.min.css",
                      "~/Content/font-awesome.css",
                      "~/Content/myStyle.css"
                      ));
        }
    }
}
