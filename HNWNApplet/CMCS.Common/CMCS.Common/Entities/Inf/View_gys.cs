using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMCS.Common.Entities.Inf
{
    /// <summary>
    /// 供应商信息
    /// </summary>
    [Serializable]
    [CMCS.DapperDber.Attrs.DapperBind("view_gys")]
    public class View_gys
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DapperDber.Attrs.DapperPrimaryKey]
        public int Dbid { get; set; }
        /// <summary>
        /// 供应商编码
        /// </summary>
        public string Gysbm { get; set; }
        /// <summary>
        /// 公司简称
        /// </summary>
        public string Gysjc { get; set; }
        /// <summary>
        /// 公司全称
        /// </summary>
        public string Gysqc { get; set; }
        /// <summary>
        /// 单位地址
        /// </summary>
        public string Dwdz { get; set; }
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
