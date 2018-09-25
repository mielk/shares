using System.Web.Optimization;

namespace Stock.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/common/external/jquery/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/common/external/jquery/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/common/external/jquery/jquery.unobtrusive*",
                        "~/Scripts/common/external/jquery/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/common/external/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/global").Include(
                        "~/Scripts/common/external/notify.js",
                        "~/Scripts/common/mielk.js",
                        "~/Scripts/common/external/jquery/jquery.sizes.js",
                        "~/Scripts/common/util.js",
                        "~/Scripts/common/tree.js",
                        "~/Scripts/common/external/spin.js",
                        "~/Scripts/common/external/select2.js",
                        "~/Scripts/common/dropdown.js"));
                        //"~/Scripts/common/external/css-element-queries/ElementQueries.js",
                        //"~/Scripts/common/external/css-element-queries/ResizeSensor.js",
                        

            bundles.Add(new ScriptBundle("~/bundles/stock").Include(
                        "~/Scripts/STOCK2/StockCommon.js",
                        "~/Scripts/STOCK2/Quotation.js",
                        "~/Scripts/STOCK2/Company.js",
                        "~/Scripts/STOCK2/QuotationsAnalyzer.js",
                        "~/Scripts/STOCK2/LabelFactory.js",
                        "~/Scripts/STOCK2/main.js"));

            bundles.Add(new ScriptBundle("~/bundles/charts").Include(
                        "~/Scripts/common/external/Raphael.js",
                        "~/Scripts/charts/Chart.js",
                        "~/Scripts/charts/ChartsContainer.js",
                        "~/Scripts/charts/ChartController.js",
                        "~/Scripts/charts/SvgPanel.js"));

            bundles.Add(new ScriptBundle("~/bundles/analysis").Include(
                        "~/Scripts/analysis/AnalysisController.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/common/select2.css",
                        "~/Content/stock/analysis.css",
                        "~/Content/stock/simulation.css",
                        "~/Content/stock/charts.css",
                        "~/Content/stock/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/common/normalize.css",
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css",
                        "~/Content/bootstrap/bootstrap-theme.css",
                        "~/Content/bootstrap/bootstrap.css"));
        }
    }
}