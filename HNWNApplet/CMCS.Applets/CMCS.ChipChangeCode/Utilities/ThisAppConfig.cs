using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CMCS.Common
{
    /// <summary>
    /// 程序配置
    /// </summary>
    public class ThisAppConfig
    {
        private static string ConfigXmlPath = "Common.AppConfig.xml";

        private static ThisAppConfig instance;

        public static ThisAppConfig GetInstance()
        {
            return instance;
        }

        static ThisAppConfig()
        {
            instance = CMCS.Common.Utilities.XOConverter.LoadConfig<ThisAppConfig>(ConfigXmlPath);
        }

        private string hFReaderIP;
        /// <summary>
        /// 读卡器Ip地址
        /// </summary>
        [Description("读卡器Ip地址")]
        public string HFReaderIP
        {
            get { return hFReaderIP; }
            set { hFReaderIP = value; }
        }

        private int hFReaderPort;
        /// <summary>
        /// 读卡器端口号
        /// </summary>
        [Description("读卡器端口号")]
        public int HFReaderPort
        {
            get { return hFReaderPort; }
            set { hFReaderPort = value; }
        }
    }
}
