
namespace CMCS.CarTransport.Queue.Core
{
    /// <summary>
    /// 硬件设备类
    /// </summary>
    public class Hardwarer
    {
        static IOC.JMDM20DIOV2.JMDM20DIOV2Iocer iocer = new IOC.JMDM20DIOV2.JMDM20DIOV2Iocer();
        /// <summary>
        /// IO控制器
        /// </summary>
        public static IOC.JMDM20DIOV2.JMDM20DIOV2Iocer Iocer
        {
            get { return iocer; }
        }

        static RW.LZR12.Net.Lzr12Rwer rwer1 = new RW.LZR12.Net.Lzr12Rwer();
        /// <summary>
        /// 读卡器1(室外)
        /// </summary>
        public static RW.LZR12.Net.Lzr12Rwer Rwer1
        {
            get { return rwer1; }
        }

        /// <summary>
        /// 出厂读卡器
        /// </summary>
        static RW.LZR12.Net.Lzr12Rwer rwerOut = new RW.LZR12.Net.Lzr12Rwer();
        /// <summary>
        /// 读卡器1(室外)
        /// </summary>
        public static RW.LZR12.Net.Lzr12Rwer RwerOut
        {
            get { return rwerOut; }
        }

        static RW.LZR12.Lzr12Rwer rwer2 = new RW.LZR12.Lzr12Rwer();
        /// <summary>
        /// 读卡器3(室内发卡器)
        /// </summary>
        public static RW.LZR12.Lzr12Rwer Rwer2
        {
            get { return rwer2; }
        }

        static LED.Dynamic.YB19.YBDynamicLeder led1 = new LED.Dynamic.YB19.YBDynamicLeder(1);
        /// <summary>
        /// LED屏1
        /// </summary>
        public static LED.Dynamic.YB19.YBDynamicLeder Led1
        {
            get { return led1; }
        }
    }
}
