using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;

namespace BeerAppreciation.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.unobtrusive*",
                "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
            "~/Scripts/angular.js",
            "~/Scripts/angular-route.js",
            "~/Scripts/angular-resource.js",
            "~/Scripts/angular-animate.js",
            "~/Scripts/angular-sanitize.js",
            "~/Scripts/angular-cookies.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular-ui").Include(
                "~/Scripts/angular-ui/ui-bootstrap.js",
                "~/Scripts/angular-ui/ui-bootstrap-tpls.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/toastr").Include(
                "~/Scripts/toastr.js"));

            bundles.Add(new ScriptBundle("~/bundles/underscore").Include(
                "~/Scripts/underscore.js"));

            bundles.Add(new ScriptBundle("~/bundles/spin").Include(
                "~/Scripts/spin.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/assets/app/app.js",
                "~/assets/app/config/*.js",
                "~/assets/app/shell/*.js",
                "~/assets/app/core/*.js",
                "~/assets/app/core/directives/*.js",
                "~/assets/app/core/filters/*.js",
                "~/assets/app/dashboard/*.js",
                "~/assets/app/drinking-club/*.js",
                "~/assets/app/event/*.js",
                "~/assets/app/event-beverage/*.js",
                "~/assets/app/beer/*.js",
                "~/assets/app/beverage/*.js",
                "~/assets/app/registration/*.js",
                "~/assets/app/statistics/*.js",
                "~/assets/app/manufacturer/*.js",
                "~/assets/app/beverage-type/*.js",
                "~/assets/app/beverage-style/*.js",
                "~/assets/app/core/services/*.js",
                "~/assets/app/rating/*.js",
                "~/assets/app/profile/*.js",
                "~/assets/app/user/*.js",
                "~/assets/app/start/*.js",
                "~/assets/app/controls/*.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                 "~/Content/bootstrap/bootstrap.css",
                 "~/Content/toastr.css",
                 "~/Content/Site.css"));

            bundles.Add(new ScriptBundle("~/bundles/moment").Include(
                "~/Scripts/moment.js"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = false;
        }
    }
}
