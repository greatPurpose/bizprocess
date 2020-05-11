<%@ Page Language="C#" %>
<% 
    WebMvc.Common.Tools.CheckLogin();
    string id = Request.QueryString["id"];
    if(!id.IsGuid())
    {
        Response.Write("参数错误!");
        Response.End();
    }
    BizProcess.Platform.ReportTemplate RT = new BizProcess.Platform.ReportTemplate();
    
    var rt = RT.Get(id.ToGuid());
    
    if(rt==null)
    {
        Response.Write("参数错误!");
        Response.End();
    }
    RT.Update(rt);

    //BizProcess.Platform.AppLibrary APP = new BizProcess.Platform.AppLibrary();
    //var app = APP.GetByCode(id);
    //if(app!=null)
    //{
    //    APP.Delete(app.ID);
    //}

    BizProcess.Platform.Log.Add("删除了流程表单", rt.Serialize(), BizProcess.Platform.Log.Types.流程相关);

    Response.Write("1");
    Response.End();
%>