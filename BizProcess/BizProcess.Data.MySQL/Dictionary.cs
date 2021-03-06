﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace BizProcess.Data.MySQL
{
    public class Dictionary : BizProcess.Data.Interface.IDictionary
    {
        private DBHelper dbHelper = new DBHelper();
        /// <summary>
        /// 构造函数
        /// </summary>
        public Dictionary()
        {
        }
        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="model">BizProcess.Data.Model.Dictionary实体类</param>
        /// <returns>操作所影响的行数</returns>
        public int Add(BizProcess.Data.Model.Dictionary model)
        {
            string sql = @"INSERT INTO Dictionary
				(ID,ParentID,Title,Code,Value,Note,Other,Sort) 
				VALUES(@ID,@ParentID,@Title,@Code,@Value,@Note,@Other,@Sort)";
            MySqlParameter[] parameters = new MySqlParameter[]{
				new MySqlParameter("@ID", MySqlDbType.VarChar, 36){ Value = model.ID },
				new MySqlParameter("@ParentID", MySqlDbType.VarChar, 36){ Value = model.ParentID },
				new MySqlParameter("@Title", MySqlDbType.Text, -1){ Value = model.Title },
				model.Code == null ? new MySqlParameter("@Code", MySqlDbType.VarChar, 500) { Value = DBNull.Value } : new MySqlParameter("@Code", MySqlDbType.VarChar, 500) { Value = model.Code },
				model.Value == null ? new MySqlParameter("@Value", MySqlDbType.Text, -1) { Value = DBNull.Value } : new MySqlParameter("@Value", MySqlDbType.Text, -1) { Value = model.Value },
				model.Note == null ? new MySqlParameter("@Note", MySqlDbType.Text, -1) { Value = DBNull.Value } : new MySqlParameter("@Note", MySqlDbType.Text, -1) { Value = model.Note },
				model.Other == null ? new MySqlParameter("@Other", MySqlDbType.Text, -1) { Value = DBNull.Value } : new MySqlParameter("@Other", MySqlDbType.Text, -1) { Value = model.Other },
				new MySqlParameter("@Sort", MySqlDbType.Int32, -1){ Value = model.Sort }
			};
            return dbHelper.Execute(sql, parameters);
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="model">BizProcess.Data.Model.Dictionary实体类</param>
        public int Update(BizProcess.Data.Model.Dictionary model)
        {
            string sql = @"UPDATE Dictionary SET 
				ParentID=@ParentID,Title=@Title,Code=@Code,Value=@Value,Note=@Note,Other=@Other,Sort=@Sort
				WHERE ID=@ID";
            MySqlParameter[] parameters = new MySqlParameter[]{
				new MySqlParameter("@ParentID", MySqlDbType.VarChar, 36){ Value = model.ParentID },
				new MySqlParameter("@Title", MySqlDbType.Text, -1){ Value = model.Title },
				model.Code == null ? new MySqlParameter("@Code", MySqlDbType.VarChar, 500) { Value = DBNull.Value } : new MySqlParameter("@Code", MySqlDbType.VarChar, 500) { Value = model.Code },
				model.Value == null ? new MySqlParameter("@Value", MySqlDbType.Text, -1) { Value = DBNull.Value } : new MySqlParameter("@Value", MySqlDbType.Text, -1) { Value = model.Value },
				model.Note == null ? new MySqlParameter("@Note", MySqlDbType.Text, -1) { Value = DBNull.Value } : new MySqlParameter("@Note", MySqlDbType.Text, -1) { Value = model.Note },
				model.Other == null ? new MySqlParameter("@Other", MySqlDbType.Text, -1) { Value = DBNull.Value } : new MySqlParameter("@Other", MySqlDbType.Text, -1) { Value = model.Other },
				new MySqlParameter("@Sort", MySqlDbType.Int32, -1){ Value = model.Sort },
				new MySqlParameter("@ID", MySqlDbType.VarChar, 36){ Value = model.ID }
			};
            return dbHelper.Execute(sql, parameters);
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        public int Delete(Guid id)
        {
            string sql = "DELETE FROM Dictionary WHERE ID=@ID";
            MySqlParameter[] parameters = new MySqlParameter[]{
				new MySqlParameter("@ID", MySqlDbType.VarChar){ Value = id }
			};
            return dbHelper.Execute(sql, parameters);
        }
        /// <summary>
        /// 将DataRedar转换为List
        /// </summary>
        private List<BizProcess.Data.Model.Dictionary> DataReaderToList(MySqlDataReader dataReader)
        {
            List<BizProcess.Data.Model.Dictionary> List = new List<BizProcess.Data.Model.Dictionary>();
            BizProcess.Data.Model.Dictionary model = null;
            while (dataReader.Read())
            {
                model = new BizProcess.Data.Model.Dictionary();
                model.ID = dataReader.GetGuid(0);
                model.ParentID = dataReader.GetGuid(1);
                model.Title = dataReader.GetString(2);
                if (!dataReader.IsDBNull(3))
                    model.Code = dataReader.GetString(3);
                if (!dataReader.IsDBNull(4))
                    model.Value = dataReader.GetString(4);
                if (!dataReader.IsDBNull(5))
                    model.Note = dataReader.GetString(5);
                if (!dataReader.IsDBNull(6))
                    model.Other = dataReader.GetString(6);
                model.Sort = dataReader.GetInt32(7);
                List.Add(model);
            }
            return List;
        }
        /// <summary>
        /// 查询所有记录
        /// </summary>
        public List<BizProcess.Data.Model.Dictionary> GetAll()
        {
            string sql = "SELECT * FROM Dictionary";
            MySqlDataReader dataReader = dbHelper.GetDataReader(sql);
            List<BizProcess.Data.Model.Dictionary> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List;
        }
        /// <summary>
        /// 查询记录数
        /// </summary>
        public long GetCount()
        {
            string sql = "SELECT COUNT(*) FROM Dictionary";
            long count;
            return long.TryParse(dbHelper.GetFieldValue(sql), out count) ? count : 0;
        }
        /// <summary>
        /// 根据主键查询一条记录
        /// </summary>
        public BizProcess.Data.Model.Dictionary Get(Guid id)
        {
            string sql = "SELECT * FROM Dictionary WHERE ID=@ID";
            MySqlParameter[] parameters = new MySqlParameter[]{
				new MySqlParameter("@ID", MySqlDbType.VarChar){ Value = id }
			};
            MySqlDataReader dataReader = dbHelper.GetDataReader(sql, parameters);
            List<BizProcess.Data.Model.Dictionary> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List.Count > 0 ? List[0] : null;
        }


        /// <summary>
        /// 查询根记录
        /// </summary>
        public BizProcess.Data.Model.Dictionary GetRoot()
        {
            string sql = "SELECT * FROM Dictionary WHERE ParentID=@ParentID";
            MySqlParameter[] parameters = new MySqlParameter[]{
				new MySqlParameter("@ParentID", MySqlDbType.VarChar){ Value = Guid.Empty }
			};
            MySqlDataReader dataReader = dbHelper.GetDataReader(sql, parameters);
            List<BizProcess.Data.Model.Dictionary> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List.Count > 0 ? List[0] : null;
        }

        /// <summary>
        /// 查询下级记录
        /// </summary>
        public List<BizProcess.Data.Model.Dictionary> GetChilds(Guid id)
        {
            string sql = "SELECT * FROM Dictionary WHERE ParentID=@ParentID ORDER BY Sort";
            MySqlParameter[] parameters = new MySqlParameter[]{
				new MySqlParameter("@ParentID", MySqlDbType.VarChar){ Value = id }
			};
            MySqlDataReader dataReader = dbHelper.GetDataReader(sql, parameters);
            List<BizProcess.Data.Model.Dictionary> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List;
        }
        /// <summary>
        /// 查询下级记录
        /// </summary>
        public List<BizProcess.Data.Model.Dictionary> GetChilds(string code)
        {
            string sql = "SELECT * FROM Dictionary WHERE ParentID=(SELECT ID FROM Dictionary WHERE Code=@Code) ORDER BY Sort";
            MySqlParameter[] parameters = new MySqlParameter[]{
				new MySqlParameter("@Code", MySqlDbType.VarChar){ Value = code }
			};
            MySqlDataReader dataReader = dbHelper.GetDataReader(sql, parameters);
            List<BizProcess.Data.Model.Dictionary> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List;
        }

        /// <summary>
        /// 查询上级记录
        /// </summary>
        public BizProcess.Data.Model.Dictionary GetParent(Guid id)
        {
            string sql = "SELECT * FROM Dictionary WHERE ID=(SELECT ParentID FROM Dictionary WHERE ID=@ID)";
            MySqlParameter[] parameters = new MySqlParameter[]{
				new MySqlParameter("@ID", MySqlDbType.VarChar){ Value = id }
			};
            MySqlDataReader dataReader = dbHelper.GetDataReader(sql, parameters);
            List<BizProcess.Data.Model.Dictionary> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List.Count > 0 ? List[0] : null;
        }

        /// <summary>
        /// 是否包含下级记录
        /// </summary>
        public bool HasChilds(Guid id)
        {
            string sql = "SELECT ID FROM Dictionary WHERE ParentID=@ParentID";
            MySqlParameter[] parameters = new MySqlParameter[]{
				new MySqlParameter("@ParentID", MySqlDbType.VarChar){ Value = id }
			};
            MySqlDataReader dataReader = dbHelper.GetDataReader(sql, parameters);
            bool has = dataReader.HasRows;
            dataReader.Close();
            return has;
        }

        /// <summary>
        /// 得到最大排序
        /// </summary>
        public int GetMaxSort(Guid id)
        {
            string sql = "SELECT MAX(Sort)+1 FROM Dictionary WHERE ParentID=@ParentID";
            MySqlParameter[] parameters = new MySqlParameter[]{
				new MySqlParameter("@ParentID", MySqlDbType.VarChar){ Value = id }
			};
            string max = dbHelper.GetFieldValue(sql, parameters);
            int max1;
            return max.IsInt(out max1) ? max1 : 1;
        }

        /// <summary>
        /// 更新排序
        /// </summary>
        public int UpdateSort(Guid id, int sort)
        {
            string sql = "UPDATE Dictionary SET Sort=@Sort WHERE ID=@ID";
            MySqlParameter[] parameters = new MySqlParameter[]{
				new MySqlParameter("@Sort", MySqlDbType.Int32){ Value = sort },
                new MySqlParameter("@ID", MySqlDbType.VarChar){ Value = id }
			};
            return dbHelper.Execute(sql, parameters);
        }

        /// <summary>
        /// 根据代码查询一条记录
        /// </summary>
        public BizProcess.Data.Model.Dictionary GetByCode(string code)
        {
            string sql = "SELECT * FROM Dictionary WHERE Code=@Code";
            MySqlParameter[] parameters = new MySqlParameter[]{
				new MySqlParameter("@Code", MySqlDbType.VarChar){ Value = code }
			};
            MySqlDataReader dataReader = dbHelper.GetDataReader(sql, parameters);
            List<BizProcess.Data.Model.Dictionary> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List.Count > 0 ? List[0] : null;
        }
    }
}