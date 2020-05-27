using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMCS.DumblyConcealer.Tasks.AutoMaker.KYEntities
{
    /// <summary>
    /// 制样记录表
    /// </summary>
    [Serializable]
    [CMCS.DapperDber.Attrs.DapperBind("Zy_record_tb")]
    public class Zy_record_tb
    {
        /// <summary>
        /// ID
        /// </summary>
        [DapperDber.Attrs.DapperPrimaryKey]
        public int Id { get; set; }
        /// <summary>
        /// 制样机编号
        /// </summary>
        public string Machinecode { get; set; }
        /// <summary>
        /// 制样编码
        /// </summary>
        public string Sampleid { get; set; }
        /// <summary>
        /// 封装码
        /// </summary>
        public string Packcode { get; set; }
        /// <summary>
        /// ZYWeight
        /// </summary>
        public int Zyweight { get; set; }
        /// <summary>
        /// 制样方式（1：在线制样；2：离线制样）
        /// </summary>
        public int Zytype { get; set; }
        /// <summary>
        /// 出样类型(0:6mm全水样1；1:6mm全水样2；2:3mm分析样；3:0.2mm分析样；4:0.2mm存查样)
        /// </summary>
        public int Sampletype { get; set; }
        /// <summary>
        /// 来样粒度（1:13mm以上；2:6mm）
        /// </summary>
        public int Size { get; set; }
        /// <summary>
        /// Water
        /// </summary>
        public int Water { get; set; }
        /// <summary>
        /// 制样开始时间
        /// </summary>
        public DateTime Starttime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime Endtime { get; set; }
        /// <summary>
        /// 出样重量
        /// </summary>
        public int Samepleweight { get; set; }
        /// <summary>
        /// Person_ID
        /// </summary>
        public string Person_id { get; set; }
        /// <summary>
        /// 工作人员
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// 数据发送状态（0：未读取；1：已读取）
        /// </summary>
        public int Datastatus { get; set; }
    }
}
