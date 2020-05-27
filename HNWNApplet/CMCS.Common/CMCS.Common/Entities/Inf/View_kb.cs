using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMCS.Common.Entities.Inf
{
    /// <summary>
    /// 矿点信息
    /// </summary>
    [Serializable]
    [CMCS.DapperDber.Attrs.DapperBind("view_kb")]
    public class View_kb 
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DapperDber.Attrs.DapperPrimaryKey]
        public int Dbid { get; set; }
        /// <summary>
        /// 矿点编码
        /// </summary>
        public string Kbbm { get; set; }
        /// <summary>
        /// 矿点名称
        /// </summary>
        public string Kbmc { get; set; }
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
