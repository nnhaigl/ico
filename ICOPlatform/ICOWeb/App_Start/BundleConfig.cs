using System.Web;
using System.Web.Optimization;

namespace ICOWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            // ANGULARJS Plugin
            bundles.Add(new ScriptBundle("~/core/angularjs").Include(
                   "~/content/plugins/angular-1.5.7/angular.js",
                   "~/content/plugins/angular-1.5.7/angular-messages.min.js",
                   "~/content/plugins/angular-1.5.7/angular-animate.min.js",
                   "~/content/plugins/angular-1.5.7/angular-sanitize.min.js",
                   "~/content/plugins/angular-1.5.7/angular-touch.min.js",
                   "~/content/plugins/angular-1.5.7/plugins/ocLazyLoad.min.js",
                   "~/content/plugins/angular-1.5.7/plugins/ui-bootstrap-tpls-1.3.2.min.js",
                   "~/content/plugins/angular-1.5.7/angular-route.min.js",
                   "~/content/plugins/angular-1.5.7/plugins/angular-ui-router.min.js"
                   ));

            // PLUGINS 
            bundles.Add(new ScriptBundle("~/global/js").Include(
                "~/Content/global/js/GlobalConstants.js",
                "~/Content/global/js/GlobalUtils.js",
                "~/Content/global/js/GlobalConfig.js",
                "~/Content/global/js/angularInit.js",
                "~/Content/global/js/angularSessionStorage.js",
                "~/Content/global/js/angularLocalStorage.js",
                "~/Content/global/js/angularInterceptorCfg.js",
                "~/Content/global/js/angularDirectives.js"
               ));


            // PLUGINS 
            bundles.Add(new ScriptBundle("~/global/plugins").Include(
                 "~/Content/plugins/nadyTree/js/js_jquery-ui-1.10.4.custom.min.js",
                 "~/Content/plugins/nadyTree/nadyTree.js",
                 "~/Content/plugins/fast-sha256/sha256.min.js",
                 "~/Content/plugins/fast-sha256/nacl-util.min.js",
                 "~/Content/plugins/fast-sha256/encoding-indexes.js",
                 "~/Content/plugins/fast-sha256/encoding.js",
                 "~/Content/plugins/notyfy/jquery.notyfy.js",
                 "~/Content/plugins/angular-busy-master/dist/angular-busy.min.js",
                 "~/content/plugins/underscore/underscore-min.js",
                 "~/Content/plugins/qrcode/jquery.qrcode.0.12.0.js",
                 "~/Content/plugins/ngClipboard/ngClipboard.js",
                 "~/Content/plugins/bootbox/bootbox.min.js"
               ));

            // User
            bundles.Add(new ScriptBundle("~/front/js/User/login").Include(
                "~/Content/app/front/User/login.js"
               ));
        }
    }
}
