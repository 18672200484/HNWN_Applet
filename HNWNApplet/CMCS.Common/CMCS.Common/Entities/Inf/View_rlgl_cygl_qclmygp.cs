using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMCS.Common.Entities.Inf
{
    /// <summary>
    /// 汽车来煤预归批
    /// </summary>
    [Serializable]
    [CMCS.DapperDber.Attrs.DapperBind("view_rlgl_cygl_qclmygp")]
    public class View_rlgl_cygl_qclmygp
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DapperDber.Attrs.DapperPrimaryKey]
        public decimal Dbid { get; set; }
        /// <summary>
        /// 煤批批号
        /// </summary>
        public string Mpph { get; set; }
        /// <summary>
        /// 计划到货日期
        /// </summary>
        public DateTime Plantime { get; set; }
        /// <summary>
        /// 供应商
        /// </summary>
        public decimal Gysid { get; set; }
        /// <summary>
        /// 矿别
        /// </summary>
        public decimal Kbid { get; set; }
        /// <summary>
        /// 煤种
        /// </summary>
        public decimal Mzid { get; set; }
        /// <summary>
        /// 调运编号
        /// </summary>
        public string Dybh { get; set; }
        /// <summary>
        /// 承运数量
        /// </summary>
        public decimal Chyshl { get; set; }
        /// <summary>
        /// 标称最大粒度ID
        /// </summary>
        public decimal Bchzdld { get; set; }
        /// <summary>
        /// 采样机编码
        /// </summary>
        public string Cyjbm { get; set; }
        /// <summary>
        /// 卸煤点
        /// </summary>
        public decimal Xmd { get; set; }
        /// <summary>
        /// 采样方案类型
        /// </summary>
        public string Cyfa_type { get; set; }
        /// <summary>
        /// 采样码
        /// </summary>
        public string Cym { get; set; }
        /// <summary>
        /// 采样编号
        /// </summary>
        public string Cybh { get; set; }
        /// <summary>
        /// 变异系数
        /// </summary>
        public decimal Byxh { get; set; }
    }
}
