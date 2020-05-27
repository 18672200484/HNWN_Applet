using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMCS.Common.Entities.Inf
{
    /// <summary>
    /// 公司信息
    /// </summary>
    [Serializable]
    [CMCS.DapperDber.Attrs.DapperBind("view_rlgl_dygl_gs")]
    public class View_rlgl_dygl_gs
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DapperDber.Attrs.DapperPrimaryKey]
        public int Dbid { get; set; }
        /// <summary>
        /// 公司纳税号
        /// </summary>
        public string Taxnumber { get; set; }
        /// <summary>
        /// 公司全称
        /// </summary>
        public string Corpname { get; set; }
        /// <summary>
        /// 公司简称
        /// </summary>
        public string Corpshortname { get; set; }
        /// <summary>
        /// 对应供应商（华能主数据编码）
        /// </summary>
        public string Gys { get; set; }
        /// <summary>
        /// 是否发货单位(1 标识是 0 表示否) 
        /// </summary>
        public string Is_fhdw { get; set; }
        /// <summary>
        /// 是否收货单位(1 标识是 0 表示否)
        /// </summary>
        public string Is_shhdw { get; set; }
    }
}
