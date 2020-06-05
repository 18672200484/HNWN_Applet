using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMCS.Common.Entities.Inf
{
    /// <summary>
    /// 车辆基本信息
    /// </summary>
    [Serializable]
    [CMCS.DapperDber.Attrs.DapperBind("view_rlgl_chlgl_chlgl")]
    public class View_rlgl_chlgl_chlgl
    {
        /// <summary>
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
        /// 核载量
        /// </summary>
        public decimal Hzl { get; set; }
        /// <summary>
        /// 车厢数
        /// </summary>
        public decimal Chxsh { get; set; }
        /// <summary>
        /// 车身长（mm）
        /// </summary>
        public decimal Cshch { get; set; }
        /// <summary>
        /// 车厢长（mm）
        /// </summary>
        public decimal Chxch { get; set; }
        /// <summary>
        /// 车厢宽（mm）
        /// </summary>
        public decimal Chxk { get; set; }
        /// <summary>
        /// 车厢高（mm）
        /// </summary>
        public decimal Chxg { get; set; }
        /// <summary>
        /// 底盘高（mm）
        /// </summary>
        public decimal Dpg { get; set; }
        /// <summary>
        /// 拉筋位置(多个拉筋用”,”隔开)
        /// </summary>
        public string Ljwzh { get; set; }
        /// <summary>
        /// 车厢间隔（mm）
        /// </summary>
        public decimal Chxjg { get; set; }
        /// <summary>
        /// 车厢长2（mm）
        /// </summary>
        public decimal Chxch2 { get; set; }
        /// <summary>
        /// 车厢宽2（mm）
        /// </summary>
        public decimal Chxk2 { get; set; }
        /// <summary>
        /// 车厢高2（mm）
        /// </summary>
        public decimal Chxg2 { get; set; }
        /// <summary>
        /// 底盘高2（mm）
        /// </summary>
        public decimal Dpg2 { get; set; }
        /// <summary>
        /// 拉筋位置2(多个拉筋用”,”隔开)
        /// </summary>
        public string Ljwzh2 { get; set; }
        /// <summary>
        /// 是否加入黑名单((1 标识是 0 表示否))
        /// </summary>
        public string Is_hmd { get; set; }
        /// <summary>
        /// 保险有效时间止
        /// </summary>
        public DateTime Bx_enddate { get; set; }
        /// <summary>
        /// 行驶证有效时间止
        /// </summary>
        public DateTime Xshzh_enddate { get; set; }
        /// <summary>
        /// 道路从业资格证有效时间止
        /// </summary>
        public DateTime Zgzh_enddate { get; set; }

        /// <summary>
        /// 车辆类型 965煤车，966物资车
        /// </summary>
        public int Cllx { get; set; }
    }
}
