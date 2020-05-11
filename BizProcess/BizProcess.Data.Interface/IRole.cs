using System;
using System.Collections.Generic;

namespace BizProcess.Data.Interface
{
    public interface IRole
    {
        /// <summary>
        /// 新增
        /// </summary>
        int Add(BizProcess.Data.Model.Role model);

        /// <summary>
        /// 更新
        /// </summary>
        int Update(BizProcess.Data.Model.Role model);

        /// <summary>
        /// 查询所有记录
        /// </summary>
        List<BizProcess.Data.Model.Role> GetAll();

        /// <summary>
        /// 查询单条记录
        /// </summary>
        Model.Role Get(Guid id);

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
