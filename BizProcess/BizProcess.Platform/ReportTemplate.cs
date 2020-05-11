using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using System.Web.UI.WebControls;

namespace BizProcess.Platform
{
    public class ReportTemplate
    {
        private string cacheKey = BizProcess.Utility.Keys.CacheKeys.ReportTemplate.ToString();
        private BizProcess.Data.Interface.IReportTemplate dataReportTemplate;
        public ReportTemplate()
        {
            this.dataReportTemplate = Data.Factory.Factory.GetReportTemplate();
        }
        /// <summary>
        /// 新增
        /// </summary>
        public int Add(BizProcess.Data.Model.ReportTemplate model)
        {
            return dataReportTemplate.Add(model);
        }
        /// <summary>
        /// 更新
        /// </summary>
        public int Update(BizProcess.Data.Model.ReportTemplate model)
        {
            return dataReportTemplate.Update(model);
        }
        /// <summary>
        /// 查询所有记录
        /// </summary>
        public List<BizProcess.Data.Model.ReportTemplate> GetAll(bool fromCache = false)
        {
            if (!fromCache)
            {
                return dataReportTemplate.GetAll();
            }
            else
            {
                object obj = BizProcess.Cache.IO.Opation.Get(cacheKey);
                if (obj != null)
                {
                    return obj as List<BizProcess.Data.Model.ReportTemplate>;
                }
                else
                {
                    var list = dataReportTemplate.GetAll();
                    BizProcess.Cache.IO.Opation.Set(cacheKey, list);
                    return list;
                }
            }
        }
        /// <summary>
        /// 查询单条记录
        /// </summary>
        public BizProcess.Data.Model.ReportTemplate Get(Guid id, bool fromCache = false)
        {
            if (!fromCache)
            {
                return dataReportTemplate.Get(id);
            }
            else
            {
                var all = GetAll(true);
                var report = all.Find(p => p.ID == id);
                return report == null ? dataReportTemplate.Get(id) : report;
            }
        }
        /// <summary>
        /// 清除缓存
        /// </summary>
        public void ClearCache()
        {
            BizProcess.Cache.IO.Opation.Remove(cacheKey);
        }
        /// <summary>
        /// 删除
        /// </summary>
        public int Delete(Guid id)
        {
            return dataReportTemplate.Delete(id);
        }
        /// <summary>
        /// 查询记录条数
        /// </summary>
        public long GetCount()
        {
            return dataReportTemplate.GetCount();
        }

        /// <summary>
        /// 得到一页数据
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="size"></param>
        /// <param name="numbe"></param>
        /// <param name="title"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<BizProcess.Data.Model.ReportTemplate> GetPagerData(out string pager, string query = "", string title = "", string type = "")
        {
            return dataReportTemplate.GetPagerData(out pager, query, "Type,Title", BizProcess.Utility.Tools.GetPageSize(),
                BizProcess.Utility.Tools.GetPageNumber(), title, type);
        }
        /// <summary>
        /// 查询一个类别下所有记录
        /// </summary>
        /// 

        public List<BizProcess.Data.Model.ReportTemplate> GetAllByType(Guid type)
        {
            if (type.IsEmptyGuid())
            {
                return new List<BizProcess.Data.Model.ReportTemplate>();
            }
            return dataReportTemplate.GetAllByType(GetAllChildsIDString(type)).OrderBy(p => p.Title).ToList();
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        public int Delete(string[] idArray)
        {
            return dataReportTemplate.Delete(idArray);
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        public int Delete(string idstring)
        {
            return idstring.IsNullOrEmpty() ? 0 : dataReportTemplate.Delete(idstring.Split(','));
        }
        /// <summary>
        /// 得到类型选择项
        /// </summary>
        /// <returns></returns>
        public string GetTypeOptions(string value="")
        {
            return new Dictionary().GetOptionsByCode("ReportTemplateTypes", Dictionary.OptionValueField.ID, value);
        }

        /// <summary>
        /// 得到下级ID字符串
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetAllChildsIDString(Guid id, bool isSelf = true)
        {
            return new Dictionary().GetAllChildsIDString(id, true);
        }

        /// <summary>
        /// 得到一个类型选择项
        /// </summary>
        /// <param name="type">报表类型</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string GetReportTemplatesOptions(Guid type, string value = "")
        {
            if (type.IsEmptyGuid()) return "";
            var reports = GetAllByType(type);
            StringBuilder options = new StringBuilder();
            foreach (var report in reports)
            {
                options.AppendFormat("<option value=\"{0}\" {1}>{2}</option>", report.ID, 
                    string.Compare(report.ID.ToString(), value, true) == 0 ? "selected=\"selected\"" : "",
                    report.Title
                    );
            }
            return options.ToString();
        }
        /// <summary>
        /// 根据ID得到类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetTypeByID(Guid id)
        {
            var report = Get(id);
            return report == null ? "" : report.Type.ToString();
        }

        /// <summary>
        /// 更新报表库使用人员缓存
        /// </summary>
        /// <param name="reportid"></param>
        /// <param name="userIdString"></param>
        public List<Guid> UpdateUseMemberCache(Guid reportid)
        {
            string key = BizProcess.Utility.Keys.CacheKeys.ReportTemplateUseMember.ToString();
            var obj = BizProcess.Cache.IO.Opation.Get(key);
            Dictionary<Guid, List<Guid>> dict;
            if (obj != null && obj is Dictionary<Guid, List<Guid>>)
            {
                dict = obj as Dictionary<Guid, List<Guid>>;
            }
            else
            {
                dict = new Dictionary<Guid, List<Guid>>();
            }
            var report = new ReportTemplate().Get(reportid);
            if (report == null)
            {
                return new List<Guid>();
            }
            if (dict.ContainsKey(reportid))
            {
                if (report.UseMember.IsNullOrEmpty())
                {
                    dict.Remove(reportid);
                    return new List<Guid>();
                }
                else
                {
                    var userIDs = new Organize().GetAllUsersIdList(report.UseMember);
                    dict[reportid] = userIDs;
                    return userIDs;
                }
            }
            else if(!report.UseMember.IsNullOrEmpty())
            {
                var userIDs = new Organize().GetAllUsersIdList(report.UseMember);
                dict.Add(reportid, userIDs);
                return userIDs;
            }
            return new List<Guid>();
        }
        /// <summary>
        /// 得到一个报表库的使用人员
        /// </summary>
        /// <param name="reportid"></param>
        /// <returns></returns>
        public List<Guid> GetUseMemberCache(Guid reportid)
        {
            string key = BizProcess.Utility.Keys.CacheKeys.ReportTemplateUseMember.ToString();
            var obj = BizProcess.Cache.IO.Opation.Get(key);
            if (obj != null && obj is Dictionary<Guid, List<Guid>>)
            {
                var dict = obj as Dictionary<Guid, List<Guid>>;
                if (dict.ContainsKey(reportid))
                {
                    return dict[reportid];
                }
            }
            var report = new ReportTemplate().Get(reportid);
            if (report == null || report.UseMember.IsNullOrEmpty())
            {
                return new List<Guid>();
            }
            return UpdateUseMemberCache(reportid);
        }
        /// <summary>
        /// 清除应用程序库的使用人员缓存
        /// </summary>
        public void ClearUseMemberCache()
        {
            string key = BizProcess.Utility.Keys.CacheKeys.ReportTemplateUseMember.ToString();
            Cache.IO.Opation.Remove(key);
        }

        /// <summary>
        /// 根据sql得到下拉列表项
        /// </summary>
        /// <param name="dbconn"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string GetOptionsFromSql(string connID, string sql, string value)
        {
            Guid cid;
            if (!connID.IsGuid(out cid))
            {
                return "";
            }
            DBConnection dbConn = new DBConnection();
            var dbconn = dbConn.Get(cid);
            if (dbconn == null)
            {
                return "";
            }
            DataTable dt = dbConn.GetDataTable(dbconn, sql.ReplaceSelectSql());
            if (dt.Rows.Count == 0)
            {
                return "";
            }
            List<ListItem> items = new List<ListItem>();
            foreach (DataRow dr in dt.Rows)
            {
                if (dt.Columns.Count == 0)
                {
                    continue;
                }
                string value1 = dr[0].ToString();
                string title = value1;
                if (dt.Columns.Count > 1)
                {
                    title = dr[1].ToString();
                }

                items.Add(new ListItem(title, value1) { Selected = value == value1 });
            }
            return BizProcess.Utility.Tools.GetOptionsString(items.ToArray());
        }

        /// <summary>
        /// 根据sql得到单选按钮组
        /// </summary>
        /// <param name="dbconn"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string GetRadioFromSql(string connID, string sql, string name, string value, string attr = "")
        {
            Guid cid;
            if (!connID.IsGuid(out cid))
            {
                return "";
            }
            DBConnection dbConn = new DBConnection();
            var dbconn = dbConn.Get(cid);
            if (dbconn == null)
            {
                return "";
            }
            DataTable dt = dbConn.GetDataTable(dbconn, sql.ReplaceSelectSql());
            if (dt.Rows.Count == 0)
            {
                return "";
            }
            List<ListItem> items = new List<ListItem>();
            foreach (DataRow dr in dt.Rows)
            {
                if (dt.Columns.Count == 0)
                {
                    continue;
                }
                string value1 = dr[0].ToString();
                string title = value1;
                if (dt.Columns.Count > 1)
                {
                    title = dr[1].ToString();
                }

                items.Add(new ListItem(title, value1) { Selected = value == value1 });
            }
            return BizProcess.Utility.Tools.GetRadioString(items.ToArray(), name, attr);
        }

        /// <summary>
        /// 根据sql得到复选框
        /// </summary>
        /// <param name="dbconn"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string GetCheckboxFromSql(string connID, string sql, string name, string value, string attr = "")
        {
            Guid cid;
            if (!connID.IsGuid(out cid))
            {
                return "";
            }
            DBConnection dbConn = new DBConnection();
            var dbconn = dbConn.Get(cid);
            if (dbconn == null)
            {
                return "";
            }
            DataTable dt = dbConn.GetDataTable(dbconn, sql.ReplaceSelectSql());
            if (dt.Rows.Count == 0)
            {
                return "";
            }
            List<ListItem> items = new List<ListItem>();
            foreach (DataRow dr in dt.Rows)
            {
                if (dt.Columns.Count == 0)
                {
                    continue;
                }
                string value1 = dr[0].ToString();
                string title = value1;
                if (dt.Columns.Count > 1)
                {
                    title = dr[1].ToString();
                }

                items.Add(new ListItem(title, value1));
            }
            return BizProcess.Utility.Tools.GetCheckBoxString(items.ToArray(), name, (value ?? "").Split(','), attr);
        }
        /// <summary>
        /// 得到Grid的html
        /// </summary>
        /// <param name="dataFormat"></param>
        /// <param name="dataSource"></param>
        /// <param name="dataSource1"></param>
        /// <returns></returns>
        public string GetFormGridHtml(string connID, string dataFormat, string dataSource, string dataSource1)
        {
            if (!dataFormat.IsInt() || !dataSource.IsInt() || dataSource1.IsNullOrEmpty())
            {
                return "";
            }

            switch (dataSource)
            {
                case "0":
                    DBConnection dbConn = new DBConnection();
                    var dbconn = dbConn.Get(connID.ToGuid());
                    if (dbconn == null)
                    {
                        return "";
                    }
                    DataTable dt = dbConn.GetDataTable(dbconn, dataSource1.ReplaceSelectSql());
                    switch (dataFormat)
                    {
                        case "0":
                            return dataTableToHtml(dt);
                        case "1":
                            return dt.Rows.Count > 0 ? dt.Rows[0][0].ToString() : "";
                        case "2":
                            return dt.Rows.Count > 0 ? jsonToHtml(dt.Rows[0][0].ToString()) : "";
                        default:
                            return "";
                    }

                case "1":
                    string str = string.Empty;
                    try
                    {
                        str = BizProcess.Utility.HttpHelper.SendGet(dataSource1);
                        switch (dataFormat)
                        {
                            case "0":
                            case "1":
                                return str;
                            case "2":
                                return jsonToHtml(str);
                            default:
                                return "";
                        }
                    }
                    catch
                    {
                        return "";
                    }
                case "2":
                    BizProcess.Data.Model.WorkFlowCustomEventParams eventParams = new BizProcess.Data.Model.WorkFlowCustomEventParams();
                    eventParams.FlowID = (System.Web.HttpContext.Current.Request.QueryString["FlowID"] ?? "").ToGuid();
                    eventParams.GroupID = (System.Web.HttpContext.Current.Request.QueryString["GroupID"] ?? "").ToGuid();
                    eventParams.StepID = (System.Web.HttpContext.Current.Request.QueryString["StepID"] ?? "").ToGuid();
                    eventParams.TaskID = (System.Web.HttpContext.Current.Request.QueryString["TaskID"] ?? "").ToGuid();
                    eventParams.InstanceID = System.Web.HttpContext.Current.Request.QueryString["InstanceID"] ?? "";
                    object obj = null;
                    try
                    {
                        obj = new WorkFlowTask().ExecuteFlowCustomEvent(dataSource1, eventParams);
                        switch (dataFormat)
                        {
                            case "0":
                                return dataTableToHtml((DataTable)obj);
                            case "1":
                                return obj.ToString();
                            case "2":
                                return jsonToHtml(obj.ToString());
                            default:
                                return "";
                        }
                    }
                    catch
                    {
                        return "";
                    }
            }

            return "";
        }

        /// <summary>
        /// 将一个DataTable转换为HTML表格
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string dataTableToHtml(DataTable dt)
        {
            StringBuilder table = new StringBuilder(2000);
            table.Append("<table border=\"1\" style=\"border-collapse:collapse;width:100%;\">");
            table.Append("<thead><tr style=\"height:25px;\">");
            foreach (DataColumn column in dt.Columns)
            {
                table.AppendFormat("<th>{0}</th>", column.ColumnName);
            }
            table.Append("</tr></thead>");
            table.Append("<tbody>");
            foreach (DataRow dr in dt.Rows)
            {
                table.Append("<tr style=\"height:22px;\">");
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    table.AppendFormat("<td>{0}</td>", dr[i].ToString().HtmlEncode());
                }
                table.Append("</tr>");
            }
            table.Append("</tbody>");
            table.Append("</table>");
            return table.ToString();
        }

        /// <summary>
        /// 将json转换为HTML表格
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private string jsonToHtml(string jsonStr)
        {
            LitJson.JsonData json = LitJson.JsonMapper.ToObject(jsonStr);
            if (!json.IsArray)
            {
                return "";
            }
            StringBuilder table = new StringBuilder(2000);
            table.Append("<table border=\"1\" style=\"border-collapse:collapse;width:100%;\">");
            table.Append("<tbody><tr style=\"height:25px;\">");
            foreach (LitJson.JsonData tr in json)
            {
                table.Append("<tr style=\"height:22px;\">");
                foreach (LitJson.JsonData td in tr)
                {
                    table.AppendFormat("<td>{0}</td>", td.ToString());
                }
                table.Append("</tr>");
            }
            table.Append("</tbody>");
            table.Append("</table>");
            return table.ToString();
        }
        /// <summary>
        /// 得到默认值下拉选项字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string GetDefaultValueSelect(string value)
        {
            StringBuilder options = new StringBuilder(1000);
            options.Append("<option value=\"\"></option>");
            options.Append("<optgroup label=\"组织机构相关选项\"></optgroup>");
            options.AppendFormat("<option value=\"u_@BizProcess.Platform.Users.CurrentUserID.ToString()\" {0}>当前步骤用户ID</option>", "10" == value ? "selected=\"selected\"" : "");
            options.AppendFormat("<option value=\"@(BizProcess.Platform.Users.CurrentUserName)\" {0}>当前步骤用户姓名</option>", "11" == value ? "selected=\"selected\"" : "");
            options.AppendFormat("<option value=\"@(BizProcess.Platform.Users.CurrentDeptID)\" {0}>当前步骤用户部门ID</option>", "12" == value ? "selected=\"selected\"" : "");
            options.AppendFormat("<option value=\"@(BizProcess.Platform.Users.CurrentDeptName)\" {0}>当前步骤用户部门名称</option>", "13" == value ? "selected=\"selected\"" : "");
            options.AppendFormat("<option value=\"@(new BizProcess.Platform.Users().GetName(new BizProcess.Platform.WorkFlowTask().GetFirstSnderID(FlowID.ToGuid(), GroupID.ToGuid(), true)))\" {0}>流程发起者姓名</option>", "15" == value ? "selected=\"selected\"" : "");
            options.Append("<optgroup label=\"日期时间相关选项\"></optgroup>");
            options.AppendFormat("<option value=\"@(BizProcess.Utility.DateTimeNew.ShortDate)\" {0}>短日期格式(2014/4/15)</option>", "20" == value ? "selected=\"selected\"" : "");
            options.AppendFormat("<option value=\"@(BizProcess.Utility.DateTimeNew.LongDate)\" {0}>长日期格式(2014年4月15日)</option>", "21" == value ? "selected=\"selected\"" : "");
            options.AppendFormat("<option value=\"@(BizProcess.Utility.DateTimeNew.ShortTime)\" {0}>短时间格式(23:59)</option>", "22" == value ? "selected=\"selected\"" : "");
            options.AppendFormat("<option value=\"@(BizProcess.Utility.DateTimeNew.LongTime)\" {0}>长时间格式(23时59分)</option>", "23" == value ? "selected=\"selected\"" : "");
            options.AppendFormat("<option value=\"@(BizProcess.Utility.DateTimeNew.ShortDateTime)\" {0}>短日期时间格式(2014/4/15 22:31)</option>", "24" == value ? "selected=\"selected\"" : "");
            options.AppendFormat("<option value=\"@(BizProcess.Utility.DateTimeNew.LongDateTime)\" {0}>长日期时间格式(2014年4月15日 22时31分)</option>", "25" == value ? "selected=\"selected\"" : "");
            return options.ToString();
        }

    }
}
