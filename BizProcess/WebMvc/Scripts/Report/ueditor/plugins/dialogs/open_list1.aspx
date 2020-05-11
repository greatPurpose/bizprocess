<%@ Page Language="C#" %>
<%
    WebMvc.Common.Tools.CheckLogin();
    string typeid = Request.QueryString["typeid"];
    if(!typeid.IsGuid())
    {
        Response.Write("");
        Response.End();
    }
    BizProcess.Platform.ReportTemplate reportFrom = new BizProcess.Platform.ReportTemplate();
    var reports = reportFrom.GetAllByType(typeid.ToGuid());
    System.Text.StringBuilder html = new System.Text.StringBuilder();
    foreach(var report in reports)
    {
        html.Append("<tr>");
        html.AppendFormat("<td style='background:#ffffff;'>{0}</td>",report.Title);
        html.AppendFormat("<td style='background:#ffffff;'>{0}</td>", string.Format(@"<a href=""javascript:void(0);"" onclick=""openform('{0}');return false;"">
                    <img src=""{1}Images/ico/folder_classic_opened.png"" alt="""" style=""vertical-align:middle; border:0;"" />
                    <span style=""vertical-align:middle;"">打开</span>
                    </a>
                    <a href=""javascript:void(0);"" onclick=""delform('{0}');return false;"">
                    <img src=""{1}Images/ico/cancel.gif"" alt="""" style=""vertical-align:middle; border:0; margin-left:5px;"" />
                    <span style=""vertical-align:middle;"">删除</span>
                    </a>", report.ID, WebMvc.Common.Tools.BaseUrl));
        html.Append("</tr>");
    }
    Response.Write(html.ToString());
    Response.End();
%>