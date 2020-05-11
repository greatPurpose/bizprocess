using System;
using System.Collections.Generic;

namespace BizProcess.Data.Interface
{
    public interface IDBExtract
    {
        /// <summary>
        /// 新增
        /// </summary>
        int Add(BizProcess.Data.Model.DBExtract model);

        /// <summary>
        /// 更新
        /// </summary>
        int Update(BizProcess.Data.Model.DBExtract model);

        /// <summary>
        /// 查询所有记录
        /// </summary>
        List<BizProcess.Data.Model.DBExtract> GetAll();

        /// <summary>
        /// 查询单条记录
        /// </summary>
        Model.DBExtract Get(Guid id);

        /// <summary>
        /// 删除
        /// </summary>
        int Delete(Guid id);

        /// <summary>
        /// 查询记录条数
        /// </summary>
        long GetCount();

        int ExecuteStatement(string sql);
        int ExecuteStatement(List<string> sqlList);
    }
}
