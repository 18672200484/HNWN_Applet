using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMCS.DumblyConcealer.Tasks.AutoMaker.KYEntities
{
    /// <summary>
    /// 制样设备状态表
    /// </summary>
    [Serializable]
    [CMCS.DapperDber.Attrs.DapperBind("Zy_state_tb")]
    public class Zy_state_tb
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        public string Machinecode { get; set; }
        /// <summary>
        /// 详细设备编号
        /// </summary>
        public string Devicecode { get; set; }
        /// <summary>
        /// 标示当前制的是哪一批次的样品
        /// </summary>
        public string Sampleid { get; set; }
        /// <summary>
        /// 设备的详细名称
        /// </summary>
        public string Devicename { get; set; }
        /// <summary>
        /// 设备状态（0：停止；1：运行；2：故障）
        /// </summary>
        public int Devicestatus { get; set; }
        /// <summary>
        /// LastUpdateTime
        /// </summary>
        public DateTime Lastupdatetime { get; set; }
        /// <summary>
        /// DataStatus
        /// </summary>
        public int Datastatus { get; set; }
    }
}
