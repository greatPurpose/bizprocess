﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Oracle.DataAccess.Client;

namespace BizProcess.Data.Oracle
{
    public class ReportTemplate : BizProcess.Data.Interface.IReportTemplate
    {
        private DBHelper dbHelper = new DBHelper();
        /// <summary>
        /// 构造函数
        /// </summary>
        public ReportTemplate()
        {
        }
        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="model">Data.Model.ReportTemplate实体类</param>
        /// <returns>操作所影响的行数</returns>
        public int Add(Data.Model.ReportTemplate model)
        {
            string sql = @"INSERT INTO ReportTemplate
				(ID,Title,Type,Html,DesignJSON,UseMember) 
				VALUES(:ID,:Title,:Type,:Html,:DesignJSON,:UseMember)";
            OracleParameter[] parameters = new OracleParameter[]{
				new OracleParameter(":ID", OracleDbType.Char, 36){ Value = model.ID },
				new OracleParameter(":Title", OracleDbType.NVarchar2, 510){ Value = model.Title },
				new OracleParameter(":Type", OracleDbType.Char, 36){ Value = model.Type },
				model.Html == null ? new OracleParameter(":Html", OracleDbType.Clob) { Value = DBNull.Value } : new OracleParameter(":Html", OracleDbType.Clob) { Value = model.Html },
				model.DesignJSON == null ? new OracleParameter(":DesignJSON", OracleDbType.Clob) { Value = DBNull.Value } : new OracleParameter(":DesignJSON", OracleDbType.Clob) { Value = model.DesignJSON },
				model.UseMember == null ? new OracleParameter(":UseMember", OracleDbType.Clob) { Value = DBNull.Value } : new OracleParameter(":UseMember", OracleDbType.Clob) { Value = model.UseMember }
			};
            return dbHelper.Execute(sql, parameters);
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="model">Data.Model.ReportTemplate实体类</param>
        public int Update(Data.Model.ReportTemplate model)
        {
            string sql = @"UPDATE ReportTemplate SET 
				Title=:Title,Type=:Type,Html=:Html,DesignJSON=:DesignJSON,UseMember=:UseMember
				WHERE ID=:ID";
            OracleParameter[] parameters = new OracleParameter[]{
				new OracleParameter(":Title", OracleDbType.NVarchar2, 510){ Value = model.Title },
				new OracleParameter(":Type", OracleDbType.Char, 36){ Value = model.Type },
				model.Html == null ? new OracleParameter(":Html", OracleDbType.Clob) { Value = DBNull.Value } : new OracleParameter(":Html", OracleDbType.Clob) { Value = model.Html },
                model.DesignJSON == null ? new OracleParameter(":DesignJSON", OracleDbType.Clob) { Value = DBNull.Value } : new OracleParameter(":DesignJSON", OracleDbType.Clob) { Value = model.DesignJSON },
				model.UseMember == null ? new OracleParameter(":UseMember", OracleDbType.Clob) { Value = DBNull.Value } : new OracleParameter(":UseMember", OracleDbType.Clob) { Value = model.UseMember },
				new OracleParameter(":ID", OracleDbType.Char, 36){ Value = model.ID }
			};
            return dbHelper.Execute(sql, parameters);
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        public int Delete(Guid id)
        {
            string sql = "DELETE FROM ReportTemplate WHERE ID=:ID";
            OracleParameter[] parameters = new OracleParameter[]{
				new OracleParameter(":ID", OracleDbType.Char){ Value = id }
			};
            return dbHelper.Execute(sql, parameters);
        }
        /// <summary>
        /// 将DataRedar转换为List
        /// </summary>
        private List<Data.Model.ReportTemplate> DataReaderToList(OracleDataReader dataReader)
        {
            List<Data.Model.ReportTemplate> List = new List<Data.Model.ReportTemplate>();
            Data.Model.ReportTemplate model = null;
            while (dataReader.Read())
            {
                model = new Data.Model.ReportTemplate();
                model.ID = dataReader.GetGuid(0);
                model.Title = dataReader.GetString(1);
                model.Type = dataReader.GetGuid(2);
                if (!dataReader.IsDBNull(3))
                    model.Html = dataReader.GetString(3);
                if (!dataReader.IsDBNull(4))
                    model.DesignJSON = dataReader.GetString(4);
                if (!dataReader.IsDBNull(5))
                    model.UseMember = dataReader.GetString(5);
                List.Add(model);
            }
            return List;
        }
        /// <summary>
        /// 查询所有记录
        /// </summary>
        public List<Data.Model.ReportTemplate> GetAll()
        {
            string sql = "SELECT * FROM ReportTemplate";
            OracleDataReader dataReader = dbHelper.GetDataReader(sql);
            List<Data.Model.ReportTemplate> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List;
        }
        /// <summary>
        /// 查询记录数
        /// </summary>
        public long GetCount()
        {
            string sql = "SELECT COUNT(*) FROM ReportTemplate";
            long count;
            return long.TryParse(dbHelper.GetFieldValue(sql), out count) ? count : 0;
        }
        /// <summary>
        /// 根据主键查询一条记录
        /// </summary>
        public Data.Model.ReportTemplate Get(Guid id)
        {
            string sql = "SELECT * FROM ReportTemplate WHERE ID=:ID";
            OracleParameter[] parameters = new OracleParameter[]{
				new OracleParameter(":ID", OracleDbType.Char){ Value = id }
			};
            OracleDataReader dataReader = dbHelper.GetDataReader(sql, parameters);
            List<Data.Model.ReportTemplate> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List.Count > 0 ? List[0] : null;
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
        public List<BizProcess.Data.Model.ReportTemplate> GetPagerData(out string pager, string query = "", string order = "Type,Title", int size = 15, int number = 1, string title = "", string type = "")
        {
            StringBuilder WHERE = new StringBuilder();
            List<OracleParameter> parList = new List<OracleParameter>();
            if (!title.IsNullOrEmpty())
            {
                WHERE.Append("AND INSTR(Title,:Title)>0 ");
                parList.Add(new OracleParameter(":Title", OracleDbType.NVarchar2) { Value = title });
            }
            if (!type.IsNullOrEmpty())
            {
                WHERE.AppendFormat("AND Type IN({0}) ", BizProcess.Utility.Tools.GetSqlInString(type));
            }
            long count;
            string sql = dbHelper.GetPaerSql("ReportTemplate", "*", WHERE.ToString(), order, size, number, out count, parList.ToArray());

            pager = BizProcess.Utility.Tools.GetPagerHtml(count, size, number, query);
            OracleDataReader dataReader = dbHelper.GetDataReader(sql, parList.ToArray());
            List<BizProcess.Data.Model.ReportTemplate> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List;
        }

        /// <summary>
        /// 查询一个类别下所有记录
        /// </summary>
        public List<BizProcess.Data.Model.ReportTemplate> GetAllByType(string types)
        {
            string sql = "SELECT * FROM ReportTemplate WHERE Type IN(" + BizProcess.Utility.Tools.GetSqlInString(types) + ")";
            OracleDataReader dataReader = dbHelper.GetDataReader(sql);
            List<BizProcess.Data.Model.ReportTemplate> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List;
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        public int Delete(string[] idArray)
        {
            string sql = "DELETE FROM ReportTemplate WHERE ID in(" + BizProcess.Utility.Tools.GetSqlInString(idArray) + ")";
            return dbHelper.Execute(sql);
        }

    }
}