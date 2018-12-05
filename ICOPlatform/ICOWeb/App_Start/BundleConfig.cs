using System.Web;
using System.Web.Optimization;

namespace ICOWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region Core

            // TEMPLATE Global Stylesheets 
            bundles.Add(new StyleBundle("~/template/css").Include(
                      "~/Content/template/layout_1/LTR/default/assets/css/icons/icomoon/styles.css",
                      "~/Content/template/layout_1/LTR/default/assets/css/bootstrap.css",
                      "~/Content/template/layout_1/LTR/default/assets/css/core.css",
                      "~/Content/template/layout_1/LTR/default/assets/css/components.css",
                      "~/Content/template/layout_1/LTR/default/assets/css/colors.css"
                      ));


            // Global Stylesheets 
            bundles.Add(new StyleBundle("~/global/css").Include(
                        "~/Content/plugins/notyfy/jquery.notyfy.css",
                        "~/Content/plugins/notyfy/themes/default.css",
                        "~/Content/plugins/nadyTree/themes/1.css",
                        "~/Content/plugins/nadyTree/nadytree.css",
                        "~/Content/plugins/angular-busy-master/dist/angular-busy.min.css",
                        "~/Content/global/css/custom-style.css"
                      ));

            // TEMPLATE Global Core Scripts 
            bundles.Add(new ScriptBundle("~/template/coreJs").Include(
                        "~/Content/template/layout_1/LTR/default/assets/js/plugins/loaders/pace.min.js",
                        //"~/Content/template/layout_1/LTR/default/assets/js/core/libraries/jquery.min.js",
                        "~/Content/plugins/jquery/jquery-3.1.1.min.js",
                        "~/Content/template/layout_1/LTR/default/assets/js/core/libraries/bootstrap.min.js",
                        "~/Content/template/layout_1/LTR/default/assets/js/plugins/loaders/blockui.min.js"
                        ));

            // TEMPLATE Global Theme Scripts 
            bundles.Add(new ScriptBundle("~/template/coreThemeJs").Include(
                        "~/Content/template/layout_1/LTR/default/assets/js/core/libraries/jquery_ui/core.min.js",
                        "~/Content/template/layout_1/LTR/default/assets/js/plugins/forms/styling/uniform.min.js",
                        //"~/Content/template/layout_1/LTR/default/assets/js/plugins/forms/styling/switchery.min.js",
                        "~/Content/template/layout_1/LTR/default/assets/js/plugins/forms/selects/select2.min.js",
                        "~/Content/template/layout_1/LTR/default/assets/js/plugins/forms/selects/bootstrap_multiselect.js",
                        "~/Content/template/layout_1/LTR/default/assets/js/plugins/forms/selects/selectboxit.min.js",
                        "~/Content/template/layout_1/LTR/default/assets/js/plugins/forms/selects/bootstrap_select.min.js",

                        "~/Content/template/layout_1/LTR/default/assets/js/plugins/notifications/pnotify.min.js",
                        "~/Content/template/layout_1/LTR/default/assets/js/plugins/notifications/noty.min.js",
                        "~/Content/template/layout_1/LTR/default/assets/js/plugins/notifications/jgrowl.min.js",

                        //"~/Content/template/layout_1/LTR/default/assets/js/plugins/visualization/d3/d3.min.js",
                        //"~/Content/template/layout_1/LTR/default/assets/js/plugins/visualization/d3/d3_tooltip.js",
                        //"~/Content/template/layout_1/LTR/default/assets/js/plugins/ui/moment/moment.min.js",
                        //"~/Content/template/layout_1/LTR/default/assets/js/plugins/pickers/daterangepicker.js",

                        "~/Content/template/layout_1/LTR/default/assets/js/core/app.js",
                        "~/Content/template/layout_1/LTR/default/assets/js/pages/components_popups.js"

                        //"~/Content/template/layout_1/LTR/default/assets/js/pages/colors_primary.js"

                        ));

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



            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));
            #endregion

            #region Front
            // btcwallet 
            bundles.Add(new ScriptBundle("~/front/js/btcwallet").Include(
                "~/Content/app/front/BTCWallet/index.js"
               ));
            // bonus 
            bundles.Add(new ScriptBundle("~/front/js/bonus").Include(
                "~/Content/app/front/bonus/index.js"
               ));
            // gethelp 
            bundles.Add(new ScriptBundle("~/front/js/gethelp").Include(
                "~/Content/app/front/gethelp/index.js"
               ));
            // me 
            bundles.Add(new ScriptBundle("~/front/js/me/dashboard").Include(
                "~/Content/app/front/me/dashboard.js"
               ));
            bundles.Add(new ScriptBundle("~/front/js/me/profile").Include(
               "~/Content/app/front/me/profile.js"
              ));
            bundles.Add(new ScriptBundle("~/front/js/me/setting").Include(
            "~/Content/app/front/me/setting.js"
           ));

            // member
            bundles.Add(new ScriptBundle("~/front/js/member/placement").Include(
                "~/Content/app/front/member/placement.js"
               ));
            bundles.Add(new ScriptBundle("~/front/js/member/tree").Include(
            "~/Content/app/front/member/tree.js"
           ));
            // ProvideHelp
            bundles.Add(new ScriptBundle("~/front/js/ProvideHelp/index").Include(
                "~/Content/app/front/ProvideHelp/index.js"
               ));
            bundles.Add(new ScriptBundle("~/front/js/ProvideHelp/transaction").Include(
            "~/Content/app/front/ProvideHelp/transaction.js"
           ));

            // ProvideHelpTransaction
            bundles.Add(new ScriptBundle("~/front/js/ProvideHelpTransaction/index").Include(
                "~/Content/app/front/ProvideHelpTransaction/index.js"
               ));

            // Token
            bundles.Add(new ScriptBundle("~/front/js/Token/history").Include(
                "~/Content/app/front/Token/history.js"
               ));
            bundles.Add(new ScriptBundle("~/front/js/Token/transfer").Include(
            "~/Content/app/front/Token/transfer.js"
           ));

            // User
            bundles.Add(new ScriptBundle("~/front/js/User/login").Include(
                "~/Content/app/front/User/login.js"
               ));
            // bundles.Add(new ScriptBundle("~/front/js/User/register").Include(
            // "~/Content/app/front/User/register.js"
            //));

            #endregion
        }
    }
}
