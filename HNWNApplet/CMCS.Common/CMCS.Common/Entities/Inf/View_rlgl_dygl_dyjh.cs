using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMCS.Common.Entities.Inf
{
    /// <summary>
    /// 车辆日调运计划
    /// </summary>
    [Serializable]
    [CMCS.DapperDber.Attrs.DapperBind("view_rlgl_dygl_dyjh")]
    public class View_rlgl_dygl_dyjh
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DapperDber.Attrs.DapperPrimaryKey]
        public int Dbid { get; set; }
        /// <summary>
        /// 计划到货日期
        /// </summary>
        public string Plantime { get; set; }
        /// <summary>
        /// 发货方纳税号
        /// </summary>
        public string Consignertaxnumber { get; set; }
        /// <summary>
        /// 收货方纳税号
        /// </summary>
        public string Receivertaxnumber { get; set; }
        /// <summary>
        /// 承运商计划号
        /// </summary>
        public string Planno { get; set; }
        /// <summary>
        /// 运输订单编号
        /// </summary>
        public string Transportno { get; set; }
        /// <summary>
        /// 承运数量
        /// </summary>
        public decimal Shipnumber { get; set; }
        /// <summary>
        /// 指定采样机（CYJ01 1号采样机，CYJ02 2号采样机，CYJ03 3号采样机）
        /// </summary>
        public string Cyjbm { get; set; }
    }
}
