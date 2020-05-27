using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities;
using CMCS.Common.Entities.iEAA;

namespace CMCS.Common
{
    /// <summary>
    /// 全局变量
    /// </summary>
    public static class GlobalVars
    {
        /// <summary>
        /// 当前登录用户
        /// </summary>
        public static CmcsUser LoginUser;

        /// <summary>
        /// 管理员账号
        /// </summary>
        public static string AdminAccount = "admin";
        /// <summary>
        /// 公共程序配置键名
        /// </summary>
        public static string CommonAppletConfigName = "公共配置";
        /// <summary>
        /// 第三方设备上位机心跳状态名
        /// </summary>
        public static string EquHeartbeatName = "上位机心跳";

        #region 火车机械采样机

        /// <summary>
        /// 设备编码 - 火车机械采样机 #1
        /// </summary>
        public static string MachineCode_HCJXCYJ_1 = "#1火车机械采样机";

        /// <summary>
        /// 设备编码 - 火车机械采样机 #2
        /// </summary>
        public static string MachineCode_HCJXCYJ_2 = "#2火车机械采样机";

        /// <summary>
        /// 接口类型 - 火车机械采样机
        /// </summary>
        public static string InterfaceType_HCJXCYJ = "徐州赛摩火车机械采样机";

        #endregion

        #region 入炉皮带采样机

        /// <summary>
        /// 设备编码 - 入炉皮带采样机 1#
        /// </summary>
        public static string MachineCode_HCPDCYJ_1 = "#1入炉皮带采样机";

        /// <summary>
        /// 设备编码 - 入炉皮带采样机 2#
        /// </summary>
        public static string MachineCode_HCPDCYJ_2 = "#2入炉皮带采样机";

        /// <summary>
        /// 接口类型 - 入炉皮带采样机
        /// </summary>
        public static string InterfaceType_HCPDCYJ = "入炉皮带采样机";

        #endregion

        #region 全自动制样机

        /// <summary>
        /// 设备编码 - 全自动制样机 #1
        /// </summary>
        public static string MachineCode_QZDZYJ_1 = "#1全自动制样机";

        /// <summary>
        /// 接口类型 - 全自动制样机
        /// </summary>
        public static string InterfaceType_QZDZYJ = "全自动制样机";

        #endregion

        /// <summary>
        /// 设备编码 - 全水分析仪 #1
        /// </summary>
        public static string DeviceCode_QSFXY_1 = "#1全水分析仪";

        #region 智能存样柜

        /// <summary>
        /// 设备编码 - 智能存样柜
        /// </summary>
        public static string MachineCode_CYG1 = "#1智能存样柜";

        /// <summary>
        /// 设备编码 - 智能存样柜
        /// </summary>
        public static string MachineCode_CYG2 = "#2智能存样柜";

        #endregion

        #region 汽车机械采样机

        /// <summary>
        /// 设备编码 - 汽车机械采样机 #1
        /// </summary>
        public static string MachineCode_QCJXCYJ_1 = "#1汽车机械采样机";

        /// <summary>
        /// 设备编码 - 汽车机械采样机 #2
        /// </summary>
        public static string MachineCode_QCJXCYJ_2 = "#2汽车机械采样机";

        /// <summary>
        /// 设备编码 - 汽车机械采样机 #3
        /// </summary>
        public static string MachineCode_QCJXCYJ_3 = "#3汽车机械采样机";

        #endregion

        #region 汽车智能化

        /// <summary>
        /// 设备编码-汽车智能化-入厂端
        /// </summary>
        public static string MachineCode_QC_Queue_1 = "汽车智能化-入厂端";
        /// <summary>
        /// 设备编码-汽车智能化-#1过衡端
        /// </summary>
        public static string MachineCode_QC_Weighter_1 = "汽车智能化-#1过衡端";
        /// <summary>
        /// 设备编码-汽车智能化-#2过衡端
        /// </summary>
        public static string MachineCode_QC_Weighter_2 = "汽车智能化-#2过衡端";
        /// <summary>
        /// 设备编码-汽车智能化-#3过衡端
        /// </summary>
        public static string MachineCode_QC_Weighter_3 = "汽车智能化-#3过衡端";
        /// <summary>
        /// 设备编码-汽车智能化-#4过衡端
        /// </summary>
        public static string MachineCode_QC_Weighter_4 = "汽车智能化-#4过衡端";

        /// <summary>
        /// 设备编码-汽车智能化-#1机械采样机端
        /// </summary>
        public static string MachineCode_QC_JxSampler_1 = "汽车智能化-#1机械采样机端";
        /// <summary>
        /// 设备编码-汽车智能化-#2机械采样机端
        /// </summary>
        public static string MachineCode_QC_JxSampler_2 = "汽车智能化-#2机械采样机端";
        /// <summary>
        /// 设备编码-汽车智能化-#3机械采样机端
        /// </summary>
        public static string MachineCode_QC_JxSampler_3 = "汽车智能化-#3机械采样机端";

        /// <summary>
        /// 设备编码-汽车智能化-出厂端
        /// </summary>
        public static string MachineCode_QC_Out_1 = "汽车智能化-出厂端";

        #endregion

        #region 化验室网络管理

        /// <summary>
        /// 设备编码 - 化验室网络管理
        /// </summary>
        public static string MachineCode_AssayManage = "化验室网络管理";

        #endregion

        #region 集控首页

        /// <summary>
        /// 信号前缀名-集控首页
        /// </summary>
        public static string MachineCode_HomePage_1 = "集控首页";

        #endregion
    }
}
