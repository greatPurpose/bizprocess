using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
//using Campus.Report;
//using Campus.Report.Base;
using System.Drawing;
namespace WebMvc.Controllers
{
    public class ReportController : Controller
    {
        //
        // GET: /Report/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Tree()
        {
            return View();
        }

        public ActionResult List()
        {
            string title1 = Request.QueryString["title1"];
            return query(title1);
        }
        [HttpPost]
        public RedirectToRouteResult Delete()
        {
            BizProcess.Platform.ReportTemplate bReportTemplate = new BizProcess.Platform.ReportTemplate();
            string deleteID = Request.Form["checkbox_report"];
            System.Text.StringBuilder delxml = new System.Text.StringBuilder();
            foreach (string id in deleteID.Split(','))
            {
                Guid gid;
                if (id.IsGuid(out gid))
                {
                    var report = bReportTemplate.Get(gid);
                    if (report != null)
                    {
                        delxml.Append(report.Serialize());
                        bReportTemplate.Delete(gid);
                    }
                }
            }
            BizProcess.Platform.Log.Add("删除了一批报表库", delxml.ToString(), BizProcess.Platform.Log.Types.角色应用);
            return RedirectToAction("List", Common.Tools.GetRouteValueDictionary());
        }

        [HttpPost]
        public ActionResult List(FormCollection collection)
        {
            string title1 = collection["title1"];
            return query(title1);
        }

        private ActionResult query(string title1)
        {
            string pager;
            string appid = Request.QueryString["appid"];
            string tabid = Request.QueryString["tabid"];
            string typeid = Request.QueryString["typeid"];
            BizProcess.Platform.Dictionary bdict = new BizProcess.Platform.Dictionary();
            BizProcess.Platform.ReportTemplate breport = new BizProcess.Platform.ReportTemplate();
            string typeidstring = typeid.IsGuid() ? breport.GetAllChildsIDString(typeid.ToGuid()) : "";
            string query = string.Format("&appid={0}&tabid={1}&title1={2}&typeid={3}",
                        Request.QueryString["appid"],
                        Request.QueryString["tabid"],
                        title1.UrlEncode(), typeid
                        );
            string query1 = string.Format("{0}&pagesize={1}&pagenumber={2}", query, Request.QueryString["pagesize"], Request.QueryString["pagenumber"]);
            List<BizProcess.Data.Model.ReportTemplate> reportList = breport.GetPagerData(out pager, query, title1, typeidstring);
            ViewBag.Pager = pager;
            ViewBag.AppID = appid;
            ViewBag.TabID = tabid;
            ViewBag.TypeID = typeid;
            ViewBag.Title1 = title1;
            ViewBag.Query1 = query1;
            return View(reportList);
        }

        public ActionResult Edit()
        {
            return Edit(null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection collection)
        {
            string editID = Request.QueryString["id"];
            string type = Request.QueryString["typeid"];

            BizProcess.Platform.ReportTemplate bReportTemplate = new BizProcess.Platform.ReportTemplate();
            BizProcess.Data.Model.ReportTemplate reportTemplate = null;
            if (editID.IsGuid())
            {
                reportTemplate = bReportTemplate.Get(editID.ToGuid());
            }
            bool isAdd = !editID.IsGuid();
            string oldXML = string.Empty;
            if (reportTemplate == null)
            {
                reportTemplate = new BizProcess.Data.Model.ReportTemplate();
                reportTemplate.ID = Guid.NewGuid();
                ViewBag.TypeOptions = new BizProcess.Platform.ReportTemplate().GetTypeOptions(type);
            }
            else
            {
                oldXML = reportTemplate.Serialize();
                ViewBag.TypeOptions = new BizProcess.Platform.ReportTemplate().GetTypeOptions(reportTemplate.Type.ToString());
            }

            if (collection != null)
            {
                string title = collection["title"];
                string html = collection["Html"];
                string designjson = collection["DesignJSON"];
                string useMember = collection["UseMember"];
                type = collection["type"];

                reportTemplate.Html = html.Trim();
                reportTemplate.DesignJSON = designjson.Trim();
                reportTemplate.Title = title;
                reportTemplate.Type = type.ToGuid();

                if (!useMember.IsNullOrEmpty())
                {
                    reportTemplate.UseMember = useMember;
                }
                else
                {
                    reportTemplate.UseMember = null;
                }

                if (isAdd)
                {
                    bReportTemplate.Add(reportTemplate);
                    BizProcess.Platform.Log.Add("添加了报表库", reportTemplate.Serialize(), BizProcess.Platform.Log.Types.角色应用);
                    ViewBag.Script = "alert('添加成功!');new BPUI.Window().reloadOpener();new BPUI.Window().close();";
                }
                else
                {
                    bReportTemplate.Update(reportTemplate);
                    BizProcess.Platform.Log.Add("修改了报表库", "", BizProcess.Platform.Log.Types.角色应用, oldXML, reportTemplate.Serialize());
                    ViewBag.Script = "alert('修改成功!');new BPUI.Window().reloadOpener();new BPUI.Window().close();";
                }
                bReportTemplate.UpdateUseMemberCache(reportTemplate.ID);
                bReportTemplate.ClearCache();
                new BizProcess.Platform.RoleApp().ClearAllDataTableCache();
            }
            return View(reportTemplate);
        }
        /*
        public ActionResult Print()
        {
            //we will change this part as downloading stream.
            
            var directory = @"c:\pub\";


            Test1(directory, "pdf");
            Test2(directory, "excell");
            return View();
        }
        static void Test1(string directory, string name)
        {

            var bigTextblock = String.Empty;

            #region Big textblock

            bigTextblock = @"dd";

            #endregion;

            var dataTable1 = new DataTable();
            var dataTable2 = new DataTable();

            for (int i = 0; i < 10; i++)
            {
                dataTable1.Columns.Add(i.ToString());
            }

            for (int i = 0; i < 15; i++)
            {
                var row = dataTable1.NewRow();

                for (int j = 0; j < dataTable1.Columns.Count; j++)
                {
                    row[j] = (i * j) + " ячейка";
                }

                dataTable1.Rows.Add(row);
            }

            dataTable2.Columns.Add("Column 1");
            dataTable2.Columns.Add("Column 2");

            for (int i = 0; i < 5; i++)
            {
                var row = dataTable2.NewRow();
                row[0] = i;
                row[1] = "строка";
                dataTable2.Rows.Add(row);
            }

            var style0 = new TableStyle();

            var style1 = new TableStyle
            {
                Foreground = Color.Blue,
                PatternType = PatternType.Solid,
                BorderLine = BoderLine.Thick,
                BorderColor = Color.Orange,
                DocumentTitle = DocumentTitle.Title
            };

            var style2 = new TableStyle
            {
                FontColor = Color.Black,
                FontSize = 5,
                FontName = "Calibri",
                Foreground = Color.Gray,
                PatternType = PatternType.Solid,
                BorderLine = BoderLine.Thin,
                BorderColor = Color.Red,
                DocumentTitle = DocumentTitle.Heading1
            };

            var style3 = new TableStyle()
            {
                FontColor = Color.Bisque,
                FontSize = 16,
                FontName = "C",
                Foreground = Color.Green,
                PatternType = PatternType.Solid,
                DocumentTitle = DocumentTitle.Heading2
            };

            var style4 = new TableStyle()
            {
                FontColor = Color.LimeGreen,
                FontSize = 30,
                FontName = "Blackadder ITC",
                Foreground = Color.Yellow,
                PatternType = PatternType.Solid,
                DocumentTitle = DocumentTitle.Heading3
            };

            var style5 = new TableStyle()
            {
                FontColor = Color.Black,
                FontSize = 12,
                FontName = "Calibri",
                Foreground = Color.White,
                PatternType = PatternType.Solid,
                DocumentTitle = DocumentTitle.None
            };

            var tableStyle1 = new TableStyle
            {
                Foreground = Color.Blue,
                PatternType = PatternType.Solid,
                BorderLine = BoderLine.Thick,
                BorderColor = Color.Orange,
                DocumentTitle = DocumentTitle.None,
                FontSize = 10,
            };

            var tableStyle2 = new TableStyle
            {
                FontColor = Color.Black,
                FontSize = 10,
                FontName = "Calibri",
                Foreground = Color.Gray,
                PatternType = PatternType.Solid,
                BorderLine = BoderLine.Thick,
                BorderColor = Color.Red,
                DocumentTitle = DocumentTitle.None
            };

            var tableStyle3 = new TableStyle
            {
                FontColor = Color.Black,
                FontSize = 10,
                FontName = "Calibri",
                Foreground = Color.Gray,
                PatternType = PatternType.Solid,
                BorderLine = BoderLine.Thick,
                BorderColor = Color.Red,
                DocumentTitle = DocumentTitle.None
            };

            string[] abs = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };

            var reportBuilder = new ReportBuilder();

            reportBuilder.AppendTextBlock("Test title1", style0);
            reportBuilder.AppendTextBlock("Test title2", style2);
            reportBuilder.AppendTextBlock("Test title3", style3);
            reportBuilder.AppendTextBlock("Test title4", style4);
            reportBuilder.AppendTextBlock("Test title3", style3);
            reportBuilder.AppendTextBlock("Test title1", style1);
            reportBuilder.AppendTextBlock("Test title4", style4);
            reportBuilder.AppendNewSection();


            reportBuilder.AppendTable(dataTable1, abs, tableStyle1, tableStyle2);
            reportBuilder.AppendNewSection();
            reportBuilder.AppendTable(dataTable1, abs, tableStyle2, tableStyle3);
            reportBuilder.AppendNewLine();
            reportBuilder.AppendTable(dataTable1);


            reportBuilder.AppendTextBlock(bigTextblock, style5);
            //reportBuilder.AppendTable(dataTable2, new [] {"Колонка 1", "rjkjyrf 2"});
            reportBuilder.AppendTable(dataTable2, new[] { "Колонка 1", "rjkjyrf 2" }, style5, style1);
            reportBuilder.AppendTextBlock(bigTextblock, style5);


            var report = reportBuilder.Build();

            var reportRender = new ReportRenderer(report);


            Save(reportRender, directory, name);
        }

        static void Test2(string directory, string name)
        {
            var headerStyle = new TableStyle
                {
                    Foreground = Color.Blue,
                    PatternType = PatternType.Solid,
                    BorderLine = BoderLine.Thick,
                    BorderColor = Color.Orange,
                    DocumentTitle = DocumentTitle.Title,
                    FontSize = 9,
                };

            var bodyStyle = new TableStyle
                {
                    FontColor = Color.Black,
                    FontSize = 11,
                    FontName = "Calibri",
                    Foreground = Color.Bisque,
                    PatternType = PatternType.Solid,
                    BorderLine = BoderLine.Thin,
                    BorderColor = Color.Red,
                    DocumentTitle = DocumentTitle.Heading1
                };

            var reportBuilder = new ReportBuilder();
            reportBuilder.AppendComplexHeader(DataTables.RNPTable.RNPHeader(0, 0, 2013, "6.050201 Системна інженерія", "Компютеризовані та робототехнічні системи", "бакалавр", "Технічна кібернетика", "Факультет інформатики та обчислювальної техніки", "денна", "3 роки 10 місяців", "Молодший інженер з компютерної техніки"));
            reportBuilder.AppendComplexHeader(DataTables.RNPTable.RNPTableHeader(0, 7, 3, 18, 18));
            var report = reportBuilder.Build();

            var reportRender = new ReportRenderer(report);
            Save(reportRender, directory, name);
        }

        private static void Save(ReportRenderer renderer, string directory, string name)
        {
            renderer.ToExcel(directory + name + ".xlsx");
            renderer.ToHtml(directory + name + ".html");
            renderer.ToPdf(directory + name + ".pdf");
            renderer.ToWord(directory + name + ".docx");
        }
        */
        public ActionResult Table()
        {
            return Table(null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Table(FormCollection collection)
        {
            string editID = Request.QueryString["id"];
            string type = Request.QueryString["typeid"];

            BizProcess.Platform.ReportTemplate bReportTemplate = new BizProcess.Platform.ReportTemplate();
            BizProcess.Data.Model.ReportTemplate reportTemplate = null;
            if (editID.IsGuid())
            {
                reportTemplate = bReportTemplate.Get(editID.ToGuid());
            }
            bool isAdd = !editID.IsGuid();
            string oldXML = string.Empty;
            if (reportTemplate == null)
            {
                reportTemplate = new BizProcess.Data.Model.ReportTemplate();
                reportTemplate.ID = Guid.NewGuid();
                ViewBag.TypeOptions = new BizProcess.Platform.ReportTemplate().GetTypeOptions(type);
            }
            else
            {
                oldXML = reportTemplate.Serialize();
                ViewBag.TypeOptions = new BizProcess.Platform.ReportTemplate().GetTypeOptions(reportTemplate.Type.ToString());
            }

            if (collection != null)
            {
                string title = collection["title"];
                string html = collection["Html"];
                string designjson = collection["DesignJSON"];
                string useMember = collection["UseMember"];
                type = collection["type"];

                reportTemplate.Html = html.Trim();
                reportTemplate.DesignJSON = designjson.Trim();
                reportTemplate.Title = title;
                reportTemplate.Type = type.ToGuid();

                if (!useMember.IsNullOrEmpty())
                {
                    reportTemplate.UseMember = useMember;
                }
                else
                {
                    reportTemplate.UseMember = null;
                }

                if (isAdd)
                {
                    bReportTemplate.Add(reportTemplate);
                    BizProcess.Platform.Log.Add("添加了报表库", reportTemplate.Serialize(), BizProcess.Platform.Log.Types.角色应用);
                    ViewBag.Script = "alert('添加成功!');new BPUI.Window().reloadOpener();new BPUI.Window().close();";
                }
                else
                {
                    bReportTemplate.Update(reportTemplate);
                    BizProcess.Platform.Log.Add("修改了报表库", "", BizProcess.Platform.Log.Types.角色应用, oldXML, reportTemplate.Serialize());
                    ViewBag.Script = "alert('修改成功!');new BPUI.Window().reloadOpener();new BPUI.Window().close();";
                }
                bReportTemplate.UpdateUseMemberCache(reportTemplate.ID);
                bReportTemplate.ClearCache();
                new BizProcess.Platform.RoleApp().ClearAllDataTableCache();
            }
            return View(reportTemplate);
        }

        public ActionResult Set_Edit()
        {
            return View();
        }
        public ActionResult Set_List()
        {
            return View();
        }
        public ActionResult Set_Table()
        {
            return View();
        }

        public string GetTables()
        {
            Response.Charset = "utf-8";
            string connID = Request.QueryString["connid"];
            if (!connID.IsGuid())
            {
                return "[]";
            }
            List<string> tables = new BizProcess.Platform.DBConnection().GetTables(connID.ToGuid());
            System.Text.StringBuilder sb = new System.Text.StringBuilder("[", 1000);
            foreach (string table in tables)
            {
                sb.Append("{\"name\":");
                sb.AppendFormat("\"{0}\"", table);
                sb.Append("},");
            }
            return sb.ToString().TrimEnd(',') + "]";
        }

        public string GetFields()
        {
            string table = Request.QueryString["table"];
            string connid = Request.QueryString["connid"];

            if (table.IsNullOrEmpty() || !connid.IsGuid())
            {
                return "[]";
            }
            Dictionary<string, string> fields = new BizProcess.Platform.DBConnection().GetFields(connid.ToGuid(), table);
            System.Text.StringBuilder sb = new System.Text.StringBuilder("[", 1000);

            foreach (var field in fields)
            {
                sb.Append("{");
                sb.AppendFormat("\"name\":\"{0}\",\"note\":\"{1}\"", field.Key, field.Value);
                sb.Append("},");
            }
            return sb.ToString().TrimEnd(',') + "]";
        }
        public string GetHtml()
        {
            string id = Request["id"];
            Guid gid;
            if (!id.IsGuid(out gid))
            {
                return "";
            }

            var rt = new BizProcess.Platform.ReportTemplate().Get(gid);
            if (rt == null)
            {
                return "";
            }
            else
            {
                return rt.Html;
            }
        }

        public string GetAttribute()
        {
            string id = Request["id"];
            Guid gid;
            if (!id.IsGuid(out gid))
            {
                return "";
            }

            var rt = new BizProcess.Platform.ReportTemplate().Get(gid);
            if (rt == null)
            {
                return "";
            }
            else
            {
                return rt.Attribute;
            }
        }
        public string Save()
        {
            string html = Request["html"];
            string name = Request["name"];
            string att = Request["att"];
            string id = Request["id"];
            string type = Request["type"];
            if (name.IsNullOrEmpty())
            {
                return "表单名称不能为空!";
            }

            Guid reportID;
            if (!id.IsGuid(out reportID))
            {
                return "表单ID无效!";
            }

            BizProcess.Platform.ReportTemplate RT = new BizProcess.Platform.ReportTemplate();
            BizProcess.Data.Model.ReportTemplate rt = RT.Get(reportID);
            bool isAdd = false;
            string oldXML = string.Empty;
            if (rt == null)
            {
                rt = new BizProcess.Data.Model.ReportTemplate();
                rt.ID = reportID;
                rt.Type = type.ToGuid();
                isAdd = true;
            }
            else
            {
                oldXML = rt.Serialize();
            }

            rt.DesignJSON = att;
            rt.Html = html;
            rt.Title = name;

            if (isAdd)
            {
                RT.Add(rt);
                BizProcess.Platform.Log.Add("添加了流程表单", rt.Serialize(), BizProcess.Platform.Log.Types.流程相关);
            }
            else
            {
                RT.Update(rt);
                BizProcess.Platform.Log.Add("修改了流程表单", "", BizProcess.Platform.Log.Types.流程相关, oldXML, rt.Serialize());
            }
            return "保存成功!";
        }
        /*
        public string Publish()
        {
            string html = Request["html"];
            string name = Request["name"];
            string att = Request["att"];
            string id = Request["id"];

            Guid gid;
            if (!id.IsGuid(out gid) || name.IsNullOrEmpty() || att.IsNullOrEmpty() || html.IsNullOrEmpty())
            {
                return "参数错误!";
            }
            BizProcess.Platform.ReportTemplate RT = new BizProcess.Platform.ReportTemplate();

            BizProcess.Data.Model.ReportTemplate rt = RT.Get(gid);
            if (rt == null)
            {
                return "未找到表单!";
            }

            string fileName = id + ".cshtml";

            System.Text.StringBuilder serverScript = new System.Text.StringBuilder("@{\r\n");
            var attrJSON = LitJson.JsonMapper.ToObject(att);
            serverScript.Append("\tstring FlowID = Request.QueryString[\"flowid\"];\r\n");
            serverScript.Append("\tstring StepID = Request.QueryString[\"stepid\"];\r\n");
            serverScript.Append("\tstring GroupID = Request.QueryString[\"groupid\"];\r\n");
            serverScript.Append("\tstring TaskID = Request.QueryString[\"taskid\"];\r\n");
            serverScript.Append("\tstring InstanceID = Request.QueryString[\"instanceid\"];\r\n");
            serverScript.Append("\tstring DisplayModel = Request.QueryString[\"display\"] ?? \"0\";\r\n");
            serverScript.AppendFormat("\tstring DBConnID = \"{0}\";\r\n", attrJSON["dbconn"].ToString());
            serverScript.AppendFormat("\tstring DBTable = \"{0}\";\r\n", attrJSON["dbtable"].ToString());
            serverScript.AppendFormat("\tstring DBTablePK = \"{0}\";\r\n", attrJSON["dbtablepk"].ToString());
            serverScript.AppendFormat("\tstring DBTableTitle = \"{0}\";\r\n", attrJSON["dbtabletitle"].ToString());

            serverScript.Append("\tBizProcess.Platform.Dictionary BDictionary = new BizProcess.Platform.Dictionary();\r\n");
            serverScript.Append("\tBizProcess.Platform.WorkFlow BWorkFlow = new BizProcess.Platform.WorkFlow();\r\n");
            serverScript.Append("\tBizProcess.Platform.WorkFlowTask BWorkFlowTask = new BizProcess.Platform.WorkFlowTask();\r\n");
            serverScript.Append("\tstring fieldStatus = BWorkFlow.GetFieldStatus(FlowID, StepID);\r\n");
            serverScript.Append("\tLitJson.JsonData initData = BWorkFlow.GetFormData(DBConnID, DBTable, DBTablePK, InstanceID, fieldStatus);\r\n");
            serverScript.Append("\tstring TaskTitle = BWorkFlow.GetFromFieldData(initData, DBTable, DBTableTitle);\r\n");

            serverScript.Append("}\r\n");
            serverScript.Append("<link href=\"~/Scripts/FlowRun/Forms/flowform.css\" rel=\"stylesheet\" type=\"text/css\" />\r\n");
            serverScript.Append("<script src=\"~/Scripts/FlowRun/Forms/common.js\" type=\"text/javascript\" ></script>\r\n");

            if (attrJSON.ContainsKey("hasEditor") && "1" == attrJSON["hasEditor"].ToString())
            {
                serverScript.Append("<script src=\"~/Scripts/Ueditor/ueditor.config.js\" type=\"text/javascript\" ></script>\r\n");
                serverScript.Append("<script src=\"~/Scripts/Ueditor/ueditor.all.min.js\" type=\"text/javascript\" ></script>\r\n");
                serverScript.Append("<script src=\"~/Scripts/Ueditor/lang/zh-cn/zh-cn.js\" type=\"text/javascript\" ></script>\r\n");
                serverScript.Append("<input type=\"hidden\" id=\"Form_HasUEditor\" name=\"Form_HasUEditor\" value=\"1\" />\r\n");
            }
            string validatePropType = attrJSON.ContainsKey("validatealerttype") ? attrJSON["validatealerttype"].ToString() : "2";
            serverScript.Append("<input type=\"hidden\" id=\"Form_ValidateAlertType\" name=\"Form_ValidateAlertType\" value=\"" + validatePropType + "\" />\r\n");
            if (attrJSON.ContainsKey("autotitle") && attrJSON["autotitle"].ToString().ToLower() == "true")
            {
                serverScript.AppendFormat("<input type=\"hidden\" id=\"{0}\" name=\"{0}\" value=\"{1}\" />\r\n",
                    string.Concat(attrJSON["dbtable"].ToString(), ".", attrJSON["dbtabletitle"].ToString()),
                    "@(TaskTitle.IsNullOrEmpty() ? BWorkFlow.GetAutoTaskTitle(FlowID, StepID, Request.QueryString[\"groupid\"]) : TaskTitle)"
                    );
            }
            serverScript.AppendFormat("<input type=\"hidden\" id=\"Form_TitleField\" name=\"Form_TitleField\" value=\"{0}\" />\r\n", string.Concat(attrJSON["dbtable"].ToString(), ".", attrJSON["dbtabletitle"].ToString()));
            //serverScript.AppendFormat("<input type=\"hidden\" id=\"Form_Name\" name=\"Form_Name\" value=\"{0}\" />\r\n", attrJSON["name"].ToString());
            serverScript.AppendFormat("<input type=\"hidden\" id=\"Form_DBConnID\" name=\"Form_DBConnID\" value=\"{0}\" />\r\n", attrJSON["dbconn"].ToString());
            serverScript.AppendFormat("<input type=\"hidden\" id=\"Form_DBTable\" name=\"Form_DBTable\" value=\"{0}\" />\r\n", attrJSON["dbtable"].ToString());
            serverScript.AppendFormat("<input type=\"hidden\" id=\"Form_DBTablePk\" name=\"Form_DBTablePk\" value=\"{0}\" />\r\n", attrJSON["dbtablepk"].ToString());
            serverScript.AppendFormat("<input type=\"hidden\" id=\"Form_DBTableTitle\" name=\"Form_DBTableTitle\" value=\"{0}\" />\r\n", attrJSON["dbtabletitle"].ToString());
            serverScript.AppendFormat("<input type=\"hidden\" id=\"Form_AutoSaveData\" name=\"Form_AutoSaveData\" value=\"{0}\" />\r\n", "1");
            serverScript.Append("<script type=\"text/javascript\">\r\n");
            serverScript.Append("\tvar initData = @Html.Raw(BWorkFlow.GetFormDataJsonString(initData));\r\n");
            serverScript.Append("\tvar fieldStatus = @Html.Raw(fieldStatus);\r\n");
            serverScript.Append("\tvar displayModel = '@DisplayModel';\r\n");
            serverScript.Append("\t$(window).load(function (){\r\n");
            serverScript.AppendFormat("\t\tformrun.initData(initData, \"{0}\", fieldStatus, displayModel);\r\n", attrJSON["dbtable"].ToString());
            serverScript.Append("\t});\r\n");
            serverScript.Append("</script>\r\n");


            string file = Server.MapPath("~/Views/WorkFlowFormDesigner/Forms/" + fileName);
            System.IO.Stream stream = System.IO.File.Open(file, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            stream.SetLength(0);

            StreamWriter sw = new StreamWriter(stream, System.Text.Encoding.UTF8);
            sw.Write(serverScript.ToString());
            sw.Write(Server.HtmlDecode(html));

            sw.Close();
            stream.Close();


            string attr = wff.Attribute;
            string appType = LitJson.JsonMapper.ToObject(attr)["apptype"].ToString();
            BizProcess.Platform.AppLibrary App = new BizProcess.Platform.AppLibrary();
            var app = App.GetByCode(id);
            bool isAdd = false;
            if (app == null)
            {
                app = new BizProcess.Data.Model.AppLibrary();
                app.ID = Guid.NewGuid();
                app.Code = id;
                isAdd = true;
            }
            app.Address = "/Views/WorkFlowFormDesigner/Forms/" + fileName;
            app.Note = "流程表单";
            app.OpenMode = 0;
            app.Params = "";
            app.Title = name.Trim();
            app.Type = appType.IsGuid() ? appType.ToGuid() : new BizProcess.Platform.Dictionary().GetIDByCode("FormTypes");
            if (isAdd)
            {
                App.Add(app);
            }
            else
            {
                App.Update(app);
            }

            BizProcess.Platform.Log.Add("发布了流程表单", app.Serialize() + "内容：" + html, BizProcess.Platform.Log.Types.流程相关);
            wff.Status = 1;
            WFF.Update(wff);
            return "发布成功!";
        }
*/
    }
}
