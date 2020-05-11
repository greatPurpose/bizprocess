using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace BizProcess.Data.Factory
{
    public class Factory
    {
        /*
        private static string dataType = BizProcess.Utility.Config.DataBaseType;
        private static string cacheKey = BizProcess.Utility.Keys.CacheKeys.ClassInstance_.ToString();
        public static object CreateInstance(string className)
        {
            string cacheKey1 = cacheKey + className;
            string typeName = "BizProcess.Data." + dataType + "." + className;
            object obj = BizProcess.Cache.IO.Opation.Get(cacheKey1);
            if (obj == null)
            {
                Type type = Assembly.Load("BizProcess.Data." + dataType).GetType(typeName, true);
                obj = Activator.CreateInstance(type);
             
                BizProcess.Cache.IO.Opation.Set(cacheKey1, obj);
        
                return obj;
            }
            else
            {
                return obj;
            }
        }
        */

        public static Data.Interface.IAppLibrary GetAppLibrary()
        {
            Data.Interface.IAppLibrary iObj = null;

            switch (BizProcess.Utility.Config.DataBaseType)
            {
                case "MSSQL":
                    iObj = new Data.MSSQL.AppLibrary();
                    break;
                case "MYSQL":
                    iObj = new Data.MySQL.AppLibrary();
                    break;
                case "ORACLE":
                default:
                    iObj = new Data.Oracle.AppLibrary();
                    break;
            }
            return iObj;
        }

        public static Data.Interface.IReportTemplate GetReportTemplate()
        {
            Data.Interface.IReportTemplate iObj = null;

            switch (BizProcess.Utility.Config.DataBaseType)
            {
                case "MSSQL":
                    iObj = new Data.MSSQL.ReportTemplate();
                    break;
                case "MYSQL":
                    iObj = new Data.MySQL.ReportTemplate();
                    break;
                case "ORACLE":
                default:
                    iObj = new Data.Oracle.ReportTemplate();
                    break;
            }
            return iObj;
        }

        public static Data.Interface.IDBConnection GetDBConnection()
        {
            Data.Interface.IDBConnection iObj = null;

            switch (BizProcess.Utility.Config.DataBaseType)
            {
                case "MSSQL":
                    iObj = new Data.MSSQL.DBConnection();
                    break;
                case "MYSQL":
                    iObj = new Data.MySQL.DBConnection();
                    break;
                case "ORACLE":
                default:
                    iObj = new Data.Oracle.DBConnection();
                    break;
            }
            return iObj;
        }

        public static Data.Interface.IDBExtract GetDBExtract()
        {
            Data.Interface.IDBExtract iObj = null;

            switch (BizProcess.Utility.Config.DataBaseType)
            {
                case "MSSQL":
                    iObj = new Data.MSSQL.DBExtract();
                    break;
                case "MYSQL":
                    iObj = new Data.MySQL.DBExtract();
                    break;
                case "ORACLE":
                default:
                    iObj = new Data.Oracle.DBExtract();
                    break;
            }
            return iObj;
        }

        public static Data.Interface.IDictionary GetDictionary()
        {
            Data.Interface.IDictionary iObj = null;

            switch (BizProcess.Utility.Config.DataBaseType)
            {
                case "MSSQL":
                    iObj = new Data.MSSQL.Dictionary();
                    break;
                case "MYSQL":
                    iObj = new Data.MySQL.Dictionary();
                    break;
                case "ORACLE":
                default:
                    iObj = new Data.Oracle.Dictionary();
                    break;
            }
            return iObj;
        }

        public static Data.Interface.ILog GetLog()
        {
            Data.Interface.ILog iObj = null;

            switch (BizProcess.Utility.Config.DataBaseType)
            {
                case "MSSQL":
                    iObj = new Data.MSSQL.Log();
                    break;
                case "MYSQL":
                    iObj = new Data.MySQL.Log();
                    break;
                case "ORACLE":
                default:
                    iObj = new Data.Oracle.Log();
                    break;
            }
            return iObj;
        }

        public static Data.Interface.IOrganize GetOrganize()
        {
            Data.Interface.IOrganize iObj = null;

            switch (BizProcess.Utility.Config.DataBaseType)
            {
                case "MSSQL":
                    iObj = new Data.MSSQL.Organize();
                    break;
                case "MYSQL":
                    iObj = new Data.MySQL.Organize();
                    break;
                case "ORACLE":
                default:
                    iObj = new Data.Oracle.Organize();
                    break;
            }
            return iObj;
        }

        public static Data.Interface.IRole GetRole()
        {
            Data.Interface.IRole iObj = null;

            switch (BizProcess.Utility.Config.DataBaseType)
            {
                case "MSSQL":
                    iObj = new Data.MSSQL.Role();
                    break;
                case "MYSQL":
                    iObj = new Data.MySQL.Role();
                    break;
                case "ORACLE":
                default:
                    iObj = new Data.Oracle.Role();
                    break;
            }
            return iObj;
        }

        public static Data.Interface.IRoleApp GetRoleApp()
        {
            Data.Interface.IRoleApp iObj = null;

            switch (BizProcess.Utility.Config.DataBaseType)
            {
                case "MSSQL":
                    iObj = new Data.MSSQL.RoleApp();
                    break;
                case "MYSQL":
                    iObj = new Data.MySQL.RoleApp();
                    break;
                case "ORACLE":
                default:
                    iObj = new Data.Oracle.RoleApp();
                    break;
            }
            return iObj;
        }

        public static Data.Interface.IUsers GetUsers()
        {
            Data.Interface.IUsers iObj = null;

            switch (BizProcess.Utility.Config.DataBaseType)
            {
                case "MSSQL":
                    iObj = new Data.MSSQL.Users();
                    break;
                case "MYSQL":
                    iObj = new Data.MySQL.Users();
                    break;
                case "ORACLE":
                default:
                    iObj = new Data.Oracle.Users();
                    break;
            }
            return iObj;
        }

        public static Data.Interface.IUsersApp GetUsersApp()
        {
            Data.Interface.IUsersApp iObj = null;

            switch (BizProcess.Utility.Config.DataBaseType)
            {
                case "MSSQL":
                    iObj = new Data.MSSQL.UsersApp();
                    break;
                case "MYSQL":
                    iObj = new Data.MySQL.UsersApp();
                    break;
                case "ORACLE":
                default:
                    iObj = new Data.Oracle.UsersApp();
                    break;
            }
            return iObj;
        }

        public static Data.Interface.IUsersInfo GetUsersInfo()
        {
            Data.Interface.IUsersInfo iObj = null;

            switch (BizProcess.Utility.Config.DataBaseType)
            {
                case "MSSQL":
                    iObj = new Data.MSSQL.UsersInfo();
                    break;
                case "MYSQL":
                    iObj = new Data.MySQL.UsersInfo();
                    break;
                case "ORACLE":
                default:
                    iObj = new Data.Oracle.UsersInfo();
                    break;
            }
            return iObj;
        }

        public static Data.Interface.IUsersRelation GetUsersRelation()
        {
            Data.Interface.IUsersRelation iObj = null;

            switch (BizProcess.Utility.Config.DataBaseType)
            {
                case "MSSQL":
                    iObj = new Data.MSSQL.UsersRelation();
                    break;
                case "MYSQL":
                    iObj = new Data.MySQL.UsersRelation();
                    break;
                case "ORACLE":
                default:
                    iObj = new Data.Oracle.UsersRelation();
                    break;
            }
            return iObj;
        }

        public static Data.Interface.IUsersRole GetUsersRole()
        {
            Data.Interface.IUsersRole iObj = null;

            switch (BizProcess.Utility.Config.DataBaseType)
            {
                case "MSSQL":
                    iObj = new Data.MSSQL.UsersRole();
                    break;
                case "MYSQL":
                    iObj = new Data.MySQL.UsersRole();
                    break;
                case "ORACLE":
                default:
                    iObj = new Data.Oracle.UsersRole();
                    break;
            }
            return iObj;
        }

        public static Data.Interface.IWorkFlow GetWorkFlow()
        {
            Data.Interface.IWorkFlow iObj = null;

            switch (BizProcess.Utility.Config.DataBaseType)
            {
                case "MSSQL":
                    iObj = new Data.MSSQL.WorkFlow();
                    break;
                case "MYSQL":
                    iObj = new Data.MySQL.WorkFlow();
                    break;
                case "ORACLE":
                default:
                    iObj = new Data.Oracle.WorkFlow();
                    break;
            }
            return iObj;
        }

        public static Data.Interface.IWorkFlowArchives GetWorkFlowArchives()
        {
            Data.Interface.IWorkFlowArchives iObj = null;

            switch (BizProcess.Utility.Config.DataBaseType)
            {
                case "MSSQL":
                    iObj = new Data.MSSQL.WorkFlowArchives();
                    break;
                case "MYSQL":
                    iObj = new Data.MySQL.WorkFlowArchives();
                    break;
                case "ORACLE":
                default:
                    iObj = new Data.Oracle.WorkFlowArchives();
                    break;
            }
            return iObj;
        }

        public static Data.Interface.IWorkFlowButtons GetWorkFlowButtons()
        {
            Data.Interface.IWorkFlowButtons iObj = null;

            switch (BizProcess.Utility.Config.DataBaseType)
            {
                case "MSSQL":
                    iObj = new Data.MSSQL.WorkFlowButtons();
                    break;
                case "MYSQL":
                    iObj = new Data.MySQL.WorkFlowButtons();
                    break;
                case "ORACLE":
                default:
                    iObj = new Data.Oracle.WorkFlowButtons();
                    break;
            }
            return iObj;
        }

        public static Data.Interface.IWorkFlowComment GetWorkFlowComment()
        {
            Data.Interface.IWorkFlowComment iObj = null;

            switch (BizProcess.Utility.Config.DataBaseType)
            {
                case "MSSQL":
                    iObj = new Data.MSSQL.WorkFlowComment();
                    break;
                case "MYSQL":
                    iObj = new Data.MySQL.WorkFlowComment();
                    break;
                case "ORACLE":
                default:
                    iObj = new Data.Oracle.WorkFlowComment();
                    break;
            }
            return iObj;
        }

        public static Data.Interface.IWorkFlowData GetWorkFlowData()
        {
            Data.Interface.IWorkFlowData iObj = null;

            switch (BizProcess.Utility.Config.DataBaseType)
            {
                case "MSSQL":
                    iObj = new Data.MSSQL.WorkFlowData();
                    break;
                case "MYSQL":
                    iObj = new Data.MySQL.WorkFlowData();
                    break;
                case "ORACLE":
                default:
                    iObj = new Data.Oracle.WorkFlowData();
                    break;
            }
            return iObj;
        }

        public static Data.Interface.IWorkFlowDelegation GetWorkFlowDelegation()
        {
            Data.Interface.IWorkFlowDelegation iObj = null;

            switch (BizProcess.Utility.Config.DataBaseType)
            {
                case "MSSQL":
                    iObj = new Data.MSSQL.WorkFlowDelegation();
                    break;
                case "MYSQL":
                    iObj = new Data.MySQL.WorkFlowDelegation();
                    break;
                case "ORACLE":
                default:
                    iObj = new Data.Oracle.WorkFlowDelegation();
                    break;
            }
            return iObj;
        }

        public static Data.Interface.IWorkFlowForm GetWorkFlowForm()
        {
            Data.Interface.IWorkFlowForm iObj = null;

            switch (BizProcess.Utility.Config.DataBaseType)
            {
                case "MSSQL":
                    iObj = new Data.MSSQL.WorkFlowForm();
                    break;
                case "MYSQL":
                    iObj = new Data.MySQL.WorkFlowForm();
                    break;
                case "ORACLE":
                default:
                    iObj = new Data.Oracle.WorkFlowForm();
                    break;
            }
            return iObj;
        }

        public static Data.Interface.IWorkFlowTask GetWorkFlowTask()
        {
            Data.Interface.IWorkFlowTask iObj = null;

            switch (BizProcess.Utility.Config.DataBaseType)
            {
                case "MSSQL":
                    iObj = new Data.MSSQL.WorkFlowTask();
                    break;
                case "MYSQL":
                    iObj = new Data.MySQL.WorkFlowTask();
                    break;
                case "ORACLE":
                default:
                    iObj = new Data.Oracle.WorkFlowTask();
                    break;
            }
            return iObj;
        }

        public static Data.Interface.IWorkGroup GetWorkGroup()
        {
            Data.Interface.IWorkGroup iObj = null;

            switch (BizProcess.Utility.Config.DataBaseType)
            {
                case "MSSQL":
                    iObj = new Data.MSSQL.WorkGroup();
                    break;
                case "MYSQL":
                    iObj = new Data.MySQL.WorkGroup();
                    break;
                case "ORACLE":
                default:
                    iObj = new Data.Oracle.WorkGroup();
                    break;
            }
            return iObj;
        }
    }
}
