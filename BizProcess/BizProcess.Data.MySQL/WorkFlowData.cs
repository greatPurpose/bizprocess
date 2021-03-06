﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace BizProcess.Data.MySQL
{
    public class WorkFlowData : BizProcess.Data.Interface.IWorkFlowData
    {
        private DBHelper dbHelper = new DBHelper();
        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkFlowData()
        {
        }
        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="model">BizProcess.Data.Model.WorkFlowData实体类</param>
        /// <returns>操作所影响的行数</returns>
        public int Add(BizProcess.Data.Model.WorkFlowData model)
        {
            string sql = @"INSERT INTO WorkFlowData
				(ID,InstanceID,LinkID,TableName,FieldName,Value) 
				VALUES(@ID,@InstanceID,@LinkID,@TableName,@FieldName,@Value)";
            MySqlParameter[] parameters = new MySqlParameter[]{
				new MySqlParameter("@ID", MySqlDbType.VarChar, -1){ Value = model.ID },
				new MySqlParameter("@InstanceID", MySqlDbType.VarChar, -1){ Value = model.InstanceID },
				new MySqlParameter("@LinkID", MySqlDbType.VarChar, -1){ Value = model.LinkID },
				new MySqlParameter("@TableName", MySqlDbType.VarChar, 500){ Value = model.TableName },
				new MySqlParameter("@FieldName", MySqlDbType.VarChar, 500){ Value = model.FieldName },
				new MySqlParameter("@Value", MySqlDbType.VarChar, 8000){ Value = model.Value }
			};
            return dbHelper.Execute(sql, parameters);
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="model">BizProcess.Data.Model.WorkFlowData实体类</param>
        public int Update(BizProcess.Data.Model.WorkFlowData model)
        {
            string sql = @"UPDATE WorkFlowData SET 
				InstanceID=@InstanceID,LinkID=@LinkID,TableName=@TableName,FieldName=@FieldName,Value=@Value
				WHERE ID=@ID";
            MySqlParameter[] parameters = new MySqlParameter[]{
				new MySqlParameter("@InstanceID", MySqlDbType.VarChar, -1){ Value = model.InstanceID },
				new MySqlParameter("@LinkID", MySqlDbType.VarChar, -1){ Value = model.LinkID },
				new MySqlParameter("@TableName", MySqlDbType.VarChar, 500){ Value = model.TableName },
				new MySqlParameter("@FieldName", MySqlDbType.VarChar, 500){ Value = model.FieldName },
				new MySqlParameter("@Value", MySqlDbType.VarChar, 8000){ Value = model.Value },
				new MySqlParameter("@ID", MySqlDbType.VarChar, -1){ Value = model.ID }
			};
            return dbHelper.Execute(sql, parameters);
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        public int Delete(Guid id)
        {
            string sql = "DELETE FROM WorkFlowData WHERE ID=@ID";
            MySqlParameter[] parameters = new MySqlParameter[]{
				new MySqlParameter("@ID", MySqlDbType.VarChar){ Value = id }
			};
            return dbHelper.Execute(sql, parameters);
        }
        /// <summary>
        /// 将DataRedar转换为List
        /// </summary>
        private List<BizProcess.Data.Model.WorkFlowData> DataReaderToList(MySqlDataReader dataReader)
        {
            List<BizProcess.Data.Model.WorkFlowData> List = new List<BizProcess.Data.Model.WorkFlowData>();
            BizProcess.Data.Model.WorkFlowData model = null;
            while (dataReader.Read())
            {
                model = new BizProcess.Data.Model.WorkFlowData();
                model.ID = dataReader.GetGuid(0);
                model.InstanceID = dataReader.GetGuid(1);
                model.LinkID = dataReader.GetGuid(2);
                model.TableName = dataReader.GetString(3);
                model.FieldName = dataReader.GetString(4);
                model.Value = dataReader.GetString(5);
                List.Add(model);
            }
            return List;
        }
        /// <summary>
        /// 查询所有记录
        /// </summary>
        public List<BizProcess.Data.Model.WorkFlowData> GetAll()
        {
            string sql = "SELECT * FROM WorkFlowData";
            MySqlDataReader dataReader = dbHelper.GetDataReader(sql);
            List<BizProcess.Data.Model.WorkFlowData> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List;
        }
        /// <summary>
        /// 查询记录数
        /// </summary>
        public long GetCount()
        {
            string sql = "SELECT COUNT(*) FROM WorkFlowData";
            long count;
            return long.TryParse(dbHelper.GetFieldValue(sql), out count) ? count : 0;
        }
        /// <summary>
        /// 根据主键查询一条记录
        /// </summary>
        public BizProcess.Data.Model.WorkFlowData Get(Guid id)
        {
            string sql = "SELECT * FROM WorkFlowData WHERE ID=@ID";
            MySqlParameter[] parameters = new MySqlParameter[]{
				new MySqlParameter("@ID", MySqlDbType.VarChar){ Value = id }
			};
            MySqlDataReader dataReader = dbHelper.GetDataReader(sql, parameters);
            List<BizProcess.Data.Model.WorkFlowData> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List.Count > 0 ? List[0] : null;
        }


        /// <summary>
        /// 查询一个实例ID所有记录
        /// </summary>
        public List<BizProcess.Data.Model.WorkFlowData> GetAll(Guid instanceID)
        {
            string sql = "SELECT * FROM WorkFlowData WHERE InstanceID=@InstanceID";
            MySqlParameter[] parameters = new MySqlParameter[]{
				new MySqlParameter("@InstanceID", MySqlDbType.VarChar){ Value = instanceID }
			};
            MySqlDataReader dataReader = dbHelper.GetDataReader(sql, parameters);
            List<BizProcess.Data.Model.WorkFlowData> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List;
        }
    }
}