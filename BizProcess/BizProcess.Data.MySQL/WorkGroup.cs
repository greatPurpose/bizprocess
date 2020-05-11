using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace BizProcess.Data.MySQL
{
    public class WorkGroup : BizProcess.Data.Interface.IWorkGroup
    {
        private DBHelper dbHelper = new DBHelper();
        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkGroup()
        {
        }
        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="model">BizProcess.Data.Model.WorkGroup实体类</param>
        /// <returns>操作所影响的行数</returns>
        public int Add(BizProcess.Data.Model.WorkGroup model)
        {
            string sql = @"INSERT INTO WorkGroup
				(ID,Name,Members,Note) 
				VALUES(@ID,@Name,@Members,@Note)";
            MySqlParameter[] parameters = new MySqlParameter[]{
				new MySqlParameter("@ID", MySqlDbType.VarChar, -1){ Value = model.ID },
				new MySqlParameter("@Name", MySqlDbType.VarChar, 500){ Value = model.Name },
				new MySqlParameter("@Members", MySqlDbType.Text, -1){ Value = model.Members },
				model.Note == null ? new MySqlParameter("@Note", MySqlDbType.Text, -1) { Value = DBNull.Value } : new MySqlParameter("@Note", MySqlDbType.Text, -1) { Value = model.Note }
			};
            return dbHelper.Execute(sql, parameters);
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="model">BizProcess.Data.Model.WorkGroup实体类</param>
        public int Update(BizProcess.Data.Model.WorkGroup model)
        {
            string sql = @"UPDATE WorkGroup SET 
				Name=@Name,Members=@Members,Note=@Note
				WHERE ID=@ID";
            MySqlParameter[] parameters = new MySqlParameter[]{
				new MySqlParameter("@Name", MySqlDbType.VarChar, 500){ Value = model.Name },
				new MySqlParameter("@Members", MySqlDbType.Text, -1){ Value = model.Members },
				model.Note == null ? new MySqlParameter("@Note", MySqlDbType.Text, -1) { Value = DBNull.Value } : new MySqlParameter("@Note", MySqlDbType.Text, -1) { Value = model.Note },
				new MySqlParameter("@ID", MySqlDbType.VarChar, -1){ Value = model.ID }
			};
            return dbHelper.Execute(sql, parameters);
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        public int Delete(Guid id)
        {
            string sql = "DELETE FROM WorkGroup WHERE ID=@ID";
            MySqlParameter[] parameters = new MySqlParameter[]{
				new MySqlParameter("@ID", MySqlDbType.VarChar){ Value = id }
			};
            return dbHelper.Execute(sql, parameters);
        }
        /// <summary>
        /// 将DataRedar转换为List
        /// </summary>
        private List<BizProcess.Data.Model.WorkGroup> DataReaderToList(MySqlDataReader dataReader)
        {
            List<BizProcess.Data.Model.WorkGroup> List = new List<BizProcess.Data.Model.WorkGroup>();
            BizProcess.Data.Model.WorkGroup model = null;
            while (dataReader.Read())
            {
                model = new BizProcess.Data.Model.WorkGroup();
                model.ID = dataReader.GetGuid(0);
                model.Name = dataReader.GetString(1);
                model.Members = dataReader.GetString(2);
                if (!dataReader.IsDBNull(3))
                    model.Note = dataReader.GetString(3);
                List.Add(model);
            }
            return List;
        }
        /// <summary>
        /// 查询所有记录
        /// </summary>
        public List<BizProcess.Data.Model.WorkGroup> GetAll()
        {
            string sql = "SELECT * FROM WorkGroup";
            MySqlDataReader dataReader = dbHelper.GetDataReader(sql);
            List<BizProcess.Data.Model.WorkGroup> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List;
        }
        /// <summary>
        /// 查询记录数
        /// </summary>
        public long GetCount()
        {
            string sql = "SELECT COUNT(*) FROM WorkGroup";
            long count;
            return long.TryParse(dbHelper.GetFieldValue(sql), out count) ? count : 0;
        }
        /// <summary>
        /// 根据主键查询一条记录
        /// </summary>
        public BizProcess.Data.Model.WorkGroup Get(Guid id)
        {
            string sql = "SELECT * FROM WorkGroup WHERE ID=@ID";
            MySqlParameter[] parameters = new MySqlParameter[]{
				new MySqlParameter("@ID", MySqlDbType.VarChar){ Value = id }
			};
            MySqlDataReader dataReader = dbHelper.GetDataReader(sql, parameters);
            List<BizProcess.Data.Model.WorkGroup> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List.Count > 0 ? List[0] : null;
        }
    }
}