<%@ Page Language="C#" %>
<% 
    WebMvc.Common.Tools.CheckLogin();
    string id = Request.QueryString["id"];
    if(!id.IsGuid())
    {
        Response.Write("参数错误!");
        Response.End();
    }
    BizProcess.Platform.WorkFlowForm WFF = new BizProcess.Platform.WorkFlowForm();
    
    var wff = WFF.Get(id.ToGuid());
    
    if(wff==null)
    {
        Response.Write("参数错误!");
        Response.End();
    }
    wff.Status = 2;
    WFF.Update(wff);

    //BizProcess.Platform.AppLibrary APP = new BizProcess.Platform.AppLibrary();
    //var app = APP.GetByCode(id);
    //if(app!=null)
    //{
    //    APP.Delete(app.ID);
    //}

    BizProcess.Platform.Log.Add("删除了流程表单", wff.Serialize(), BizProcess.Platform.Log.Types.流程相关);

    Response.Write("1");
    Response.End();
%>