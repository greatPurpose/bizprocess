using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Config 的摘要说明
/// </summary>
/// 

namespace ReportTemplate
{
    public class ConfigHandler : Handler
    {
        public ConfigHandler(HttpContext context) : base(context) { }

        public override void Process()
        {
            WriteJson(Config.Items);
        }
    }
}