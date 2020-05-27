using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMCS.CarTransport.Queue.Enums
{
    /// <summary>
    /// 流程标识
    /// </summary>
    public enum eFlowFlag
    {
        等待车辆,
        验证车辆,
        匹配调运,
        匹配采样,
        数据录入,
        自动保存,
        等待离开,
        异常重置1,
        异常重置2,
        识别车辆,
        入厂煤验证保存,
        其他物资验证保存
    }
}
