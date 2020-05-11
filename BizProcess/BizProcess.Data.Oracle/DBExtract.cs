using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Oracle.DataAccess.Client;

namespace BizProcess.Data.Oracle
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
				(ID,Name,""COMMENT"",DBConnID,DesignJSON,ExtractType,RunTime,OnlyIncrement,LastRunTime) 
				VALUES(:ID,:Name,:Comment1,:DBConnID,:DesignJSON,:ExtractType,:RunTime,:OnlyIncrement,:LastRunTime)";
            OracleParameter[] parameters = new OracleParameter[]{
				new OracleParameter(":ID", OracleDbType.Char, 36){ Value = model.ID },
				new OracleParameter(":Name", OracleDbType.Varchar2, 50){ Value = model.Name },
                model.Comment == null ? new OracleParameter(":Comment1", OracleDbType.Varchar2) { Value = DBNull.Value } : new OracleParameter(":Comment1", OracleDbType.Varchar2) { Value = model.Comment },
                model.DBConnID.IsEmptyGuid() ? new OracleParameter(":DBConnID", OracleDbType.Char, 36) { Value = DBNull.Value } : new OracleParameter(":DBConnID", OracleDbType.Char, 36) { Value = model.DBConnID },
                model.DesignJSON == null ? new OracleParameter(":DesignJSON", OracleDbType.Clob) { Value = DBNull.Value } : new OracleParameter(":DesignJSON", OracleDbType.Clob) { Value = model.DesignJSON },
                new OracleParameter(":ExtractType", OracleDbType.Int16){ Value = model.ExtractType?1:0 },
                model.RunTime == null ? new OracleParameter(":RunTime", OracleDbType.Varchar2) { Value = DBNull.Value } : new OracleParameter(":RunTime", OracleDbType.Varchar2) { Value = model.RunTime },
                new OracleParameter(":OnlyIncrement", OracleDbType.Int16){ Value = model.OnlyIncrement?1:0 },
                model.LastRunTime == null ? new OracleParameter(":LastRunTime", OracleDbType.Date) { Value = DBNull.Value } : new OracleParameter(":LastRunTime", OracleDbType.Date) { Value = model.LastRunTime }
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
				Name=:Name,""COMMENT""=:Comment1,DBConnID=:DBConnID,DesignJSON=:DesignJSON,ExtractType=:ExtractType,RunTime=:RunTime,OnlyIncrement=:OnlyIncrement,LastRunTime=:LastRunTime
				WHERE ID=:ID";
            OracleParameter[] parameters = new OracleParameter[]{
				new OracleParameter(":Name", OracleDbType.Varchar2, 50){ Value = model.Name },
                model.Comment == null ? new OracleParameter(":Comment1", OracleDbType.Varchar2) { Value = DBNull.Value } : new OracleParameter(":Comment1", OracleDbType.Varchar2) { Value = model.Comment },
                model.DBConnID.IsEmptyGuid() ? new OracleParameter(":DBConnID", OracleDbType.Char, 36) { Value = DBNull.Value } : new OracleParameter(":DBConnID", OracleDbType.Char, 36) { Value = model.DBConnID },
                model.DesignJSON == null ? new OracleParameter(":DesignJSON", OracleDbType.Clob) { Value = DBNull.Value } : new OracleParameter(":DesignJSON", OracleDbType.Clob) { Value = model.DesignJSON },
                new OracleParameter(":ExtractType", OracleDbType.Int16){ Value = model.ExtractType?1:0 },
                model.RunTime == null ? new OracleParameter(":RunTime", OracleDbType.Varchar2) { Value = DBNull.Value } : new OracleParameter(":RunTime", OracleDbType.Varchar2) { Value = model.RunTime },
                new OracleParameter(":OnlyIncrement", OracleDbType.Int16){ Value = model.OnlyIncrement?1:0 },
                model.LastRunTime == null ? new OracleParameter(":LastRunTime", OracleDbType.Date) { Value = DBNull.Value } : new OracleParameter(":LastRunTime", OracleDbType.Date) { Value = model.LastRunTime },
				new OracleParameter(":ID", OracleDbType.Char, 36){ Value = model.ID }
			};
            return dbHelper.Execute(sql, parameters);
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        public int Delete(Guid id)
        {
            string sql = "DELETE FROM DBExtract WHERE ID=:ID";
            OracleParameter[] parameters = new OracleParameter[]{
				new OracleParameter(":ID", OracleDbType.Char){ Value = id }
			};
            return dbHelper.Execute(sql, parameters);
        }
        /// <summary>
        /// 将DataRedar转换为List
        /// </summary>
        private List<BizProcess.Data.Model.DBExtract> DataReaderToList(OracleDataReader dataReader)
        {
            List<BizProcess.Data.Model.DBExtract> List = new List<BizProcess.Data.Model.DBExtract>();
            BizProcess.Data.Model.DBExtract model = null;
            while (dataReader.Read())
            {
                model = new BizProcess.Data.Model.DBExtract();
                model.ID = dataReader.GetString(0).ToGuid();
                model.Name = dataReader.GetString(1);
                if (!dataReader.IsDBNull(2))
                    model.Comment = dataReader.GetString(2);
                if (!dataReader.IsDBNull(3))
                    model.DBConnID = dataReader.GetString(3).ToGuid();
                if (!dataReader.IsDBNull(4))
                    model.DesignJSON = dataReader.GetString(4);
                model.ExtractType = dataReader.GetInt16(5)==1?true:false;
                if (!dataReader.IsDBNull(6))
                    model.RunTime = dataReader.GetString(6);
                model.OnlyIncrement = dataReader.GetInt16(7)==1?true:false;
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
            OracleDataReader dataReader = dbHelper.GetDataReader(sql);
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
            string sql = "SELECT * FROM DBExtract WHERE ID=:ID";
            OracleParameter[] parameters = new OracleParameter[]{
				new OracleParameter(":ID", OracleDbType.Char){ Value = id }
			};
            OracleDataReader dataReader = dbHelper.GetDataReader(sql, parameters);
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