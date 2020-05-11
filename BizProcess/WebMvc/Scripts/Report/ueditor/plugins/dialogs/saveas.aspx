<%@ Page Language="C#" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <script type="text/javascript" src="../../dialogs/internal.js"></script>
    <script type="text/javascript" src="../common.js"></script>
    <%=WebMvc.Common.Tools.IncludeFiles %>
</head>
<body>
<% 
    WebMvc.Common.Tools.CheckLogin();
    BizProcess.Platform.ReportTemplate reportFrom = new BizProcess.Platform.ReportTemplate();
    if (!Request.Form["name"].IsNullOrEmpty())
    {
        string id = Request.Form["id"];
        string name = Request.Form["name"];

        if (!id.IsGuid() || name.IsNullOrEmpty())
        {
           Response.Write("<script>alert('数据验证错误!');</script>");
        }
        else
        {
            var rt = reportFrom.Get(id.ToGuid());
            if (rt != null)
            {
                rt.ID = Guid.NewGuid();
                rt.Title = name.Trim();

                var json = LitJson.JsonMapper.ToObject(rt.DesignJSON);
                json["id"] = rt.ID.ToString();
                json["name"] = rt.Title;
                rt.DesignJSON = LitJson.JsonMapper.ToJson(json);
                
                reportFrom.Add(rt);
                BizProcess.Platform.Log.Add("以另存的方式创建了表单", rt.Serialize(), BizProcess.Platform.Log.Types.流程相关);
                Response.Write("<script>alert('表单已另存为：" + name + "');dialog.close();</script>");
            }
        }
    }
%>

<form method="post" onsubmit="return new BPUI.Validate().validateForm(this);">
    <br /><br />
    <table cellpadding="0" cellspacing="1" border="0" width="95%"  align="center">
        <tr>
            <td>表单名称：</td>
        </tr>
        <tr>
            <td>
                <input type="hidden" id="id" name="id" value="" />
                <input type="text" class="mytext" id="name" name="name" validate="empty" style="width:75%"/></td>
        </tr>
    </table>
    <div class="buttondiv">
        <input type="submit" value="确定保存" name="save1" id="save1" class="mybutton" />
        <input type="button" value="取消关闭" class="mybutton" onclick="dialog.close();" />
    </div>
</form>
<script type="text/javascript">
    var attJSON = parent.reportattributeJSON;
    $(window).load(function ()
    {
        if (!attJSON.id || $.trim(attJSON.id).length == 0)
        {
            alert('请先打开一个表单,再另存为!');
            dialog.close();
        }
        else
        {
            $("#id").val(attJSON.id);
        }
    })
</script>
</body>
</html>