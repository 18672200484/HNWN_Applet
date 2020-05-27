using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// 
using log4net.Appender;
using log4net;
using System.IO;
using log4net.Repository.Hierarchy;

namespace CMCS.Common.Utilities
{
    /// <summary>
    /// 日志记录类
    /// </summary>
    public static class Log4Neter
    {
        /// <summary>
        /// log4net - NormalLoger
        /// </summary>
        private static readonly log4net.ILog NormalLoger = log4net.LogManager.GetLogger("NormalLoger");

        /// <summary>
        /// log4net - ErrorLoger
        /// </summary>
        private static readonly log4net.ILog ErrorLoger = log4net.LogManager.GetLogger("ErrorLoger");

        private static readonly string ErrorFilePath = "Logs/Error/";

        private static readonly string NormalFilePath = "Logs/Normal/";

        /// <summary>
        /// 记录异常日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void Error(object message, Exception ex)
        {
            ErrorLoger.Error(message, ex);
        }

        /// <summary>
        /// 记录普通日志
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string message)
        {
            NormalLoger.Info(string.Format("{0} - {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), message));
        }

        /// <summary>
        /// 删除指定天数之前的错误日志文件
        /// </summary>
        /// <param name="day"></param>
        public static void DeleteErrorFiles(int day)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(ErrorFilePath);
            foreach (FileInfo item in directoryInfo.GetFiles())
            {
                if (item.CreationTime < DateTime.Now.AddDays(-day))
                    item.Delete();
            }
        }

        /// <summary>
        /// 删除指定天数之前的常规日志文件
        /// </summary>
        /// <param name="day"></param>
        public static void DeleteNormalFiles(int day)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(ErrorFilePath);
            foreach (FileInfo item in directoryInfo.GetFiles())
            {
                if (item.CreationTime < DateTime.Now.AddDays(-day))
                    item.Delete();
            }
        }
    }
}
