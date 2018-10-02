using System.Web;
using System.Web.Optimization;

namespace Fakulteti
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                         "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/DataTables/jquery.dataTables.js",
                      "~/Scripts/DataTables/dataTables.responsive.min.js",
                      "~/Scripts/bootstrap-datepicker.min.js",
                      "~/Content/MultiSelect/select2.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                   "~/Content/bootstrap.css",
                   "~/css/font-awesome.min.css",
                   "~/Content/DataTables/css/jquery.dataTables.min.css",
                   "~/Content/DataTables/css/responsive.dataTables.min.css",
                   "~/Content/bootstrap-datepicker.min.cs",
                   "~/Content/MultiSelect/jquery-ui-1.10.4.custom.css",
                   "~/Content/MultiSelect/select2.css",
                   "~/Content/site.css"));
        }
    }
}
