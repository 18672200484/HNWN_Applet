using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities.Sys;

namespace CMCS.Common.Entities.Inf
{
    /// <summary>
    /// 验票反馈表(管控系统)
    /// </summary>
    [Serializable]
    [CMCS.DapperDber.Attrs.DapperBind("view_rlgl_chlgl_yapfk")]
    public class View_rlgl_chlgl_yapfk : EntityBase1
    {
        /// <summary>
        /// 验票时间
        /// </summary>
        public DateTime Yptime { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public string Chph { get; set; }
        /// <summary>
        /// RFID卡序列号
        /// </summary>
        public string Rfid_xlh { get; set; }
        /// <summary>
        /// 矿发量
        /// </summary>
        public string Kfl { get; set; }
        /// <summary>
        /// 调运编号
        /// </summary>
        public string Dybh { get; set; }
        /// <summary>
        /// 指定采样机（CYJ01 1号采样机，CYJ02 2号采样机，CYJ03 3号采样机）
        /// </summary>
        public string Cyjbm { get; set; }
        /// <summary>
        /// 是否已同步
        /// </summary>
        public int IsSync { get; set; }
    }

    /// <summary>
    /// 验票反馈表(全过程表)
    /// </summary>
    [Serializable]
    [CMCS.DapperDber.Attrs.DapperBind("view_rlgl_chlgl_yapfk")]
    public class View_rlgl_chlgl_yapfk_QGC
    {
        /// <summary>
        /// 验票时间
        /// </summary>
        public DateTime Yptime { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public string Chph { get; set; }
        /// <summary>
        /// RFID卡序列号
        /// </summary>
        public string Rfid_xlh { get; set; }
        /// <summary>
        /// 矿发量
        /// </summary>
        public string Kfl { get; set; }
        /// <summary>
        /// 调运编号
        /// </summary>
        public string Dybh { get; set; }
        /// <summary>
        /// 指定采样机（CYJ01 1号采样机，CYJ02 2号采样机，CYJ03 3号采样机）
        /// </summary>
        public string Cyjbm { get; set; }
    }
}
