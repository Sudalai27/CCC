using System.Web;
using System.Web.Optimization;

namespace ProsolOnline
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {


            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(               
                       "~/Scripts/theme.js",
                       "~/Scripts/modernizr.js",
                       "~/Scripts/nanoscroller.js",                              
                       "~/Scripts/fileupload.js"                     
                       ));

          

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));


                    bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                               "~/Scripts/angular.js",
                                "~/Scripts/jquery.js",
                             "~/Scripts/jquery-ui.js",
                              "~/Scripts/jquery.dataTables.min.js",
                               "~/Scripts/angular-datatables.min.js",
                                   "~/Scripts/angular-route.js",
                                    "~/Scripts/angular-filter.min.js"
                                   //"~/Scripts/angular-animate.js",
                                   //"~/Scripts/angular-busy.min.js"
                                   // "~/Scripts/angular-aria.js",

                                   //   "~/Scripts/angular-messages.js",
                                   //    "~/Scripts/svg-assets-cache.js",
                                   //    "~/Scripts/angular-material.js"
                                   ));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/css/font-awesome.css",
                      "~/Content/bootstrap-fileupload.min.css",
                      "~/Content/theme.css",
                      "~/Content/jquery-ui.css",
                      "~/Content/jquery.dataTables.min.css",
                      "~/Content/Style.css",
                      "~/Content/animate.css"
                      ));

            BundleTable.EnableOptimizations = false;
        }
    }
}
 
   