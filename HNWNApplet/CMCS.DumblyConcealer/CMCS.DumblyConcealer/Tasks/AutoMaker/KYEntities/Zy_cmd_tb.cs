using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMCS.DumblyConcealer.Tasks.AutoMaker.KYEntities
{
    /// <summary>
    /// 控制信息表
    /// </summary>
    [Serializable]
    [CMCS.DapperDber.Attrs.DapperBind("zy_cmd_tb")]
    public class Zy_cmd_tb
    {
        /// <summary>
        /// 制样机编号
        /// </summary>
        public string Machinecode { get; set; }
        /// <summary>
        /// 命令代码（1：启动；2：急停；3）
        /// </summary>
        public int Commandcode { get; set; }
        /// <summary>
        /// 制样编码信息
        /// </summary>
        public string Samplecode { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime Sendcommandtime { get; set; }
        /// <summary>
        /// 数据状态（0：未读取；1：已读取）
        /// </summary>
        public int Datastatus { get; set; }
    }
}
