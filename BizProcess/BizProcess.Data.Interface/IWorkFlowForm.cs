using System;
using System.Collections.Generic;

namespace BizProcess.Data.Interface
{
    public interface IWorkFlowForm
    {
        /// <summary>
        /// 新增
        /// </summary>
        int Add(BizProcess.Data.Model.WorkFlowForm model);

        /// <summary>
        /// 更新
        /// </summary>
        int Update(BizProcess.Data.Model.WorkFlowForm model);

        /// <summary>
        /// 查询所有记录
        /// </summary>
        List<BizProcess.Data.Model.WorkFlowForm> GetAll();

        /// <summary>
        /// 查询单条记录
        /// </summary>
        Model.WorkFlowForm Get(Guid id);

        /// <summary>
        /// 删除
        /// </summary>
        int Delete(Guid id);

        /// <summary>
        /// 查询记录条数
        /// </summary>
        long GetCount();

        /// <summary>
        /// 查询一个分类所有记录
        /// </summary>
        List<BizProcess.Data.Model.WorkFlowForm> GetAllByType(string types);
    }
}
