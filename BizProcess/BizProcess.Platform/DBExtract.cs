using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using MySql.Data;
using MySql.Data.MySqlClient;
using Oracle.DataAccess.Client;

namespace BizProcess.Platform
{
    public class DBExtract
    {
        private BizProcess.Data.Interface.IDBExtract dataDBExtract;
        public DBExtract()
        {
            this.dataDBExtract = Data.Factory.Factory.GetDBExtract();
        }

        /// <summary>
        /// 新增
        /// </summary>
        public int Add(BizProcess.Data.Model.DBExtract model)
        {
            int i = dataDBExtract.Add(model);
            if (i > 0) {
                i = CreateTable(model);
            }
            
            ClearCache();
            return i;
        }
        /// <summary>
        /// 更新
        /// </summary>
        public int Update(BizProcess.Data.Model.DBExtract model, bool tableOperation=true)
        {
            int i = dataDBExtract.Update(model);
            if (i > 0 && tableOperation)
            {
                i = dropTable(model);
                i = CreateTable(model);
            }
            ClearCache();
            return i;
        }
        /// <summary>
        /// 查询所有记录
        /// </summary>
        public List<BizProcess.Data.Model.DBExtract> GetAll(bool fromCache = false)
        {
            if (!fromCache)
            {
                return dataDBExtract.GetAll();
            }
            else
            {
                string key = BizProcess.Utility.Keys.CacheKeys.DBExtracts.ToString();
                object obj = BizProcess.Cache.IO.Opation.Get(key);
                if (obj != null && obj is List<BizProcess.Data.Model.DBExtract>)
                {
                    return obj as List<BizProcess.Data.Model.DBExtract>;
                }
                else
                {
                    var list = dataDBExtract.GetAll();
                    BizProcess.Cache.IO.Opation.Set(key, list);
                    return list;
                }
            }
        }
        /// <summary>
        /// 查询单条记录
        /// </summary>
        public BizProcess.Data.Model.DBExtract Get(Guid id)
        {
            return dataDBExtract.Get(id);
        }
        /// <summary>
        /// 删除
        /// </summary>
        public int Delete(Guid id)
        {
            BizProcess.Data.Model.DBExtract model = dataDBExtract.Get(id);
            dropTable(model);

            int i = dataDBExtract.Delete(id);

            ClearCache();
            return i;
        }
        /// <summary>
        /// 查询记录条数
        /// </summary>
        public long GetCount()
        {
            return dataDBExtract.GetCount();
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        public void ClearCache()
        {
            string key = BizProcess.Utility.Keys.CacheKeys.DBExtracts.ToString();
            BizProcess.Cache.IO.Opation.Remove(key);
        }

        #region create table

        public int CreateTable(BizProcess.Data.Model.DBExtract dbe)
        {
            int ret = 0;

            switch (BizProcess.Utility.Config.DataBaseType)
            {
                case "MSSQL":
                    ret = createTable_SqlServer(dbe);
                    break;
                case "ORACLE":
                    ret = createTable_Oracle(dbe);
                    break;
                case "MYSQL":
                    //ret = createTable_MySql(dbe);
                    break;
            }

            return ret;
        }

        private int createTable_SqlServer(BizProcess.Data.Model.DBExtract dbe)
        {
            System.Text.StringBuilder sql = new System.Text.StringBuilder();
            sql.AppendFormat("CREATE TABLE {0}(", dbe.Name);
            //sql.AppendFormat("", dbe.Comment);

            var jsonData = LitJson.JsonMapper.ToObject(dbe.DesignJSON);
            BizProcess.Platform.DBConnection bdbConn = new BizProcess.Platform.DBConnection();
            BizProcess.Data.Model.DBConnection conn = bdbConn.Get(dbe.DBConnID);
            using (System.Data.IDbConnection iconn = bdbConn.GetConnection(conn))
            {
                try
                {
                    if (iconn.State == ConnectionState.Closed)
                    {
                        iconn.Open();
                    }

                    System.Data.DataTable schemaDt = bdbConn.GetTableSchema(iconn, jsonData["table"].ToString(), conn.Type);

                    //primary key field
                    string fieldname = jsonData["primarykey"].ToString();
                    System.Data.DataRow[] schemaDrs = schemaDt.Select(string.Format("f_name='{0}'", fieldname));
                    if (schemaDrs.Length == 0)
                    {
                        return 0;
                    }
                    sql.Append(getFieldString_SqlServer(schemaDrs));

                    //fields
                    var fields = LitJson.JsonMapper.ToObject(jsonData["fields"].ToJson());
                    foreach (LitJson.JsonData field in fields)
                    {
                        fieldname = field["field"].ToString();
                        schemaDrs = schemaDt.Select(string.Format("f_name='{0}'", fieldname));
                        if (schemaDrs.Length == 0)
                        {
                            return 0;
                        }
                        sql.Append(getFieldString_SqlServer(schemaDrs));
                    }
                    sql.AppendFormat("CONSTRAINT [PK_{0}] PRIMARY KEY CLUSTERED ([{1}] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]",
                        dbe.Name,
                        jsonData["primarykey"].ToString());
                }
                catch(SqlException ex)
                {

                }
                    
            }

            //create table into the system database
            return dataDBExtract.ExecuteStatement(sql.ToString());
        }

        private string getFieldString_SqlServer(System.Data.DataRow[] schemaDrs)
        {
            StringBuilder str = new StringBuilder();

            string type = schemaDrs[0]["t_name"].ToString();
            string length = schemaDrs[0]["length"].ToString();

            //from mysql
            if (type.Equals("text"))
            {
                type = "varchar";
                length = "-1";
            }

            //field, type
            str.AppendFormat("[{0}] [{1}]", schemaDrs[0]["f_name"].ToString(), type);

            //length
            if (!type.Equals("uniqueidentifier")
                && !type.Equals("datetime")
                && !type.Equals("int"))
            {
                if (length.Equals("-1"))
                {
                    str.Append("(MAX)");
                }
                else if (type == "nvarchar")
                {
                    str.AppendFormat("({0})", length.ToInt16() / 2);
                }
                else
                {
                    str.AppendFormat("({0})", length);
                }
            }

            //is_nullable
            string isNull = schemaDrs[0]["is_null"].ToString();
            str.AppendFormat(" {0}", (isNull.Equals("1")?"NULL":"NOT NULL"));

            str.Append(",");

            return str.ToString();
        }

        private int createTable_Oracle(BizProcess.Data.Model.DBExtract dbe)
        {
            System.Text.StringBuilder sql = new System.Text.StringBuilder();
            sql.AppendFormat("CREATE TABLE {0}(", dbe.Name);
            //sql.AppendFormat("", dbe.Comment);

            var jsonData = LitJson.JsonMapper.ToObject(dbe.DesignJSON);
            BizProcess.Platform.DBConnection bdbConn = new BizProcess.Platform.DBConnection();
            BizProcess.Data.Model.DBConnection conn = bdbConn.Get(dbe.DBConnID);
            using (System.Data.IDbConnection iconn = bdbConn.GetConnection(conn))
            {
                try
                {
                    if (iconn.State == ConnectionState.Closed)
                    {
                        iconn.Open();
                    }

                    System.Data.DataTable schemaDt = bdbConn.GetTableSchema(iconn, jsonData["table"].ToString(), conn.Type);

                    //primary key field
                    string fieldname = jsonData["primarykey"].ToString();
                    System.Data.DataRow[] schemaDrs = schemaDt.Select(string.Format("f_name='{0}'", fieldname));
                    if (schemaDrs.Length == 0)
                    {
                        return 0;
                    }
                    sql.Append(getFieldString_Oracle(schemaDrs));

                    //fields
                    var fields = LitJson.JsonMapper.ToObject(jsonData["fields"].ToJson());
                    foreach (LitJson.JsonData field in fields)
                    {
                        fieldname = field["field"].ToString();
                        schemaDrs = schemaDt.Select(string.Format("f_name='{0}'", fieldname));
                        if (schemaDrs.Length == 0)
                        {
                            return 0;
                        }
                        sql.Append(getFieldString_Oracle(schemaDrs));
                    }
                    sql.AppendFormat("CONSTRAINT {0}_pk PRIMARY KEY ({1}))",
                        dbe.Name,
                        jsonData["primarykey"].ToString());
                }
                catch (OracleException ex)
                {
                }

            }

            //create table into the system database
            return dataDBExtract.ExecuteStatement(sql.ToString());
        }

        private string getFieldString_Oracle(System.Data.DataRow[] schemaDrs)
        {
            StringBuilder str = new StringBuilder();

            string type = schemaDrs[0]["t_name"].ToString();
            string length = schemaDrs[0]["length"].ToString();

            //from oracle
            if (type.ToLower().Equals("clob")
                || type.ToLower().Equals("nclob")
                || type.ToLower().Equals("date"))
            {
                length = "-1";
            }
            //from mysql
            if (type.ToLower().Equals("text"))
            {
                type = "CLOB";
                length = "-1";
            }
            //from mssql
            if (type.ToLower().Equals("uniqueidentifier"))
            {
                type = "CHAR";
                length = "36";
            }
            else if (type.ToLower().Equals("varchar"))
            {
                type = "VARCHAR2";
                if (length.Equals("-1"))
                {
                    type = "CLOB";
                }
            }
            else if (type.ToLower().Equals("nvarchar"))
            {
                type = "NVARCHAR2";
                if (length.Equals("-1"))
                {
                    type = "CLOB";
                }
                else
                {
                    length = (length.ToInt16() / 2).ToString();
                }
            }
            else if (type.ToLower().Equals("int"))
            {
                type = "NUMBER";
                length = "8";
            }
            else if (type.ToLower().Equals("bit"))
            {
                type = "NUMBER";
                length = "4";
            }

            //field, type
            str.AppendFormat("{0} {1}", schemaDrs[0]["f_name"].ToString(), type);

            //length
            if (length.Equals("-1"))
            {
            }
            else
            {
                str.AppendFormat("({0})", length);
            }

            //is_nullable
            string isNull = schemaDrs[0]["is_null"].ToString();
            str.AppendFormat(" {0}", (isNull.Equals("1") ? "NULL" : "NOT NULL"));

            str.Append(",");

            return str.ToString();
        }

        #endregion

        public int dropTable(BizProcess.Data.Model.DBExtract dbe)
        {
            string sql = string.Format("drop table {0}", dbe.Name);
            return dataDBExtract.ExecuteStatement(sql.ToString());
        }

        #region Data Transfer

        public int Transfer(BizProcess.Data.Model.DBExtract dbe)
        {
            //check if the table exists
            if (!TableExists(dbe.Name))
            {
                return -1;  //table doesn't exist
            }

            //read data
            BizProcess.Platform.DBConnection bdbConn = new BizProcess.Platform.DBConnection();
            BizProcess.Data.Model.DBConnection conn = bdbConn.Get(dbe.DBConnID);
            List<string> keys = null;
            if (dbe.OnlyIncrement)
            {
                //get all the primary keys stored in the system database
                keys = GetAllKeys(dbe.Name, dbe.DesignJSON);
                //get the sql which selects records except all the inserted primary keys
            }
            string sql = GetSelectSql(dbe.DesignJSON, keys);
            System.Data.DataTable dt = GetDataTable(conn, sql);

            //transfer data
            List<string> sqlList = GetInsertSql(dbe.Name, dbe.DesignJSON, dt);
            if (sqlList.Count == 0)
            {
                return 1;
            }
            return dataDBExtract.ExecuteStatement(sqlList);
        }

        private List<string> GetAllKeys(string table, string designjson)
        {
            var jsonData = LitJson.JsonMapper.ToObject(designjson);
            string pkey = jsonData["primarykey"].ToString();
            List<string> keys = null;

            string sql = string.Format("SELECT {0} FROM {1}", pkey, table);
            switch (BizProcess.Utility.Config.DataBaseType)
            {
                case "MSSQL":
                    keys = getKeys_SqlServer(BizProcess.Utility.Config.PlatformConnectionStringMSSQL, sql);
                    break;
                case "ORACLE":
                    keys = getKeys_Oracle(BizProcess.Utility.Config.PlatformConnectionStringORACLE, sql);
                    break;
                case "MYSQL":
                    keys = getKeys_MySql(BizProcess.Utility.Config.PlatformConnectionStringMySQL, sql);
                    break;
            }

            return keys;
        }

        private List<string> getKeys_SqlServer(string connectionString, string sql)
        {
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConn.Open();
                }
                catch (SqlException err)
                {
                    Log.Add(err);
                    return new List<string>();
                }
                List<string> keys = new List<string>();
                using (SqlCommand sqlCmd = new SqlCommand(sql, sqlConn))
                {
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        keys.Add("'" + dr.GetString(0) + "'");
                    }
                    dr.Close();
                    return keys;
                }
            }
        }

        private List<string> getKeys_Oracle(string connectionString, string sql)
        {
            using (OracleConnection oraConn = new OracleConnection(connectionString))
            {
                try
                {
                    oraConn.Open();
                }
                catch (OracleException err)
                {
                    Log.Add(err);
                    return new List<string>();
                }
                List<string> keys = new List<string>();
                using (OracleCommand sqlCmd = new OracleCommand(sql, oraConn))
                {
                    OracleDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        keys.Add("'" + dr.GetString(0) + "'");
                    }
                    dr.Close();
                    return keys;
                }
            }
        }

        private List<string> getKeys_MySql(string connectionString, string sql)
        {
            using (MySqlConnection sqlConn = new MySqlConnection(connectionString))
            {
                try
                {
                    sqlConn.Open();
                }
                catch (MySqlException err)
                {
                    Log.Add(err);
                    return new List<string>();
                }
                List<string> keys = new List<string>();
                using (MySqlCommand sqlCmd = new MySqlCommand(sql, sqlConn))
                {
                    MySqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        keys.Add("'" + dr.GetString(0) + "'");
                    }
                    dr.Close();
                    return keys;
                }
            }
        }

        private List<string> GetInsertSql(string table, string designjson, System.Data.DataTable dt)
        {
            var jsonData = LitJson.JsonMapper.ToObject(designjson);
            var fields = LitJson.JsonMapper.ToObject(jsonData["fields"].ToJson());
            List<string> fieldsList = new List<string>();
            fieldsList.Add(jsonData["primarykey"].ToString());
            foreach (LitJson.JsonData field in fields)
            {
                fieldsList.Add(field["field"].ToString());
            }
            string fieldsString = fieldsList.ToString(",");

            List<string> sqlList = new List<string>();

            foreach(DataRow row in dt.Rows)
            {
                List<string> values = new List<string>();
                foreach (var cell in row.ItemArray)
                {
                    values.Add("'" + cell.ToString() + "'");
                }
                sqlList.Add(string.Format("INSERT INTO {0} ({1}) VALUES ({2})", table, fieldsString, values.ToString(",")));
            }

            return sqlList;
        }

        private string GetSelectSql(string designjson, List<string> keys)
        {
            var jsonData = LitJson.JsonMapper.ToObject(designjson);
            string table = jsonData["table"].ToString();
            var fields = LitJson.JsonMapper.ToObject(jsonData["fields"].ToJson());
            string pkey = jsonData["primarykey"].ToString();
            List<string> fieldsList = new List<string>();
            fieldsList.Add(pkey);
            foreach (LitJson.JsonData field in fields)
            {
                fieldsList.Add(field["field"].ToString());
            }
            string fieldsString = fieldsList.ToString(",");
            string sql = string.Format("SELECT {0} FROM {1}", fieldsString, table);
            if (keys != null && keys.Count > 0)
            {
                sql += string.Format(" WHERE {0} NOT IN ({1})", pkey, keys.ToString(","));
            }
            return sql;
        }

        /// <summary>
        /// 根据连接实体得到数据表
        /// </summary>
        /// <param name="linkID"></param>
        /// <returns></returns>
        private System.Data.DataTable GetDataTable(BizProcess.Data.Model.DBConnection dbconn, string sql)
        {
            if (dbconn == null || dbconn.Type.IsNullOrEmpty() || dbconn.ConnectionString.IsNullOrEmpty())
            {
                return null;
            }
            DataTable dt = new DataTable();
            switch (dbconn.Type)
            {
                #region SqlServer
                case "SqlServer":
                    using (SqlConnection conn = new SqlConnection(dbconn.ConnectionString))
                    {
                        try
                        {
                            conn.Open();
                            using (SqlDataAdapter dap = new SqlDataAdapter(sql, conn))
                            {
                                dap.Fill(dt);
                            }
                        }
                        catch (SqlException ex)
                        {
                            Platform.Log.Add(ex);
                        }
                    }
                    break;
                #endregion

                #region Oracle
                case "Oracle":
                    using (OracleConnection conn = new OracleConnection(dbconn.ConnectionString))
                    {
                        try
                        {
                            conn.Open();
                            using (OracleDataAdapter dap = new OracleDataAdapter(sql, conn))
                            {
                                dap.Fill(dt);
                            }
                        }
                        catch (OracleException ex)
                        {
                            Platform.Log.Add(ex);
                        }
                    }
                    break;

                #endregion

                #region MySql
                case "MySql":
                    using (MySqlConnection conn = new MySqlConnection(dbconn.ConnectionString))
                    {
                        try
                        {
                            conn.Open();
                            using (MySqlDataAdapter dap = new MySqlDataAdapter(sql, conn))
                            {
                                dap.Fill(dt);
                            }
                        }
                        catch (MySqlException ex)
                        {
                            Platform.Log.Add(ex);
                        }
                    }
                    break;

                #endregion
            }

            return dt;
        }

        private bool TableExists(string table)
        {
            return GetTables().Contains(table.ToLower());
        }

        /// <summary>
        /// 根据连接ID得到所有表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private List<string> GetTables()
        {
            List<string> tables = new List<string>();
            switch (BizProcess.Utility.Config.DataBaseType)
            {
                case "MSSQL":
                    tables = getTables_SqlServer(BizProcess.Utility.Config.PlatformConnectionStringMSSQL);
                    break;
                case "ORACLE":
                    tables = getTables_Oracle(BizProcess.Utility.Config.PlatformConnectionStringORACLE);
                    break;
                case "MYSQL":
                    tables = getTables_MySql(BizProcess.Utility.Config.PlatformConnectionStringMySQL);
                    break;
            }
            return tables;
        }

        /// <summary>
        /// 得到一个连接所有表
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        private List<string> getTables_SqlServer(string connectionString)
        {
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConn.Open();
                }
                catch (SqlException err)
                {
                    Log.Add(err);
                    return new List<string>();
                }
                List<string> tables = new List<string>();
                string sql = "SELECT name FROM sysobjects WHERE xtype='U' ORDER BY name";
                using (SqlCommand sqlCmd = new SqlCommand(sql, sqlConn))
                {
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        tables.Add(dr.GetString(0).ToLower());
                    }
                    dr.Close();
                    return tables;
                }
            }
        }

        /// <summary>
        /// 得到一个连接所有表(oracle)
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        private List<string> getTables_Oracle(string connectionString)
        {
            using (OracleConnection oraConn = new OracleConnection(connectionString))
            {
                try
                {
                    oraConn.Open();
                }
                catch (OracleException err)
                {
                    Log.Add(err);
                    return new List<string>();
                }
                List<string> tables = new List<string>();
                string sql = "select * from tab where instr(tname,'$',1,1)=0";
                using (OracleCommand sqlCmd = new OracleCommand(sql, oraConn))
                {
                    OracleDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        tables.Add(dr.GetString(0).ToLower());
                    }
                    dr.Close();
                    return tables;
                }
            }
        }

        /// <summary>
        /// 得到一个连接所有表(MySql)
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        private List<string> getTables_MySql(string connectionString)
        {
            using (MySqlConnection sqlConn = new MySqlConnection(connectionString))
            {
                try
                {
                    sqlConn.Open();
                }
                catch (MySqlException err)
                {
                    Log.Add(err);
                    return new List<string>();
                }
                List<string> tables = new List<string>();
                string sql = "SHOW TABLES";
                using (MySqlCommand sqlCmd = new MySqlCommand(sql, sqlConn))
                {
                    MySqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        tables.Add(dr.GetString(0).ToLower());
                    }
                    dr.Close();
                    return tables;
                }
            }
        }

        #endregion
    }
}
