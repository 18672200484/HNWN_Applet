using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities;
using CMCS.Monitor.Win.Frms.Sys;
using CMCS.Common.Entities.iEAA;
using CMCS.Monitor.Win.Frms;

namespace CMCS.Monitor.Win.Core
{
    /// <summary>
    /// 变量集合
    /// </summary>
    public static class SelfVars
    {
        /// <summary>
        /// 当前登录用户
        /// </summary>
        public static CmcsUser LoginUser;

        /// <summary>
        /// 主窗体引用
        /// </summary>
        public static FrmMainFrame MainFrameForm;

        /// <summary>
        /// 采样机窗体引用
        /// </summary>
        public static FrmCarSampler CarSamplerForm;

        /// <summary>
        /// 汽车衡窗体引用
        /// </summary>
        public static FrmTruckWeighter TruckWeighterForm;

        /// <summary>
        /// 网页地址 - CefTester
        /// </summary>
        public static string Url_CefTester = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Web/CefTester/index.htm");

        /// <summary>
        /// 网页地址 - 集中管控首页
        /// </summary>
        public static string Url_HomePage = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Web/HomePage/index.htm");

        #region 皮带采样机

        /// <summary>
        /// 网页地址 - 皮带采样机
        /// </summary>
        public static string Url_BeltSampler = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Web/TrainBeltSampler/index.htm");

        #endregion

        #region 汽车采样机
        /// <summary>
        /// 网页地址 - 汽车采样机
        /// </summary>
        public static string Url_CarSampler = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Web/CarSampler/index.htm");

        #endregion

        #region 全自动制样机

        /// <summary>
        /// 网页地址 - 火车全自动制样机 #1
        /// </summary>
        public static string Url_AutoMaker = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Web/AutoMaker/index.htm");

        #endregion

        #region 存样柜

        /// <summary>
        /// 网页地址 - 存样柜
        /// </summary>
        public static string Url_SampleStorage = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Web/SampleStorage/index.htm");

        public static String CurrentSelectedCell = "原煤样样柜";
        #endregion

        #region 翻车机

        /// <summary>
        /// 网页地址 - 翻车机
        /// </summary>
        public static string Url_TrunOver = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Web/TrainTipper/index.htm");

        /// <summary>
        /// 网页地址 - 翻车机
        /// </summary>
        public static string Url_TrainUpender = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Web/TrainUpender/index.htm");

        #endregion

        #region 汽车智能化

        /// <summary>
        /// 网页地址 - 汽车重车衡监控
        /// </summary>
        public static string Url_TruckWeighter = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Web/TruckWeighter/index.htm");
        /// <summary>
        /// 网页地址 - 汽车原煤仓监控
        /// </summary>
        public static string Url_TruckOrder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Web/TruckOrder/index.htm");

        #endregion

        #region 化验室网络管理

        /// <summary>
        /// 网页地址 - 化验室网络管理
        /// </summary>
        public static string Url_AssayManage = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Web/AssayManage/index.htm");

        #endregion

        #region 设备监控

        /// <summary>
        /// 网页地址 - 设备监控
        /// </summary>
        public static string Url_CarMonitor = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Web/CarMonitor/index.htm");

        #endregion
    }
}
