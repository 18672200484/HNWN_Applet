using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMCS.DumblyConcealer.Tasks.AutoMaker.KYEntities
{
    /// <summary>
    /// 制样机总体状态表
    /// </summary>
    [Serializable]
    [CMCS.DapperDber.Attrs.DapperBind("Zy_status_tb")]
    public class Zy_status_tb
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        public string Machinecode { get; set; }
        /// <summary>
        /// 制样机状态（0：停止；1；正在进行中，不可以制下一批样；2：故障，不可以制下一批样；3：准备就绪，可以制下一批样；4：急停）
        /// </summary>
        public int Samready { get; set; }
    }
}
