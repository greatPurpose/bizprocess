using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BizProcess.Data.MSSQL
{
    public class DBExtract : BizProcess.Data.Interface.IDBExtract
    {
        private DBHelper dbHelper = new DBHelper();
        /// <summary>
        /// 构造函数
        /// </summary>
        public DBExtract()
        {
        }
        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="model">BizProcess.Data.Model.DBExtract实体类</param>
        /// <returns>操作所影响的行数</returns>
        public int Add(BizProcess.Data.Model.DBExtract model)
        {
            string sql = @"INSERT INTO DBExtract
				(ID,Name,Comment,DBConnID,DesignJSON,ExtractType,RunTime,OnlyIncrement) 
				VALUES(@ID,@Name,@Comment,@DBConnID,@DesignJSON,@ExtractType,@RunTime,@OnlyIncrement,@LastRunTime)";
            SqlParameter[] parameters = new SqlParameter[]{
				new SqlParameter("@ID", SqlDbType.UniqueIdentifier, -1){ Value = model.ID },
				new SqlParameter("@Name", SqlDbType.VarChar, 50){ Value = model.Name },
                model.Comment == null ? new SqlParameter("@Comment", SqlDbType.VarChar, -1) { Value = DBNull.Value } : new SqlParameter("@Comment", SqlDbType.VarChar, -1) { Value = model.Comment },
                model.DBConnID == null ? new SqlParameter("@DBConnID", SqlDbType.UniqueIdentifier, -1) { Value = DBNull.Value } : new SqlParameter("@DBConnID", SqlDbType.UniqueIdentifier, -1) { Value = model.DBConnID },
                model.DesignJSON == null ? new SqlParameter("@DesignJSON", SqlDbType.VarChar, -1) { Value = DBNull.Value } : new SqlParameter("@DesignJSON", SqlDbType.VarChar, -1) { Value = model.DesignJSON },
                new SqlParameter("@ExtractType", SqlDbType.Bit, -1){ Value = model.ExtractType },
                model.RunTime == null ? new SqlParameter("@RunTime", SqlDbType.VarChar, -1) { Value = DBNull.Value } : new SqlParameter("@RunTime", SqlDbType.VarChar, -1) { Value = model.RunTime },
                new SqlParameter("@OnlyIncrement", SqlDbType.Bit, -1){ Value = model.OnlyIncrement },
                model.LastRunTime == null ? new SqlParameter(":LastRunTime", SqlDbType.DateTime) { Value = DBNull.Value } : new SqlParameter(":LastRunTime", SqlDbType.DateTime) { Value = model.LastRunTime }
			};
            return dbHelper.Execute(sql, parameters);
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="model">BizProcess.Data.Model.DBExtract实体类</param>
        public int Update(BizProcess.Data.Model.DBExtract model)
        {
            string sql = @"UPDATE DBExtract SET 
				Name=@Name,Comment=@Comment,DBConnID=@DBConnID,DesignJSON=@DesignJSON,ExtractType=@ExtractType,RunTime=@RunTime,OnlyIncrement=@OnlyIncrement,LastRunTime=@LastRunTime
				WHERE ID=@ID";
            SqlParameter[] parameters = new SqlParameter[]{
				new SqlParameter("@Name", SqlDbType.VarChar, 50){ Value = model.Name },
                model.Comment == null ? new SqlParameter("@Comment", SqlDbType.VarChar, -1) { Value = DBNull.Value } : new SqlParameter("@Comment", SqlDbType.VarChar, -1) { Value = model.Comment },
                model.DBConnID == null ? new SqlParameter("@DBConnID", SqlDbType.UniqueIdentifier, -1) { Value = DBNull.Value } : new SqlParameter("@DBConnID", SqlDbType.UniqueIdentifier, -1) { Value = model.DBConnID },
                model.DesignJSON == null ? new SqlParameter("@DesignJSON", SqlDbType.VarChar, -1) { Value = DBNull.Value } : new SqlParameter("@DesignJSON", SqlDbType.VarChar, -1) { Value = model.DesignJSON },
                new SqlParameter("@ExtractType", SqlDbType.Bit, -1){ Value = model.ExtractType },
                model.RunTime == null ? new SqlParameter("@RunTime", SqlDbType.VarChar, -1) { Value = DBNull.Value } : new SqlParameter("@RunTime", SqlDbType.VarChar, -1) { Value = model.RunTime },
                new SqlParameter("@OnlyIncrement", SqlDbType.Bit, -1){ Value = model.OnlyIncrement },
				new SqlParameter("@ID", SqlDbType.UniqueIdentifier, -1){ Value = model.ID },
                model.LastRunTime == null ? new SqlParameter(":LastRunTime", SqlDbType.DateTime) { Value = DBNull.Value } : new SqlParameter(":LastRunTime", SqlDbType.DateTime) { Value = model.LastRunTime }
			};
            return dbHelper.Execute(sql, parameters);
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        public int Delete(Guid id)
        {
            string sql = "DELETE FROM DBExtract WHERE ID=@ID";
            SqlParameter[] parameters = new SqlParameter[]{
				new SqlParameter("@ID", SqlDbType.UniqueIdentifier){ Value = id }
			};
            return dbHelper.Execute(sql, parameters);
        }
        /// <summary>
        /// 将DataRedar转换为List
        /// </summary>
        private List<BizProcess.Data.Model.DBExtract> DataReaderToList(SqlDataReader dataReader)
        {
            List<BizProcess.Data.Model.DBExtract> List = new List<BizProcess.Data.Model.DBExtract>();
            BizProcess.Data.Model.DBExtract model = null;
            while (dataReader.Read())
            {
                model = new BizProcess.Data.Model.DBExtract();
                model.ID = dataReader.GetGuid(0);
                model.Name = dataReader.GetString(1);
                if (!dataReader.IsDBNull(2))
                    model.Comment = dataReader.GetString(2);
                if (!dataReader.IsDBNull(3))
                    model.DBConnID = dataReader.GetGuid(3);
                if (!dataReader.IsDBNull(4))
                    model.DesignJSON = dataReader.GetString(4);
                model.ExtractType = dataReader.GetBoolean(5);
                if (!dataReader.IsDBNull(6))
                    model.RunTime = dataReader.GetString(6);
                model.OnlyIncrement = dataReader.GetBoolean(7);
                if (!dataReader.IsDBNull(8))
                    model.LastRunTime = dataReader.GetDateTime(8);
                List.Add(model);
            }
            return List;
        }
        /// <summary>
        /// 查询所有记录
        /// </summary>
        public List<BizProcess.Data.Model.DBExtract> GetAll()
        {
            string sql = "SELECT * FROM DBExtract";
            SqlDataReader dataReader = dbHelper.GetDataReader(sql);
            List<BizProcess.Data.Model.DBExtract> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List;
        }
        /// <summary>
        /// 查询记录数
        /// </summary>
        public long GetCount()
        {
            string sql = "SELECT COUNT(*) FROM DBExtract";
            long count;
            return long.TryParse(dbHelper.GetFieldValue(sql), out count) ? count : 0;
        }
        /// <summary>
        /// 根据主键查询一条记录
        /// </summary>
        public BizProcess.Data.Model.DBExtract Get(Guid id)
        {
            string sql = "SELECT * FROM DBExtract WHERE ID=@ID";
            SqlParameter[] parameters = new SqlParameter[]{
				new SqlParameter("@ID", SqlDbType.UniqueIdentifier){ Value = id }
			};
            SqlDataReader dataReader = dbHelper.GetDataReader(sql, parameters);
            List<BizProcess.Data.Model.DBExtract> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List.Count > 0 ? List[0] : null;
        }

        public int ExecuteStatement(string sql)
        {
            return dbHelper.Execute(sql);
        }

        public int ExecuteStatement(List<string> sql)
        {
            return dbHelper.Execute(sql);
        }
    }
}