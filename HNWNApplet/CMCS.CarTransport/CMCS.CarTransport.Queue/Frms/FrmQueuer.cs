using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CMCS.CarTransport.DAO;
using CMCS.CarTransport.Queue.Core;
using CMCS.CarTransport.Queue.Enums;
using CMCS.CarTransport.Queue.Frms.BaseInfo.Autotruck;
using CMCS.CarTransport.Queue.Frms.BaseInfo.Mine;
using CMCS.CarTransport.Queue.Frms.BaseInfo.Supplier;
using CMCS.CarTransport.Queue.Frms.BaseInfo.SupplyReceive;
using CMCS.CarTransport.Queue.Frms.BaseInfo.TransportCompany;
using CMCS.CarTransport.Queue.Frms.Sys;
using CMCS.Common;
using CMCS.Common.DAO;
using CMCS.Common.Entities;
using CMCS.Common.Entities.BaseInfo;
using CMCS.Common.Entities.CarTransport;
using CMCS.Common.Entities.Fuel;
using CMCS.Common.Entities.Inf;
using CMCS.Common.Entities.Sys;
using CMCS.Common.Enums;
using CMCS.Common.Utilities;
using CMCS.Common.Views;
using CMCS.Common.WS;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.SuperGrid;
using HikISCApi.Core;
using LED.Dynamic.YB19;
using LED.YB14;

namespace CMCS.CarTransport.Queue.Frms
{
    public partial class FrmQueuer : DevComponents.DotNetBar.Metro.MetroForm
    {
        /// <summary>
        /// 窗体唯一标识符
        /// </summary>
        public static string UniqueKey = "FrmQueuer";

        public FrmQueuer()
        {
            InitializeComponent();
        }

        #region Vars

        CarTransportDAO carTransportDAO = CarTransportDAO.GetInstance();
        QueuerDAO queuerDAO = QueuerDAO.GetInstance();
        OuterDAO outerDAO = OuterDAO.GetInstance();
        CommonDAO commonDAO = CommonDAO.GetInstance();
        CommService commService;

        IocControler iocControler;
        /// <summary>
        /// 语音播报
        /// </summary>
        VoiceSpeaker voiceSpeaker = new VoiceSpeaker();

        bool inductorCoil1 = false;
        /// <summary>
        /// 地感1状态 true=有信号  false=无信号
        /// </summary>
        public bool InductorCoil1
        {
            get
            {
                return inductorCoil1;
            }
            set
            {
                inductorCoil1 = value;

                panCurrentCarNumber.Refresh();

                commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.地感1信号.ToString(), value ? "1" : "0");
            }
        }

        int inductorCoil1Port;
        /// <summary>
        /// 地感1端口
        /// </summary>
        public int InductorCoil1Port
        {
            get { return inductorCoil1Port; }
            set { inductorCoil1Port = value; }
        }

        bool inductorCoil2 = false;
        /// <summary>
        /// 地感2状态 true=有信号  false=无信号
        /// </summary>
        public bool InductorCoil2
        {
            get
            {
                return inductorCoil2;
            }
            set
            {
                inductorCoil2 = value;

                panCurrentCarNumber.Refresh();

                commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.地感2信号.ToString(), value ? "1" : "0");
            }
        }

        int inductorCoil2Port;
        /// <summary>
        /// 地感2端口
        /// </summary>
        public int InductorCoil2Port
        {
            get { return inductorCoil2Port; }
            set { inductorCoil2Port = value; }
        }

        bool inductorCoil3 = false;
        /// <summary>
        /// 地感3状态 true=有信号  false=无信号
        /// </summary>
        public bool InductorCoil3
        {
            get
            {
                return inductorCoil3;
            }
            set
            {
                inductorCoil3 = value;

                panCurrentCarNumber.Refresh();

                commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.地感3信号.ToString(), value ? "1" : "0");
            }
        }

        int inductorCoil3Port;
        /// <summary>
        /// 地感3端口
        /// </summary>
        public int InductorCoil3Port
        {
            get { return inductorCoil3Port; }
            set { inductorCoil3Port = value; }
        }

        bool inductorCoil4 = false;
        /// <summary>
        /// 地感4状态 true=有信号  false=无信号
        /// </summary>
        public bool InductorCoil4
        {
            get
            {
                return inductorCoil4;
            }
            set
            {
                inductorCoil4 = value;

                panCurrentCarNumber.Refresh();

                commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.地感4信号.ToString(), value ? "1" : "0");
            }
        }

        int inductorCoil4Port;
        /// <summary>
        /// 地感4端口
        /// </summary>
        public int InductorCoil4Port
        {
            get { return inductorCoil4Port; }
            set { inductorCoil4Port = value; }
        }

        bool autoHandMode = true;
        /// <summary>
        /// 自动模式=true  手动模式=false
        /// </summary>
        public bool AutoHandMode
        {
            get { return autoHandMode; }
            set
            {
                autoHandMode = value;

                #region 入厂煤

                btnSelectAutotruck_BuyFuel.Visible = !value;
                btnSelectSupplier_BuyFuel.Visible = !value;
                btnSelectMine_BuyFuel.Visible = !value;
                btnSelectTransportCompany_BuyFuel.Visible = !value;

                #endregion

                #region 其他物资

                btnSelectAutotruck_Goods.Visible = !value;
                btnbtnSelectSupply_Goods.Visible = !value;
                btnSelectReceive_Goods.Visible = !value;
                btnSelectGoodsType_Goods.Visible = !value;

                #endregion
            }
        }

        #region 入厂

        public static PassCarQueuer passCarQueuer = new PassCarQueuer();

        ImperfectCar currentImperfectCar;
        /// <summary>
        /// 识别或选择的车辆凭证
        /// </summary>
        public ImperfectCar CurrentImperfectCar
        {
            get { return currentImperfectCar; }
            set
            {
                currentImperfectCar = value;

                if (value != null)
                    panCurrentCarNumber.Text = value.Voucher;
                else
                    panCurrentCarNumber.Text = "等待车辆";
            }
        }

        eFlowFlag currentFlowFlag = eFlowFlag.等待车辆;
        /// <summary>
        /// 当前业务流程标识
        /// </summary>
        public eFlowFlag CurrentFlowFlag
        {
            get { return currentFlowFlag; }
            set
            {
                currentFlowFlag = value;

                lblFlowFlag.Text = value.ToString();
            }
        }

        CmcsAutotruck currentAutotruck;
        /// <summary>
        /// 当前车
        /// </summary>
        public CmcsAutotruck CurrentAutotruck
        {
            get { return currentAutotruck; }
            set
            {
                currentAutotruck = value;

                txtCarNumber_BuyFuel.ResetText();
                txtCarNumber_SaleFuel.ResetText();
                txtCarNumber_Goods.ResetText();

                txtTagId_BuyFuel.ResetText();
                txtTagId_SaleFuel.ResetText();
                txtTagId_Goods.ResetText();

                panCurrentCarNumber.ResetText();

                if (value != null)
                {
                    commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.当前车Id.ToString(), value.Id);
                    commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.当前车号.ToString(), value.CarNumber);

                    CmcsEPCCard ePCCard = Dbers.GetInstance().SelfDber.Get<CmcsEPCCard>(value.EPCCardId);
                    if (value.CarType == eCarType.入厂煤.ToString())
                    {
                        if (ePCCard != null) txtTagId_BuyFuel.Text = ePCCard.TagId;

                        txtCarNumber_BuyFuel.Text = value.CarNumber;
                        txtTagId_BuyFuel.Text = value.EPCCardId;

                        superTabControlMain.SelectedTab = superTabItem_BuyFuel;
                    }
                    else if (value.CarType == eCarType.销售煤.ToString())
                    {
                        if (ePCCard != null) txtTagId_SaleFuel.Text = ePCCard.TagId;

                        txtCarNumber_SaleFuel.Text = value.CarNumber;
                        superTabControlMain.SelectedTab = superTabItem_SaleFuel;
                    }
                    else if (value.CarType == eCarType.其他物资.ToString())
                    {
                        if (ePCCard != null) txtTagId_Goods.Text = ePCCard.TagId;

                        txtCarNumber_Goods.Text = value.CarNumber;
                        txtTagId_Goods.Text = value.EPCCardId;

                        superTabControlMain.SelectedTab = superTabItem_Goods;
                    }

                    panCurrentCarNumber.Text = value.CarNumber;
                }
                else
                {
                    commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.当前车Id.ToString(), string.Empty);
                    commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.当前车号.ToString(), string.Empty);

                    txtCarNumber_BuyFuel.ResetText();
                    txtCarNumber_SaleFuel.ResetText();
                    txtCarNumber_Goods.ResetText();

                    txtTagId_BuyFuel.ResetText();
                    txtTagId_SaleFuel.ResetText();
                    txtTagId_Goods.ResetText();

                    panCurrentCarNumber.ResetText();
                }
            }
        }

        /// <summary>
        /// 当前调运计划信息
        /// </summary>
        public ZkBizResult CurrentZkBizResult { get; set; }

        /// <summary>
        /// 当前全过程汽车来煤预归批信息
        /// </summary>
        public View_rlgl_cygl_qclmygp CurrentQCLMYGP { get; set; }

        /// <summary>
        /// 当前采样机编号
        /// </summary>
        public string CurrentSampler { get; set; }

        /// <summary>
        /// 是否启用证件到期
        /// </summary>
        bool IsPaperWorkPass = false;

        /// <summary>
        /// 采样通道车数
        /// </summary>
        int SampleWayCount = 0;

        /// <summary>
        /// 启用采样通道车数
        /// </summary>
        bool isSampleWayCount = false;

        /// <summary>
        /// 厂内总车数
        /// </summary>
        int FactoryCount = 0;

        /// <summary>
        /// 启用厂内总车数
        /// </summary>
        bool isFactoryCount = false;

        #endregion

        #region 出厂

        public static PassCarQueuer passCarQueuerOut = new PassCarQueuer();

        ImperfectCar currentImperfectCarOut;
        /// <summary>
        /// 识别或选择的车辆凭证
        /// </summary>
        public ImperfectCar CurrentImperfectCarOut
        {
            get { return currentImperfectCarOut; }
            set
            {
                currentImperfectCarOut = value;

                if (value != null)
                    panCurrentOutCarNumber.Text = value.Voucher;
                else
                    panCurrentOutCarNumber.Text = "等待车辆";
            }
        }

        eFlowFlag currentFlowFlagOut = eFlowFlag.等待车辆;
        /// <summary>
        /// 当前业务流程标识
        /// </summary>
        public eFlowFlag CurrentFlowFlagOut
        {
            get { return currentFlowFlagOut; }
            set
            {
                currentFlowFlagOut = value;

                lblOutFlowFlag.Text = value.ToString();
            }
        }

        CmcsAutotruck currentAutotruckOut;
        /// <summary>
        /// 当前车
        /// </summary>
        public CmcsAutotruck CurrentAutotruckOut
        {
            get { return currentAutotruckOut; }
            set
            {
                currentAutotruckOut = value;

                if (currentAutotruckOut != null)
                    panCurrentOutCarNumber.Text = value.CarNumber;
                else
                    panCurrentOutCarNumber.ResetText();
            }
        }

        CmcsBuyFuelTransport currentBuyFuelTransportOut;
        /// <summary>
        /// 当前入厂煤运输记录
        /// </summary>
        public CmcsBuyFuelTransport CurrentBuyFuelTransportOut
        {
            get { return currentBuyFuelTransportOut; }
            set { currentBuyFuelTransportOut = value; }
        }

        CmcsGoodsTransport currentGoodsTransportOut;
        /// <summary>
        /// 当前其他物资运输记录
        /// </summary>
        public CmcsGoodsTransport CurrentGoodsTransportOut
        {
            get { return currentGoodsTransportOut; }
            set { currentGoodsTransportOut = value; }
        }

        #endregion

        #endregion

        /// <summary>
        /// 窗体初始化
        /// </summary>
        private void InitForm()
        {
            FrmDebugConsole.GetInstance();

            //中矿接口地址
            commService = new CommService(commonDAO.GetCommonAppletConfigString("中控接口连接地址"));

            //初始化视频
            CameraSDK.InitSDK(commonDAO.GetCommonAppletConfigString("海康平台地址"), commonDAO.GetCommonAppletConfigInt32("海康协议端口号"), commonDAO.GetCommonAppletConfigString("海康Appkey"), commonDAO.GetCommonAppletConfigString("海康Secret"));

            // 默认自动
            sbtnChangeAutoHandMode.Value = true;

            // 重置程序远程控制命令
            commonDAO.ResetAppRemoteControlCmd(CommonAppConfig.GetInstance().AppIdentifier);

            LoadFuelkind(cmbFuelName_BuyFuel, cmbFuelName_SaleFuel);
            //LoadSampleType(cmbSamplingType_BuyFuel);

            cmbCoalProduct.Items.Add("#1");
            cmbCoalProduct.Items.Add("#2");
            cmbCoalProduct.Items.Add("#3");
            cmbCoalProduct.SelectedIndex = 0;

            btnRefresh_Click(null, null);
        }

        private void FrmQueuer_Load(object sender, EventArgs e)
        {
        }

        private void FrmQueuer_Shown(object sender, EventArgs e)
        {
            InitHardware();

            InitForm();
        }

        private void FrmQueuer_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 卸载设备
            UnloadHardware();
        }

        #region 设备相关

        #region IO控制器

        void Iocer_StatusChange(bool status)
        {
            // 接收IO控制器状态 
            InvokeEx(() =>
            {
                slightIOC.LightColor = (status ? Color.Green : Color.Red);

                commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.IO控制器_连接状态.ToString(), status ? "1" : "0");
            });
        }

        /// <summary>
        /// IO控制器接收数据时触发
        /// </summary>
        /// <param name="receiveValue"></param>
        void Iocer_Received(int[] receiveValue)
        {
            // 接收地感状态  
            InvokeEx(() =>
            {
                this.InductorCoil1 = (receiveValue[this.InductorCoil1Port - 1] == 1);
                this.InductorCoil2 = (receiveValue[this.InductorCoil2Port - 1] == 1);
                this.InductorCoil3 = (receiveValue[this.InductorCoil3Port - 1] == 1);
                this.InductorCoil4 = (receiveValue[this.InductorCoil4Port - 1] == 1);
            });
        }

        /// <summary>
        /// 允许通行
        /// </summary>
        void LetPass()
        {
            if (this.CurrentImperfectCar == null) return;

            if (this.CurrentImperfectCar.PassWay == ePassWay.Way1)
            {
                this.iocControler.Gate1Up();
                this.iocControler.GreenLight1();
            }
            else if (this.CurrentImperfectCar.PassWay == ePassWay.Way2)
            {
                this.iocControler.Gate2Up();
                this.iocControler.GreenLight2();
            }
        }

        /// <summary>
        /// 阻断前行
        /// </summary>
        void LetBlocking()
        {
            if (this.CurrentImperfectCar == null) return;

            if (this.CurrentImperfectCar.PassWay == ePassWay.Way1)
            {
                this.iocControler.Gate1Down();
                this.iocControler.RedLight1();
            }
            else if (this.CurrentImperfectCar.PassWay == ePassWay.Way2)
            {
                this.iocControler.Gate2Down();
                this.iocControler.RedLight2();
            }
        }

        #endregion

        #region 入厂读卡器

        void Rwer1_OnScanError(Exception ex)
        {
            Log4Neter.Error("入厂读卡器", ex);
        }

        void Rwer1_OnStatusChange(bool status)
        {
            // 接收读卡器1状态 
            InvokeEx(() =>
             {
                 slightRwer1.LightColor = (status ? Color.Green : Color.Red);

                 commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.读卡器1_连接状态.ToString(), status ? "1" : "0");
             });
        }

        #endregion

        #region 出厂读卡器

        void RwerOut_OnScanError(Exception ex)
        {
            Log4Neter.Error("出厂读卡器", ex);
        }

        void RwerOut_OnStatusChange(bool status)
        {
            // 接收读卡器1状态 
            InvokeEx(() =>
            {
                slightRwer2.LightColor = (status ? Color.Green : Color.Red);

                commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.读卡器2_连接状态.ToString(), status ? "1" : "0");
            });
        }

        #endregion

        #region LED显示屏

        /// <summary>
        /// 生成12字节的文本内容
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GenerateFillLedContent12(string value)
        {
            int length = Encoding.Default.GetByteCount(value);
            if (length < 12) return value + "".PadRight(12 - length, ' ');

            return value;
        }

        /// <summary>
        /// 更新LED动态区域
        /// </summary>
        /// <param name="value1">第一行内容</param>
        /// <param name="value2">第二行内容</param>
        private void UpdateLedShow(string value1 = "", string value2 = "")
        {
            if (this.CurrentImperfectCar == null) return;

            UpdateLed1Show(value1, value2);
        }

        /// <summary>
        /// 显示调试窗口信息
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        private void UpdateShowDebug(string value1 = "", string value2 = "")
        {
            FrmDebugConsole.GetInstance().Output("出厂信息:|" + value1 + "|" + value2 + "|");
        }

        #region LED1控制卡

        /// <summary>
        /// LED1控制卡屏号
        /// </summary>
        int LED1nScreenNo = 1;
        /// <summary>
        /// LED1动态区域号
        /// </summary>
        int LED1DYArea_ID = 1;
        /// <summary>
        /// LED1更新标识
        /// </summary>
        bool LED1m_bSendBusy = false;

        private bool _LED1ConnectStatus;
        /// <summary>
        /// LED1连接状态
        /// </summary>
        public bool LED1ConnectStatus
        {
            get
            {
                return _LED1ConnectStatus;
            }

            set
            {
                _LED1ConnectStatus = value;

                slightLED1.LightColor = (value ? Color.Green : Color.Red);

                commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.LED屏1_连接状态.ToString(), value ? "1" : "0");
            }
        }

        /// <summary>
        /// LED1显示内容文本
        /// </summary>
        string LED1TempFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Led1TempFile.txt");

        /// <summary>
        /// LED1上一次显示内容
        /// </summary>
        string LED1PrevLedFileContent = string.Empty;

        /// <summary>
        /// 更新LED1动态区域
        /// </summary>
        /// <param name="value1">第一行内容</param>
        /// <param name="value2">第二行内容</param>
        private void UpdateLed1Show(string value1 = "", string value2 = "")
        {
            FrmDebugConsole.GetInstance().Output("更新LED1:|" + value1 + "|" + value2 + "|");

            if (!this.LED1ConnectStatus) return;
            if (this.LED1PrevLedFileContent == value1 + value2) return;

            string ledContent = GenerateFillLedContent12(value1) + GenerateFillLedContent12(value2);

            File.WriteAllText(this.LED1TempFile, ledContent, Encoding.UTF8);

            Hardwarer.Led1.UpdateLED(ledContent);

            this.LED1PrevLedFileContent = value1 + value2;
        }

        #endregion

        void Led1_OnScanError(Exception ex)
        {
            Log4Neter.Error("LED屏1", ex);
        }

        #endregion

        #region 设备初始化与卸载

        /// <summary>
        /// 初始化外接设备
        /// </summary>
        private void InitHardware()
        {
            try
            {
                bool success = false;

                this.InductorCoil1Port = commonDAO.GetAppletConfigInt32("IO控制器_地感1端口");
                this.InductorCoil2Port = commonDAO.GetAppletConfigInt32("IO控制器_地感2端口");
                this.InductorCoil3Port = commonDAO.GetAppletConfigInt32("IO控制器_地感3端口");
                this.InductorCoil4Port = commonDAO.GetAppletConfigInt32("IO控制器_地感4端口");

                //公共配置
                this.IsPaperWorkPass = commonDAO.GetCommonAppletConfigString("启用证件到期") == "1";
                this.SampleWayCount = commonDAO.GetCommonAppletConfigInt32("采样通道车数");
                this.isSampleWayCount = (commonDAO.GetCommonAppletConfigString("启用采样通道车数") == "1");
                this.FactoryCount = commonDAO.GetCommonAppletConfigInt32("厂内总车数");
                this.isFactoryCount = (commonDAO.GetCommonAppletConfigString("启用厂内总车数") == "1");

                // IO控制器
                //Hardwarer.Iocer.OnReceived += new IOC.JMDM20DIOV2.JMDM20DIOV2Iocer.ReceivedEventHandler(Iocer_Received);
                //Hardwarer.Iocer.OnStatusChange += new IOC.JMDM20DIOV2.JMDM20DIOV2Iocer.StatusChangeHandler(Iocer_StatusChange);
                success = Hardwarer.Iocer.OpenCom(commonDAO.GetAppletConfigInt32("IO控制器_串口"), commonDAO.GetAppletConfigInt32("IO控制器_波特率"), commonDAO.GetAppletConfigInt32("IO控制器_数据位"), (StopBits)commonDAO.GetAppletConfigInt32("IO控制器_停止位"), (Parity)commonDAO.GetAppletConfigInt32("IO控制器_校验位"));
                if (!success) MessageBoxEx.Show("IO控制器连接失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.iocControler = new IocControler(Hardwarer.Iocer);

                // 读卡器1
                Hardwarer.Rwer1.StartWith = commonDAO.GetAppletConfigString("读卡器_标签过滤");
                Hardwarer.Rwer1.OnStatusChange += new RW.LZR12.Net.Lzr12Rwer.StatusChangeHandler(Rwer1_OnStatusChange);
                Hardwarer.Rwer1.OnScanError += new RW.LZR12.Net.Lzr12Rwer.ScanErrorEventHandler(Rwer1_OnScanError);
                success = Hardwarer.Rwer1.OpenCom(commonDAO.GetAppletConfigString("读卡器1_Ip"), commonDAO.GetAppletConfigInt32("读卡器1_端口"));
                if (!success) MessageBoxEx.Show("入厂读卡器连接失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Hardwarer.RwerOut.StartWith = commonDAO.GetAppletConfigString("读卡器_标签过滤");
                Hardwarer.RwerOut.OnStatusChange += new RW.LZR12.Net.Lzr12Rwer.StatusChangeHandler(RwerOut_OnStatusChange);
                Hardwarer.RwerOut.OnScanError += new RW.LZR12.Net.Lzr12Rwer.ScanErrorEventHandler(RwerOut_OnScanError);
                success = Hardwarer.RwerOut.OpenCom(commonDAO.GetAppletConfigString("读卡器2_Ip"), commonDAO.GetAppletConfigInt32("读卡器2_端口"));
                if (!success) MessageBoxEx.Show("出厂读卡器连接失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                #region LED控制卡1

                string led1SocketIP = commonDAO.GetAppletConfigString("LED显示屏1_IP地址");
                if (!string.IsNullOrEmpty(led1SocketIP))
                {
                    if (CommonUtil.PingReplyTest(led1SocketIP))
                    {
                        Hardwarer.Led1.OnScanError += new YBDynamicLeder.ScanErrorEventHandler(Led1_OnScanError);
                        if (Hardwarer.Led1.OpenLED(led1SocketIP))
                        {
                            // 初始化成功
                            this.LED1ConnectStatus = true;
                            UpdateLed1Show("  等待车辆");
                        }
                        else
                        {
                            this.LED1ConnectStatus = false;
                            Log4Neter.Error("初始化LED1控制卡失败", new Exception("连接设备"));
                            MessageBoxEx.Show("LED1控制卡连接失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        this.LED1ConnectStatus = false;
                        Log4Neter.Error("初始化LED1控制卡，网络连接失败", new Exception("网络异常"));
                        MessageBoxEx.Show("LED1控制卡网络连接失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.LED屏1_连接状态.ToString(), this.LED1ConnectStatus ? "1" : "0");

                #endregion

                //语音设置
                voiceSpeaker.SetVoice(commonDAO.GetAppletConfigInt32("语速"), commonDAO.GetAppletConfigInt32("音量"), commonDAO.GetAppletConfigString("语音包"));

                //入厂业务
                timer1.Enabled = true;

                //出厂业务
                timer_Out.Enabled = true;
            }
            catch (Exception ex)
            {
                Log4Neter.Error("设备初始化", ex);
            }
        }

        /// <summary>
        /// 卸载设备
        /// </summary>
        private void UnloadHardware()
        {
            // 注意此段代码
            Application.DoEvents();

            try
            {
                commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.当前车Id.ToString(), string.Empty);
                commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.当前车号.ToString(), string.Empty);
            }
            catch { }
            try
            {
                //Hardwarer.Iocer.OnReceived -= new IOC.JMDM20DIOV2.JMDM20DIOV2Iocer.ReceivedEventHandler(Iocer_Received);
                //Hardwarer.Iocer.OnStatusChange -= new IOC.JMDM20DIOV2.JMDM20DIOV2Iocer.StatusChangeHandler(Iocer_StatusChange);

                Hardwarer.Iocer.CloseCom();
            }
            catch { }
            try
            {
                Hardwarer.Rwer1.CloseCom();
            }
            catch { }
            try
            {
                Hardwarer.Rwer2.CloseCom();
            }
            catch { }
            try
            {
                if (this.LED1ConnectStatus)
                {
                    Hardwarer.Led1.CloseLED();
                }
            }
            catch { }
        }

        #endregion

        #endregion

        #region 道闸控制按钮

        /// <summary>
        /// 道闸1升杆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGate1Up_Click(object sender, EventArgs e)
        {
            if (this.iocControler != null) this.iocControler.Gate1Up();
        }

        /// <summary>
        /// 道闸1降杆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGate1Down_Click(object sender, EventArgs e)
        {
            if (this.iocControler != null) this.iocControler.Gate1Down();
        }

        /// <summary>
        /// 道闸2升杆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGate2Up_Click(object sender, EventArgs e)
        {
            if (this.iocControler != null) this.iocControler.Gate2Up();
        }

        /// <summary>
        /// 道闸2降杆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGate2Down_Click(object sender, EventArgs e)
        {
            if (this.iocControler != null) this.iocControler.Gate2Down();
        }

        #endregion

        #region 公共业务

        /// <summary>
        /// 读卡、车号识别任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            timer1.Interval = 2000;

            try
            {
                // 执行远程命令
                ExecAppRemoteControlCmd();

                switch (this.CurrentFlowFlag)
                {
                    case eFlowFlag.等待车辆:
                        #region

                        //提高灵敏度
                        timer1.Interval = 1000;

                        // PassWay.Way1
                        if (Hardwarer.Rwer1.Status)
                        {
                            // 当读卡区域地感有信号，触发读卡或者车号识别

                            List<string> tags = Hardwarer.Rwer1.ScanTags();
                            if (tags.Count > 0) passCarQueuer.Enqueue(ePassWay.Way1, tags[0], true);
                        }
                        // PassWay.Way2
                        else if (Hardwarer.Rwer2.Status)
                        {
                            // 当读卡区域地感有信号，触发读卡或者车号识别

                            List<string> tags = Hardwarer.Rwer2.ScanTags();
                            if (tags.Count > 0) passCarQueuer.Enqueue(ePassWay.Way2, tags[0], true);
                        }

                        if (passCarQueuer.Count > 0) this.CurrentFlowFlag = eFlowFlag.验证车辆;

                        #endregion
                        break;

                    case eFlowFlag.验证车辆:
                        #region

                        // 队列中无车时，等待车辆
                        if (passCarQueuer.Count == 0)
                        {
                            this.CurrentFlowFlag = eFlowFlag.等待车辆;
                            break;
                        }

                        this.CurrentImperfectCar = passCarQueuer.Dequeue();

                        // 方式一：根据识别的车牌号查找车辆信息
                        this.CurrentAutotruck = carTransportDAO.GetAutotruckByCarNumber(this.CurrentImperfectCar.Voucher);
                        if (this.CurrentAutotruck == null)
                            // 方式二：根据识别的标签卡查找车辆信息
                            this.CurrentAutotruck = carTransportDAO.GetAutotruckByTagId(this.CurrentImperfectCar.Voucher);

                        if (this.CurrentAutotruck != null)
                        {
                            UpdateLedShow(this.CurrentAutotruck.CarNumber);

                            if (this.CurrentAutotruck.IsUse == 1)
                            {
                                //判断车辆保险、行驶证、道路从业资格证有效时间
                                if (this.IsPaperWorkPass)
                                {
                                    if (this.CurrentAutotruck.BX_EndDate < DateTime.Now)
                                    {
                                        UpdateLedShow(this.CurrentAutotruck.CarNumber, "保险已到期");
                                        timer1.Interval = 8000;
                                        this.CurrentFlowFlag = eFlowFlag.异常重置1;
                                        break;
                                    }
                                    if (this.CurrentAutotruck.Xshzh_EndDate < DateTime.Now)
                                    {
                                        UpdateLedShow(this.CurrentAutotruck.CarNumber, "行驶证已到期");
                                        timer1.Interval = 8000;
                                        this.CurrentFlowFlag = eFlowFlag.异常重置1;
                                        break;
                                    }
                                    if (this.CurrentAutotruck.Zgzh_EndDate < DateTime.Now)
                                    {
                                        UpdateLedShow(this.CurrentAutotruck.CarNumber, "资格证已到期");
                                        timer1.Interval = 8000;
                                        this.CurrentFlowFlag = eFlowFlag.异常重置1;
                                        break;
                                    }
                                }

                                // 判断是否存在未完结的运输记录，若存在则需用户确认
                                bool hasUnFinish = false;

                                CmcsUnFinishTransport unFinishTransport = carTransportDAO.GetUnFinishTransportByAutotruckId(this.CurrentAutotruck.Id, this.CurrentAutotruck.CarType);
                                if (unFinishTransport != null)
                                {
                                    UpdateLedShow(this.CurrentAutotruck.CarNumber, "已进厂");
                                    timer1.Interval = 8000;
                                    this.CurrentFlowFlag = eFlowFlag.异常重置1;
                                    break;
                                }

                                if (!hasUnFinish)
                                {
                                    if (this.CurrentAutotruck.CarType == eCarType.入厂煤.ToString())
                                    {
                                        CmcsBuyFuelTransport cmcsBuyFuelTransport = commonDAO.SelfDber.Entity<CmcsBuyFuelTransport>("where CarNumber=:CarNumber order by TareTime desc", new { CarNumber = this.CurrentAutotruck.CarNumber });
                                        if (cmcsBuyFuelTransport != null && cmcsBuyFuelTransport.TareTime.AddHours(1) > DateTime.Now)
                                        {
                                            //防止出厂车辆被入厂读卡器识别到
                                            UpdateLedShow(this.CurrentAutotruck.CarNumber, "车号误识别");
                                            timer1.Interval = 4000;
                                            this.CurrentFlowFlag = eFlowFlag.异常重置1;
                                            break;
                                        }

                                        this.timer_BuyFuel_Cancel = false;

                                        if (this.AutoHandMode)
                                            this.CurrentFlowFlag = eFlowFlag.匹配调运;
                                        else
                                            this.CurrentFlowFlag = eFlowFlag.数据录入;
                                    }
                                    else if (this.CurrentAutotruck.CarType == eCarType.销售煤.ToString())
                                    {
                                        this.timer_SaleFuel_Cancel = false;
                                        this.CurrentFlowFlag = eFlowFlag.数据录入;
                                    }
                                    else if (this.CurrentAutotruck.CarType == eCarType.其他物资.ToString())
                                    {
                                        this.timer_Goods_Cancel = false;

                                        if (this.AutoHandMode)
                                            this.CurrentFlowFlag = eFlowFlag.匹配调运;
                                        else
                                            this.CurrentFlowFlag = eFlowFlag.数据录入;
                                    }
                                }
                            }
                            else
                            {
                                UpdateLedShow(this.CurrentAutotruck.CarNumber, "已停用");
                                this.voiceSpeaker.Speak("车牌号 " + this.CurrentAutotruck.CarNumber + " 已停用，禁止通过", 1, false);

                                timer1.Interval = 8000;
                            }
                        }
                        else
                        {
                            UpdateLedShow(this.CurrentImperfectCar.Voucher, "未登记");

                            // 方式二：刷卡方式
                            this.voiceSpeaker.Speak("卡号未登记，禁止通过", 1, false);

                            timer1.Interval = 8000;
                        }

                        #endregion
                        break;

                    case eFlowFlag.异常重置1:
                        #region

                        ResetBuyFuel();

                        #endregion
                        break;
                }
            }
            catch (Exception ex)
            {
                Log4Neter.Error("timer1_Tick", ex);
            }
            finally
            {
                timer1.Start();
            }
        }

        /// <summary>
        /// 慢速任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Stop();
            // 三分钟执行一次
            timer2.Interval = 180000;

            try
            {

            }
            catch (Exception ex)
            {
                Log4Neter.Error("timer2_Tick", ex);
            }
            finally
            {
                timer2.Start();
            }
        }

        bool lastIOStatus = false;

        /// <summary>
        /// 设备状态显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer3_Tick(object sender, EventArgs e)
        {
            try
            {
                #region 控制器

                this.InductorCoil1 = (Hardwarer.Iocer.ReceiveValue[this.InductorCoil1Port - 1] == 1);
                this.InductorCoil2 = (Hardwarer.Iocer.ReceiveValue[this.InductorCoil2Port - 1] == 1);
                this.InductorCoil3 = (Hardwarer.Iocer.ReceiveValue[this.InductorCoil3Port - 1] == 1);
                this.InductorCoil4 = (Hardwarer.Iocer.ReceiveValue[this.InductorCoil4Port - 1] == 1);

                if (lastIOStatus != Hardwarer.Iocer.Status)
                {
                    slightIOC.LightColor = (Hardwarer.Iocer.Status ? Color.Green : Color.Red);
                    commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.IO控制器_连接状态.ToString(), Hardwarer.Iocer.Status ? "1" : "0");
                }

                #endregion

                lastIOStatus = Hardwarer.Iocer.Status;
            }
            catch { }
        }

        /// <summary>
        /// 有车辆在当前道路上
        /// </summary>
        /// <returns></returns>
        bool HasCarOnCurrentWay()
        {
            if (this.CurrentImperfectCar == null) return false;

            if (this.CurrentImperfectCar.PassWay == ePassWay.UnKnow)
                return false;
            else if (this.CurrentImperfectCar.PassWay == ePassWay.Way1)
                return this.InductorCoil1 || this.InductorCoil2;
            else if (this.CurrentImperfectCar.PassWay == ePassWay.Way2)
                return this.InductorCoil3 || this.InductorCoil4;

            return true;
        }

        /// <summary>
        /// 加载煤种
        /// </summary>
        void LoadFuelkind(params ComboBoxEx[] comboBoxEx)
        {
            foreach (ComboBoxEx item in comboBoxEx)
            {
                item.DisplayMember = "Name";
                item.ValueMember = "Id";
                item.DataSource = Dbers.GetInstance().SelfDber.Entities<CmcsFuelKind>("where IsStop=0 and ParentId is not null");
            }
        }

        /// <summary>
        /// 加载采样方式
        /// </summary>
        void LoadSampleType(ComboBoxEx comboBoxEx)
        {
            comboBoxEx.DisplayMember = "Content";
            comboBoxEx.ValueMember = "Code";
            comboBoxEx.DataSource = commonDAO.GetCodeContentByKind("采样方式");

            comboBoxEx.Text = eSamplingType.机械采样.ToString();
        }

        /// <summary>
        /// 执行远程命令
        /// </summary>
        void ExecAppRemoteControlCmd()
        {
            // 获取最新的命令
            CmcsAppRemoteControlCmd appRemoteControlCmd = commonDAO.GetNewestAppRemoteControlCmd(CommonAppConfig.GetInstance().AppIdentifier);
            if (appRemoteControlCmd != null)
            {
                if (appRemoteControlCmd.CmdCode == "控制道闸")
                {
                    Log4Neter.Info("接收远程命令：" + appRemoteControlCmd.CmdCode + "，参数：" + appRemoteControlCmd.Param);

                    if (appRemoteControlCmd.Param.Equals("Gate1Up", StringComparison.CurrentCultureIgnoreCase))
                        this.iocControler.Gate1Up();
                    else if (appRemoteControlCmd.Param.Equals("Gate1Down", StringComparison.CurrentCultureIgnoreCase))
                        this.iocControler.Gate1Down();
                    else if (appRemoteControlCmd.Param.Equals("Gate2Up", StringComparison.CurrentCultureIgnoreCase))
                        this.iocControler.Gate2Up();
                    else if (appRemoteControlCmd.Param.Equals("Gate2Down", StringComparison.CurrentCultureIgnoreCase))
                        this.iocControler.Gate2Down();

                    // 更新执行结果
                    commonDAO.SetAppRemoteControlCmdResultCode(appRemoteControlCmd, eEquInfCmdResultCode.成功);
                }
            }
        }

        /// <summary>
        /// 刷新列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // 入厂煤
            LoadTodayUnFinishBuyFuelTransport();
            LoadTodayFinishBuyFuelTransport();

            // 销售煤 
            LoadTodayUnFinishSaleFuelTransport();
            LoadTodayFinishSaleFuelTransport();

            // 其他物资
            LoadTodayUnFinishGoodsTransport();
            LoadTodayFinishGoodsTransport();
        }

        private void sbtnChangeAutoHandMode_ValueChanged(object sender, EventArgs e)
        {
            this.AutoHandMode = sbtnChangeAutoHandMode.Value;
        }

        #endregion

        #region 入厂煤业务

        bool timer_BuyFuel_Cancel = true;

        private CmcsSupplier selectedSupplier_BuyFuel;
        /// <summary>
        /// 选择的供煤单位
        /// </summary>
        public CmcsSupplier SelectedSupplier_BuyFuel
        {
            get { return selectedSupplier_BuyFuel; }
            set
            {
                selectedSupplier_BuyFuel = value;

                if (value != null)
                {
                    txtSupplierName_BuyFuel.Text = value.Name;
                }
                else
                {
                    txtSupplierName_BuyFuel.ResetText();
                }
            }
        }

        private CmcsTransportCompany selectedTransportCompany_BuyFuel;
        /// <summary>
        /// 选择的运输单位
        /// </summary>
        public CmcsTransportCompany SelectedTransportCompany_BuyFuel
        {
            get { return selectedTransportCompany_BuyFuel; }
            set
            {
                selectedTransportCompany_BuyFuel = value;

                if (value != null)
                {
                    txtTransportCompanyName_BuyFuel.Text = value.Name;
                }
                else
                {
                    txtTransportCompanyName_BuyFuel.ResetText();
                }
            }
        }

        private CmcsMine selectedMine_BuyFuel;
        /// <summary>
        /// 选择的矿点
        /// </summary>
        public CmcsMine SelectedMine_BuyFuel
        {
            get { return selectedMine_BuyFuel; }
            set
            {
                selectedMine_BuyFuel = value;

                if (value != null)
                {
                    txtMineName_BuyFuel.Text = value.Name;
                }
                else
                {
                    txtMineName_BuyFuel.ResetText();
                }
            }
        }

        private CmcsFuelKind selectedFuelKind_BuyFuel;
        /// <summary>
        /// 选择的煤种
        /// </summary>
        public CmcsFuelKind SelectedFuelKind_BuyFuel
        {
            get { return selectedFuelKind_BuyFuel; }
            set
            {
                if (value != null)
                {
                    selectedFuelKind_BuyFuel = value;
                    cmbFuelName_BuyFuel.Text = value.Name;
                }
                else
                {
                    cmbFuelName_BuyFuel.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// 选择车辆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectAutotruck_BuyFuel_Click(object sender, EventArgs e)
        {
            FrmAutotruck_Select frm = new FrmAutotruck_Select("and CarType='" + eCarType.入厂煤.ToString() + "' and IsUse=1 order by CarNumber asc");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                passCarQueuer.Enqueue(ePassWay.UnKnow, frm.Output.CarNumber, false);
                this.CurrentFlowFlag = eFlowFlag.验证车辆;
            }
        }

        /// <summary>
        /// 选择供煤单位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectSupplier_BuyFuel_Click(object sender, EventArgs e)
        {
            FrmSupplier_Select frm = new FrmSupplier_Select("where IsStop=0 order by Name asc");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                this.SelectedSupplier_BuyFuel = frm.Output;
            }
        }

        /// <summary>
        /// 选择矿点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectMine_BuyFuel_Click(object sender, EventArgs e)
        {
            FrmMine_Select frm = new FrmMine_Select("where IsStop=0 order by Name asc");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                this.SelectedMine_BuyFuel = frm.Output;
            }
        }

        /// <summary>
        /// 选择运输单位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectTransportCompany_BuyFuel_Click(object sender, EventArgs e)
        {
            FrmTransportCompany_Select frm = new FrmTransportCompany_Select("where IsStop=0 order by Name asc");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                this.SelectedTransportCompany_BuyFuel = frm.Output;
            }
        }

        /// <summary>
        /// 选择煤种
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbFuelName_BuyFuel_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SelectedFuelKind_BuyFuel = cmbFuelName_BuyFuel.SelectedItem as CmcsFuelKind;
        }

        /// <summary>
        /// 新车登记
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewAutotruck_BuyFuel_Click(object sender, EventArgs e)
        {
            // eCarType.入厂煤

            new FrmAutotruck_Oper(Guid.NewGuid().ToString(), eEditMode.新增).Show();
        }

        /// <summary>
        /// 选择入厂煤来煤预报
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectForecast_BuyFuel_Click(object sender, EventArgs e)
        {
            FrmBuyFuelForecast_Select frm = new FrmBuyFuelForecast_Select();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                this.SelectedFuelKind_BuyFuel = commonDAO.SelfDber.Get<CmcsFuelKind>(frm.Output.FuelKindId);
                this.SelectedMine_BuyFuel = commonDAO.SelfDber.Get<CmcsMine>(frm.Output.MineId);
                this.SelectedSupplier_BuyFuel = commonDAO.SelfDber.Get<CmcsSupplier>(frm.Output.SupplierId);
                this.SelectedTransportCompany_BuyFuel = commonDAO.SelfDber.Get<CmcsTransportCompany>(frm.Output.TransportCompanyId);
            }
        }

        /// <summary>
        /// 保存入厂煤运输记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveTransport_BuyFuel_Click(object sender, EventArgs e)
        {
            SaveBuyFuelTransport();
        }

        /// <summary>
        /// 保存运输记录
        /// </summary>
        /// <returns></returns>
        bool SaveBuyFuelTransport()
        {
            if (this.CurrentAutotruck == null)
            {
                MessageBoxEx.Show("请选择车辆", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(this.CurrentAutotruck.EPCCardId))
            {
                MessageBoxEx.Show("车辆标签卡不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (this.SelectedFuelKind_BuyFuel == null)
            {
                MessageBoxEx.Show("请选择煤种", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (this.SelectedMine_BuyFuel == null)
            {
                MessageBoxEx.Show("请选择矿点", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (this.SelectedSupplier_BuyFuel == null)
            {
                MessageBoxEx.Show("请选择供煤单位", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (this.SelectedTransportCompany_BuyFuel == null)
            {
                MessageBoxEx.Show("请选择运输单位", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (txtTicketWeight_BuyFuel.Value <= 0)
            {
                MessageBoxEx.Show("请输入有效的矿发量", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            try
            {
                // 生成入厂煤排队记录，同时生成批次信息以及采制化三级编码
                if (queuerDAO.JoinQueueBuyFuelTransport(this.CurrentAutotruck, this.SelectedSupplier_BuyFuel, this.SelectedMine_BuyFuel, this.SelectedTransportCompany_BuyFuel, this.SelectedFuelKind_BuyFuel, (decimal)txtTicketWeight_BuyFuel.Value, DateTime.Now, (this.CurrentQCLMYGP == null ? "" : this.CurrentQCLMYGP.Mpph), this.CurrentSampler, txtRemark_BuyFuel.Text, CommonAppConfig.GetInstance().AppIdentifier))
                {
                    #region 向智能调运反馈验票结果
                    try { ZkBizResult entity = commService.SetVerifyResult(this.CurrentAutotruck.CarNumber, true, "", "", 1); }
                    catch (Exception ex) { Log4Neter.Error("向智能调运反馈验票结果", ex); }
                    #endregion

                    #region 向全过程反馈验票结果
                    try
                    {
                        commonDAO.SelfDber.Insert(new View_rlgl_chlgl_yapfk()
                        {
                            Yptime = DateTime.Now,
                            Rfid_xlh = this.CurrentAutotruck.EPCCardId,
                            Kfl = txtTicketWeight_BuyFuel.Value.ToString(),
                            Dybh = this.CurrentZkBizResult.DispatchInfo.TaskNo,
                            Chph = this.CurrentAutotruck.CarNumber,
                            Cyjbm = this.CurrentSampler,
                        });
                    }
                    catch (Exception ex) { Log4Neter.Error("向全过程反馈验票结果", ex); }
                    #endregion

                    btnSaveTransport_BuyFuel.Enabled = false;

                    if (this.AutoHandMode)
                        UpdateLedShow(this.CurrentAutotruck.CarNumber, "请前往" + this.CurrentSampler + "采样机");
                    else
                        MessageBoxEx.Show(this.CurrentAutotruck.CarNumber + "排队成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 降低灵敏度
                    timer_BuyFuel.Interval = 8000;
                    this.CurrentFlowFlag = eFlowFlag.等待离开;

                    LoadTodayUnFinishBuyFuelTransport();
                    LoadTodayFinishBuyFuelTransport();

                    LetPass();

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("保存失败\r\n" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);

                Log4Neter.Error("保存运输记录", ex);
            }

            return false;
        }

        /// <summary>
        /// 重置入厂煤运输记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_BuyFuel_Click(object sender, EventArgs e)
        {
            ResetBuyFuel();
        }

        /// <summary>
        /// 重置信息
        /// </summary>
        void ResetBuyFuel()
        {
            this.timer_BuyFuel_Cancel = true;

            this.CurrentFlowFlag = eFlowFlag.等待车辆;

            this.CurrentAutotruck = null;
            this.SelectedMine_BuyFuel = null;
            this.SelectedSupplier_BuyFuel = null;
            this.SelectedTransportCompany_BuyFuel = null;
            this.CurrentZkBizResult = null;
            this.CurrentQCLMYGP = null;
            this.CurrentSampler = string.Empty;

            txtTagId_BuyFuel.ResetText();
            txtTicketWeight_BuyFuel.Value = 0;
            txtRemark_BuyFuel.ResetText();

            btnSaveTransport_BuyFuel.Enabled = true;

            UpdateLedShow("  等待车辆");

            // 最后重置
            this.CurrentImperfectCar = null;
        }

        /// <summary>
        /// 入厂煤运输记录业务定时器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_BuyFuel_Tick(object sender, EventArgs e)
        {
            if (this.timer_BuyFuel_Cancel) return;

            timer_BuyFuel.Stop();
            timer_BuyFuel.Interval = 2000;

            try
            {
                switch (this.CurrentFlowFlag)
                {
                    case eFlowFlag.匹配调运:
                        #region

                        try
                        {
                            //测试
                            //this.CurrentZkBizResult = new ZkBizResult() { DispatchInfo = new DispatchInfo() { TaskNo = "W899191201", SendTare = 32.2 }, Message = "", Status = true };
                            this.CurrentZkBizResult = commService.GetRunningElecDisp(this.CurrentAutotruck.CarNumber, 1);
                            if (this.CurrentZkBizResult == null)
                            {
                                UpdateLedShow("智能调运读取失败，请联系中矿");
                                timer_BuyFuel.Interval = 8000;
                                this.CurrentFlowFlag = eFlowFlag.异常重置2;
                                break;
                            }
                            if (!this.CurrentZkBizResult.Status)
                            {
                                UpdateLedShow("智能调运读取失败，请联系中矿");
                                timer_BuyFuel.Interval = 8000;
                                this.CurrentFlowFlag = eFlowFlag.异常重置2;
                                break;
                            }

                            if (this.CurrentZkBizResult.DispatchInfo.SendTare <= 0)
                            {
                                UpdateLedShow("矿发净重为0，请联系中矿");
                                timer_BuyFuel.Interval = 8000;
                                this.CurrentFlowFlag = eFlowFlag.异常重置2;
                                break;
                            }

                            //获取调运信息
                            txtRemark_BuyFuel.Text = "运输单位：" + this.CurrentZkBizResult.DispatchInfo.Carrier + "；货物品种：" +
                                this.CurrentZkBizResult.DispatchInfo.Catagory + "；发货单位:" + this.CurrentZkBizResult.DispatchInfo.Shipper
                                + "；矿发净重:" + this.CurrentZkBizResult.DispatchInfo.SendTare + "；调运计划卡：" + this.CurrentZkBizResult.DispatchInfo.TaskNo;
                            txtTicketWeight_BuyFuel.Value = CurrentZkBizResult.DispatchInfo.SendTare;

                            #region 调运计划卡

                            //View_dyjhk view_dyjhk = Dbers.GetInstance().SelfDber.Entity<View_dyjhk>("where Dybh=:Dybh", new { Dybh = this.CurrentZkBizResult.DispatchInfo.TaskNo });
                            //if (view_dyjhk == null)
                            //{
                            //    UpdateLedShow("调运计划卡读取失败，请稍等");
                            //    timer_BuyFuel.Interval = 8000;
                            //    this.CurrentFlowFlag = eFlowFlag.异常重置2;
                            //    break;
                            //}

                            //this.SelectedFuelKind_BuyFuel = Dbers.GetInstance().SelfDber.Entity<CmcsFuelKind>("where Code=:Code", new { Code = view_dyjhk.Mzbm });
                            //this.SelectedMine_BuyFuel = Dbers.GetInstance().SelfDber.Entity<CmcsMine>("where Code=:Code", new { Code = view_dyjhk.Kbbm });
                            //this.SelectedSupplier_BuyFuel = Dbers.GetInstance().SelfDber.Entity<CmcsSupplier>("where Code=:Code", new { Code = view_dyjhk.Gysbm });
                            //this.SelectedTransportCompany_BuyFuel = Dbers.GetInstance().SelfDber.Entity<CmcsTransportCompany>("where Code=:Code", new { Code = view_dyjhk.Gysbm });
                            //this.CurrentFlowFlag = eFlowFlag.自动保存;
                            //break;

                            #endregion

                            #region 汽车来煤预归批

                            this.CurrentQCLMYGP = Dbers.GetInstance().SelfDber.Entity<View_rlgl_cygl_qclmygp>("where Dybh=:Dybh and to_char(PlanTime,'YYYYMMDD')=to_char(:PlanTime,'YYYYMMDD')", new { Dybh = CurrentZkBizResult.DispatchInfo.TaskNo, PlanTime = DateTime.Now });
                            if (this.CurrentQCLMYGP == null)
                            {
                                UpdateLedShow("来煤预归批读取失败，请稍等");
                                timer_BuyFuel.Interval = 8000;
                                this.CurrentFlowFlag = eFlowFlag.异常重置2;
                                break;
                            }

                            this.SelectedFuelKind_BuyFuel = Dbers.GetInstance().SelfDber.Entity<CmcsFuelKind>("where PkId=:PkId", new { PkId = this.CurrentQCLMYGP.Mzid });
                            this.SelectedMine_BuyFuel = Dbers.GetInstance().SelfDber.Entity<CmcsMine>("where PkId=:PkId", new { PkId = this.CurrentQCLMYGP.Kbid });
                            this.SelectedSupplier_BuyFuel = Dbers.GetInstance().SelfDber.Entity<CmcsSupplier>("where PkId=:PkId", new { PkId = this.CurrentQCLMYGP.Gysid });
                            this.SelectedTransportCompany_BuyFuel = Dbers.GetInstance().SelfDber.Entity<CmcsTransportCompany>("where PkId=:PkId", new { PkId = this.CurrentQCLMYGP.Gysid });
                            this.CurrentFlowFlag = eFlowFlag.匹配采样;

                            #endregion

                        }
                        catch (Exception ex)
                        {
                            Log4Neter.Error("获取车辆任务数据", ex);

                            UpdateLedShow("智能调运读取失败，请稍等" + (this.CurrentZkBizResult == null ? "" : this.CurrentZkBizResult.Message));
                            timer_BuyFuel.Interval = 8000;
                            this.CurrentFlowFlag = eFlowFlag.异常重置2;
                            break;
                        }

                        #endregion
                        break;

                    case eFlowFlag.数据录入:
                        #region


                        #endregion
                        break;

                    case eFlowFlag.匹配采样:
                        #region

                        try
                        {
                            string message = string.Empty;
                            string sampler = string.Empty;

                            //判断厂内车数
                            if (!queuerDAO.GetIsFactory(this.FactoryCount, this.isFactoryCount, ref message))
                            {
                                UpdateLedShow(message);
                                timer_BuyFuel.Interval = 8000;
                                this.CurrentFlowFlag = eFlowFlag.异常重置2;
                                break;
                            }

                            //如果给供应商指定了采样机就不需要随机分配了
                            if (this.SelectedSupplier_BuyFuel != null)
                            {
                                CmcsSupplierAssignSampler cmcsSupplierAssignSampler = commonDAO.SelfDber.Entity<CmcsSupplierAssignSampler>("where SupplierId=:SupplierId", new { SupplierId = this.SelectedSupplier_BuyFuel.Id });
                                if (cmcsSupplierAssignSampler != null)
                                {
                                    sampler = cmcsSupplierAssignSampler.Sampler;
                                }
                                else
                                {
                                    //判断待采样车数
                                    if (!queuerDAO.GetSamplerMachineCode(this.CurrentQCLMYGP.Cyjbm, this.SampleWayCount, this.isSampleWayCount, ref sampler, ref message))
                                    {
                                        UpdateLedShow(message);
                                        timer_BuyFuel.Interval = 8000;
                                        this.CurrentFlowFlag = eFlowFlag.异常重置2;
                                        break;
                                    }
                                }
                            }

                            this.CurrentSampler = sampler;
                            this.CurrentFlowFlag = eFlowFlag.自动保存;
                        }
                        catch (Exception ex)
                        {
                            Log4Neter.Error("匹配采样机编号", ex);
                        }

                        #endregion
                        break;

                    case eFlowFlag.自动保存:
                        #region

                        //降低灵敏度
                        timer_BuyFuel.Interval = 4000;

                        SaveBuyFuelTransport();

                        #endregion
                        break;

                    case eFlowFlag.等待离开:
                        #region

                        // 当前道路地感无信号时重置
                        if (!HasCarOnCurrentWay()) ResetBuyFuel();

                        // 降低灵敏度
                        timer_BuyFuel.Interval = 4000;

                        #endregion
                        break;

                    case eFlowFlag.异常重置2:

                        ResetBuyFuel();

                        break;
                }
            }
            catch (Exception ex)
            {
                Log4Neter.Error("timer_BuyFuel_Tick", ex);
            }
            finally
            {
                timer_BuyFuel.Start();
            }
        }

        /// <summary>
        /// 获取未完成的入厂煤记录
        /// </summary>
        void LoadTodayUnFinishBuyFuelTransport()
        {
            superGridControl1_BuyFuel.PrimaryGrid.DataSource = queuerDAO.GetUnFinishBuyFuelTransport();
        }

        /// <summary>
        /// 获取指定日期已完成的入厂煤记录
        /// </summary>
        void LoadTodayFinishBuyFuelTransport()
        {
            superGridControl2_BuyFuel.PrimaryGrid.DataSource = queuerDAO.GetFinishedBuyFuelTransport(DateTime.Now.Date, DateTime.Now.Date.AddDays(1));
        }

        /// <summary>
        /// 提取预报信息
        /// </summary>
        /// <param name="lMYB">来煤预报</param>
        void BorrowForecast_BuyFuel(CmcsLMYB lMYB)
        {
            if (lMYB == null) return;

            this.SelectedFuelKind_BuyFuel = commonDAO.SelfDber.Get<CmcsFuelKind>(lMYB.FuelKindId);
            this.SelectedMine_BuyFuel = commonDAO.SelfDber.Get<CmcsMine>(lMYB.MineId);
            this.SelectedSupplier_BuyFuel = commonDAO.SelfDber.Get<CmcsSupplier>(lMYB.SupplierId);
            this.SelectedTransportCompany_BuyFuel = commonDAO.SelfDber.Get<CmcsTransportCompany>(lMYB.TransportCompanyId);
        }

        /// <summary>
        /// 双击行时，自动填充供煤单位、矿点等信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void superGridControl_BuyFuel_CellDoubleClick(object sender, DevComponents.DotNetBar.SuperGrid.GridCellDoubleClickEventArgs e)
        {
            GridRow gridRow = (sender as SuperGridControl).PrimaryGrid.ActiveRow as GridRow;
            if (gridRow == null) return;

            View_BuyFuelTransport entity = (gridRow.DataItem as View_BuyFuelTransport);
            if (entity != null)
            {
                this.SelectedFuelKind_BuyFuel = commonDAO.SelfDber.Get<CmcsFuelKind>(entity.FuelKindId);
                this.SelectedMine_BuyFuel = commonDAO.SelfDber.Get<CmcsMine>(entity.MineId);
                this.SelectedSupplier_BuyFuel = commonDAO.SelfDber.Get<CmcsSupplier>(entity.SupplierId);
                this.SelectedTransportCompany_BuyFuel = commonDAO.SelfDber.Get<CmcsTransportCompany>(entity.TransportCompanyId);

            }
        }

        private void superGridControl1_BuyFuel_CellClick(object sender, GridCellClickEventArgs e)
        {
            View_BuyFuelTransport entity = e.GridCell.GridRow.DataItem as View_BuyFuelTransport;
            if (entity == null) return;

            // 更改有效状态
            if (e.GridCell.GridColumn.Name == "ChangeIsUse") queuerDAO.ChangeBuyFuelTransportToInvalid(entity.Id, Convert.ToBoolean(e.GridCell.Value));
        }

        private void superGridControl1_BuyFuel_DataBindingComplete(object sender, GridDataBindingCompleteEventArgs e)
        {
            foreach (GridRow gridRow in e.GridPanel.Rows)
            {
                View_BuyFuelTransport entity = gridRow.DataItem as View_BuyFuelTransport;
                if (entity == null) return;

                // 填充有效状态
                gridRow.Cells["ChangeIsUse"].Value = Convert.ToBoolean(entity.IsUse);
            }
        }

        private void superGridControl2_BuyFuel_CellClick(object sender, GridCellClickEventArgs e)
        {
            View_BuyFuelTransport entity = e.GridCell.GridRow.DataItem as View_BuyFuelTransport;
            if (entity == null) return;

            // 更改有效状态
            if (e.GridCell.GridColumn.Name == "ChangeIsUse") queuerDAO.ChangeBuyFuelTransportToInvalid(entity.Id, Convert.ToBoolean(e.GridCell.Value));
        }

        private void superGridControl2_BuyFuel_DataBindingComplete(object sender, GridDataBindingCompleteEventArgs e)
        {
            foreach (GridRow gridRow in e.GridPanel.Rows)
            {
                View_BuyFuelTransport entity = gridRow.DataItem as View_BuyFuelTransport;
                if (entity == null) return;

                // 填充有效状态
                gridRow.Cells["ChangeIsUse"].Value = Convert.ToBoolean(entity.IsUse);
            }
        }

        #endregion

        #region 销售煤业务

        bool timer_SaleFuel_Cancel = true;

        List<String> StorageNames = new List<string>();

        private CmcsTransportSales selectedCmcsTransportSales;
        /// <summary>
        /// 选择的销售煤订单
        /// </summary>
        public CmcsTransportSales SelectedCmcsTransportSales
        {
            get { return selectedCmcsTransportSales; }
            set
            {
                selectedCmcsTransportSales = value;

                if (value != null)
                {
                    txtSupplierName_SaleFuel.Text = value.Consignee;
                    txtTransportCompanyName_SaleFuel.Text = value.TransportCompayName;
                    StorageNames = Dbers.GetInstance().SelfDber.Entities<CmcsTransportSalesDetail>(" where LMYBId=:LMYBId ", new { LMYBId = value.Id }).Select(a => a.StorageName).ToList();
                    if (StorageNames.Count > 0) { cmbCoalProduct.SelectedItem = StorageNames[0]; }
                }
                else
                {
                    txtSupplierName_SaleFuel.ResetText();
                    txtTransportCompanyName_SaleFuel.ResetText();
                }
            }
        }

        private CmcsSupplier selectedSupplier_SaleFuel;
        /// <summary>
        /// 选择的供煤单位
        /// </summary>
        public CmcsSupplier SelectedSupplier_SaleFuel
        {
            get { return selectedSupplier_SaleFuel; }
            set
            {
                selectedSupplier_SaleFuel = value;

                if (value != null)
                {
                    txtSupplierName_SaleFuel.Text = value.Name;
                }
                else
                {
                    txtSupplierName_SaleFuel.ResetText();
                }
            }
        }

        private CmcsTransportCompany selectedTransportCompany_SaleFuel;
        /// <summary>
        /// 选择的运输单位
        /// </summary>
        public CmcsTransportCompany SelectedTransportCompany_SaleFuel
        {
            get { return selectedTransportCompany_SaleFuel; }
            set
            {
                selectedTransportCompany_SaleFuel = value;

                if (value != null)
                {
                    txtTransportCompanyName_SaleFuel.Text = value.Name;
                }
                else
                {
                    txtTransportCompanyName_SaleFuel.ResetText();
                }
            }
        }

        private CmcsFuelKind selectedFuelKind_SaleFuel;
        /// <summary>
        /// 选择的煤种
        /// </summary>
        public CmcsFuelKind SelectedFuelKind_SaleFuel
        {
            get { return selectedFuelKind_SaleFuel; }
            set
            {
                if (value != null)
                {
                    selectedFuelKind_SaleFuel = value;
                    cmbFuelName_SaleFuel.Text = value.Name;
                }
                else
                {
                    cmbFuelName_SaleFuel.SelectedIndex = 0;
                }
            }
        }

        private void btnSaveTransport_SaleFuel_Click(object sender, EventArgs e)
        {
            SaveSaleFuelTransport();
        }

        void LoadTodayUnFinishSaleFuelTransport()
        {
            superGridControl1_SaleFuel.PrimaryGrid.DataSource = queuerDAO.GetUnFinishSaleFuelTransport();
        }

        void LoadTodayFinishSaleFuelTransport()
        {
            superGridControl2_SaleFuel.PrimaryGrid.DataSource = queuerDAO.GetFinishedSaleFuelTransport(DateTime.Now.Date, DateTime.Now.Date.AddDays(1));
        }

        bool SaveSaleFuelTransport()
        {
            if (this.CurrentAutotruck == null)
            {
                MessageBoxEx.Show("请选择车辆", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (this.SelectedFuelKind_SaleFuel == null)
            {
                MessageBoxEx.Show("请选择煤种", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (this.SelectedSupplier_SaleFuel == null)
            {
                MessageBoxEx.Show("请选择供煤单位", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (this.SelectedTransportCompany_SaleFuel == null)
            {
                MessageBoxEx.Show("请选择运输单位", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            //if (this.SelectedCmcsTransportSales == null)
            //{
            //    MessageBoxEx.Show("请选择销售煤订单", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return false;
            //}
            //if (StorageNames.IndexOf(cmbCoalProduct.SelectedItem.ToString()) < 0 && MessageBoxEx.Show(cmbCoalProduct.SelectedItem.ToString() + "成品仓不再销售订单内，确定选择？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            //{
            //    return false;
            //}
            try
            {
                // 生成销售煤排队记录
                if (queuerDAO.JoinQueueSaleFuelTransport(this.CurrentAutotruck, this.SelectedSupplier_SaleFuel, this.SelectedTransportCompany_SaleFuel, this.SelectedFuelKind_SaleFuel, DateTime.Now, txt_ReMark1.Text, CommonAppConfig.GetInstance().AppIdentifier, cmbCoalProduct.SelectedItem.ToString()))
                {
                    btnSaveTransport_BuyFuel.Enabled = false;
                    this.CurrentFlowFlag = eFlowFlag.等待离开;

                    UpdateLedShow("排队成功", "请离开");
                    MessageBoxEx.Show("排队成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadTodayUnFinishSaleFuelTransport();
                    LoadTodayFinishSaleFuelTransport();

                    LetPass();

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("保存失败\r\n" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);

                Log4Neter.Error("保存运输记录", ex);
            }

            return false;
        }

        void ResetSaleFuel()
        {

            this.timer_SaleFuel_Cancel = true;

            this.CurrentFlowFlag = eFlowFlag.等待车辆;

            this.CurrentAutotruck = null;
            this.SelectedCmcsTransportSales = null;

            txtTagId_SaleFuel.ResetText();
            txtRemark_BuyFuel.ResetText();

            btnSaveTransport_BuyFuel.Enabled = true;

            UpdateLedShow("  等待车辆");

            // 最后重置
            this.CurrentImperfectCar = null;
        }

        private void btnSelectForecast_SaleFuel_Click(object sender, EventArgs e)
        {
            FrmSaleFuelForecast_Select frm = new FrmSaleFuelForecast_Select();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                SelectedCmcsTransportSales = frm.Output;
            }
        }

        private void btnSelectAutotruck_SaleFuel_Click(object sender, EventArgs e)
        {
            FrmAutotruck_Select frm = new FrmAutotruck_Select("and CarType='" + eCarType.销售煤.ToString() + "' and IsUse=1 order by CarNumber asc");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                passCarQueuer.Enqueue(ePassWay.UnKnow, frm.Output.CarNumber, false);
                this.CurrentFlowFlag = eFlowFlag.验证车辆;
            }
        }

        /// <summary>
        /// 选择供煤单位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectSupplier_SaleFuel_Click(object sender, EventArgs e)
        {
            FrmSupplier_Select frm = new FrmSupplier_Select("where IsStop=0 order by Name asc");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                this.SelectedSupplier_SaleFuel = frm.Output;
            }
        }

        /// <summary>
        /// 选择的运输单位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectTransportCompany_SaleFuel_Click(object sender, EventArgs e)
        {
            FrmTransportCompany_Select frm = new FrmTransportCompany_Select("where IsStop=0 order by Name asc");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                this.SelectedTransportCompany_SaleFuel = frm.Output;
            }
        }

        /// <summary>
        /// 选择煤种
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbFuelName_SaleFuel_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SelectedFuelKind_SaleFuel = cmbFuelName_SaleFuel.SelectedItem as CmcsFuelKind;
        }

        private void timer_SaleFuel_Tick(object sender, EventArgs e)
        {
            if (this.timer_SaleFuel_Cancel) return;

            timer_SaleFuel.Stop();
            timer_SaleFuel.Interval = 2000;

            try
            {
                switch (this.CurrentFlowFlag)
                {
                    case eFlowFlag.匹配调运:
                        #region

                        //List<CmcsLMYB> lMYBs = queuerDAO.GetBuyFuelForecastByCarNumber(this.CurrentAutotruck.CarNumber, DateTime.Now);
                        //if (lMYBs.Count > 1)
                        //{
                        //    // 当来煤预报存在多条时，弹出选择确认框
                        //    FrmBuyFuelForecast_Confirm frm = new FrmBuyFuelForecast_Confirm(lMYBs);
                        //    if (frm.ShowDialog() == DialogResult.OK) BorrowForecast_BuyFuel(frm.Output);
                        //}
                        //else if (lMYBs.Count == 1)
                        //{
                        //    BorrowForecast_BuyFuel(lMYBs[0]);
                        //}

                        this.CurrentFlowFlag = eFlowFlag.数据录入;

                        #endregion
                        break;

                    case eFlowFlag.数据录入:
                        #region



                        #endregion
                        break;

                    case eFlowFlag.等待离开:
                        #region

                        // 当前道路地感无信号时重置
                        if (!HasCarOnCurrentWay()) ResetSaleFuel();

                        // 降低灵敏度
                        timer_SaleFuel.Interval = 4000;

                        #endregion
                        break;
                }

                // 当前道路地感无信号时重置
                if (!HasCarOnCurrentWay() && this.CurrentFlowFlag != eFlowFlag.等待车辆 && (this.CurrentImperfectCar != null && this.CurrentImperfectCar.IsFromDevice)) ResetSaleFuel();
            }
            catch (Exception ex)
            {
                Log4Neter.Error("timer_SaleFuel_Tick", ex);
            }
            finally
            {
                timer_SaleFuel.Start();
            }
        }

        private void btnReset_SaleFuel_Click(object sender, EventArgs e)
        {
            ResetSaleFuel();
        }

        private void btnNewAutotruck_SaleFuel_Click(object sender, EventArgs e)
        {
            new FrmAutotruck_Oper(Guid.NewGuid().ToString(), eEditMode.新增).Show();
        }

        #endregion

        #region 其他物资业务

        bool timer_Goods_Cancel = true;

        private CmcsSupplier selectedSupplyUnit_Goods;
        /// <summary>
        /// 选择的供货单位
        /// </summary>
        public CmcsSupplier SelectedSupplyUnit_Goods
        {
            get { return selectedSupplyUnit_Goods; }
            set
            {
                selectedSupplyUnit_Goods = value;

                if (value != null)
                {
                    txtSupplyUnitName_Goods.Text = value.Name;
                }
                else
                {
                    txtSupplyUnitName_Goods.ResetText();
                }
            }
        }

        private CmcsSupplier selectedReceiveUnit_Goods;
        /// <summary>
        /// 选择的收货单位
        /// </summary>
        public CmcsSupplier SelectedReceiveUnit_Goods
        {
            get { return selectedReceiveUnit_Goods; }
            set
            {
                selectedReceiveUnit_Goods = value;

                if (value != null)
                {
                    txtReceiveUnitName_Goods.Text = value.Name;
                }
                else
                {
                    txtReceiveUnitName_Goods.ResetText();
                }
            }
        }

        private CmcsGoodsType selectedGoodsType_Goods;
        /// <summary>
        /// 选择的物资类型
        /// </summary>
        public CmcsGoodsType SelectedGoodsType_Goods
        {
            get { return selectedGoodsType_Goods; }
            set
            {
                selectedGoodsType_Goods = value;

                if (value != null)
                {
                    txtGoodsTypeName_Goods.Text = value.GoodsName;
                }
                else
                {
                    txtGoodsTypeName_Goods.ResetText();
                }
            }
        }

        /// <summary>
        /// 选择车辆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectAutotruck_Goods_Click(object sender, EventArgs e)
        {
            FrmAutotruck_Select frm = new FrmAutotruck_Select("and CarType='" + eCarType.其他物资.ToString() + "' and IsUse=1 order by CarNumber asc");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                passCarQueuer.Enqueue(ePassWay.UnKnow, frm.Output.CarNumber, false);
                this.CurrentFlowFlag = eFlowFlag.验证车辆;
            }
        }

        /// <summary>
        /// 选择供货单位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnbtnSelectSupply_Goods_Click(object sender, EventArgs e)
        {
            FrmSupplier_Select frm = new FrmSupplier_Select("where IsStop=0 order by Name asc");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                this.SelectedSupplyUnit_Goods = frm.Output;
            }
        }

        /// <summary>
        /// 选择收货单位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectReceive_Goods_Click(object sender, EventArgs e)
        {
            FrmSupplier_Select frm = new FrmSupplier_Select("where IsStop=0 order by Name asc");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                this.SelectedReceiveUnit_Goods = frm.Output;
            }
        }

        /// <summary>
        /// 选择物资类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectGoodsType_Goods_Click(object sender, EventArgs e)
        {
            FrmGoodsType_Select frm = new FrmGoodsType_Select();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                this.SelectedGoodsType_Goods = frm.Output;
            }
        }

        /// <summary>
        /// 新车登记
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewAutotruck_Goods_Click(object sender, EventArgs e)
        {
            // eCarType.其他物资 

            new FrmAutotruck_Oper(Guid.NewGuid().ToString(), eEditMode.新增).Show();
        }

        /// <summary>
        /// 保存排队记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveTransport_Goods_Click(object sender, EventArgs e)
        {
            SaveGoodsTransport();
        }

        /// <summary>
        /// 保存运输记录
        /// </summary>
        /// <returns></returns>
        bool SaveGoodsTransport()
        {
            if (this.CurrentAutotruck == null)
            {
                MessageBoxEx.Show("请选择车辆", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(this.CurrentAutotruck.EPCCardId))
            {
                MessageBoxEx.Show("车辆标签卡不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (this.SelectedSupplyUnit_Goods == null)
            {
                MessageBoxEx.Show("请选择供货单位", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (this.SelectedReceiveUnit_Goods == null)
            {
                MessageBoxEx.Show("请选择收货单位", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (this.SelectedGoodsType_Goods == null)
            {
                MessageBoxEx.Show("请选择物资类型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            try
            {
                // 生成排队记录
                if (queuerDAO.JoinQueueGoodsTransport(this.CurrentAutotruck, this.SelectedSupplyUnit_Goods, this.SelectedReceiveUnit_Goods, this.SelectedGoodsType_Goods, DateTime.Now, txtRemark_Goods.Text, CommonAppConfig.GetInstance().AppIdentifier))
                {
                    btnSaveTransport_Goods.Enabled = false;

                    if (this.AutoHandMode)
                        UpdateLedShow(this.CurrentAutotruck.CarNumber, "请进厂");
                    else
                        MessageBoxEx.Show(this.CurrentAutotruck.CarNumber + "排队成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.CurrentFlowFlag = eFlowFlag.等待离开;

                    LoadTodayUnFinishGoodsTransport();
                    LoadTodayFinishGoodsTransport();

                    LetPass();

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("保存失败\r\n" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);

                Log4Neter.Error("保存运输记录", ex);
            }

            return false;
        }

        /// <summary>
        /// 重置信息
        /// </summary>
        void ResetGoods()
        {
            this.timer_Goods_Cancel = true;

            this.CurrentFlowFlag = eFlowFlag.等待车辆;

            this.CurrentAutotruck = null;
            this.SelectedSupplyUnit_Goods = null;
            this.SelectedReceiveUnit_Goods = null;
            this.SelectedGoodsType_Goods = null;

            txtTagId_Goods.ResetText();
            txtRemark_Goods.ResetText();

            btnSaveTransport_Goods.Enabled = true;

            UpdateLedShow("  等待车辆");

            // 最后重置
            this.CurrentImperfectCar = null;
        }

        /// <summary>
        /// 重置信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Goods_Click(object sender, EventArgs e)
        {
            ResetGoods();
        }

        /// <summary>
        /// 其他物资运输记录业务定时器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Goods_Tick(object sender, EventArgs e)
        {
            if (this.timer_Goods_Cancel) return;

            timer_Goods.Stop();
            timer_Goods.Interval = 2000;

            try
            {
                switch (this.CurrentFlowFlag)
                {

                    case eFlowFlag.匹配调运:
                        #region

                        CmcsGoodsPlan cmcsGoodsPlan = Dbers.GetInstance().SelfDber.Entity<CmcsGoodsPlan>("where CarNumber=:CarNumber", new { CarNumber = this.CurrentAutotruck.CarNumber });
                        if (cmcsGoodsPlan == null)
                        {
                            UpdateLedShow(this.CurrentAutotruck.CarNumber + "未找到物资计划，请联系管理员");
                            timer_Goods.Interval = 8000;
                            this.CurrentFlowFlag = eFlowFlag.异常重置2;
                            break;
                        }

                        this.SelectedSupplyUnit_Goods = Dbers.GetInstance().SelfDber.Get<CmcsSupplier>(cmcsGoodsPlan.SupplyUnitId);
                        this.SelectedReceiveUnit_Goods = Dbers.GetInstance().SelfDber.Get<CmcsSupplier>(cmcsGoodsPlan.ReceiveUnitId);
                        this.SelectedGoodsType_Goods = Dbers.GetInstance().SelfDber.Get<CmcsGoodsType>(cmcsGoodsPlan.GoodsTypeId);
                        this.CurrentFlowFlag = eFlowFlag.自动保存;

                        #endregion
                        break;

                    case eFlowFlag.数据录入:
                        #region



                        #endregion
                        break;

                    case eFlowFlag.自动保存:
                        #region

                        //降低灵敏度
                        timer_Goods.Interval = 4000;

                        SaveGoodsTransport();

                        #endregion
                        break;

                    case eFlowFlag.等待离开:
                        #region

                        // 当前道路地感无信号时重置
                        if (!HasCarOnCurrentWay()) ResetGoods();

                        #endregion
                        break;

                    case eFlowFlag.异常重置2:

                        ResetGoods();

                        break;
                }
            }
            catch (Exception ex)
            {
                Log4Neter.Error("timer_Goods_Tick", ex);
            }
            finally
            {
                timer_Goods.Start();
            }
        }

        /// <summary>
        /// 获取未完成的其他物资记录
        /// </summary>
        void LoadTodayUnFinishGoodsTransport()
        {
            superGridControl1_Goods.PrimaryGrid.DataSource = queuerDAO.GetUnFinishGoodsTransport();
        }

        /// <summary>
        /// 获取指定日期已完成的其他物资记录
        /// </summary>
        void LoadTodayFinishGoodsTransport()
        {
            superGridControl2_Goods.PrimaryGrid.DataSource = queuerDAO.GetFinishedGoodsTransport(DateTime.Now.Date, DateTime.Now.Date.AddDays(1));
        }

        /// <summary>
        /// 双击行时，自动填充录入信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void superGridControl_Goods_CellDoubleClick(object sender, DevComponents.DotNetBar.SuperGrid.GridCellDoubleClickEventArgs e)
        {
            GridRow gridRow = (sender as SuperGridControl).PrimaryGrid.ActiveRow as GridRow;
            if (gridRow == null) return;

            CmcsGoodsTransport entity = (gridRow.DataItem as CmcsGoodsTransport);
            if (entity != null)
            {
                this.SelectedSupplyUnit_Goods = commonDAO.SelfDber.Get<CmcsSupplier>(entity.SupplyUnitId);
                this.SelectedReceiveUnit_Goods = commonDAO.SelfDber.Get<CmcsSupplier>(entity.ReceiveUnitId);
                this.SelectedGoodsType_Goods = commonDAO.SelfDber.Get<CmcsGoodsType>(entity.GoodsTypeId);
            }
        }

        private void superGridControl1_Goods_CellClick(object sender, GridCellClickEventArgs e)
        {
            CmcsGoodsTransport entity = e.GridCell.GridRow.DataItem as CmcsGoodsTransport;
            if (entity == null) return;

            // 更改有效状态
            if (e.GridCell.GridColumn.Name == "ChangeIsUse") queuerDAO.ChangeGoodsTransportToInvalid(entity.Id, Convert.ToBoolean(e.GridCell.Value));
        }

        private void superGridControl1_Goods_DataBindingComplete(object sender, GridDataBindingCompleteEventArgs e)
        {
            foreach (GridRow gridRow in e.GridPanel.Rows)
            {
                CmcsGoodsTransport entity = gridRow.DataItem as CmcsGoodsTransport;
                if (entity == null) return;

                // 填充有效状态
                gridRow.Cells["ChangeIsUse"].Value = Convert.ToBoolean(entity.IsUse);
            }
        }

        private void superGridControl2_Goods_CellClick(object sender, GridCellClickEventArgs e)
        {
            CmcsGoodsTransport entity = e.GridCell.GridRow.DataItem as CmcsGoodsTransport;
            if (entity == null) return;

            // 更改有效状态
            if (e.GridCell.GridColumn.Name == "ChangeIsUse") queuerDAO.ChangeGoodsTransportToInvalid(entity.Id, Convert.ToBoolean(e.GridCell.Value));
        }

        private void superGridControl2_Goods_DataBindingComplete(object sender, GridDataBindingCompleteEventArgs e)
        {
            foreach (GridRow gridRow in e.GridPanel.Rows)
            {
                CmcsGoodsTransport entity = gridRow.DataItem as CmcsGoodsTransport;
                if (entity == null) return;

                // 填充有效状态
                gridRow.Cells["ChangeIsUse"].Value = Convert.ToBoolean(entity.IsUse);
            }
        }

        #endregion

        #region 出厂业务

        /// <summary>
        /// 出厂业务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Out_Tick(object sender, EventArgs e)
        {
            timer_Out.Stop();
            timer_Out.Interval = 2000;

            try
            {
                switch (this.CurrentFlowFlagOut)
                {
                    case eFlowFlag.等待车辆:
                        #region

                        //提高灵敏度
                        timer_Out.Interval = 500;

                        //List<string> tags = Hardwarer.RwerOut.ScanTags();
                        List<string> tags = new List<string>() { "BBAAC4DCCEBCC4CF00000461" };
                        if (tags.Count > 0) passCarQueuerOut.Enqueue(ePassWay.Way1, tags[0], true);

                        if (passCarQueuerOut.Count > 0) this.CurrentFlowFlagOut = eFlowFlag.识别车辆;

                        #endregion
                        break;

                    case eFlowFlag.识别车辆:
                        #region

                        // 队列中无车时，等待车辆
                        if (passCarQueuerOut.Count == 0)
                        {
                            UpdateShowDebug("  等待车辆");
                            this.CurrentFlowFlagOut = eFlowFlag.等待车辆;
                            break;
                        }

                        this.CurrentImperfectCarOut = passCarQueuerOut.Dequeue();

                        // 方式一：根据识别的车牌号查找车辆信息
                        this.CurrentAutotruckOut = carTransportDAO.GetAutotruckByCarNumber(this.CurrentImperfectCarOut.Voucher);

                        if (this.CurrentAutotruckOut == null)
                            // 方式二：根据识别的标签卡查找车辆信息
                            this.CurrentAutotruckOut = carTransportDAO.GetAutotruckByTagId(this.CurrentImperfectCarOut.Voucher);

                        if (this.CurrentAutotruckOut != null)
                        {
                            if (this.CurrentAutotruckOut.IsUse == 1)
                            {
                                if (this.CurrentAutotruckOut.CarType == eCarType.入厂煤.ToString())
                                    this.CurrentFlowFlagOut = eFlowFlag.入厂煤验证保存;

                                else if (this.CurrentAutotruckOut.CarType == eCarType.其他物资.ToString())
                                    this.CurrentFlowFlagOut = eFlowFlag.其他物资验证保存;
                            }
                            else
                            {
                                UpdateShowDebug(this.CurrentAutotruckOut.CarNumber, "已停用");

                                timer_Out.Interval = 8000;
                            }
                        }
                        else
                        {
                            UpdateShowDebug(this.CurrentImperfectCarOut.Voucher, "未登记");

                            timer_Out.Interval = 8000;
                        }

                        #endregion
                        break;

                    case eFlowFlag.入厂煤验证保存:
                        #region

                        this.CurrentBuyFuelTransportOut = commonDAO.SelfDber.Entity<CmcsBuyFuelTransport>("where AutotruckId=:AutotruckId order by InFactoryTime desc", new { AutotruckId = this.CurrentAutotruckOut.Id });
                        if (this.CurrentBuyFuelTransportOut != null)
                        {
                            if (this.CurrentBuyFuelTransportOut.StepName != eTruckInFactoryStep.出厂.ToString())
                            {
                                // 判断路线设置
                                string nextPlace;
                                if (carTransportDAO.CheckNextTruckInFactoryWay(this.CurrentAutotruckOut.CarType, this.CurrentBuyFuelTransportOut.StepName, eTruckInFactoryStep.出厂.ToString(), CommonAppConfig.GetInstance().AppIdentifier, out nextPlace))
                                {
                                    if (this.CurrentBuyFuelTransportOut.SuttleWeight > 0)
                                    {
                                        // 自动模式
                                        if (!SaveBuyFuelTransportOut())
                                        {
                                            UpdateLedShow(this.CurrentAutotruckOut.CarNumber, "保存失败");

                                            timer_Out.Interval = 4000;
                                        }
                                    }
                                    else
                                    {
                                        UpdateShowDebug(this.CurrentAutotruckOut.CarNumber, "称重未完成");
                                        this.CurrentFlowFlagOut = eFlowFlag.异常重置2;

                                        timer_Out.Interval = 8000;
                                    }
                                }
                                else
                                {
                                    UpdateShowDebug("路线错误", "禁止通过");
                                    this.CurrentFlowFlagOut = eFlowFlag.异常重置2;

                                    timer_Out.Interval = 8000;
                                }
                            }
                            else
                            {
                                UpdateShowDebug(this.CurrentAutotruckOut.CarNumber, "请离开");
                                this.CurrentFlowFlagOut = eFlowFlag.等待离开;

                                timer_Out.Interval = 2000;
                            }
                        }
                        else
                        {
                            UpdateShowDebug(this.CurrentAutotruckOut.CarNumber, "未找到运输记录");
                            this.CurrentFlowFlagOut = eFlowFlag.异常重置2;

                            timer_Out.Interval = 8000;
                        }

                        #endregion
                        break;

                    case eFlowFlag.其他物资验证保存:
                        #region

                        this.CurrentGoodsTransportOut = commonDAO.SelfDber.Entity<CmcsGoodsTransport>("where AutotruckId=:AutotruckId order by InFactoryTime desc", new { AutotruckId = this.CurrentAutotruckOut.Id });
                        if (this.CurrentGoodsTransportOut != null)
                        {
                            if (this.CurrentGoodsTransportOut.StepName != eTruckInFactoryStep.出厂.ToString())
                            {
                                // 判断路线设置
                                string nextPlace;
                                if (carTransportDAO.CheckNextTruckInFactoryWay(this.CurrentAutotruckOut.CarType, this.CurrentGoodsTransportOut.StepName, "出厂", CommonAppConfig.GetInstance().AppIdentifier, out nextPlace))
                                {
                                    if (this.CurrentGoodsTransportOut.SuttleWeight > 0)
                                    {
                                        // 自动模式
                                        if (!SaveGoodsTransportOut())
                                        {
                                            UpdateShowDebug(this.CurrentAutotruckOut.CarNumber, "保存失败");

                                            timer_Out.Interval = 8000;
                                        }
                                    }
                                    else
                                    {
                                        UpdateShowDebug(this.CurrentAutotruckOut.CarNumber, "称重未完成");

                                        timer_Out.Interval = 8000;
                                    }
                                }
                                else
                                {
                                    UpdateShowDebug("路线错误", "禁止通过");

                                    timer_Out.Interval = 8000;
                                }
                            }
                        }
                        else
                        {
                            UpdateShowDebug(this.CurrentAutotruckOut.CarNumber, "未找到运输记录");
                            this.CurrentFlowFlagOut = eFlowFlag.异常重置2;

                            timer_Out.Interval = 8000;
                        }

                        #endregion
                        break;

                    case eFlowFlag.等待离开:
                        #region

                        // 当前道路地感无信号时重置
                        ResetOut();

                        // 降低灵敏度
                        timer_Out.Interval = 4000;

                        #endregion
                        break;

                    case eFlowFlag.异常重置2:

                        ResetOut();

                        break;
                }
            }
            catch (Exception ex)
            {
                Log4Neter.Error("timer1_Tick", ex);
            }
            finally
            {
                timer_Out.Start();
            }
        }

        /// <summary>
        /// 保存出厂运输记录
        /// </summary>
        /// <returns></returns>
        bool SaveBuyFuelTransportOut()
        {
            if (this.CurrentBuyFuelTransportOut == null) return false;

            try
            {
                if (outerDAO.SaveBuyFuelTransport(this.CurrentBuyFuelTransportOut.Id, DateTime.Now))
                {
                    // 打印磅单

                    this.CurrentFlowFlagOut = eFlowFlag.等待离开;

                    UpdateShowDebug(this.CurrentAutotruckOut.CarNumber + "出厂成功", "请离开");

                    LetPass();

                    return true;
                }
            }
            catch (Exception ex)
            {
                Log4Neter.Error("入厂煤出厂保存运输记录", ex);
            }

            return false;
        }

        /// <summary>
        /// 保存出厂运输记录
        /// </summary>
        /// <returns></returns>
        bool SaveGoodsTransportOut()
        {
            if (this.CurrentGoodsTransportOut == null) return false;

            try
            {
                if (outerDAO.SaveGoodsTransport(this.CurrentGoodsTransportOut.Id, DateTime.Now))
                {
                    this.CurrentGoodsTransportOut = commonDAO.SelfDber.Get<CmcsGoodsTransport>(this.CurrentGoodsTransportOut.Id);

                    this.CurrentFlowFlagOut = eFlowFlag.等待离开;

                    UpdateShowDebug(this.CurrentAutotruckOut.CarNumber + "出厂成功", "请离开");

                    LetPass();

                    return true;
                }
            }
            catch (Exception ex)
            {
                Log4Neter.Error("其他物资出厂保存运输记录", ex);
            }

            return false;
        }

        /// <summary>
        /// 重置信息
        /// </summary>
        void ResetOut()
        {
            this.CurrentFlowFlagOut = eFlowFlag.等待车辆;

            this.CurrentAutotruckOut = null;

            UpdateShowDebug("  等待车辆");

            // 最后重置
            this.CurrentImperfectCarOut = null;
        }

        #endregion

        #region 其他函数

        Pen redPen3 = new Pen(Color.Red, 3);
        Pen greenPen3 = new Pen(Color.Lime, 3);
        Pen greenPen1 = new Pen(Color.Lime, 1);

        /// <summary>
        /// 当前车号面板绘制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panCurrentCarNumber_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                PanelEx panel = sender as PanelEx;

                // 绘制地感1
                e.Graphics.DrawLine(this.InductorCoil1 ? redPen3 : greenPen3, 15, 10, 15, 30);
                // 绘制地感2                                                               
                e.Graphics.DrawLine(this.InductorCoil2 ? redPen3 : greenPen3, 25, 10, 25, 30);
                // 绘制分割线
                e.Graphics.DrawLine(greenPen1, 5, 34, 35, 34);
                // 绘制地感3
                e.Graphics.DrawLine(this.InductorCoil3 ? redPen3 : greenPen3, 15, 38, 15, 58);
                // 绘制地感4                                                               
                e.Graphics.DrawLine(this.InductorCoil4 ? redPen3 : greenPen3, 25, 38, 25, 58);
            }
            catch (Exception ex)
            {
                Log4Neter.Error("panCurrentCarNumber_Paint异常", ex);
            }
        }

        private void superGridControl_BeginEdit(object sender, DevComponents.DotNetBar.SuperGrid.GridEditEventArgs e)
        {
            if (e.GridCell.GridColumn.DataPropertyName != "IsUse")
            {
                // 取消进入编辑
                e.Cancel = true;
            }
        }

        /// <summary>
        /// 设置行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void superGridControl_GetRowHeaderText(object sender, DevComponents.DotNetBar.SuperGrid.GridGetRowHeaderTextEventArgs e)
        {
            e.Text = (e.GridRow.RowIndex + 1).ToString();
        }

        /// <summary>
        /// Invoke封装
        /// </summary>
        /// <param name="action"></param>
        public void InvokeEx(Action action)
        {
            if (this.IsDisposed || !this.IsHandleCreated) return;

            this.Invoke(action);
        }

        #endregion

    }
}
