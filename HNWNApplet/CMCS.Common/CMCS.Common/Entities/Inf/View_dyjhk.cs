using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMCS.Common.Entities.Inf
{
    /// <summary>
    /// 调运计划卡
    /// </summary>
    [Serializable]
    [CMCS.DapperDber.Attrs.DapperBind("view_dyjhk")]
    public class View_dyjhk
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DapperDber.Attrs.DapperPrimaryKey]
        public decimal Dbid { get; set; }
        /// <summary>
        /// 计划编号
        /// </summary>
        public string Plancardno { get; set; }
        /// <summary>
        /// 调运编号
        /// </summary>
        public string Dybh { get; set; }
        /// <summary>
        /// 供应商全称
        /// </summary>
        public string Gysqc { get; set; }
        /// <summary>
        /// 供应商编码
        /// </summary>
        public string Gysbm { get; set; }
        /// <summary>
        /// 矿别编码
        /// </summary>
        public string Kbbm { get; set; }
        /// <summary>
        /// 矿别名称
        /// </summary>
        public string Kbmc { get; set; }
        /// <summary>
        /// 煤种编码
        /// </summary>
        public string Mzbm { get; set; }
        /// <summary>
        /// 煤种名称
        /// </summary>
        public string Mzmc { get; set; }
    }
}
