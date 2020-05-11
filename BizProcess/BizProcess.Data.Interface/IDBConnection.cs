using System;
using System.Collections.Generic;

namespace BizProcess.Data.Interface
{
    public interface IDBConnection
    {
        /// <summary>
        /// 新增
        /// </summary>
        int Add(BizProcess.Data.Model.DBConnection model);

        /// <summary>
        /// 更新
        /// </summary>
        int Update(BizProcess.Data.Model.DBConnection model);

        /// <summary>
        /// 查询所有记录
        /// </summary>
        List<BizProcess.Data.Model.DBConnection> GetAll();

        /// <summary>
        /// 查询单条记录
        /// </summary>
        Model.DBConnection Get(Guid id);

        /// <summary>
        /// 删除
        /// </summary>
        int Delete(Guid id);

        /// <summary>
        /// 查询记录条数
        /// </summary>
        long GetCount();
    }
}
