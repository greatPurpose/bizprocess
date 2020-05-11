<%@ Page Language="C#" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
    BizProcess.Platform.DBConnection bdbConn = new BizProcess.Platform.DBConnection();
    string link_DBConnOptions = bdbConn.GetAllOptions();
%>
<br />
<table cellpadding="0" cellspacing="1" border="0" width="95%" class="formtable">
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
        <th>默认值:</th>
        <td><input type="text" class="mytext" id="defaultvalue" style="width:290px; margin-right:6px;"/><select class="myselect" onchange="setDefaultValue(document.getElementById('defaultvalue'), this.value);" style="width:150px"><%=reportFrom.GetDefaultValueSelect("") %></select></td>
    </tr>
    <tr>
        <th>宽度:</th>
        <td><input type="text" id="width" class="mytext" style="width:150px" /></td>
    </tr>
    <tr>
        <th>最大字符数:</th>
        <td><input type="text" id="maxlength"  class="mytext" style="width:150px" /></td>
    </tr>
</table>

<script type="text/javascript">
    var oNode = null, thePlugins = 'reporttext';
    var attJSON = parent.reportattributeJSON;
    var dbconn = attJSON.dbconn || "";
    var dbtable = attJSON.dbtable || "";
    var dbtablepk = attJSON.dbtablepk || "";

    var parentEvents = parent.formEvents;
    var events = [];
    var eventsid = BPUI.Core.newid(false);

    $(function ()
    {
        db_change($("#dbconn").get(0), "");

        if (UE.plugins[thePlugins].editdom)
        {
            oNode = UE.plugins[thePlugins].editdom;
        }
        if (oNode)
        {
            $text = $(oNode);
            $("#defaultvalue").val($text.attr('defaultvalue'));
            if ($text.attr('width1')) $("#width").val($text.attr('width1'));
            $("#maxlength").val($text.attr('maxlength'));
            if ($text.attr('eventsid')) {
                eventsid = $text.attr('eventsid');
                events = getEvents(eventsid);
            }
        }
    });
    function db_change(obj, table) {
        if (!obj || !obj.value) return;
        $("#dbtable").html(getTableOps(obj.value, table));
        table_change($("#dbtable").get(0), dbtablepk);
    }
    function table_change(obj, fields) {
        if (!obj || !obj.value) return;
        var conn = $("#dbconn").val();
        var opts = getFieldsOps(conn, obj.value, fields);
        $("#dbpk").html(opts);
    }

    dialog.oncancel = function ()
    {
        if (UE.plugins[thePlugins].editdom)
        {
            delete UE.plugins[thePlugins].editdom;
        }
    };
    dialog.onok = function ()
    {
        var id = attJSON.dbconn && attJSON.dbtable && bindfiled ? attJSON.dbtable + '.' + bindfiled : "";
        var width = $("#width").val();
        var defaultvalue = $("#defaultvalue").val();
        var maxlength = $("#maxlength").val();

        var html = '<input type="text" id="' + id + '" type1="flow_text" name="' + id + '" value="文本框" ';
        if (width)
        {
            html += 'style="width:' + width + '" ';
            html += 'width1="' + width + '" ';
        }
        html += 'defaultvalue="'+defaultvalue+'" ';
        if (parseInt(maxlength) > 0)
        {
            html += 'maxlength="' + maxlength + '" ';
        }
        if (events.length > 0) {
            html += 'eventsid="' + eventsid + '" ';
            setEvents(eventsid);
        }
        html += '/>';
   
        if (oNode)
        {
            $(oNode).after(html);
            domUtils.remove(oNode, false);
        }
        else
        {
            editor.execCommand('insertHtml', html);
        }
        delete UE.plugins[thePlugins].editdom;
    }
</script>
</body>
</html>