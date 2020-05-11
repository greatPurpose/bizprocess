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
    BizProcess.Platform.DBConnection bdbConn = new BizProcess.Platform.DBConnection();
    BizProcess.Platform.ReportTemplate bReportTemplate = new BizProcess.Platform.ReportTemplate();
    string link_DBConnOptions = bdbConn.GetAllOptions();
    string typeOptions = bReportTemplate.GetTypeOptions("");
//    string validatePromptType = new BizProcess.Platform.ReportTemplate().GetValidatePropTypeRadios("validatealerttype","","");
%>
<br />
<table cellpadding="0" cellspacing="1" border="0" width="95%" class="formtable">
    <tr>
        <th style="width:80px;">表单名称:</th>
        <td><input type="text" class="mytext" id="name" style="width:223px"/></td>
    </tr>
    <tr>
        <th>数据连接:</th>
        <td><select class="myselect" id="dbconn" onchange="db_change(this)" style="width:227px"><%=link_DBConnOptions %></select></td>
    </tr>
    <tr>
        <th>数据表:</th>
        <td><select class="myselect" id="dbtable" onchange="table_change(this)" style="width:227px"></select></td>
    </tr>
    <tr>
        <th>主键:</th>
        <td><select class="myselect" id="dbpk" style="width:227px"></select></td>
    </tr>
    <tr>
        <th>标题字段:</th>
        <td><select class="myselect" id="dbtitle" style="width:227px"></select></td>
    </tr>
    <tr>
        <th>程序库分类:</th>
        <td>
            <select class="myselect" id="type" style=""><option value=""></option><%=typeOptions %></select>
        </td>
    </tr>
    <tr>
        <td colspan="2" style="height:23px; text-align:center; color:blue;">提示：属性设置完成点击确定后即可在编辑器区域设计表单</td>
    </tr>
</table>

<script type="text/javascript">
    var attJSON = parent.reportattributeJSON;
    var dbconn = attJSON.dbconn || "";
    var dbtable = attJSON.dbtable || "";
    var dbtablepk = attJSON.dbtablepk || "";
    var dbtabletitle = attJSON.dbtabletitle || "";
    var isnew = "1" == "<%=Request["new"]%>";
    $(function ()
    {
        db_change($("#dbconn").get(0), isnew ? "" : dbtable);
        if (!isnew)
        {
            table_change($("#dbtable").get(0), "");
            $("#dbpk").val(dbtablepk);
            $("#dbtitle").val(dbtabletitle);
            $("#dbconn").val(dbconn);
            $("#name").val(attJSON.name || "");
            $("#type").val(attJSON.apptype || "");
            $("#typeselect").val(attJSON.apptype || "");
            $("input[name='validatealerttype'][value='" + attJSON.validatealerttype + "']").prop('checked', true);
        }
    });
    function db_change(obj, table)
    {
        if (!obj || !obj.value) return;
        $("#dbtable").html(getTableOps(obj.value, table));
        table_change($("#dbtable").get(0), dbtablepk);
    }
    function table_change(obj, fields)
    {
        if (!obj || !obj.value) return;
        var conn = $("#dbconn").val();
        var opts = getFieldsOps(conn, obj.value, fields);
        $("#dbpk").html(opts);
        $("#dbtitle").html(opts);
    }
    
    function typeChange(value)
    {
        $("#typeselect option").each(function ()
        {
            if ($(this).val() == value)
            {
                $("#type").val(!$(this).val() ? "" : $(this).text());
                return false;
            }
        });
    }
    dialog.onok = function ()
    {
        var json = {};
        json.name = $("#name").val();
        json.dbconn = $("#dbconn").val();
        json.dbtable = $("#dbtable").val();
        json.dbtablepk = $("#dbpk").val();
        json.dbtabletitle = $("#dbtitle").val();
        json.apptype = $("#type").val();
        json.validatealerttype = $(":checked[name='validatealerttype']").val() || "1";
        
        parent.reportattributeJSON.name = json.name;
        parent.reportattributeJSON.dbconn = json.dbconn;
        parent.reportattributeJSON.dbtable = json.dbtable;
        parent.reportattributeJSON.dbtablepk = json.dbtablepk;
        parent.reportattributeJSON.dbtabletitle = json.dbtabletitle;
        parent.reportattributeJSON.apptype = json.apptype;
        parent.reportattributeJSON.validatealerttype = json.validatealerttype;
        
        if (isnew)
        {
            parent.reportattributeJSON.id = "";
            editor.setContent("");
        }
    }
</script>
</body>
</html>

