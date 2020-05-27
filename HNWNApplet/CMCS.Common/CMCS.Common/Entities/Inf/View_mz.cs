using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMCS.Common.Entities.Inf
{
    /// <summary>
    /// 煤种信息
    /// </summary>
    [Serializable]
    [CMCS.DapperDber.Attrs.DapperBind("view_mz")]
    public class View_mz
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DapperDber.Attrs.DapperPrimaryKey]
        public int Dbid { get; set; }
        /// <summary>
        /// 煤种编码
        /// </summary>
        public string Mzbm { get; set; }
        /// <summary>
        /// 煤种名称
        /// </summary>
        public string Mzmc { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime Update_date { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public string Is_valid { get; set; }
    }
}
