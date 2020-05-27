using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMCS.Common.Entities.Inf
{
    /// <summary>
    /// 采样方案表
    /// </summary>
    [Serializable]
    [CMCS.DapperDber.Attrs.DapperBind("view_rlgl_cygl_cyfa")]
    public class View_rlgl_cygl_cyfa
    {/// <summary>
        /// 主键
        /// </summary>
        [DapperDber.Attrs.DapperPrimaryKey]
        public int Dbid { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public string Chph { get; set; }
        /// <summary>
        /// RFID卡序列号
        /// </summary>
        public string Rfid_xlh { get; set; }
        /// <summary>
        /// 指定采样机（CYJ01 1号采样机，CYJ02 2号采样机，CYJ03 3号采样机）
        /// </summary>
        public string Cyjbm { get; set; }
        /// <summary>
        /// 批次号
        /// </summary>
        public string Mpph { get; set; }
        /// <summary>
        /// 采样码
        /// </summary>
        public string Cym { get; set; }
        /// <summary>
        /// 预设采样点数
        /// </summary>
        public decimal Yshcydsh { get; set; }
        /// <summary>
        /// 采样状态（0：未采样1：采样中2：已采样）
        /// </summary>
        public string Zt { get; set; }
    }
}
