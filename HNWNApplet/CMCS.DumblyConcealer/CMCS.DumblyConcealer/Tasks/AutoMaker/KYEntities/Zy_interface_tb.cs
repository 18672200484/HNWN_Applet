using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMCS.DumblyConcealer.Tasks.AutoMaker.KYEntities
{
    /// <summary>
    /// 煤样信息中间表
    /// </summary>
    [Serializable]
    [CMCS.DapperDber.Attrs.DapperBind("Zy_interface_tb")]
    public class Zy_interface_tb
    {
        /// <summary>
        /// 制样编码
        /// </summary>
        public string Sampleid { get; set; }
        /// <summary>
        /// Weight
        /// </summary>
        public int Weight { get; set; }
        /// <summary>
        /// 煤种
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 粒度（1:13mm以上；2：6mm）
        /// </summary>
        public int Size { get; set; }
        /// <summary>
        /// 水分
        /// </summary>
        public int Water { get; set; }
        /// <summary>
        /// BarrelNumber
        /// </summary>
        public int Barrelnumber { get; set; }
        /// <summary>
        /// 数据发送状态
        /// </summary>
        public int Datastatus { get; set; }
    }
}
