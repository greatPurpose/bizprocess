using System.Web;
using System.Web.Optimization;

namespace WebMvc
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/script")
                    .Include("~/Scripts/jquery-1.11.1.js")
                    .Include("~/Scripts/jquery.cookie.js")
                    .Include("~/Scripts/json.js")
                    .Include("~/Scripts/bpui.core.js")
                    .Include("~/Scripts/bpui.button.js")
                    .Include("~/Scripts/bpui.calendar.js")
                    .Include("~/Scripts/bpui.file.js")
                    .Include("~/Scripts/bpui.member.js")
                    .Include("~/Scripts/bpui.dict.js")
                    .Include("~/Scripts/bpui.menu.js")
                    .Include("~/Scripts/bpui.select.js")
                    .Include("~/Scripts/bpui.combox.js")
                    .Include("~/Scripts/bpui.tab.js")
                    .Include("~/Scripts/bpui.text.js")
                    .Include("~/Scripts/bpui.textarea.js")
                    .Include("~/Scripts/bpui.editor.js")
                    .Include("~/Scripts/bpui.tree.js")
                    .Include("~/Scripts/bpui.validate.js")
                    .Include("~/Scripts/bpui.window.js")
                    .Include("~/Scripts/bpui.dragsort.js")
                    .Include("~/Scripts/bpui.selectico.js")
                    .Include("~/Scripts/bpui.accordion.js")
                    .Include("~/Scripts/bpui.grid.js")
                    .Include("~/Scripts/bpui.init.js")
                );
        }
    }
}