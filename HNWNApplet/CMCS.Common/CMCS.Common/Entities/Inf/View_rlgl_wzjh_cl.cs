using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMCS.Common.Entities.Inf
{
    /// <summary>
    /// 物资车信息
    /// </summary>
    [Serializable]
    [CMCS.DapperDber.Attrs.DapperBind("view_rlgl_wzjh_cl")]
    public class View_rlgl_wzjh_cl
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DapperDber.Attrs.DapperPrimaryKey]
        public decimal Dbid { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public string Chph { get; set; }
        /// <summary>
        /// RFID卡序列号
        /// </summary>
        public string Rfid_xlh { get; set; }
        /// <summary>
        /// 计划卡号
        /// </summary>
        public string PlanCardNo { get; set; }
        /// <summary>
        /// 物料编码
        /// </summary>
        public string Wlbm { get; set; }
        /// <summary>
        /// 物料简称
        /// </summary>
        public string Wljc { get; set; }
        /// <summary>
        /// 流向名称
        /// </summary>
        public string Lxmc { get; set; }
        /// <summary>
        /// 流向编码
        /// </summary>
        public string Lx_bm { get; set; }
        /// <summary>
        /// 供应商全称
        /// </summary>
        public string Gysqc { get; set; }
        /// <summary>
        /// 供应商编码
        /// </summary>
        public string Gysjc { get; set; }
        /// <summary>
        /// 消费客户全称
        /// </summary>
        public string Khqc { get; set; }
        /// <summary>
        /// 消费客户编码
        /// </summary>
        public string Khbm { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Zt { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public string Is_del { get; set; }
        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime Begdate { get; set; }
        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime Enddate { get; set; }
        /// <summary>
        /// 是否黑名单
        /// </summary>
        public string Is_hmd { get; set; }
    }
}
