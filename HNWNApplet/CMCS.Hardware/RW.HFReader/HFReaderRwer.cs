using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReaderB;

namespace RW.HFReader
{
    public class HFReaderRwer
    {
        private string ip = string.Empty;
        /// <summary>
        /// 当前IP
        /// </summary>
        public string IP
        {
            get { return ip; }
            set { ip = value; }
        }

        private int port;
        /// <summary>
        /// 当前端口
        /// </summary>
        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        private int secNumber;
        /// <summary>
        /// 当前读卡器扇区
        /// </summary>
        public int SecNumber
        {
            get { return secNumber; }
            set { secNumber = value; }
        }

        private int blockNumber;
        /// <summary>
        /// 当前读卡器块区
        /// </summary>
        public int BlockNumber
        {
            get { return blockNumber; }
            set { blockNumber = value; }
        }

        /// <summary>
        /// 端口索引
        /// </summary>
        private int portFrmIndex = -1;

        /// <summary>
        /// 端口器地址
        /// </summary>
        private byte readerAddr = 0XFF;

        /// <summary>
        /// 指向输入数组变量，标签的序列号（UID），由防碰撞过程获取。长度为4个字节。低字节在前。
        /// </summary>
        private byte[] SNR = new byte[4];

        private string errorStr = string.Empty;
        /// <summary>
        /// 当前错误信息
        /// </summary>
        public string ErrorStr
        {
            get { return errorStr; }
            set { errorStr = value; }
        }

        private string readData = string.Empty;
        /// <summary>
        /// 当前读取到的数据块
        /// </summary>
        public string ReadData
        {
            get { return readData; }
            set { readData = value; }
        }

        private string readKey = string.Empty;
        /// <summary>
        /// 当前读取到的数据密钥块
        /// </summary>
        public string ReadKey
        {
            get { return readKey; }
            set { readKey = value; }
        }

        private bool status = false;
        /// <summary>
        /// 连接状态
        /// </summary>
        public bool Status
        {
            get { return status; }
        }

        /// <summary>
        /// 设置连接状态
        /// </summary>
        /// <param name="status"></param>
        public void SetStatus(bool status)
        {
            if (this.OnStatusChange != null) this.OnStatusChange(status);
            this.status = status;
        }

        public delegate void ScanErrorEventHandler(Exception ex);
        public event ScanErrorEventHandler OnScanError;

        public delegate void StatusChangeHandler(bool status);
        public event StatusChangeHandler OnStatusChange;

        public HFReaderRwer()
        {

        }

        /// <summary>
        /// 打开读卡器网口连接
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <param name="port">端口号</param>
        /// <param name="readerAddr">读卡器地址</param>
        /// <returns></returns>
        public bool OpenNetPort(string ip, int port, string readAddr = "FF")
        {
            readerAddr = Convert.ToByte(readAddr, 16); // $FF;

            int openresult = StaticClassReaderB.OpenNetPort(port, ip, ref readerAddr, ref portFrmIndex);
            if (openresult == 0)
            {
                if (OpenRF())
                {
                    SetStatus(true);
                    return true;
                }
                else
                    return false;
            }
            else if ((openresult == 0x35) || (openresult == 0x30))
            {
                this.ErrorStr = "TCPIP error";
                SetStatus(false);
                return false;
            }
            return false;
        }

        /// <summary>
        /// 关闭读卡器网口连接
        /// </summary>
        /// <returns></returns>
        public bool CloseNetPort()
        {
            if (portFrmIndex > 1023)
            {
                int fCmdRet = StaticClassReaderB.CloseNetPort(portFrmIndex);
                if (fCmdRet == 0)
                {
                    portFrmIndex = -1;
                    SetStatus(false);
                    return true;
                }
                else
                    return false;
            }
            return false;
        }

        /// <summary>
        /// 打开读卡器
        /// </summary>
        /// <returns></returns>
        public bool OpenRF()
        {
            int fCmdRet = 0x30;

            fCmdRet = StaticClassReaderB.OpenRf(ref readerAddr, portFrmIndex);
            if (fCmdRet == 0)
            {
                SetStatus(true);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 关闭读卡器
        /// </summary>
        /// <returns></returns>
        public bool CloseRF()
        {
            int fCmdRet = 0x30;

            fCmdRet = StaticClassReaderB.CloseRf(ref readerAddr, portFrmIndex);
            if (fCmdRet == 0)
            {
                SetStatus(false);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 切换到14443A模式
        /// </summary>
        /// <returns></returns>
        public bool ChangeToISO14443A()
        {
            int fCmdRet = 0x30;

            fCmdRet = StaticClassReaderB.ChangeTo14443A(ref readerAddr, portFrmIndex);

            if (fCmdRet == 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 14443ARequest 获取卡类型
        /// </summary>
        /// <returns></returns>
        public bool Request14443A()
        {
            int fCmdRet = 0x30;
            byte[] Data = new byte[2];
            byte errorCode = 0;

            fCmdRet = StaticClassReaderB.ISO14443ARequest(ref readerAddr, 1, Data, ref errorCode, portFrmIndex);
            if (fCmdRet == 0)
            {
                string ReaderType = ByteArrayToHexString(Data).Replace(" ", "");
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 14443AAnticol 获取卡号
        /// </summary>
        /// <returns></returns>
        public bool Anticoll14443A()
        {
            int fCmdRet = 0x30;
            byte reserved = 0;
            byte errorCode = 0;

            fCmdRet = StaticClassReaderB.ISO14443AAnticoll(ref readerAddr, reserved, SNR, ref errorCode, portFrmIndex);

            if (fCmdRet == 0)
            {
                string ReaderNumber = ByteArrayToHexString(SNR).Replace(" ", "");
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 14443ASelect 获取卡容量大小
        /// </summary>
        /// <returns></returns>
        public bool Select14443A()
        {
            int fCmdRet = 0x30;
            byte errorCode = 0;
            byte size = 0;

            fCmdRet = StaticClassReaderB.ISO14443ASelect(ref readerAddr, SNR, ref size, ref errorCode, portFrmIndex);

            if (fCmdRet == 0)
            {
                string ReaderSize = Convert.ToString(size, 16);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 使标签卡处于静止状态
        /// </summary>
        /// <returns></returns>
        public bool Halt14443A()
        {
            int fCmdRet = 0x30;
            byte errorCode = 0;

            fCmdRet = StaticClassReaderB.ISO14443AHalt(ref readerAddr, ref errorCode, portFrmIndex);
            return fCmdRet == 0;
        }

        /// <summary>
        /// 标签密钥直接证实
        /// </summary>
        /// <param name="accessSector">扇区号</param>
        /// <param name="keyStyle">证实模式，使用密钥A还是密钥B执行证实操作</param>
        /// <param name="keyValue">密钥</param>
        /// <returns></returns>
        public bool AuthKey14443A(byte accessSector, byte keyStyle, string keyValue = "FFFFFFFFFFFF")
        {
            byte errorCode = 0;
            byte[] key = new byte[5];
            int fCmdRet = 0x30;

            if ((keyValue == "") || (keyValue.Length != 12))
            {
                this.ErrorStr = "密钥长度不正确";
                return false;
            }
            key = HexStringToByteArray(keyValue);

            fCmdRet = StaticClassReaderB.ISO14443AAuthKey(ref readerAddr, keyStyle, accessSector, key, ref errorCode, portFrmIndex);
            if (fCmdRet == 0)
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="SecNumber"></param>
        /// <param name="blockNumber"></param>
        /// <returns></returns>
        public bool CheckRw(int SecNumber, int blockNumber)
        {
            if (ChangeToISO14443A())
            {
                if (Request14443A())
                {
                    if (Anticoll14443A())
                    {
                        if (Select14443A())
                        {
                            if (AuthKey14443A(Convert.ToByte(SecNumber), 0))
                            {
                                return true;
                            }
                            else
                            {
                                this.ErrorStr = "标签密钥验证失败";
                                return false;
                            }
                        }
                        else
                        {
                            this.ErrorStr = "获取卡容量失败";
                            return false;
                        }
                    }
                    else
                    {
                        this.ErrorStr = "获取卡号失败";
                        return false;
                    }
                }
                else
                {
                    this.ErrorStr = "获取卡类型失败";
                    return false;
                }
            }
            else
            {
                this.ErrorStr = "切换到1443模式失败";
                return false;
            }
        }

        /// <summary>
        /// 读标签数据
        /// </summary>
        /// <param name="SecNumber">扇区</param>
        /// <param name="blockNumber">块号</param>
        /// <returns></returns>
        public string RWRead14443A(int SecNumber, int blockNumber)
        {
            ReadData = "";
            byte block, errorCode = 0;
            byte[] data = new byte[16];
            string temp;
            int fCmdRet = 0x30;

            if (SecNumber >= 32)
                block = (byte)(128 + (SecNumber - 32) * 16 + blockNumber);
            else
                block = (byte)(SecNumber * 4 + blockNumber);

            if (CheckRw(SecNumber, blockNumber))
            {
                fCmdRet = StaticClassReaderB.ISO14443ARead(ref readerAddr, block, data, ref errorCode, portFrmIndex);
                if (fCmdRet == 0)
                {
                    Rf_Beep();

                    temp = ByteArrayToHexString(data).Replace(" ", "");
                    if ((SecNumber < 32 && blockNumber == 3) || (SecNumber >= 32 && blockNumber == 15))
                    {
                        ReadData = "";
                        ReadKey = temp;
                    }
                    else
                    {
                        ReadData = temp;
                        ReadKey = "";
                    }
                }
            }
            return ReadData;
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="str">写入的数据</param>
        /// <param name="blockNumber">扇区</param>
        /// <param name="sectorNumber">块区</param>
        /// <returns></returns>
        public bool Write14443(string str, int sectorNumber, int blockNumber)
        {
            byte block = 0, errorCode = 0;
            byte[] data = new byte[16];
            int fCmdRet = 0x30;
            if (sectorNumber > 32)
                block = (byte)(128 + (sectorNumber - 32) * 16 + blockNumber);
            else
                block = (byte)(sectorNumber * 4 + blockNumber);
            str = StringToHexString(str);
            data = HexStringToByteArray(str);
            fCmdRet = StaticClassReaderB.ISO14443AWrite(ref readerAddr, block, data, ref errorCode, portFrmIndex);
            return fCmdRet == 0;
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="str">写入的数据</param>
        /// <param name="blockNumber">扇区</param>
        /// <param name="sectorNumber">块区</param>
        /// <returns></returns>
        public bool WriteData(string str, int sectorNumber, int blockNumber)
        {
            byte block = 0, errorCode = 0;
            byte[] data = new byte[16];
            int fCmdRet = 0x30;
            if (sectorNumber > 32)
                block = (byte)(128 + (sectorNumber - 32) * 16 + blockNumber);
            else
                block = (byte)(sectorNumber * 4 + blockNumber);
            str = StringToHexString(str);
            data = HexStringToByteArray(str);
            if (CheckRw(SecNumber, blockNumber))
            {
                fCmdRet = StaticClassReaderB.ISO14443AWrite(ref readerAddr, block, data, ref errorCode, portFrmIndex);
                Rf_Beep();
                return fCmdRet == 0;
            }
            return false;
        }

        /// <summary>
        /// 执行蜂鸣
        /// </summary>
        public void Rf_Beep()
        {
            StaticClassReaderB.SetBeep(ref readerAddr, 5, 0, 1, portFrmIndex);
            System.Threading.Thread.Sleep(300);
            StaticClassReaderB.SetLED(ref readerAddr, 5, 0, 1, portFrmIndex);
        }

        #region  16进制字符串到数组之间的相互转换
        private byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }

        /// <summary>
        /// 字符串转换为16进制
        /// </summary>
        /// <param name="s"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        private string StringToHexString(string s)
        {
            byte[] b = Encoding.UTF8.GetBytes(s);//按照指定编码将string编程字节数组
            string result = string.Empty;
            for (int i = 0; i < b.Length; i++)//逐字节变为16进制字符
            {
                result += Convert.ToString(b[i], 16);
            }
            return result;
        }

        private string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
            return sb.ToString().ToUpper();
        }

        /// <summary>
        /// 16进制转换为字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string Byte16ToString(string data)
        {
            if (!string.IsNullOrWhiteSpace(data))
            {
                string Resutl = string.Empty;
                data = data.Trim();
                data.Replace(" ", "");
                data = InsertFormat(data, 2, " ");
                string[] hexValuesSplit = data.Split(' ');
                foreach (String hex in hexValuesSplit)
                {
                    int value = Convert.ToInt32(hex, 16);
                    string stringValue = Char.ConvertFromUtf32(value);
                    char charValue = (char)value;
                    Resutl += charValue.ToString();
                }
                return Resutl;
            }
            return string.Empty;
        }

        /// <summary>  
        /// 每隔n个字符插入一个字符  
        /// </summary>  
        /// <param name="input">源字符串</param>  
        /// <param name="interval">间隔字符数</param>  
        /// <param name="value">待插入值</param>  
        /// <returns>返回新生成字符串</returns>  
        public static string InsertFormat(string input, int interval, string value)
        {
            for (int i = interval; i < input.Length; i += interval + 1)
                input = input.Insert(i, value);
            return input;
        }
        #endregion

    }
}
