using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMCS.Common.Enums
{
    /// <summary>
    /// 第三方设备接口 - 全自动制样机系统状态
    /// </summary>
    public enum eEquInfAutoMakerSystemStatus
    {
        停止状态 = 0,
        正在运行 = 1,
        发生故障 = 2,
        卸料就绪 = 3,
        急停状态 = 4,
        制样就绪 = 5
    }
}
