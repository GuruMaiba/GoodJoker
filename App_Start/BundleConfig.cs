using System.Web.Optimization;

namespace GoodJoker
{
    public class BundleConfig
    {
        //Дополнительные сведения об объединении см. по адресу: http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-3.1.1.min.js",
                        "~/Scripts/jquery.cookie-1.4.1.min.js",
                        "~/Scripts/startup.js"));

            bundles.Add(new ScriptBundle("~/Scripts/action").Include(
                        "~/Scripts/jquery.mCustomScrollbar.concat.min.js",
                        "~/Scripts/action.js"));

            bundles.Add(new ScriptBundle("~/Scripts/ThirdParty").Include(
                        "~/Scripts/device.min.js",
                        "~/Scripts/parallax.min.js"));
        }
    }
}