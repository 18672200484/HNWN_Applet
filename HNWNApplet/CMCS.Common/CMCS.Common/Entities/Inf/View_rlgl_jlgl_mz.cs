using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities.Sys;

namespace CMCS.Common.Entities.Inf
{
    /// <summary>
    /// 重车计量反馈
    /// </summary>
    [Serializable]
    [CMCS.DapperDber.Attrs.DapperBind("view_rlgl_jlgl_mz")]
    public class View_rlgl_jlgl_mz : EntityBase1
    {
        /// <summary>
        /// 重车计量时间
        /// </summary>
        public DateTime Mztime { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public string Chph { get; set; }
        /// <summary>
        /// RFID卡序列号
        /// </summary>
        public string Rfid_xlh { get; set; }
        /// <summary>
        /// 批次号
        /// </summary>
        public string Mpph { get; set; }
        /// <summary>
        /// 毛重
        /// </summary>
        public string Mz { get; set; }
        /// <summary>
        /// 是否已同步
        /// </summary>
        public int Issync { get; set; }
    }
    
    /// <summary>
    /// 验票反馈表(全过程表)
    /// </summary>
    [Serializable]
    [CMCS.DapperDber.Attrs.DapperBind("view_rlgl_jlgl_mz")]
    public class View_rlgl_jlgl_mz_QGC
    {
        /// <summary>
        /// 重车计量时间
        /// </summary>
        public DateTime Mztime { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public string Chph { get; set; }
        /// <summary>
        /// RFID卡序列号
        /// </summary>
        public string Rfid_xlh { get; set; }
        /// <summary>
        /// 批次号
        /// </summary>
        public string Mpph { get; set; }
        /// <summary>
        /// 毛重
        /// </summary>
        public string Mz { get; set; }
    }
}
