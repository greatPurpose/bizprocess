using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BizProcess.Data.Model
{
    [Serializable]
    public class DBExtract
    {
        /// <summary>
        /// ID
        /// </summary>
        [DisplayName("ID")]
        public Guid ID { get; set; }

        /// <summary>
        /// Table Name
        /// </summary>
        [DisplayName("Table Name")]
        public string Name { get; set; }

        /// <summary>
        /// Table Comment
        /// </summary>
        [DisplayName("Table Comment")]
        public string Comment { get; set; }

        /// <summary>
        /// DBConnID
        /// </summary>
        [DisplayName("DBConnection ID")]
        public Guid DBConnID { get; set; }

        /// <summary>
        /// DesignJSON
        /// </summary>
        [DisplayName("DesignJSON")]
        public string DesignJSON { get; set; }

        /// <summary>
        /// ExtractType
        /// true - auto, false - manual
        /// </summary>
        [DisplayName("ExtractType")]
        public bool ExtractType { get; set; }

        /// <summary>
        /// RunTime - 10 varchar
        /// </summary>
        [DisplayName("RunTime")]
        public string RunTime { get; set; }

        /// <summary>
        /// Only Increment
        /// true - only, false - all
        /// </summary>
        [DisplayName("Only increment")]
        public bool OnlyIncrement { get; set; }

        public DateTime LastRunTime { get; set; }
    }
}
