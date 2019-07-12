using System.Web;
using System.Web.Optimization;

namespace RCT.Web
{
    public class BundleConfig
    {
        // 如需「搭配」的詳細資訊，請瀏覽 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            string themeName = System.Configuration.ConfigurationManager.AppSettings["jQueryTheme"];
            string themeFolder = "~/Content/themes/" + themeName;

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.validate.min.js",
                        "~/Scripts/jquery.validate.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-1.12.1.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryjqgrid").Include(
                        "~/Scripts/i18n/grid.locale-tw*",
                        "~/Scripts/jquery.jqGrid*"));

            // 使用開發版本的 Modernizr 進行開發並學習。然後，當您
            // 準備好實際執行時，請使用 http://modernizr.com 上的建置工具，只選擇您需要的測試。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-dialog.js",
                      "~/Scripts/bootstrap-treeview.js",
                      "~/Scripts/fubon.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/Customer").Include(
                      "~/Scripts/customerUtility.js",
                      "~/Scripts/customJqgrid.js",
                      "~/Scripts/toastr.js",
                      "~/Scripts/verification.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-dialog.css",
                      "~/Content/bootstrap-treeview.css",
                      "~/Content/fubon.css",
                      "~/Content/jquery.jqGrid/ui.jqgrid.css",
                      themeFolder + "/jquery.ui.theme.css",
                      themeFolder + "/minified/jquery-ui.min.css",
                      "~/Content/site.css",
                      "~/Content/font-awesome.css",
                      "~/Content/toastr.css",
                      "~/Content/customerUtility.css"));
        }
    }
}
