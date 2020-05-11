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
<table cellpadding="0" cellspacing="1" border="0" width="100%" id="ds_sql" style="display:none; margin-top:5px;" align="center">
    <tr>
        <td colspan="2">
            <fieldset style="padding:3px;">
            <legend style=""> SQL语句 </legend>
            <table border="0" style="width:100%;">
                <tr>
                    <td style="width:80%">
                        <div>1.SQL应返回两个字段的数据源</div>
                        <div>2.第一个字段为值，第二个字段为标题</div>
                        <div>3.如果只返回一个字段则值和标题一样</div>
                    </td>
                    <td>
                        <input type="button" value="测试SQL" onclick="testSql($('#ds_sql_value').val());" class="mybutton" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="padding-top:4px;">
                        <textarea cols="1" rows="1" id="ds_sql_value" style="width:99%; height:80px; font-family:Verdana;" class="mytextarea"></textarea>
                    </td>
                </tr>
            </table>
            </fieldset>
        </td>
    </tr>
</table>
<script type="text/javascript">
    var oNode = null, thePlugins = 'reportselect';
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
        biddingFileds(attJSON, oNode ? $(oNode).attr("id") : "", $("#bindfiled"));
        if (oNode)
        {
            $text = $(oNode);
            $("#hasempty").prop("checked", "1" == $text.attr("hasempty"));
            $("input[name='datasource'][value='" + $text.attr('datasource') + "']").prop('checked', true);
            if ("1" == $text.attr("datasource"))
            {
                $("#ds_dict").hide();
                $("#ds_sql").hide();
                $("#ds_custom").show();
                var custionJSONString = $text.attr("customopts");
                if ($.trim(custionJSONString).length > 0)
                {
                    var customJSON = JSON.parse(custionJSONString);
                    var customString = [];
                    for (var i = 0; i < customJSON.length; i++)
                    {
                        customString.push(customJSON[i].title + "," + customJSON[i].value);
                    }
                    $("#custom_string").val(customString.join(';'));
                }
                new BPUI.DragSort($("#custom_sort"));
            }
            else if ("0" == $text.attr("datasource"))
            {
                $("#ds_dict").show();
                $("#ds_sql").hide();
                $("#ds_custom").hide();
                $("#ds_dict_value").val($text.attr('dictid'));
                new BPUI.Dict().setValue($("#ds_dict_value"));
            }
            else if ("2" == $text.attr("datasource"))
            {
                $("#ds_dict").hide();
                $("#ds_sql").show();
                $("#ds_custom").hide();
                $("#ds_sql_value").val($text.attr('sql'));
            }

            if ($text.attr('eventsid'))
            {
                eventsid = $text.attr('eventsid');
                events = getEvents(eventsid);
            }
        }

        new BPUI.DragSort($("#custom_sort"));
        initTabs();
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
    function loadOptions()
    {
        var $listDiv = $("#text_default_list");
        var datasource = $(":checked[name='datasource']").val();
        var dvalue = $(":checked", $listDiv).val() || ($(oNode).attr("defaultvalue") || "");
        $listDiv.html('');
        if ("1" == datasource)
        {
            var custom_string = ($("#custom_string").val() || "").split(';');
            for (var i = 0; i < custom_string.length; i++)
            {
                var titlevalue = custom_string[i].split(',');
                if (titlevalue.length != 2)
                {
                    continue;
                }
                var title = titlevalue[0];
                var value = titlevalue[1];
                var option = '<div><input type="radio" ' + (value == dvalue ? 'checked="checked"' : '') + ' value="' + value + '" id="defaultvalue_' + value + '" style="vertical-align:middle;" name="defaultvalue"/><label style="vertical-align:middle;" for="defaultvalue_' + value + '">' + title + '(' + value + ')</label></div>';
                $listDiv.append(option);
            }
        }
        else if ("0" == datasource)
        {
            var dictid = $("#ds_dict_value").val();
            $.ajax({
                url: "getdictchilds.aspx?dictid=" + dictid, cache: false, async: false, dataType: "json", success: function (json)
                {
                    for (var i = 0; i < json.length; i++)
                    {
                        var title = json[i].title;
                        var value = json[i].id;
                        var option = '<div><input type="radio" ' + (value == dvalue ? 'checked="checked"' : '') + ' value="' + value + '" id="defaultvalue_' + value + '" style="vertical-align:middle;" name="defaultvalue"/><label style="vertical-align:middle;" for="defaultvalue_' + value + '">' + title + '(' + value + ')</label></div>';
                        $listDiv.append(option);
                    }
                }
            });
        }
        else if ("2" == datasource)
        {
            var sql = $("#ds_sql_value").val();
            $.ajax({
                url: "getsqljson.aspx", type: "post", data: { sql: sql, conn: attJSON.dbconn }, cache: false, async: false, dataType: "json", success: function (json)
                {
                    for (var i = 0; i < json.length; i++)
                    {
                        var title = json[i].title;
                        var value = json[i].id;
                        var option = '<div><input type="radio" ' + (value == dvalue ? 'checked="checked"' : '') + ' value="' + value + '" id="defaultvalue_' + value + '" style="vertical-align:middle;" name="defaultvalue"/><label style="vertical-align:middle;" for="defaultvalue_' + value + '">' + title + '(' + value + ')</label></div>';
                        $listDiv.append(option);
                    }
                }
            });
        }
    }

    function dsChange(value)
    {
        if (value == 0)
        {
            $("#ds_dict").show();
            $("#ds_custom").hide();
            $("#ds_sql").hide();
        }
        else if (value == 1)
        {
            $("#ds_dict").hide();
            $("#ds_sql").hide();
            $("#ds_custom").show();
        }
        else if (value == 2)
        {
            $("#ds_dict").hide();
            $("#ds_custom").hide();
            $("#ds_sql").show();
        }
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
        var bindfiled = $("#bindfiled").val();
        var id = attJSON.dbconn && attJSON.dbtable && bindfiled ? attJSON.dbtable + '.' + bindfiled : "";
        var datasource = $(":checked[name='datasource']").val();
        var width = $("#width").val();
        var hasempty = $("#hasempty").prop("checked") ? "1" : "0";
        var radios = [];
        var dictid = "";
        var sql = "";
        var dvalue = $(":checked[name='defaultvalue']", $("#text_default_list")).val() || "";
        if ("1" == datasource)
        {
            var custom_string = ($("#custom_string").val() || "").split(';');
            for (var i = 0; i < custom_string.length; i++)
            {
                var titlevalue = custom_string[i].split(',');
                if (titlevalue.length != 2)
                {
                    continue;
                }
                var title = titlevalue[0];
                var value = titlevalue[1];
                radios.push({ title: title, value: value });
            }
        }
        else if ("0" == datasource)
        {
            dictid = $("#ds_dict_value").val();
        }
        else if ("2" == datasource)
        {
            sql = $("#ds_sql_value").val();
        }

        var html = '<input type="text" type1="flow_select" id="' + id + '" name="' + id + '" datasource="' + datasource + '" dictid="' + dictid + '" value="下拉列表框" defaultvalue="' + dvalue + '" hasempty="' + hasempty + '"';
        if (width)
        {
            html += 'style="width:' + width + '" ';
            html += 'width1="' + width + '" ';
        }
        if ("1" == datasource)
        {
            html += "customopts='" + JSON.stringify(radios) + "' ";
        }
        if ("2" == datasource)
        {
            html += 'sql="' + sql.replaceAll('"', '&quot;') + '" ';
        }

        if (events.length > 0)
        {
            html += 'eventsid="' + eventsid + '" ';
            setEvents(eventsid);
        }

        html += ' />';
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