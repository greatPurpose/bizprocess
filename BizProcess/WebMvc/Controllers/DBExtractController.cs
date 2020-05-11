using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebMvc.Controllers
{
    public class DBExtractController : MyController
    {
        //
        // GET: /DBConnection/

        public ActionResult Index()
        {
            string query1 = string.Format("&appid={0}&tabid={1}", Request.QueryString["appid"], Request.QueryString["tabid"]);
            BizProcess.Platform.DBExtract bdbconn = new BizProcess.Platform.DBExtract();

            if (!Request.Form["DeleteBut"].IsNullOrEmpty())
            {
                string deleteID = Request.Form["checkbox_app"];
                System.Text.StringBuilder delxml = new System.Text.StringBuilder();
                foreach (string id in deleteID.Split(','))
                {
                    Guid gid;
                    if (id.IsGuid(out gid))
                    {
                        delxml.Append(bdbconn.Get(gid).Serialize());
                        bdbconn.Delete(gid);
                    }
                }
                bdbconn.ClearCache();
                BizProcess.Platform.Log.Add("删除了数据抽取", delxml.ToString(), BizProcess.Platform.Log.Types.流程相关);
            }
            

            var connList = bdbconn.GetAll();
            ViewBag.Query1 = query1;
            return View(connList);
        }

        public ActionResult Edit()
        {
            return Edit(null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection collection)
        {
            string editid = Request.QueryString["id"];
            BizProcess.Platform.DBExtract bdbConn = new BizProcess.Platform.DBExtract();
            BizProcess.Data.Model.DBExtract dbe = null;
            if (editid.IsGuid())
            {
                dbe = bdbConn.Get(editid.ToGuid());
            }
            bool isAdd = !editid.IsGuid();
            string oldXML = string.Empty;
            if (dbe == null)
            {
                dbe = new BizProcess.Data.Model.DBExtract();
                dbe.ID = Guid.NewGuid();
            }
            else
            {
                oldXML = dbe.Serialize();
            }

            if (collection != null)
            {
                string Name = Request.Form["Name"];
                string Comment = Request.Form["Comment"];
                string DBConnID = Request.Form["DBConnID"];
                string ExtractType = Request.Form["ExtractType"];
                string RunTime = Request.Form["RunTime"];
                string OnlyIncrement = Request.Form["OnlyIncrement"];
                string DesignJSON = "";
                string db_table = Request.Form["db_table"];
                string db_primarykey = Request.Form["db_primarykey"];
                bool bSchemaChanged = false;

                if (dbe.DBConnID != DBConnID.ToGuid())
                {
                    bSchemaChanged = true;
                }

                dbe.Name = Name.Trim();
                dbe.Comment = Comment;
                dbe.DBConnID = DBConnID.ToGuid();

                System.Text.StringBuilder json = new System.Text.StringBuilder();
                json.Append("{");
                json.AppendFormat("\"table\":\"{0}\",", db_table.Trim());
                json.AppendFormat("\"primarykey\":\"{0}\",", db_primarykey.Trim());
                json.Append("\"fields\":[");

                String[] fields = Request.Form.GetValues("link_field[]");
                foreach (String field in fields)
                {
                    json.Append("{");
                    json.AppendFormat("\"field\":\"{0}\"", field.Trim());
                    if (fields.Last() != field)
                    {
                        json.Append("},");
                    }
                    else
                    {
                        json.Append("}");
                    }
                }
                json.AppendFormat("]");
                json.Append("}");
                DesignJSON = json.ToString();

                if (bSchemaChanged && (dbe.DesignJSON == null || !dbe.DesignJSON.Equals(DesignJSON)))
                {
                    bSchemaChanged = true;
                }

                dbe.DesignJSON = DesignJSON;
                dbe.ExtractType = ExtractType == "Auto" ? true : false;
                dbe.RunTime = RunTime;
                dbe.OnlyIncrement = OnlyIncrement == "OnlyIncrement" ? true : false;

                if (isAdd)
                {
                    bdbConn.Add(dbe);
                    BizProcess.Platform.Log.Add("添加了数据抽取", dbe.Serialize(), BizProcess.Platform.Log.Types.流程相关);
                    ViewBag.Script = "alert('添加成功!');new BPUI.Window().reloadOpener();new BPUI.Window().close();";
                }
                else
                {
                    bdbConn.Update(dbe, bSchemaChanged);
                    BizProcess.Platform.Log.Add("修改了数据抽取", "", BizProcess.Platform.Log.Types.流程相关, oldXML, dbe.Serialize());
                    ViewBag.Script = "alert('修改成功!');new BPUI.Window().reloadOpener();new BPUI.Window().close();";
                }
                bdbConn.ClearCache();
            }

            return View(dbe);
        }

        public string Transfer()
        {
            string dbeid = Request.QueryString["id"];
            if (!dbeid.IsGuid())
            {
                return "Invalid ID is required.";
            }

            BizProcess.Platform.DBExtract bdbConn = new BizProcess.Platform.DBExtract();
            BizProcess.Data.Model.DBExtract dbe = null;
            if (dbeid.IsGuid())
            {
                dbe = bdbConn.Get(dbeid.ToGuid());
            }

            int ret = bdbConn.Transfer(dbe);

            if (ret <= 0)
            {
                BizProcess.Platform.Log.Add("An unkown error occured while transfering data.", dbe.Serialize(), BizProcess.Platform.Log.Types.流程相关);
                return "An unkown error occured while transfering data.";
            }

            dbe.LastRunTime = DateTime.Now;
            bdbConn.Update(dbe, false);

            return "Transfer is finished successfully.";
        }

        public string AutoTransfer()
        {

            BizProcess.Platform.DBExtract bdbConn = new BizProcess.Platform.DBExtract();
            List<BizProcess.Data.Model.DBExtract> dbeList = bdbConn.GetAll();
            foreach(BizProcess.Data.Model.DBExtract dbe in dbeList)
            {
                if (dbe.ExtractType)
                {
                    if (doRun(dbe.RunTime, dbe.LastRunTime))
                    {
                        int ret = bdbConn.Transfer(dbe);

                        if (ret <= 0)
                        {
                            BizProcess.Platform.Log.Add("An unkown error occured while auto-transfering data.", dbe.Serialize(), BizProcess.Platform.Log.Types.流程相关);
                        }
                        else
                        {
                            dbe.LastRunTime = DateTime.Now;
                            bdbConn.Update(dbe, false);
                        }
                    }
                }
            }

            return "finished";
        }

        private bool doRun(string runtime, DateTime lastRunTime)
        {
            bool bRet = false;
            int pos = 0;
            string literal = "";
            int diff = 0;

            DateTime now = DateTime.Now;
            DateTime toRunTime;

            runtime = runtime.Replace(" ", "");

            //get toRunTime
            int month, day, hour, min, sec;
            if (runtime.Substring(pos, 1).Equals("*"))
            {
                month = now.Month;
                pos++;
            }
            else
            {
                literal = runtime.Substring(pos, 2);
                month = literal.ToInt();
                diff = 100000;
                pos += 2;
            }

            if (runtime.Substring(pos, 1).Equals("*"))
            {
                day = now.Day;
                pos++;
            }
            else
            {
                literal = runtime.Substring(pos, 2);
                day = literal.ToInt();
                diff = diff == 0 ? 10000 : diff;
                pos += 2;
            }

            if (runtime.Substring(pos, 1).Equals("*"))
            {
                hour = now.Hour;
                pos++;
            }
            else
            {
                literal = runtime.Substring(pos, 2);
                hour = literal.ToInt();
                diff = diff == 0 ? 1000 : diff;
                pos += 2;
            }

            if (runtime.Substring(pos, 1).Equals("*"))
            {
                min = now.Minute;
                pos++;
            }
            else
            {
                literal = runtime.Substring(pos, 2);
                min = literal.ToInt();
                diff = diff == 0 ? 100 : diff;
                pos += 2;
            }

            if (runtime.Substring(pos, 1).Equals("*"))
            {
                sec = now.Second;
                diff = 1;
                pos++;
            }
            else
            {
                literal = runtime.Substring(pos, 2);
                sec = literal.ToInt();
                diff = diff == 0 ? 10 : diff;
                pos += 2;
            }
            toRunTime = new DateTime(now.Year, month, day, hour, min, sec);
            if (lastRunTime == null)
            {
                lastRunTime = now.AddYears(-1);
            }
            TimeSpan span1 = toRunTime.Subtract(lastRunTime);

            //check if lastRunTime indicates that it already launched
            if (span1.TotalSeconds > 0)
            {
                DateTime prevRunTime;

                switch (diff)
                {
                    case 100000:
                        prevRunTime = toRunTime.AddYears(-1);
                        break;
                    case 10000:
                        prevRunTime = toRunTime.AddMonths(-1);
                        break;
                    case 1000:
                        prevRunTime = toRunTime.AddDays(-1);
                        break;
                    case 100:
                        prevRunTime = toRunTime.AddHours(-1);
                        break;
                    case 10:
                        prevRunTime = toRunTime.AddMinutes(-1);
                        break;
                    case 1:
                    default:
                        prevRunTime = toRunTime.AddSeconds(-1);
                        break;
                }
                TimeSpan span2 = prevRunTime.Subtract(lastRunTime);
                if (span2.TotalSeconds <= 0)
                {
                    bRet = true;
                }

            }
            
            return bRet;
        }

    }
}
