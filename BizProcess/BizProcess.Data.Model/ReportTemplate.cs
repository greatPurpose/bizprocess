using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BizProcess.Data.Model
{
    [Serializable]
    public class ReportTemplate
    {
        /// <summary>
        /// ID
        /// </summary>
        [DisplayName("ID")]
        public Guid ID { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DisplayName("标题")]
        public string Title { get; set; }

        /// <summary>
        /// Html
        /// </summary>
        [DisplayName("Html")]
        public string Html { get; set; }

        /// <summary>
        /// DesignJSON
        /// </summary>
        [DisplayName("DesignJSON")]
        public string DesignJSON { get; set; }

        /// <summary>
        /// 分类ID
        /// </summary>
        [DisplayName("分类ID")]
        public Guid Type { get; set; }

        /// <summary>
        /// 使用人员，为空表示所有人员可以使用
        /// </summary>
        [DisplayName("使用人员，为空表示所有人员可以使用")]
        public string UseMember { get; set; }

        /// <summary>
        /// 相关属性
        /// </summary>
        [DisplayName("相关属性")]
        public string Attribute { get; set; }

    }
}
