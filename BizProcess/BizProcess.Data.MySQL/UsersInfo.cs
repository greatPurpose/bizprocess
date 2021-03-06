﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace BizProcess.Data.MySQL
{
    public class UsersInfo : BizProcess.Data.Interface.IUsersInfo
    {
        private DBHelper dbHelper = new DBHelper();
        /// <summary>
        /// 构造函数
        /// </summary>
        public UsersInfo()
        {
        }
        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="model">BizProcess.Data.Model.UsersInfo实体类</param>
        /// <returns>操作所影响的行数</returns>
        public int Add(BizProcess.Data.Model.UsersInfo model)
        {
            string sql = @"INSERT INTO UsersInfo
				(UserID,Officer,Tel,Fax,Address,Email,QQ,MSN,Note) 
				VALUES(@UserID,@Officer,@Tel,@Fax,@Address,@Email,@QQ,@MSN,@Note)";
            MySqlParameter[] parameters = new MySqlParameter[]{
				new MySqlParameter("@UserID", MySqlDbType.VarChar, 36){ Value = model.UserID },
				model.Officer == null ? new MySqlParameter("@Officer", MySqlDbType.VarChar, 1000) { Value = DBNull.Value } : new MySqlParameter("@Officer", MySqlDbType.VarChar, 1000) { Value = model.Officer },
				model.Tel == null ? new MySqlParameter("@Tel", MySqlDbType.VarChar, 500) { Value = DBNull.Value } : new MySqlParameter("@Tel", MySqlDbType.VarChar, 500) { Value = model.Tel },
				model.Fax == null ? new MySqlParameter("@Fax", MySqlDbType.VarChar, 500) { Value = DBNull.Value } : new MySqlParameter("@Fax", MySqlDbType.VarChar, 500) { Value = model.Fax },
				model.Address == null ? new MySqlParameter("@Address", MySqlDbType.VarChar, 500) { Value = DBNull.Value } : new MySqlParameter("@Address", MySqlDbType.VarChar, 500) { Value = model.Address },
				model.Email == null ? new MySqlParameter("@Email", MySqlDbType.VarChar, 50) { Value = DBNull.Value } : new MySqlParameter("@Email", MySqlDbType.VarChar, 50) { Value = model.Email },
				model.QQ == null ? new MySqlParameter("@QQ", MySqlDbType.VarChar, 50) { Value = DBNull.Value } : new MySqlParameter("@QQ", MySqlDbType.VarChar, 50) { Value = model.QQ },
				model.MSN == null ? new MySqlParameter("@MSN", MySqlDbType.VarChar, 50) { Value = DBNull.Value } : new MySqlParameter("@MSN", MySqlDbType.VarChar, 50) { Value = model.MSN },
				model.Note == null ? new MySqlParameter("@Note", MySqlDbType.Text, -1) { Value = DBNull.Value } : new MySqlParameter("@Note", MySqlDbType.Text, -1) { Value = model.Note }
			};
            return dbHelper.Execute(sql, parameters);
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="model">BizProcess.Data.Model.UsersInfo实体类</param>
        public int Update(BizProcess.Data.Model.UsersInfo model)
        {
            string sql = @"UPDATE UsersInfo SET 
				Officer=@Officer,Tel=@Tel,Fax=@Fax,Address=@Address,Email=@Email,QQ=@QQ,MSN=@MSN,Note=@Note
				WHERE UserID=@UserID";
            MySqlParameter[] parameters = new MySqlParameter[]{
				model.Officer == null ? new MySqlParameter("@Officer", MySqlDbType.VarChar, 1000) { Value = DBNull.Value } : new MySqlParameter("@Officer", MySqlDbType.VarChar, 1000) { Value = model.Officer },
				model.Tel == null ? new MySqlParameter("@Tel", MySqlDbType.VarChar, 500) { Value = DBNull.Value } : new MySqlParameter("@Tel", MySqlDbType.VarChar, 500) { Value = model.Tel },
				model.Fax == null ? new MySqlParameter("@Fax", MySqlDbType.VarChar, 500) { Value = DBNull.Value } : new MySqlParameter("@Fax", MySqlDbType.VarChar, 500) { Value = model.Fax },
				model.Address == null ? new MySqlParameter("@Address", MySqlDbType.VarChar, 500) { Value = DBNull.Value } : new MySqlParameter("@Address", MySqlDbType.VarChar, 500) { Value = model.Address },
				model.Email == null ? new MySqlParameter("@Email", MySqlDbType.VarChar, 50) { Value = DBNull.Value } : new MySqlParameter("@Email", MySqlDbType.VarChar, 50) { Value = model.Email },
				model.QQ == null ? new MySqlParameter("@QQ", MySqlDbType.VarChar, 50) { Value = DBNull.Value } : new MySqlParameter("@QQ", MySqlDbType.VarChar, 50) { Value = model.QQ },
				model.MSN == null ? new MySqlParameter("@MSN", MySqlDbType.VarChar, 50) { Value = DBNull.Value } : new MySqlParameter("@MSN", MySqlDbType.VarChar, 50) { Value = model.MSN },
				model.Note == null ? new MySqlParameter("@Note", MySqlDbType.Text, -1) { Value = DBNull.Value } : new MySqlParameter("@Note", MySqlDbType.Text, -1) { Value = model.Note },
				new MySqlParameter("@UserID", MySqlDbType.VarChar, 36){ Value = model.UserID }
			};
            return dbHelper.Execute(sql, parameters);
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        public int Delete(Guid userid)
        {
            string sql = "DELETE FROM UsersInfo WHERE UserID=@UserID";
            MySqlParameter[] parameters = new MySqlParameter[]{
				new MySqlParameter("@UserID", MySqlDbType.VarChar){ Value = userid }
			};
            return dbHelper.Execute(sql, parameters);
        }
        /// <summary>
        /// 将DataRedar转换为List
        /// </summary>
        private List<BizProcess.Data.Model.UsersInfo> DataReaderToList(MySqlDataReader dataReader)
        {
            List<BizProcess.Data.Model.UsersInfo> List = new List<BizProcess.Data.Model.UsersInfo>();
            BizProcess.Data.Model.UsersInfo model = null;
            while (dataReader.Read())
            {
                model = new BizProcess.Data.Model.UsersInfo();
                model.UserID = dataReader.GetGuid(0);
                if (!dataReader.IsDBNull(1))
                    model.Officer = dataReader.GetString(1);
                if (!dataReader.IsDBNull(2))
                    model.Tel = dataReader.GetString(2);
                if (!dataReader.IsDBNull(3))
                    model.Fax = dataReader.GetString(3);
                if (!dataReader.IsDBNull(4))
                    model.Address = dataReader.GetString(4);
                if (!dataReader.IsDBNull(5))
                    model.Email = dataReader.GetString(5);
                if (!dataReader.IsDBNull(6))
                    model.QQ = dataReader.GetString(6);
                if (!dataReader.IsDBNull(7))
                    model.MSN = dataReader.GetString(7);
                if (!dataReader.IsDBNull(8))
                    model.Note = dataReader.GetString(8);
                List.Add(model);
            }
            return List;
        }
        /// <summary>
        /// 查询所有记录
        /// </summary>
        public List<BizProcess.Data.Model.UsersInfo> GetAll()
        {
            string sql = "SELECT * FROM UsersInfo";
            MySqlDataReader dataReader = dbHelper.GetDataReader(sql);
            List<BizProcess.Data.Model.UsersInfo> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List;
        }
        /// <summary>
        /// 查询记录数
        /// </summary>
        public long GetCount()
        {
            string sql = "SELECT COUNT(*) FROM UsersInfo";
            long count;
            return long.TryParse(dbHelper.GetFieldValue(sql), out count) ? count : 0;
        }
        /// <summary>
        /// 根据主键查询一条记录
        /// </summary>
        public BizProcess.Data.Model.UsersInfo Get(Guid userid)
        {
            string sql = "SELECT * FROM UsersInfo WHERE UserID=@UserID";
            MySqlParameter[] parameters = new MySqlParameter[]{
				new MySqlParameter("@UserID", MySqlDbType.VarChar){ Value = userid }
			};
            MySqlDataReader dataReader = dbHelper.GetDataReader(sql, parameters);
            List<BizProcess.Data.Model.UsersInfo> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List.Count > 0 ? List[0] : null;
        }
    }
}