using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMCS.Common.Entities.Inf
{
    /// <summary>
    /// 承运商信息
    /// </summary>
    [Serializable]
    [CMCS.DapperDber.Attrs.DapperBind("view_rlgl_dygl_chysh")]
    public class View_rlgl_dygl_chysh
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DapperDber.Attrs.DapperPrimaryKey]
        public int Dbid { get; set; }
        /// <summary>
        /// 承运商纳税号
        /// </summary>
        public string Shippertaxnumber { get; set; }
        /// <summary>
        /// 承运商名称
        /// </summary>
        public string Shipername { get; set; }
        /// <summary>
        /// 计划卡号
        /// </summary>
        public string Planno { get; set; }
        /// <summary>
        /// 对应供应商（华能主数据编码）
        /// </summary>
        public string Gys { get; set; }
    }
}
