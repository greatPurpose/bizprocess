<%@ Page Language="C#"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script type="text/javascript" src="../../dialogs/internal.js"></script>
    <script type="text/javascript" src="../common.js"></script>
    <script type="text/javascript" src="../compule.js"></script>
    <%=WebMvc.Common.Tools.IncludeFiles %>
</head>
<body style="overflow:hidden;">
    <%WebMvc.Common.Tools.CheckLogin(); %>
    <div style="margin:0 auto; text-align:center; padding-top:38px;">
        <div>
            <img src="/Images/loading/load1.gif" alt="" id="wait" />
        </div>
        <div style="margin-top:5px;">
            正在发布...
        </div>
    </div>

    <!--发布时保存-->
    <%Server.Execute("save.aspx?publish=1"); %>

    <script type="text/javascript">
        $(window).load(function ()
        {   
            publish();
        });
        function publish()
        {
            var reportattributeJSON = parent.reportattributeJSON;
            var reportid = "";
            if (!reportattributeJSON || !reportattributeJSON.id || $.trim(reportattributeJSON.id).length == 0)
            {
                alert('您未设置表单相关属性!');
                dialog.close();
                return;
            }
            else
            {
                reportattributeJSON.hasEditor = "0";
                formid = reportattributeJSON.id;
                var html = editor.getContent();
                var $controls = $("[type1^='flow_']", editor.document);
                for (var i = 0; i < $controls.size() ; i++)
                {
                    var $control = $controls.eq(i);
                    var type1Arr = $control.attr('type1').split('_');
                    var controlType = type1Arr.length > 1 ? type1Arr[1] : "";
                    switch (controlType)
                    {
                        case 'text':
                            UE.compule.getTextHtml($control);
                            break;
                        case 'select':
                            UE.compule.getSelectHtml($control);
                            break;
                        case 'dict':
                            UE.compule.getDictHtml($control);
                            break;
                        case 'datetime':
                            UE.compule.getDateTimeHtml($control);
                            break;
                        case 'html':
                            UE.compule.getHtmlHtml($control, reportattributeJSON);
                            break;
                        case "grid":
                            UE.compule.getGridHtml($control);
                            break;
                    }
                }
                $.ajax({
                    url: top.rootdir + "/Report/Publish", type: "POST",
                    data: {
                        id: reportid,
                        html: editor.getContent(),
                        name: reportattributeJSON.name,
                        att: JSON.stringify(reportattributeJSON)
                    },
                    async: true, cache: false, success: function (txt)
                    {
                        alert(txt);
                        editor.setContent(html);
                        dialog.close();
                    }, error: function (txt)
                    {
                        alert('发布表单发生了错误!');
                        editor.setContent(html);
                        dialog.close();
                    }
                });
               
            }
        }
    </script>
</body>
</html>
