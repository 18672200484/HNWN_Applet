using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using System.Runtime.InteropServices;

namespace LED.Dynamic.YB19
{
    public class YBDynamicLeder
    {
        public YBDynamicLeder(byte areaID)
        {
            this.AreaID = areaID;
        }

        #region
        public class bx5k_err
        {
            public const int ERR_NO = 0; //No Error 
            public const int ERR_OUTOFGROUP = 1; //Command Group Error 
            public const int ERR_NOCMD = 2; //Command Not Found 
            public const int ERR_BUSY = 3; //The Controller is busy now 
            public const int ERR_MEMORYVOLUME = 4; //Out of the Memory Volume 
            public const int ERR_CHECKSUM = 5; //CRC16 Checksum Error 
            public const int ERR_FILENOTEXIST = 6; //File Not Exist 
            public const int ERR_FLASH = 7;//Flash Access Error 
            public const int ERR_FILE_DOWNLOAD = 8; //File Download Error 
            public const int ERR_FILE_NAME = 9; //Filename Error 
            public const int ERR_FILE_TYPE = 10;//File type Error 
            public const int ERR_FILE_CRC16 = 11;//File CRC16 Error 
            public const int ERR_FONT_NOT_EXIST = 12;//Font Library Not Exist 
            public const int ERR_FIRMWARE_TYPE = 13;//Firmware Type Error (Check the controller type) 
            public const int ERR_DATE_TIME_FORMAT = 14;//Date Time format error 
            public const int ERR_FILE_EXIST = 15;//File Exist for File overwrite 
            public const int ERR_FILE_BLOCK_NUM = 16;//File block number error 
            public const int ERR_COMMUNICATE = 100;//通信失败
            public const int ERR_PROTOCOL = 101;//协议数据不正确
            public const int ERR_TIMEOUT = 102;//通信超时
            public const int ERR_NETCLOSE = 103;//通信断开
            public const int ERR_INVALID_HAND = 104;//无效句柄
            public const int ERR_PARAMETER = 105;//参数错误
            public const int ERR_SHOULDREPEAT = 106;//需要重复上次数据包
            public const int ERR_FILE = 107;//无效文件
        }
        #endregion
        //串口停止位
        public enum serial_stopbits : byte
        {
            COM_ONESTOPBIT = 0,
            COM_ONE5STOPBITS = 1,
            COM_TWOSTOPBITS = 2,
        }
        //串口校验模式
        public enum serial_parity : byte
        {
            COM_NOPARITY = 0,
            COM_ODDPARITY = 1,
            COM_EVENPARITY = 2,
            COM_MARKPARITY = 3,
            COM_SPACEPARITY = 4,
        }
        //串口数据位
        public enum serial_databits : byte
        {
            COM_4BITS = 4,
            COM_5BITS = 5,
            COM_6BITS = 6,
            COM_7BITS = 7,
            COM_8BITS = 8,
        }
        //控制器类型
        public enum bx_5k_card_type : ushort
        {
            BX_6Q1 = 0x0166,
            BX_6Q2 = 0x0266,
            BX_6Q3 = 0x0366,
            BX_6Q2L = 0x0466,
            BX_6Q3L = 0x0566,
            BX_6E1X = 0x0474,
            BX_6E2X = 0x0574,
            BX_5E1 = 0x0154,
            BX_5E2 = 0x0254,
            BX_5E3 = 0x0354,
        }

        public enum ColorType : int
        {
            Single,
            Dichroic,
            Three,
            TRUECOLOR
        };

        public enum MatrixType : int
        {
            RG,
            RGGR
        };
        public enum BX_Screen : int
        {
            BX_V,
            BX_VI
        };

        [System.Runtime.InteropServices.UnmanagedFunctionPointerAttribute(System.Runtime.InteropServices.CallingConvention.StdCall)]
        public delegate void CallBackClientClose(uint hand, int err);

        public delegate void ScanErrorEventHandler(Exception ex);
        public event ScanErrorEventHandler OnScanError;

        /// <summary>
        /// 初始化动态库
        /// </summary>
        /// <param name="minorVer">2</param>
        /// <param name="majorVer">2</param>
        [DllImport("LedDynamic.dll", CharSet = CharSet.Unicode)]
        public static extern void InitSdk(byte minorVer, byte majorVer);
        /// <summary>
        /// 释放动态库
        /// </summary>
        [DllImport("LedDynamic.dll", CharSet = CharSet.Unicode)]
        public static extern void ReleaseSdk();
        /// <summary>
        /// 创建固定IP通讯模式
        /// </summary>
        /// <param name="led_ip">控制卡IP</param>
        /// <param name="led_port">端口号</param>
        /// <param name="card_type">控制卡型号</param>
        /// <param name="tmout_sec">创建连接超时时间</param>
        /// <param name="mode">显⽰模式：0普通模式 ； 1动态模式（动态区优先节⽬显⽰）</param>
        /// <param name="pCloseFunc">回调函数，参数值可为null</param>
        /// <returns></returns>
        [DllImport("LedDynamic.dll", CharSet = CharSet.Unicode)]
        public static extern uint CreateClient(byte[] led_ip, uint led_port, bx_5k_card_type card_type, int tmout_sec, int mode, CallBackClientClose pCloseFunc);
        /// <summary>
        /// 创建串口通讯
        /// </summary>
        /// <param name="com">串口号</param>
        /// <param name="baudrate">波特率</param>
        /// <param name="card_type">控制卡型号</param>
        /// <param name="mode">显⽰模式：0普通模式 ； 1动态模式（动态区优先节⽬显⽰）</param>
        /// <param name="ScreenID">屏号</param>
        /// <returns></returns>
        [DllImport("LedDynamic.dll", CharSet = CharSet.Unicode)]
        public static extern uint CreateComClient(byte com, uint baudrate, bx_5k_card_type card_type, int mode, ushort ScreenID);
        //销毁通讯
        [DllImport("LedDynamic.dll", CharSet = CharSet.Unicode)]
        public static extern void Destroy(uint dwHand);
        //设置通讯超时
        [DllImport("LedDynamic.dll", CharSet = CharSet.Unicode)]
        public static extern void SetTimeout(uint dwHand, uint nSec);
        //ping
        [DllImport("LedDynamic.dll", CharSet = CharSet.Unicode)]
        public static extern int CON_PING(uint dwHand);

        //删除动态区
        [DllImport("LedDynamic.dll", CharSet = CharSet.Unicode)]
        public static extern int SCREEN_DelDynamicArea(uint dwHand, byte DeleteAreaNum, byte[] DeleteAreaId);

        [DllImport("LedDynamic.dll", CharSet = CharSet.Unicode)]
        public static extern uint CON_CreateDynamicArea(uint hbmpbyte, byte AreaID, byte RunMode, ushort Timeout, byte RelateAllPro,
            ushort RelateProNum, ushort[] RelatedProgram, byte Overlap, ushort X, ushort Y, ushort W, ushort H, byte AreaFrame);

        [DllImport("LedDynamic.dll", CharSet = CharSet.Unicode)]
        public static extern void CON_DynamicAreaAddPicPage(uint hArea, byte DisplayMode, byte DisplaySpeed, ushort StayTime, byte[] imgfile);
        [DllImport("LedDynamic.dll", CharSet = CharSet.Unicode)]
        public static extern void CON_DynamicAreaAddSound(uint hArea, byte SoundPerson, byte SoundVolum, byte SoundSpeed, byte SoundDataMode,
            int SoundReplayTimes, int SoundReplayDelay, byte SoundReservedParaLen, byte Soundnumdeal, byte Soundlanguages, byte Soundwordstyle,
            int SoundDataLen, byte[] SoundData);

        [DllImport("LedDynamic.dll", CharSet = CharSet.Unicode)]
        public static extern void CON_DynamicAreaAddStrPage(uint hArea, byte DisplayMode, byte DisplaySpeed, ushort StayTime, byte[] imgfile, byte[] font, uint fontsize, uint fontColor, uint bgColor, bool bold, bool italic, bool underline, int align, bool multiline);

        [DllImport("LedDynamic.dll", CharSet = CharSet.Unicode)]
        public static extern uint CON_CreateDynamic();
        [DllImport("LedDynamic.dll", CharSet = CharSet.Unicode)]
        public static extern uint CON_CreateImgToByte(ColorType pColorType, MatrixType pMatrixType, BX_Screen pBX_Screen);
        [DllImport("LedDynamic.dll", CharSet = CharSet.Unicode)]
        public static extern void CON_DynamicArea(uint hDynamic, uint hArea);

        [DllImport("LedDynamic.dll", CharSet = CharSet.Unicode)]
        public static extern int CON_SendDynamic(uint dwHand, uint hArea, uint pbmpbyte);

        [DllImport("LedDynamic.dll", CharSet = CharSet.Unicode)]
        public static extern void CON_DestroyDynamic(uint hDynamic);

        [DllImport("LedDynamic.dll", CharSet = CharSet.Unicode)]
        public static extern void CON_deleteDynamic(uint hDynamic);

        uint m_dwCurHand = 0;

        /// <summary>
        /// 设备型号
        /// </summary>
        ushort[] card_type_list = new ushort[10] { 0x0166, 0x0266, 0x0366, 0x0466, 0x0566, 0x0474, 0x0574, 0x0154, 0x0254, 0x0354 };

        //区域编号
        byte AreaID = 0;

        /// <summary>
        /// 打开LED设备
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="areaID"></param>
        /// <returns></returns>
        public bool OpenLED(string ip)
        {
            try
            {
                YBDynamicLeder.InitSdk(2, 2);

                byte[] broad_ip = System.Text.Encoding.ASCII.GetBytes(ip);

                m_dwCurHand = YBDynamicLeder.CreateClient(broad_ip, (uint)5005, YBDynamicLeder.bx_5k_card_type.BX_6E1X, 2, 0, null);

                return m_dwCurHand != 0;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 更新LED信息
        /// </summary>
        /// <param name="str"></param>
        public void UpdateLED(string str)
        {
            try
            {
                uint pDynamic = YBDynamicLeder.CON_CreateDynamic();

                uint pbmpbyte = YBDynamicLeder.CON_CreateImgToByte((ColorType)0, (MatrixType)0, (BX_Screen)1);

                //创建动态区域
                uint hArea = YBDynamicLeder.CON_CreateDynamicArea(pbmpbyte, AreaID, 0, 1, 1, 0, new ushort[] { }, 1, 0, 0, 96, 64, 0);

                //添加图文到区域
                YBDynamicLeder.CON_DynamicAreaAddStrPage(hArea, 1, 0, 200, Encoding.Unicode.GetBytes(str), Encoding.Unicode.GetBytes("宋体"), 12, 0xffff0000, 0xff000000, false, false, false, 0, false);

                //关联区域
                YBDynamicLeder.CON_DynamicArea(pDynamic, hArea);

                //更新区域
                int err = YBDynamicLeder.CON_SendDynamic(m_dwCurHand, pDynamic, pbmpbyte);

                //销毁区域
                YBDynamicLeder.CON_DestroyDynamic(pDynamic);
            }
            catch (Exception ex)
            {
                if (OnScanError != null) OnScanError(ex);
            }
        }

        //关闭LED屏
        public void CloseLED()
        {
            try
            {
                //删除动态区域
                int err = YBDynamicLeder.SCREEN_DelDynamicArea(m_dwCurHand, 1, new byte[] { AreaID });
            }
            catch { }
        }

        //销毁通讯
        public void Destroy()
        {
            //销毁通讯
            YBDynamicLeder.Destroy(m_dwCurHand);
            //卸载SDK
            YBDynamicLeder.ReleaseSdk();
        }
    }
}
