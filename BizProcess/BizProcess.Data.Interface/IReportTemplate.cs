using System;
using System.Collections.Generic;

namespace BizProcess.Data.Interface
{
    public interface IReportTemplate
    {
        /// <summary>
        /// 新增
        /// </summary>
        int Add(BizProcess.Data.Model.ReportTemplate model);

        /// <summary>
        /// 更新
        /// </summary>
        int Update(BizProcess.Data.Model.ReportTemplate model);

        /// <summary>
        /// 查询所有记录
        /// </summary>
        List<BizProcess.Data.Model.ReportTemplate> GetAll();

        /// <summary>
        /// 查询单条记录
        /// </summary>
        Model.ReportTemplate Get(Guid id);

        /// <summary>
        /// 删除
        /// </summary>
        int Delete(Guid id);

        /// <summary>
        /// 查询记录条数
        /// </summary>
        long GetCount();

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
        List<BizProcess.Data.Model.ReportTemplate> GetPagerData(out string pager, string query = "", string order = "Type,Title", int size = 15, int number = 1, string title = "", string type = "");
       
        /// <summary>
        /// 查询一个类别下所有记录
        /// </summary>
        List<BizProcess.Data.Model.ReportTemplate> GetAllByType(string type);

        /// <summary>
        /// 删除记录
        /// </summary>
        int Delete(string[] idArray);

    }
}
