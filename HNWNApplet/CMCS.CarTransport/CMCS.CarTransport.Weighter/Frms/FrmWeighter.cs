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
using System.Windows.Forms;
using CMCS.CarTransport.DAO;
using CMCS.CarTransport.Weighter.Core;
using CMCS.CarTransport.Weighter.Enums;
using CMCS.CarTransport.Weighter.Frms.Sys;
using CMCS.Common;
using CMCS.Common.DAO;
using CMCS.Common.Entities;
using CMCS.Common.Entities.BaseInfo;
using CMCS.Common.Entities.CarTransport;
using CMCS.Common.Entities.Sys;
using CMCS.Common.Enums;
using CMCS.Common.Utilities;
using CMCS.Common.Views;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.SuperGrid;
using HikVisionSDK.Core;
using LED.YB14;
using CMCS.Common.WS;
using LED.Dynamic.YB19;
using CMCS.CarTransport.Queue;
using System.Threading.Tasks;
using HikISCApi.Core;

namespace CMCS.CarTransport.Weighter.Frms
{
    public partial class FrmWeighter : DevComponents.DotNetBar.Metro.MetroForm
    {
        /// <summary>
        /// 窗体唯一标识符
        /// </summary>
        public static string UniqueKey = "FrmWeighter";

        public FrmWeighter()
        {
            InitializeComponent();
        }

        #region Vars

        CarTransportDAO carTransportDAO = CarTransportDAO.GetInstance();
        WeighterDAO weighterDAO = WeighterDAO.GetInstance();
        CommonDAO commonDAO = CommonDAO.GetInstance();
        CommService commService;

        /// <summary>
        /// 等待上传的抓拍
        /// </summary>
        Queue<string> waitForUpload = new Queue<string>();

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

                panCurrentWeight.Refresh();

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

                panCurrentWeight.Refresh();

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

                panCurrentWeight.Refresh();

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

                panCurrentWeight.Refresh();

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

        bool infraredSensor1 = false;
        /// <summary>
        /// 对射1状态 true=遮挡  false=连通
        /// </summary>
        public bool InfraredSensor1
        {
            get
            {
                return infraredSensor1;
            }
            set
            {
                infraredSensor1 = value;

                panCurrentWeight.Refresh();

                commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.对射1信号.ToString(), value ? "1" : "0");
            }
        }

        int infraredSensor1Port;
        /// <summary>
        /// 对射1端口
        /// </summary>
        public int InfraredSensor1Port
        {
            get { return infraredSensor1Port; }
            set { infraredSensor1Port = value; }
        }

        bool infraredSensor2 = false;
        /// <summary>
        /// 对射2状态 true=遮挡  false=连通
        /// </summary>
        public bool InfraredSensor2
        {
            get
            {
                return infraredSensor2;
            }
            set
            {
                infraredSensor2 = value;

                panCurrentWeight.Refresh();

                commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.对射2信号.ToString(), value ? "1" : "0");
            }
        }

        int infraredSensor2Port;
        /// <summary>
        /// 对射2端口
        /// </summary>
        public int InfraredSensor2Port
        {
            get { return infraredSensor2Port; }
            set { infraredSensor2Port = value; }
        }

        bool wbSteady = false;
        /// <summary>
        /// 地磅仪表稳定状态
        /// </summary>
        public bool WbSteady
        {
            get { return wbSteady; }
            set
            {
                wbSteady = value;

                this.panCurrentWeight.Style.ForeColor.Color = (value ? Color.Lime : Color.Red);

                panCurrentWeight.Refresh();

                commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.地磅仪表_稳定.ToString(), value ? "1" : "0");
            }
        }

        double wbMinWeight = 0;
        /// <summary>
        /// 地磅仪表最小称重 单位：吨
        /// </summary>
        public double WbMinWeight
        {
            get { return wbMinWeight; }
            set
            {
                wbMinWeight = value;
            }
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

                btnSelectAutotruck_BuyFuel.Visible = !value;
                btnSelectAutotruck_SaleFuel.Visible = !value;
                btnSelectAutotruck_Goods.Visible = !value;

                btnSaveTransport_BuyFuel.Visible = !value;
                btnSaveTransport_SaleFuel.Visible = !value;
                btnSaveTransport_Goods.Visible = !value;

                btnReset_BuyFuel.Visible = !value;
                btnReset_SaleFuel.Visible = !value;
                btnReset_Goods.Visible = !value;
            }
        }

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

        eDirection currentDirection;
        /// <summary>
        /// 当前上磅方向
        /// </summary>
        public eDirection CurrentDirection
        {
            get { return currentDirection; }
            set { currentDirection = value; }
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

                        superTabControl2.SelectedTab = superTabItem_BuyFuel;
                    }
                    else if (value.CarType == eCarType.销售煤.ToString())
                    {
                        if (ePCCard != null) txtTagId_SaleFuel.Text = ePCCard.TagId;

                        txtCarNumber_SaleFuel.Text = value.CarNumber;
                        superTabControl2.SelectedTab = superTabItem_SaleFuel;
                    }
                    else if (value.CarType == eCarType.其他物资.ToString())
                    {
                        if (ePCCard != null) txtTagId_Goods.Text = ePCCard.TagId;

                        txtCarNumber_Goods.Text = value.CarNumber;
                        superTabControl2.SelectedTab = superTabItem_Goods;
                    }

                    panCurrentCarNumber.Text = value.CarNumber;
                }
                else
                {
                    commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.当前车Id.ToString(), string.Empty);
                    commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.当前车号.ToString(), string.Empty);

                    txtCarNumber_BuyFuel.ResetText();
                    //txtCarNumber_SaleFuel.ResetText();
                    txtCarNumber_Goods.ResetText();

                    txtTagId_BuyFuel.ResetText();
                    //txtTagId_SaleFuel.ResetText();
                    txtTagId_Goods.ResetText();

                    panCurrentCarNumber.ResetText();
                }
            }
        }

        private bool autoPrint = false;
        /// <summary>
        /// 自动打印磅单
        /// </summary>
        public bool AutoPrint
        {
            get { return autoPrint; }
            set
            {
                autoPrint = value;

                chkAutoPrint.Checked = value;
            }
        }

        /// <summary>
        /// 是否启用智能调运
        /// </summary>
        bool IsITMS = false;

        /// <summary>
        /// 是否启用净重超差
        /// </summary>
        bool IsTicketDiff = false;

        /// <summary>
        /// 净重超差限值
        /// </summary>
        decimal TicketDiff = 0;

        /// <summary>
        /// 是否皮重排查系数
        /// </summary>
        bool IsTareCoefficient = false;

        /// <summary>
        /// 皮重排查系数
        /// </summary>
        decimal TareCoefficient = 0;

        /// <summary>
        /// 磅编号
        /// </summary>
        int PoundIndex = 1;

        #endregion

        /// <summary>
        /// 窗体初始化
        /// </summary>
        private void InitForm()
        {
            FrmDebugConsole.GetInstance();

            commService = new CommService(commonDAO.GetCommonAppletConfigString("中控接口连接地址"));

            // 默认自动
            sbtnChangeAutoHandMode.Value = true;

            // 重置程序远程控制命令
            commonDAO.ResetAppRemoteControlCmd(CommonAppConfig.GetInstance().AppIdentifier);

            //上传图片
            Thread threadUploadPic = new Thread(new ThreadStart(UploadCapturePicture));
            threadUploadPic.IsBackground = true;
            threadUploadPic.Start();

            btnRefresh_Click(null, null);
        }

        private void FrmWeighter_Load(object sender, EventArgs e)
        {
        }

        private void FrmWeighter_Shown(object sender, EventArgs e)
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
            // 接收设备状态 
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
                this.InfraredSensor1 = (receiveValue[this.InfraredSensor1Port - 1] == 1);
                this.InfraredSensor2 = (receiveValue[this.InfraredSensor2Port - 1] == 1);
            });
        }

        /// <summary>
        /// 前方升杆
        /// </summary>
        void FrontGateUp()
        {
            if (this.CurrentImperfectCar == null) return;

            if (this.CurrentImperfectCar.PassWay == eDirection.Way1)
            {
                this.iocControler.Gate2Up();
                this.iocControler.GreenLight2();
            }
            else if (this.CurrentImperfectCar.PassWay == eDirection.Way2)
            {
                this.iocControler.Gate1Up();
                this.iocControler.GreenLight1();
            }
        }

        /// <summary>
        /// 前方降杆
        /// </summary>
        void FrontGateDown()
        {
            if (this.CurrentImperfectCar == null) return;

            if (this.CurrentImperfectCar.PassWay == eDirection.Way1)
            {
                this.iocControler.Gate2Down();
                this.iocControler.RedLight2();
            }
            else if (this.CurrentImperfectCar.PassWay == eDirection.Way2)
            {
                this.iocControler.Gate1Down();
                this.iocControler.RedLight1();
            }
        }

        /// <summary>
        /// 后方升杆
        /// </summary>
        void BackGateUp()
        {
            if (this.CurrentImperfectCar == null) return;

            if (this.CurrentImperfectCar.PassWay == eDirection.Way1)
            {
                this.iocControler.Gate1Up();
                this.iocControler.GreenLight1();
            }
            else if (this.CurrentImperfectCar.PassWay == eDirection.Way2)
            {
                this.iocControler.Gate2Up();
                this.iocControler.GreenLight2();
            }
        }

        /// <summary>
        /// 后方降杆
        /// </summary>
        void BackGateDown()
        {
            if (this.CurrentImperfectCar == null) return;

            if (this.CurrentImperfectCar.PassWay == eDirection.Way1)
            {
                this.iocControler.Gate1Down();
                this.iocControler.RedLight1();
            }
            else if (this.CurrentImperfectCar.PassWay == eDirection.Way2)
            {
                this.iocControler.Gate2Down();
                this.iocControler.RedLight2();
            }
        }

        #endregion

        #region 读卡器

        void Rwer1_OnScanError(Exception ex)
        {
            Log4Neter.Error("读卡器1", ex);
        }

        void Rwer1_OnStatusChange(bool status)
        {
            // 接收设备状态 
            InvokeEx(() =>
            {
                slightRwer1.LightColor = (status ? Color.Green : Color.Red);

                commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.读卡器1_连接状态.ToString(), status ? "1" : "0");
            });
        }

        void Rwer2_OnScanError(Exception ex)
        {
            Log4Neter.Error("读卡器2", ex);
        }

        void Rwer2_OnStatusChange(bool status)
        {
            // 接收设备状态 
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
            FrmDebugConsole.GetInstance().Output("更新LED:|" + value1 + "|" + value2 + "|");

            UpdateLed1Show(value1, value2);
            UpdateLed2Show(value1, value2);
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
            if (!this.LED1ConnectStatus) return;
            if (this.LED1PrevLedFileContent == value1 + value2) return;

            string ledContent = GenerateFillLedContent12(value1) + GenerateFillLedContent12(value2);

            File.WriteAllText(this.LED1TempFile, ledContent, Encoding.UTF8);

            Hardwarer.Led1.UpdateLED(ledContent);

            this.LED1PrevLedFileContent = value1 + value2;
        }

        #endregion

        #region LED2控制卡

        /// <summary>
        /// LED2控制卡屏号
        /// </summary>
        int LED2nScreenNo = 1;
        /// <summary>
        /// LED2动态区域号
        /// </summary>
        int LED2DYArea_ID = 1;
        /// <summary>
        /// LED2更新标识
        /// </summary>
        bool LED2m_bSendBusy = false;

        private bool _LED2ConnectStatus;
        /// <summary>
        /// LED2连接状态
        /// </summary>
        public bool LED2ConnectStatus
        {
            get
            {
                return _LED2ConnectStatus;
            }

            set
            {
                _LED2ConnectStatus = value;

                slightLED2.LightColor = (value ? Color.Green : Color.Red);

                commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.LED屏2_连接状态.ToString(), value ? "1" : "0");
            }
        }

        /// <summary>
        /// LED2显示内容文本
        /// </summary>
        string LED2TempFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Led2TempFile.txt");

        /// <summary>
        /// LED2上一次显示内容
        /// </summary>
        string LED2PrevLedFileContent = string.Empty;

        /// <summary>
        /// 更新LED2动态区域
        /// </summary>
        /// <param name="value1">第一行内容</param>
        /// <param name="value2">第二行内容</param>
        private void UpdateLed2Show(string value1 = "", string value2 = "")
        {
            if (!this.LED2ConnectStatus) return;
            if (this.LED2PrevLedFileContent == value1 + value2) return;

            string ledContent = GenerateFillLedContent12(value1) + GenerateFillLedContent12(value2);

            File.WriteAllText(this.LED2TempFile, ledContent, Encoding.UTF8);

            Hardwarer.Led2.UpdateLED(ledContent);

            this.LED2PrevLedFileContent = value1 + value2;
        }

        #endregion

        #endregion

        #region 地磅仪表

        /// <summary>
        /// 重量稳定事件
        /// </summary>
        /// <param name="steady"></param>
        void Wber_OnSteadyChange(bool steady)
        {
            InvokeEx(() =>
              {
                  this.WbSteady = steady;
              });
        }

        /// <summary>
        /// 地磅仪表状态变化
        /// </summary>
        /// <param name="status"></param>
        void Wber_OnStatusChange(bool status)
        {
            // 接收设备状态 
            InvokeEx(() =>
            {
                slightWber.LightColor = (status ? Color.Green : Color.Red);

                commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.地磅仪表_连接状态.ToString(), status ? "1" : "0");
            });
        }

        void Wber_OnWeightChange(double weight)
        {
            InvokeEx(() =>
            {
                panCurrentWeight.Text = weight.ToString();
            });
        }

        #endregion

        #region 海康视频

        /// <summary>
        /// 海康网络摄像机
        /// </summary>
        IPCer iPCer1 = new IPCer();
        IPCer iPCer2 = new IPCer();

        /// <summary>
        /// 执行摄像头抓拍，并保存数据
        /// </summary>
        /// <param name="transportId">运输记录Id</param>
        private void CamareCapturePicture(string transportId)
        {
            try
            {
                // 抓拍照片服务器发布地址
                //string pictureWebUrl = commonDAO.GetCommonAppletConfigString("汽车智能化_抓拍照片发布路径");
                DateTime captureTime = DateTime.Now;
                string pictureWebUrl = string.Format("{0}\\{1}\\{2}\\", captureTime.Year, captureTime.Month.ToString().PadLeft(2, '0'), captureTime.Day.ToString().PadLeft(2, '0'));

                // 摄像机1
                string picture1FileName = Path.Combine(SelfVars.CapturePicturePath, Guid.NewGuid().ToString() + ".jpg");
                if (iPCer1.CapturePictureJPEG(picture1FileName))
                {
                    CmcsTransportPicture transportPicture = new CmcsTransportPicture()
                    {
                        CaptureTime = captureTime,
                        CaptureType = CommonAppConfig.GetInstance().AppIdentifier,
                        TransportId = transportId,
                        PicturePath = pictureWebUrl + Path.GetFileName(picture1FileName)
                    };

                    commonDAO.SelfDber.Insert(transportPicture);
                }

                // 摄像机2
                string picture2FileName = Path.Combine(SelfVars.CapturePicturePath, Guid.NewGuid().ToString() + ".jpg");
                if (iPCer2.CapturePictureJPEG(picture2FileName))
                {
                    CmcsTransportPicture transportPicture = new CmcsTransportPicture()
                    {
                        CaptureTime = captureTime,
                        CaptureType = CommonAppConfig.GetInstance().AppIdentifier,
                        TransportId = transportId,
                        PicturePath = pictureWebUrl + Path.GetFileName(picture2FileName)
                    };

                    commonDAO.SelfDber.Insert(transportPicture);
                }
            }
            catch (Exception ex)
            {
                Log4Neter.Error("摄像机抓拍", ex);
            }
        }

        /// <summary>
        /// 上传抓拍照片到服务器共享文件夹
        /// </summary>
        private void UploadCapturePicture()
        {
            while (true)
            {
                try
                {
                    string serverPath = commonDAO.GetCommonAppletConfigString("汽车智能化_抓拍照片服务器共享路径");

                    if (string.IsNullOrEmpty(serverPath)) return;

                    List<CmcsTransportPicture> list = commonDAO.SelfDber.Entities<CmcsTransportPicture>("where IsUpload=0 and CaptureType=:CaptureType", new { CaptureType = CommonAppConfig.GetInstance().AppIdentifier });
                    foreach (CmcsTransportPicture item in list)
                    {
                        try
                        {
                            string fileName = Path.GetFileName(item.PicturePath);

                            string picture1FileNameLoca = SelfVars.CapturePicturePath;

                            string pictureWebUrlServer = Path.Combine(serverPath, string.Format("{0}\\{1}\\{2}\\", item.CaptureTime.Year, item.CaptureTime.Month.ToString().PadLeft(2, '0'), item.CaptureTime.Day.ToString().PadLeft(2, '0')));

                            if (Directory.Exists(serverPath))
                            {
                                if (!Directory.Exists(pictureWebUrlServer)) Directory.CreateDirectory(pictureWebUrlServer);

                                if (File.Exists(Path.Combine(picture1FileNameLoca, fileName)))
                                {
                                    File.Copy(Path.Combine(picture1FileNameLoca, fileName), Path.Combine(pictureWebUrlServer, fileName), true);
                                    //删除本地文件
                                    File.Delete(Path.Combine(picture1FileNameLoca, fileName));
                                }

                                item.IsUpload = 1;
                                commonDAO.SelfDber.Update(item);
                            }
                        }
                        catch (Exception ex)
                        {
                            Log4Neter.Error("上传抓拍照片", ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log4Neter.Error("上传抓拍照片线程错误", ex);
                }

                // 三分钟执行一次图片上传
                Thread.Sleep(180000);
            }

        }

        #endregion

        #region 海康视频Api

        CameraSDK cameraSDK1 = new CameraSDK();
        CameraSDK cameraSDK2 = new CameraSDK();

        private void CamareCapturePictureByApi(string transportId)
        {
            try
            {
                // 抓拍照片服务器发布地址
                //string pictureWebUrl = commonDAO.GetCommonAppletConfigString("汽车智能化_抓拍照片发布路径");
                DateTime captureTime = DateTime.Now;
                string pictureWebUrl = string.Format("{0}\\{1}\\{2}\\", captureTime.Year, captureTime.Month.ToString().PadLeft(2, '0'), captureTime.Day.ToString().PadLeft(2, '0'));

                // 摄像机1
                string picture1FileName = Path.Combine(SelfVars.CapturePicturePath, Guid.NewGuid().ToString() + ".jpg");
                if (cameraSDK1.CapturePictureJPEG(picture1FileName))
                {
                    CmcsTransportPicture transportPicture = new CmcsTransportPicture()
                    {
                        CaptureTime = captureTime,
                        CaptureType = CommonAppConfig.GetInstance().AppIdentifier,
                        TransportId = transportId,
                        PicturePath = pictureWebUrl + Path.GetFileName(picture1FileName)
                    };

                    commonDAO.SelfDber.Insert(transportPicture);
                }

                // 摄像机2
                string picture2FileName = Path.Combine(SelfVars.CapturePicturePath, Guid.NewGuid().ToString() + ".jpg");
                if (cameraSDK2.CapturePictureJPEG(picture2FileName))
                {
                    CmcsTransportPicture transportPicture = new CmcsTransportPicture()
                    {
                        CaptureTime = captureTime,
                        CaptureType = CommonAppConfig.GetInstance().AppIdentifier,
                        TransportId = transportId,
                        PicturePath = pictureWebUrl + Path.GetFileName(picture2FileName)
                    };

                    commonDAO.SelfDber.Insert(transportPicture);
                }
            }
            catch (Exception ex)
            {
                Log4Neter.Error("摄像机抓拍", ex);
            }
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
                this.InfraredSensor1Port = commonDAO.GetAppletConfigInt32("IO控制器_对射1端口");
                this.InfraredSensor2Port = commonDAO.GetAppletConfigInt32("IO控制器_对射2端口");
                this.WbMinWeight = commonDAO.GetAppletConfigDouble("地磅仪表_最小称重");
                this.PoundIndex = commonDAO.GetAppletConfigInt32("磅编号");

                //公共配置
                this.AutoPrint = commonDAO.GetCommonAppletConfigString("自动打印磅单") == "1";
                this.IsITMS = commonDAO.GetCommonAppletConfigString("启用智能调运") == "1";
                this.IsTicketDiff = commonDAO.GetCommonAppletConfigString("启用净重超差限值") == "1";
                this.TicketDiff = (decimal)commonDAO.GetCommonAppletConfigDouble("净重超差限值");
                this.IsTareCoefficient = commonDAO.GetCommonAppletConfigString("启用皮重排查系数") == "1";
                this.TareCoefficient = (decimal)commonDAO.GetCommonAppletConfigDouble("皮重排查系数");

                // IO控制器
                //Hardwarer.Iocer.OnReceived += new IOC.JMDM20DIOV2.JMDM20DIOV2Iocer.ReceivedEventHandler(Iocer_Received);
                //Hardwarer.Iocer.OnStatusChange += new IOC.JMDM20DIOV2.JMDM20DIOV2Iocer.StatusChangeHandler(Iocer_StatusChange);
                success = Hardwarer.Iocer.OpenCom(commonDAO.GetAppletConfigInt32("IO控制器_串口"), commonDAO.GetAppletConfigInt32("IO控制器_波特率"), commonDAO.GetAppletConfigInt32("IO控制器_数据位"), (StopBits)commonDAO.GetAppletConfigInt32("IO控制器_停止位"), (Parity)commonDAO.GetAppletConfigInt32("IO控制器_校验位"));
                if (!success) MessageBoxEx.Show("IO控制器连接失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.iocControler = new IocControler(Hardwarer.Iocer);

                // 地磅仪表
                //Hardwarer.Wber.OnStatusChange += new WB.YAOHUA.XK3190DS3.Wagon_Meter.StatusChangeHandler(Wber_OnStatusChange);
                //Hardwarer.Wber.OnSteadyChange += new WB.YAOHUA.XK3190DS3.Wagon_Meter.SteadyChangeEventHandler(Wber_OnSteadyChange);
                //Hardwarer.Wber.OnWeightChange += new WB.YAOHUA.XK3190DS3.Wagon_Meter.WeightChangeEventHandler(Wber_OnWeightChange);
                success = Hardwarer.Wber.OpenCom(commonDAO.GetAppletConfigInt32("地磅仪表_串口"), commonDAO.GetAppletConfigInt32("地磅仪表_波特率"), commonDAO.GetAppletConfigInt32("地磅仪表_数据位"), (StopBits)commonDAO.GetAppletConfigInt32("地磅仪表_停止位"), (Parity)commonDAO.GetAppletConfigInt32("地磅仪表_校验位"));

                // 读卡器1
                Hardwarer.Rwer1.StartWith = commonDAO.GetAppletConfigString("读卡器_标签过滤");
                Hardwarer.Rwer1.OnStatusChange += new RW.LZR12.Net.Lzr12Rwer.StatusChangeHandler(Rwer1_OnStatusChange);
                Hardwarer.Rwer1.OnScanError += new RW.LZR12.Net.Lzr12Rwer.ScanErrorEventHandler(Rwer1_OnScanError);
                success = Hardwarer.Rwer1.OpenCom(commonDAO.GetAppletConfigString("读卡器1_Ip"), commonDAO.GetAppletConfigInt32("读卡器1_端口"));
                if (!success) MessageBoxEx.Show("读卡器1连接失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // 读卡器2
                Hardwarer.Rwer2.StartWith = commonDAO.GetAppletConfigString("读卡器_标签过滤");
                Hardwarer.Rwer2.OnStatusChange += new RW.LZR12.Net.Lzr12Rwer.StatusChangeHandler(Rwer2_OnStatusChange);
                Hardwarer.Rwer2.OnScanError += new RW.LZR12.Net.Lzr12Rwer.ScanErrorEventHandler(Rwer2_OnScanError);
                success = Hardwarer.Rwer2.OpenCom(commonDAO.GetAppletConfigString("读卡器2_Ip"), commonDAO.GetAppletConfigInt32("读卡器2_端口"));
                if (!success) MessageBoxEx.Show("读卡器2连接失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                #region 海康视频

                IPCer.InitSDK();

                CmcsCamera video1 = commonDAO.SelfDber.Entity<CmcsCamera>("where EquipmentCode=:EquipmentCode", new { EquipmentCode = "重车衡摄像头1" });
                if (video1 != null)
                {
                    if (CommonUtil.PingReplyTest(video1.Ip))
                    {
                        if (iPCer1.Login(video1.Ip, video1.Port, video1.UserName, video1.Password))
                        {
                            bool res = iPCer1.StartPreview(panVideo1.Handle, video1.Channel);
                        }
                    }
                }

                CmcsCamera video2 = commonDAO.SelfDber.Entity<CmcsCamera>("where EquipmentCode=:EquipmentCode", new { EquipmentCode = "摄像机2" });
                if (video2 != null)
                {
                    if (CommonUtil.PingReplyTest(video2.Ip))
                    {
                        if (iPCer2.Login(video2.Ip, video2.Port, video2.UserName, video2.Password))
                            iPCer2.StartPreview(panVideo2.Handle, video2.Channel);
                    }
                }

                #endregion

                #region 海康视频Api

                //success = CameraSDK.InitSDK(commonDAO.GetCommonAppletConfigString("海康平台地址"), commonDAO.GetCommonAppletConfigInt32("海康协议端口号"), commonDAO.GetCommonAppletConfigString("海康Appkey"), commonDAO.GetCommonAppletConfigString("海康Secret"));

                //if (success)
                //{
                //    CmcsCamera video1 = commonDAO.SelfDber.Entity<CmcsCamera>("where Name=:Name", new { Name = CommonAppConfig.GetInstance().AppIdentifier + "摄像头1" });
                //    if (video1 != null)
                //    {
                //        cameraSDK1.StartPreview(video1.EquipmentCode, panVideo1.Handle);
                //    }

                //    CmcsCamera video2 = commonDAO.SelfDber.Entity<CmcsCamera>("where Name=:Name", new { Name = CommonAppConfig.GetInstance().AppIdentifier + "摄像头2" });
                //    if (video2 != null)
                //    {
                //        cameraSDK2.StartPreview(video2.EquipmentCode, panVideo2.Handle);
                //    }
                //}
                #endregion

                #region LED控制卡1

                string led1SocketIP = commonDAO.GetAppletConfigString("LED显示屏1_IP地址");
                if (!string.IsNullOrEmpty(led1SocketIP))
                {
                    if (CommonUtil.PingReplyTest(led1SocketIP))
                    {
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

                #region LED控制卡2

                string led2SocketIP = commonDAO.GetAppletConfigString("LED显示屏2_IP地址");
                if (!string.IsNullOrEmpty(led2SocketIP))
                {
                    if (CommonUtil.PingReplyTest(led2SocketIP))
                    {
                        if (Hardwarer.Led2.OpenLED(led2SocketIP))
                        {
                            // 初始化成功
                            this.LED2ConnectStatus = true;
                            UpdateLed2Show("  等待车辆");
                        }
                        else
                        {
                            this.LED2ConnectStatus = false;
                            Log4Neter.Error("初始化LED2控制卡失败", new Exception("连接设备"));
                            MessageBoxEx.Show("LED2控制卡连接失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        this.LED2ConnectStatus = false;
                        Log4Neter.Error("初始化LED2控制卡，网络连接失败", new Exception("网络异常"));
                        MessageBoxEx.Show("LED2控制卡网络连接失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                #endregion

                //语音设置
                voiceSpeaker.SetVoice(commonDAO.GetAppletConfigInt32("语速"), commonDAO.GetAppletConfigInt32("音量"), commonDAO.GetAppletConfigString("语音包"));

                timer1.Enabled = true;
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
            try
            {
                if (this.LED2ConnectStatus)
                {
                    Hardwarer.Led2.CloseLED();
                }

                if (LED1ConnectStatus || LED2ConnectStatus)
                    Hardwarer.Led2.Destroy();
            }
            catch { }
            try
            {
                IPCer.CleanupSDK();
                //CameraSDK.CleanupSDK();
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
            timer1.Interval = 1000;

            try
            {
                switch (this.CurrentFlowFlag)
                {
                    case eFlowFlag.等待车辆:
                        #region

                        // Direction.Way1
                        if (this.InductorCoil1)
                        {
                            // 当读卡区域地感有信号，触发读卡或者车号识别
                            this.CurrentDirection = eDirection.Way1;

                            this.CurrentFlowFlag = eFlowFlag.开始读卡;
                        }
                        // Direction.Way2
                        else if (this.InductorCoil4)
                        {
                            // 当读卡区域地感有信号，触发读卡或者车号识别
                            this.CurrentDirection = eDirection.Way2;

                            this.CurrentFlowFlag = eFlowFlag.开始读卡;
                        }

                        #endregion
                        break;

                    case eFlowFlag.开始读卡:
                        #region

                        //提高读卡灵敏度
                        timer1.Interval = 500;

                        if (this.CurrentDirection == eDirection.Way1)
                        {
                            List<string> tags = Hardwarer.Rwer1.ScanTags();
                            if (tags.Count > 0) passCarQueuer.Enqueue(eDirection.Way1, tags[0]);

                            commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.上磅方向.ToString(), "1");
                        }
                        else if (this.CurrentDirection == eDirection.Way2)
                        {
                            List<string> tags = Hardwarer.Rwer2.ScanTags();
                            if (tags.Count > 0) passCarQueuer.Enqueue(eDirection.Way2, tags[0]);

                            commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.上磅方向.ToString(), "0");
                        }

                        if (passCarQueuer.Count > 0) this.CurrentFlowFlag = eFlowFlag.识别车辆;

                        else if (!this.InductorCoil1 && !this.InductorCoil2 && !this.InductorCoil3 && !this.InductorCoil4)
                        {
                            UpdateLedShow("  等待车辆");
                            this.CurrentImperfectCar = null;
                            this.CurrentDirection = eDirection.UnKnow;
                            this.CurrentFlowFlag = eFlowFlag.等待车辆;
                            break;
                        }

                        UpdateLedShow("  正在读卡");

                        #endregion
                        break;

                    case eFlowFlag.识别车辆:
                        #region

                        // 队列中无车时，等待车辆
                        if (passCarQueuer.Count == 0)
                        {
                            UpdateLedShow("  等待车辆");
                            this.CurrentImperfectCar = null;
                            this.CurrentDirection = eDirection.UnKnow;
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
                            UpdateLedShow(this.CurrentAutotruck.CarNumber, "读卡成功");
                            this.voiceSpeaker.Speak(this.CurrentAutotruck.CarNumber + " 读卡成功", 1, false);

                            if (this.CurrentAutotruck.IsUse == 1)
                            {
                                if (this.CurrentAutotruck.CarType == eCarType.入厂煤.ToString())
                                {
                                    this.timer_BuyFuel_Cancel = false;
                                    this.CurrentFlowFlag = eFlowFlag.验证信息;
                                }
                                else if (this.CurrentAutotruck.CarType == eCarType.销售煤.ToString())
                                {
                                    this.timer_SaleFuel_Cancel = false;
                                    this.CurrentFlowFlag = eFlowFlag.验证信息;
                                }
                                else if (this.CurrentAutotruck.CarType == eCarType.其他物资.ToString())
                                {
                                    this.timer_Goods_Cancel = false;
                                    this.CurrentFlowFlag = eFlowFlag.验证信息;
                                }
                            }
                            else
                            {
                                UpdateLedShow(this.CurrentAutotruck.CarNumber, "已停用");
                                this.voiceSpeaker.Speak("车牌号 " + this.CurrentAutotruck.CarNumber + " 已停用，禁止通过", 1, false);

                                timer1.Interval = 20000;
                            }
                        }
                        else
                        {
                            UpdateLedShow(this.CurrentImperfectCar.Voucher, "未登记");

                            // 方式一：车号识别
                            this.voiceSpeaker.Speak("车牌号 " + this.CurrentImperfectCar.Voucher + " 未登记，禁止通过", 1, false);
                            //// 方式二：刷卡方式
                            //this.voiceSpeaker.Speak("卡号未登记，禁止通过", 2, false);

                            timer1.Interval = 20000;
                        }

                        #endregion
                        break;
                }

                commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.地磅仪表_实时重量.ToString(), Hardwarer.Wber.Weight.ToString());
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
            // 三秒执行一次
            timer2.Interval = 3000;

            try
            {
                // 执行远程命令
                ExecAppRemoteControlCmd();

                // 定时查询公共配置
                this.AutoPrint = commonDAO.GetCommonAppletConfigString("自动打印磅单") == "1";
                this.IsITMS = commonDAO.GetCommonAppletConfigString("启用智能调运") == "1";
                this.IsTicketDiff = commonDAO.GetCommonAppletConfigString("启用净重超差限值") == "1";
                this.TicketDiff = (decimal)commonDAO.GetCommonAppletConfigDouble("净重超差限值");
                this.IsTareCoefficient = commonDAO.GetCommonAppletConfigString("启用皮重排查系数") == "1";
                this.TareCoefficient = (decimal)commonDAO.GetCommonAppletConfigDouble("皮重排查系数");
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
        bool lastWberStatus = false;
        bool lastWberSteady = false;

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
                this.InfraredSensor1 = (Hardwarer.Iocer.ReceiveValue[this.InfraredSensor1Port - 1] == 1);
                this.InfraredSensor2 = (Hardwarer.Iocer.ReceiveValue[this.InfraredSensor2Port - 1] == 1);

                if (lastIOStatus != Hardwarer.Iocer.Status)
                {
                    slightIOC.LightColor = (Hardwarer.Iocer.Status ? Color.Green : Color.Red);
                    commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.IO控制器_连接状态.ToString(), Hardwarer.Iocer.Status ? "1" : "0");
                }

                #endregion

                #region 仪表

                if (lastWberSteady != Hardwarer.Wber.Steady)
                {
                    this.panCurrentWeight.Style.ForeColor.Color = (Hardwarer.Wber.Steady ? Color.Lime : Color.Red);
                    commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.地磅仪表_稳定.ToString(), Hardwarer.Wber.Steady ? "1" : "0");
                }

                if (lastWberStatus != Hardwarer.Wber.Status)
                {
                    slightWber.LightColor = (Hardwarer.Wber.Status ? Color.Green : Color.Red);
                    commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.地磅仪表_连接状态.ToString(), Hardwarer.Wber.Status ? "1" : "0");
                }

                panCurrentWeight.Text = Hardwarer.Wber.Weight.ToString();

                this.WbSteady = Hardwarer.Wber.Steady;

                #endregion

                panCurrentWeight.Refresh();

                lastIOStatus = Hardwarer.Iocer.Status;
                lastWberSteady = Hardwarer.Wber.Steady;
                lastWberStatus = Hardwarer.Wber.Status;
            }
            catch { }
        }

        /// <summary>
        /// 有车辆在上磅的道路上
        /// </summary>
        /// <returns></returns>
        bool HasCarOnEnterWay()
        {
            if (this.CurrentImperfectCar == null) return false;

            if (this.CurrentImperfectCar.PassWay == eDirection.UnKnow)
                return false;
            else if (this.CurrentImperfectCar.PassWay == eDirection.Way1)
                return this.InductorCoil1 || this.InductorCoil2 || this.InfraredSensor1;
            else if (this.CurrentImperfectCar.PassWay == eDirection.Way2)
                return this.InductorCoil3 || this.InductorCoil4 || this.InfraredSensor2;

            return true;
        }

        /// <summary>
        /// 有车辆在下磅的道路上
        /// </summary>
        /// <returns></returns>
        bool HasCarOnLeaveWay()
        {
            if (this.CurrentImperfectCar == null) return false;

            if (this.CurrentImperfectCar.PassWay == eDirection.UnKnow)
                return false;
            else if (this.CurrentImperfectCar.PassWay == eDirection.Way1)
                return this.InductorCoil3 || this.InductorCoil4 || this.InfraredSensor2;
            else if (this.CurrentImperfectCar.PassWay == eDirection.Way2)
                return this.InductorCoil1 || this.InductorCoil2 || this.InfraredSensor1;

            return true;
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
        /// 切换手动/自动模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sbtnChangeAutoHandMode_ValueChanged(object sender, EventArgs e)
        {
            this.AutoHandMode = sbtnChangeAutoHandMode.Value;
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

        #endregion

        #region 入厂煤业务

        bool timer_BuyFuel_Cancel = true;

        CmcsBuyFuelTransport currentBuyFuelTransport;
        /// <summary>
        /// 当前运输记录
        /// </summary>
        public CmcsBuyFuelTransport CurrentBuyFuelTransport
        {
            get { return currentBuyFuelTransport; }
            set
            {
                currentBuyFuelTransport = value;

                if (value != null)
                {
                    commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.当前运输记录Id.ToString(), value.Id);

                    txtFuelKindName_BuyFuel.Text = value.FuelKindName;
                    txtMineName_BuyFuel.Text = value.MineName;
                    txtSupplierName_BuyFuel.Text = value.SupplierName;
                    txtTransportCompanyName_BuyFuel.Text = value.TransportCompanyName;

                    txtGrossWeight_BuyFuel.Text = value.GrossWeight.ToString("F2");
                    txtTicketWeight_BuyFuel.Text = value.TicketWeight.ToString("F2");
                    txtTareWeight_BuyFuel.Text = value.TareWeight.ToString("F2");
                    txtDeductWeight_BuyFuel.Text = value.DeductWeight.ToString("F2");
                    txtSuttleWeight_BuyFuel.Text = value.SuttleWeight.ToString("F2");
                }
                else
                {
                    commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.当前运输记录Id.ToString(), string.Empty);

                    txtFuelKindName_BuyFuel.ResetText();
                    txtMineName_BuyFuel.ResetText();
                    txtSupplierName_BuyFuel.ResetText();
                    txtTransportCompanyName_BuyFuel.ResetText();

                    txtGrossWeight_BuyFuel.ResetText();
                    txtTicketWeight_BuyFuel.ResetText();
                    txtTareWeight_BuyFuel.ResetText();
                    txtDeductWeight_BuyFuel.ResetText();
                    txtSuttleWeight_BuyFuel.ResetText();
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
            FrmUnFinishTransport_Select frm = new FrmUnFinishTransport_Select("where CarType='" + eCarType.入厂煤.ToString() + "' order by CreateDate desc");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (this.InductorCoil1)
                    passCarQueuer.Enqueue(eDirection.Way1, frm.Output.CarNumber);
                else if (this.InductorCoil4)
                    passCarQueuer.Enqueue(eDirection.Way2, frm.Output.CarNumber);
                else
                    passCarQueuer.Enqueue(eDirection.UnKnow, frm.Output.CarNumber);

                this.CurrentFlowFlag = eFlowFlag.识别车辆;
            }
        }

        /// <summary>
        /// 保存入厂煤运输记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveTransport_BuyFuel_Click(object sender, EventArgs e)
        {
            if (!SaveBuyFuelTransport()) MessageBoxEx.Show("保存失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 保存运输记录
        /// </summary>
        /// <returns></returns>
        bool SaveBuyFuelTransport()
        {
            if (this.CurrentBuyFuelTransport == null) return false;

            try
            {
                bool isDiff = false;
                decimal diffValue = 0;
                double deductWeight = 0;
                string catagory = string.Empty; //卸煤点信息

                decimal Weight = (decimal)Hardwarer.Wber.Weight;

                if (this.CurrentBuyFuelTransport.StepName != eTruckInFactoryStep.重车.ToString())
                {
                    //重车

                    #region 皮重排查系数
                    if (this.IsTareCoefficient && this.CurrentBuyFuelTransport.TicketWeight > 0 && this.CurrentBuyFuelTransport.HistoryTareAvg > 0)
                    {
                        //超差判断：（过衡毛重 - 历史皮重均值 ）/ 矿发量 > 1.4   或者 矿发量 /（过衡毛重 - 历史皮重均值 ）> 1.4,自动报警；
                        decimal TareWeightAvg = this.CurrentBuyFuelTransport.HistoryTareAvg;
                        if ((Weight - TareWeightAvg) / this.CurrentBuyFuelTransport.TicketWeight > this.TareCoefficient ||
                             this.CurrentBuyFuelTransport.TicketWeight / (Weight - TareWeightAvg) > this.TareCoefficient)
                        {
                            UpdateLedShow(this.CurrentAutotruck.CarNumber + "净重超差");
                            this.voiceSpeaker.Speak(this.CurrentAutotruck.CarNumber + "净重超差", 1, false);
                            return false;
                        }
                    }
                    #endregion

                }
                if (this.CurrentBuyFuelTransport.StepName == eTruckInFactoryStep.重车.ToString())
                {
                    //回皮

                    #region 智能调运

                    if (this.IsITMS)
                    {
                        try
                        {
                            ////测试
                            //ZkBizResult entity = new ZkBizResult() { DispatchInfo = new DispatchInfo() { ReceiveDeductWeight = 1, Catagory = "000000001" }, Message = "", Status = true };
                            ZkBizResult entity = commService.GetDeductWeightInfo(this.CurrentAutotruck.CarNumber, this.PoundIndex);
                            if (entity == null)
                            {
                                UpdateLedShow("智能调运读取失败，请联系中矿");
                                this.voiceSpeaker.Speak("智能调运读取失败，请联系中矿", 1, false);
                                return false;
                            }
                            if (!entity.Status)
                            {
                                UpdateLedShow("智能调运读取失败，请联系中矿");
                                this.voiceSpeaker.Speak("智能调运读取失败，请联系中矿", 1, false);
                                return false;
                            }

                            deductWeight = entity.DispatchInfo.ReceiveDeductWeight;
                            catagory = entity.DispatchInfo.Catagory;

                            commService.SetRcvWeight(this.CurrentAutotruck.CarNumber, (double)this.CurrentBuyFuelTransport.GrossWeight, (double)Weight, deductWeight, "", 0, this.PoundIndex);
                        }
                        catch (Exception ex)
                        {
                            Log4Neter.Error(this.CurrentAutotruck.CarNumber + "获取车辆扣吨信息且反馈车辆厂收信息", ex);

                            UpdateLedShow("智能调运读取失败，请联系管理员");
                            this.voiceSpeaker.Speak("智能调运读取失败，请联系管理员", 1, false);
                            return false;

                        }
                    }

                    #endregion

                    #region 净重超差

                    if (this.IsTicketDiff && this.CurrentBuyFuelTransport.TicketWeight > 0)
                    {
                        isDiff = Math.Abs(this.CurrentBuyFuelTransport.GrossWeight - Weight - this.CurrentBuyFuelTransport.TicketWeight) - this.TicketDiff > 0;
                        diffValue = Math.Abs(this.CurrentBuyFuelTransport.GrossWeight - Weight - this.CurrentBuyFuelTransport.TicketWeight);

                        if (isDiff)
                        {
                            UpdateLedShow(this.CurrentAutotruck.CarNumber + "净重超差：" + diffValue + "吨");
                            this.voiceSpeaker.Speak(this.CurrentAutotruck.CarNumber + "净重超差：" + diffValue + "吨", 1, false);
                            return false;
                        }
                    }

                    #endregion
                }

                if (weighterDAO.SaveBuyFuelTransport(this.CurrentBuyFuelTransport.Id, Weight, (decimal)deductWeight, catagory, DateTime.Now, this.CurrentAutotruck.EPCCardId, CommonAppConfig.GetInstance().AppIdentifier))
                {
                    this.CurrentBuyFuelTransport = commonDAO.SelfDber.Get<CmcsBuyFuelTransport>(this.CurrentBuyFuelTransport.Id);

                    FrontGateUp();

                    btnSaveTransport_BuyFuel.Enabled = false;
                    this.CurrentFlowFlag = eFlowFlag.等待离开;

                    UpdateLedShow("称重完毕", "请下磅,重量" + Hardwarer.Wber.Weight + "吨");
                    this.voiceSpeaker.Speak("称重完毕请下磅", 1, false);

                    LoadTodayUnFinishBuyFuelTransport();
                    LoadTodayFinishBuyFuelTransport();

                    CamareCapturePicture(this.CurrentBuyFuelTransport.Id);

                    //打印磅单 
                    if (this.AutoPrint && this.CurrentBuyFuelTransport.SuttleWeight > 0)
                    {
                        //异步打印
                        new Task(() => { ReportPrint.GetInstance().PrintBuyFuelTransport(this.CurrentBuyFuelTransport); }).Start();
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                //MessageBoxEx.Show("保存失败\r\n" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);

                UpdateLedShow("保存失败" + ex.Message);

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
            this.CurrentBuyFuelTransport = null;
            this.CurrentDirection = eDirection.UnKnow;

            txtTagId_BuyFuel.ResetText();

            btnSaveTransport_BuyFuel.Enabled = false;

            FrontGateDown();
            BackGateDown();

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
                    case eFlowFlag.验证信息:
                        #region

                        // 查找该车未完成的运输记录
                        CmcsUnFinishTransport unFinishTransport = carTransportDAO.GetUnFinishTransportByAutotruckId(this.CurrentAutotruck.Id, eCarType.入厂煤.ToString());
                        if (unFinishTransport != null)
                        {
                            this.CurrentBuyFuelTransport = commonDAO.SelfDber.Get<CmcsBuyFuelTransport>(unFinishTransport.TransportId);
                            if (this.CurrentBuyFuelTransport != null)
                            {
                                // 判断路线设置
                                string nextPlace;
                                if (carTransportDAO.CheckNextTruckInFactoryWay(this.CurrentAutotruck.CarType, this.CurrentBuyFuelTransport.StepName, "重车|轻车", CommonAppConfig.GetInstance().AppIdentifier, out nextPlace))
                                {
                                    if (this.CurrentBuyFuelTransport.SuttleWeight == 0)
                                    {
                                        BackGateUp();

                                        this.CurrentFlowFlag = eFlowFlag.等待上磅;

                                        UpdateLedShow(this.CurrentAutotruck.CarNumber, "请上磅");
                                        this.voiceSpeaker.Speak("车牌号 " + this.CurrentAutotruck.CarNumber + " 请上磅", 1, false);
                                    }
                                    else
                                    {
                                        UpdateLedShow(this.CurrentAutotruck.CarNumber, "已称重");
                                        this.voiceSpeaker.Speak("车牌号 " + this.CurrentAutotruck.CarNumber + " 已称重", 1, false);

                                        timer_BuyFuel.Interval = 20000;
                                    }
                                }
                                else
                                {
                                    UpdateLedShow("路线错误", "禁止通过");
                                    this.voiceSpeaker.Speak("路线错误 禁止通过 " + (!string.IsNullOrEmpty(nextPlace) ? "请前往" + nextPlace : ""), 1, false);

                                    timer_BuyFuel.Interval = 20000;
                                }
                            }
                            else
                            {
                                commonDAO.SelfDber.Delete<CmcsUnFinishTransport>(unFinishTransport.Id);
                            }
                        }
                        else
                        {
                            UpdateLedShow(this.CurrentAutotruck.CarNumber, "未排队");
                            this.voiceSpeaker.Speak("车牌号 " + this.CurrentAutotruck.CarNumber + " 未找到排队记录", 1, false);

                            timer_BuyFuel.Interval = 20000;
                        }

                        #endregion
                        break;

                    case eFlowFlag.等待上磅:
                        #region

                        // 当地磅仪表重量大于最小称重且来车方向的地感与对射均无信号，则判定车已经完全上磅
                        if (Hardwarer.Wber.Weight >= this.WbMinWeight && !HasCarOnEnterWay())
                        {
                            BackGateDown();

                            this.CurrentFlowFlag = eFlowFlag.等待稳定;
                        }

                        // 降低灵敏度
                        timer_BuyFuel.Interval = 4000;

                        #endregion
                        break;

                    case eFlowFlag.等待稳定:
                        #region

                        // 提高灵敏度
                        timer_BuyFuel.Interval = 1000;

                        btnSaveTransport_BuyFuel.Enabled = this.WbSteady;

                        UpdateLedShow(this.CurrentAutotruck.CarNumber, Hardwarer.Wber.Weight.ToString("#0.######"));

                        if (this.WbSteady)
                        {
                            if (this.AutoHandMode)
                            {
                                // 自动模式
                                if (!SaveBuyFuelTransport())
                                {
                                    // 降低灵敏度
                                    timer_BuyFuel.Interval = 8000;

                                    //UpdateLedShow(this.CurrentAutotruck.CarNumber, "称重失败");
                                    //this.voiceSpeaker.Speak("车牌号 " + this.CurrentAutotruck.CarNumber + " 称重失败，请联系管理员", 1, false);
                                }
                            }
                            else
                            {
                                // 手动模式 
                            }
                        }

                        #endregion
                        break;

                    case eFlowFlag.等待离开:
                        #region

                        // 当前地磅重量小于最小称重且所有地感、对射无信号时重置
                        if (Hardwarer.Wber.Weight < this.WbMinWeight && !HasCarOnLeaveWay()) ResetBuyFuel();

                        // 降低灵敏度
                        timer_BuyFuel.Interval = 4000;

                        #endregion
                        break;
                }

                // 当前地磅重量小于最小称重且所有地感、对射无信号时重置
                if (Hardwarer.Wber.Weight < this.WbMinWeight && !HasCarOnEnterWay() && !HasCarOnLeaveWay() && this.CurrentFlowFlag != eFlowFlag.等待车辆
                    && this.CurrentImperfectCar != null) ResetBuyFuel();
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
            superGridControl1_BuyFuel.PrimaryGrid.DataSource = weighterDAO.GetUnFinishBuyFuelTransport();
        }

        /// <summary>
        /// 获取指定日期已完成的入厂煤记录
        /// </summary>
        void LoadTodayFinishBuyFuelTransport()
        {
            superGridControl2_BuyFuel.PrimaryGrid.DataSource = weighterDAO.GetFinishedBuyFuelTransport(DateTime.Now.Date, DateTime.Now.Date.AddDays(1));
        }

        #endregion

        #region 销售煤业务

        bool timer_SaleFuel_Cancel = true;
        CmcsSaleFuelTransport currentSaleFuelTransport;
        /// <summary>
        /// 当前运输记录
        /// </summary>
        public CmcsSaleFuelTransport CurrentSaleFuelTransport
        {
            get { return currentSaleFuelTransport; }
            set
            {
                currentSaleFuelTransport = value;

                if (value != null)
                {
                    commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.当前运输记录Id.ToString(), value.Id);
                    txtCarNumber_SaleFuel.Text = value.CarNumber;
                    txt_YBNumber1.Text = value.TransportSalesNum;
                    txt_TransportNo1.Text = value.TransportNo;
                    txt_Consignee1.Text = value.SupplierName;
                    txt_TransportCompayName1.Text = value.TransportCompanyName;

                    txtGrossWeight_SaleFuel.Text = value.GrossWeight.ToString("F2");
                    txtTareWeight_SaleFuel.Text = value.TareWeight.ToString("F2");
                    txtSuttleWeight_SaleFuel.Text = value.SuttleWeight.ToString("F2");
                }
                else
                {
                    commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.当前运输记录Id.ToString(), string.Empty);
                    txtCarNumber_SaleFuel.ResetText();
                    txt_YBNumber1.ResetText();
                    txt_TransportNo1.ResetText();
                    txt_Consignee1.ResetText();
                    txt_TransportCompayName1.ResetText();

                    txtGrossWeight_SaleFuel.ResetText();
                    txtTareWeight_SaleFuel.ResetText();
                    txtSuttleWeight_SaleFuel.ResetText();
                }
            }
        }

        /// <summary>
        /// 选择车辆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectAutotruck_SaleFuel_Click(object sender, EventArgs e)
        {
            FrmUnFinishTransport_Select frm = new FrmUnFinishTransport_Select("where CarType='" + eCarType.销售煤.ToString() + "' order by CreateDate desc");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (this.InductorCoil1)
                    passCarQueuer.Enqueue(eDirection.Way1, frm.Output.CarNumber);
                else if (this.InductorCoil4)
                    passCarQueuer.Enqueue(eDirection.Way2, frm.Output.CarNumber);
                else
                    passCarQueuer.Enqueue(eDirection.UnKnow, frm.Output.CarNumber);

                this.CurrentFlowFlag = eFlowFlag.识别车辆;
                txtCarNumber_SaleFuel.Text = frm.Output.CarNumber;
                txtTagId_SaleFuel.Text = frm.Output.CarNumber;
            }
        }

        /// <summary>
        /// 保存排队记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveTransport_SaleFuel_Click(object sender, EventArgs e)
        {
            if (!SaveSaleFuelTransport()) MessageBoxEx.Show("保存失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 保存运输记录
        /// </summary>
        /// <returns></returns>
        bool SaveSaleFuelTransport()
        {
            if (this.CurrentSaleFuelTransport == null) return false;

            try
            {
                if (weighterDAO.SaveSaleFuelTransport(this.CurrentSaleFuelTransport.Id, (decimal)Hardwarer.Wber.Weight, DateTime.Now, CommonAppConfig.GetInstance().AppIdentifier))
                {
                    this.CurrentSaleFuelTransport = commonDAO.SelfDber.Get<CmcsSaleFuelTransport>(this.CurrentSaleFuelTransport.Id);

                    FrontGateUp();

                    btnSaveTransport_SaleFuel.Enabled = false;
                    this.CurrentFlowFlag = eFlowFlag.等待离开;

                    UpdateLedShow("称重完毕", "请前往" + this.CurrentSaleFuelTransport.LoadArea);
                    this.voiceSpeaker.Speak("称重完毕请下磅，请前往" + this.CurrentSaleFuelTransport.LoadArea, 1, false);

                    LoadTodayUnFinishSaleFuelTransport();
                    LoadTodayFinishSaleFuelTransport();

                    CamareCapturePicture(this.CurrentSaleFuelTransport.Id);

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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_SaleFuel_Click(object sender, EventArgs e)
        {
            ResetSaleFuel();
        }

        /// <summary>
        /// 重置信息
        /// </summary>
        void ResetSaleFuel()
        {
            this.timer_SaleFuel_Cancel = true;

            this.CurrentFlowFlag = eFlowFlag.等待车辆;

            this.CurrentAutotruck = null;
            this.CurrentSaleFuelTransport = null;
            this.CurrentDirection = eDirection.UnKnow;

            txtTagId_SaleFuel.ResetText();

            btnSaveTransport_SaleFuel.Enabled = false;

            FrontGateDown();
            BackGateDown();

            UpdateLedShow("  等待车辆");

            // 最后重置
            this.CurrentImperfectCar = null;
        }

        /// <summary>
        /// 其他物资运输记录业务定时器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_SaleFuel_Tick(object sender, EventArgs e)
        {
            if (this.timer_SaleFuel_Cancel) return;

            timer_SaleFuel.Stop();
            timer_SaleFuel.Interval = 2000;

            try
            {
                switch (this.CurrentFlowFlag)
                {
                    case eFlowFlag.验证信息:
                        #region

                        // 查找该车未完成的运输记录
                        CmcsUnFinishTransport unFinishTransport = carTransportDAO.GetUnFinishTransportByAutotruckId(this.CurrentAutotruck.Id, eCarType.销售煤.ToString());
                        if (unFinishTransport != null)
                        {
                            this.CurrentSaleFuelTransport = commonDAO.SelfDber.Get<CmcsSaleFuelTransport>(unFinishTransport.TransportId);
                            if (this.CurrentSaleFuelTransport != null)
                            {
                                // 判断路线设置
                                string nextPlace;
                                if (carTransportDAO.CheckNextTruckInFactoryWay(this.CurrentAutotruck.CarType, this.CurrentSaleFuelTransport.StepName, "重车|轻车", CommonAppConfig.GetInstance().AppIdentifier, out nextPlace))
                                {
                                    if (this.CurrentSaleFuelTransport.SuttleWeight == 0)
                                    {
                                        BackGateUp();

                                        this.CurrentFlowFlag = eFlowFlag.等待上磅;

                                        UpdateLedShow(this.CurrentAutotruck.CarNumber, "请上磅");
                                        this.voiceSpeaker.Speak("车牌号 " + this.CurrentAutotruck.CarNumber + " 请上磅", 1, false);
                                    }
                                    else
                                    {
                                        UpdateLedShow(this.CurrentAutotruck.CarNumber, "已称重");
                                        this.voiceSpeaker.Speak("车牌号 " + this.CurrentAutotruck.CarNumber + " 已称重", 1, false);

                                        timer_SaleFuel.Interval = 20000;
                                    }
                                }
                                else
                                {
                                    UpdateLedShow("路线错误", "禁止通过");
                                    this.voiceSpeaker.Speak("路线错误 禁止通过 " + (!string.IsNullOrEmpty(nextPlace) ? "请前往" + nextPlace : ""), 1, false);

                                    timer_SaleFuel.Interval = 20000;
                                }
                            }
                            else
                            {
                                commonDAO.SelfDber.Delete<CmcsUnFinishTransport>(unFinishTransport.Id);
                            }
                        }
                        else
                        {
                            UpdateLedShow(this.CurrentAutotruck.CarNumber, "未排队");
                            this.voiceSpeaker.Speak("车牌号 " + this.CurrentAutotruck.CarNumber + " 未找到排队记录", 1, false);

                            timer_SaleFuel.Interval = 20000;
                        }

                        #endregion
                        break;

                    case eFlowFlag.等待上磅:
                        #region

                        // 当地磅仪表重量大于最小称重且来车方向的地感与对射均无信号，则判定车已经完全上磅
                        if (Hardwarer.Wber.Weight >= this.WbMinWeight && !HasCarOnEnterWay())
                        {
                            BackGateDown();

                            this.CurrentFlowFlag = eFlowFlag.等待稳定;
                        }

                        // 降低灵敏度
                        timer_SaleFuel.Interval = 4000;

                        #endregion
                        break;

                    case eFlowFlag.等待稳定:
                        #region

                        // 提高灵敏度
                        timer_SaleFuel.Interval = 1000;

                        btnSaveTransport_SaleFuel.Enabled = this.WbSteady;

                        UpdateLedShow(this.CurrentAutotruck.CarNumber, Hardwarer.Wber.Weight.ToString("#0.######"));

                        if (this.WbSteady)
                        {
                            if (this.AutoHandMode)
                            {
                                // 自动模式
                                if (!SaveSaleFuelTransport())
                                {
                                    UpdateLedShow(this.CurrentAutotruck.CarNumber, "称重失败");
                                    this.voiceSpeaker.Speak("车牌号 " + this.CurrentAutotruck.CarNumber + " 称重失败，请联系管理员", 1, false);
                                }
                            }
                            else
                            {
                                // 手动模式 
                            }
                        }

                        #endregion
                        break;

                    case eFlowFlag.等待离开:
                        #region

                        // 当前地磅重量小于最小称重且所有地感、对射无信号时重置
                        if (Hardwarer.Wber.Weight < this.WbMinWeight && !HasCarOnLeaveWay()) ResetSaleFuel();

                        // 降低灵敏度
                        timer_SaleFuel.Interval = 4000;

                        #endregion
                        break;
                }

                // 当前地磅重量小于最小称重且所有地感、对射无信号时重置
                if (Hardwarer.Wber.Weight < this.WbMinWeight && !HasCarOnEnterWay() && !HasCarOnLeaveWay() && this.CurrentFlowFlag != eFlowFlag.等待车辆
                    && this.CurrentImperfectCar != null) ResetSaleFuel();
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

        /// <summary>
        /// 获取未完成的销售记录
        /// </summary>
        void LoadTodayUnFinishSaleFuelTransport()
        {
            superGridControl1_SaleFuel.PrimaryGrid.DataSource = weighterDAO.GetUnFinishSaleFuelTransport();
        }

        /// <summary>
        /// 获取指定日期已完成的销售记录
        /// </summary>
        void LoadTodayFinishSaleFuelTransport()
        {
            superGridControl2_SaleFuel.PrimaryGrid.DataSource = weighterDAO.GetFinishedSaleFuelTransport(DateTime.Now.Date, DateTime.Now.Date.AddDays(1));
        }
        #endregion

        #region 其他物资业务

        bool timer_Goods_Cancel = true;

        CmcsGoodsTransport currentGoodsTransport;
        /// <summary>
        /// 当前运输记录
        /// </summary>
        public CmcsGoodsTransport CurrentGoodsTransport
        {
            get { return currentGoodsTransport; }
            set
            {
                currentGoodsTransport = value;

                if (value != null)
                {
                    commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.当前运输记录Id.ToString(), value.Id);

                    txtSupplyUnitName_Goods.Text = value.SupplyUnitName;
                    txtReceiveUnitName_Goods.Text = value.ReceiveUnitName;
                    txtGoodsTypeName_Goods.Text = value.GoodsTypeName;

                    txtFirstWeight_Goods.Text = value.FirstWeight.ToString("F2");
                    txtSecondWeight_Goods.Text = value.SecondWeight.ToString("F2");
                    txtSuttleWeight_Goods.Text = value.SuttleWeight.ToString("F2");
                }
                else
                {
                    commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.当前运输记录Id.ToString(), string.Empty);

                    txtSupplyUnitName_Goods.ResetText();
                    txtReceiveUnitName_Goods.ResetText();
                    txtGoodsTypeName_Goods.ResetText();

                    txtFirstWeight_Goods.ResetText();
                    txtSecondWeight_Goods.ResetText();
                    txtSuttleWeight_Goods.ResetText();
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
            FrmUnFinishTransport_Select frm = new FrmUnFinishTransport_Select("where CarType='" + eCarType.其他物资.ToString() + "' order by CreateDate desc");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (this.InductorCoil1)
                    passCarQueuer.Enqueue(eDirection.Way1, frm.Output.CarNumber);
                else if (this.InductorCoil4)
                    passCarQueuer.Enqueue(eDirection.Way2, frm.Output.CarNumber);
                else
                    passCarQueuer.Enqueue(eDirection.UnKnow, frm.Output.CarNumber);

                this.CurrentFlowFlag = eFlowFlag.识别车辆;
            }
        }

        /// <summary>
        /// 保存排队记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveTransport_Goods_Click(object sender, EventArgs e)
        {
            if (!SaveGoodsTransport()) MessageBoxEx.Show("保存失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 保存运输记录
        /// </summary>
        /// <returns></returns>
        bool SaveGoodsTransport()
        {
            if (this.CurrentGoodsTransport == null) return false;

            try
            {
                if (weighterDAO.SaveGoodsTransport(this.CurrentGoodsTransport.Id, (decimal)Hardwarer.Wber.Weight, DateTime.Now, CommonAppConfig.GetInstance().AppIdentifier))
                {
                    this.CurrentGoodsTransport = commonDAO.SelfDber.Get<CmcsGoodsTransport>(this.CurrentGoodsTransport.Id);

                    FrontGateUp();

                    btnSaveTransport_Goods.Enabled = false;
                    this.CurrentFlowFlag = eFlowFlag.等待离开;

                    UpdateLedShow("称重完毕", "请下磅");
                    this.voiceSpeaker.Speak("称重完毕请下磅", 1, false);

                    LoadTodayUnFinishGoodsTransport();
                    LoadTodayFinishGoodsTransport();

                    CamareCapturePicture(this.CurrentGoodsTransport.Id);

                    //打印磅单 
                    if (this.AutoPrint && this.CurrentGoodsTransport.SuttleWeight > 0)
                    {
                        ReportPrint.GetInstance().PrintGoodsTransport(this.CurrentGoodsTransport);
                    }

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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Goods_Click(object sender, EventArgs e)
        {
            ResetGoods();
        }

        /// <summary>
        /// 重置信息
        /// </summary>
        void ResetGoods()
        {
            this.timer_Goods_Cancel = true;

            this.CurrentFlowFlag = eFlowFlag.等待车辆;

            this.CurrentAutotruck = null;
            this.CurrentGoodsTransport = null;
            this.CurrentDirection = eDirection.UnKnow;

            txtTagId_Goods.ResetText();

            btnSaveTransport_Goods.Enabled = false;

            FrontGateDown();
            BackGateDown();

            UpdateLedShow("  等待车辆");

            // 最后重置
            this.CurrentImperfectCar = null;
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
                    case eFlowFlag.验证信息:
                        #region

                        // 查找该车未完成的运输记录
                        CmcsUnFinishTransport unFinishTransport = carTransportDAO.GetUnFinishTransportByAutotruckId(this.CurrentAutotruck.Id, eCarType.其他物资.ToString());
                        if (unFinishTransport != null)
                        {
                            this.CurrentGoodsTransport = commonDAO.SelfDber.Get<CmcsGoodsTransport>(unFinishTransport.TransportId);
                            if (this.CurrentGoodsTransport != null)
                            {
                                // 判断路线设置
                                string nextPlace;
                                if (carTransportDAO.CheckNextTruckInFactoryWay(this.CurrentAutotruck.CarType, this.CurrentGoodsTransport.StepName, "第一次称重|第二次称重", CommonAppConfig.GetInstance().AppIdentifier, out nextPlace))
                                {
                                    if (this.CurrentGoodsTransport.SuttleWeight == 0)
                                    {
                                        BackGateUp();

                                        this.CurrentFlowFlag = eFlowFlag.等待上磅;

                                        UpdateLedShow(this.CurrentAutotruck.CarNumber, "请上磅");
                                        this.voiceSpeaker.Speak("车牌号 " + this.CurrentAutotruck.CarNumber + " 请上磅", 1, false);
                                    }
                                    else
                                    {
                                        UpdateLedShow(this.CurrentAutotruck.CarNumber, "已称重");
                                        this.voiceSpeaker.Speak("车牌号 " + this.CurrentAutotruck.CarNumber + " 已称重", 1, false);

                                        timer_Goods.Interval = 20000;
                                    }
                                }
                                else
                                {
                                    UpdateLedShow("路线错误", "禁止通过");
                                    this.voiceSpeaker.Speak("路线错误 禁止通过 " + (!string.IsNullOrEmpty(nextPlace) ? "请前往" + nextPlace : ""), 1, false);

                                    timer_Goods.Interval = 20000;
                                }
                            }
                            else
                            {
                                commonDAO.SelfDber.Delete<CmcsUnFinishTransport>(unFinishTransport.Id);
                            }
                        }
                        else
                        {
                            UpdateLedShow(this.CurrentAutotruck.CarNumber, "未排队");
                            this.voiceSpeaker.Speak("车牌号 " + this.CurrentAutotruck.CarNumber + " 未找到排队记录", 1, false);

                            timer_Goods.Interval = 20000;
                        }

                        #endregion
                        break;

                    case eFlowFlag.等待上磅:
                        #region

                        // 当地磅仪表重量大于最小称重且来车方向的地感与对射均无信号，则判定车已经完全上磅
                        if (Hardwarer.Wber.Weight >= this.WbMinWeight && !HasCarOnEnterWay())
                        {
                            BackGateDown();

                            this.CurrentFlowFlag = eFlowFlag.等待稳定;
                        }

                        // 降低灵敏度
                        timer_Goods.Interval = 4000;

                        #endregion
                        break;

                    case eFlowFlag.等待稳定:
                        #region

                        // 提高灵敏度
                        timer_Goods.Interval = 1000;

                        btnSaveTransport_Goods.Enabled = this.WbSteady;

                        UpdateLedShow(this.CurrentAutotruck.CarNumber, Hardwarer.Wber.Weight.ToString("#0.######"));

                        if (this.WbSteady)
                        {
                            if (this.AutoHandMode)
                            {
                                // 自动模式
                                if (!SaveGoodsTransport())
                                {
                                    UpdateLedShow(this.CurrentAutotruck.CarNumber, "称重失败");
                                    this.voiceSpeaker.Speak("车牌号 " + this.CurrentAutotruck.CarNumber + " 称重失败，请联系管理员", 1, false);
                                }
                            }
                            else
                            {
                                // 手动模式 
                            }
                        }

                        #endregion
                        break;

                    case eFlowFlag.等待离开:
                        #region

                        // 当前地磅重量小于最小称重且所有地感、对射无信号时重置
                        if (Hardwarer.Wber.Weight < this.WbMinWeight && !HasCarOnLeaveWay()) ResetGoods();

                        // 降低灵敏度
                        timer_Goods.Interval = 4000;

                        #endregion
                        break;
                }

                // 当前地磅重量小于最小称重且所有地感、对射无信号时重置
                if (Hardwarer.Wber.Weight < this.WbMinWeight && !HasCarOnEnterWay() && !HasCarOnLeaveWay() && this.CurrentFlowFlag != eFlowFlag.等待车辆
                    && this.CurrentImperfectCar != null) ResetGoods();
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
            superGridControl1_Goods.PrimaryGrid.DataSource = weighterDAO.GetUnFinishGoodsTransport();
        }

        /// <summary>
        /// 获取指定日期已完成的其他物资记录
        /// </summary>
        void LoadTodayFinishGoodsTransport()
        {
            superGridControl2_Goods.PrimaryGrid.DataSource = weighterDAO.GetFinishedGoodsTransport(DateTime.Now.Date, DateTime.Now.Date.AddDays(1));
        }

        #endregion

        #region 其他函数

        Font directionFont = new Font("微软雅黑", 16);

        Pen redPen1 = new Pen(Color.Red, 1);
        Pen greenPen1 = new Pen(Color.Lime, 1);
        Pen redPen3 = new Pen(Color.Red, 3);
        Pen greenPen3 = new Pen(Color.Lime, 3);

        /// <summary>
        /// 当前仪表重量面板绘制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panCurrentWeight_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                PanelEx panel = sender as PanelEx;

                int height = 12;

                // 绘制地感1
                e.Graphics.DrawLine(this.InductorCoil1 ? redPen3 : greenPen3, 15, 1, 15, height);
                e.Graphics.DrawLine(this.InductorCoil1 ? redPen3 : greenPen3, 15, panel.Height - height, 15, panel.Height - 1);
                // 绘制地感2
                e.Graphics.DrawLine(this.InductorCoil2 ? redPen3 : greenPen3, 25, 1, 25, height);
                e.Graphics.DrawLine(this.InductorCoil2 ? redPen3 : greenPen3, 25, panel.Height - height, 25, panel.Height - 1);
                // 绘制对射1
                e.Graphics.DrawLine(this.InfraredSensor1 ? redPen1 : greenPen1, 35, 1, 35, height);
                e.Graphics.DrawLine(this.InfraredSensor1 ? redPen1 : greenPen1, 35, panel.Height - height, 35, panel.Height - 1);

                // 绘制对射2
                e.Graphics.DrawLine(this.InfraredSensor2 ? redPen1 : greenPen1, panel.Width - 35, 1, panel.Width - 35, height);
                e.Graphics.DrawLine(this.InfraredSensor2 ? redPen1 : greenPen1, panel.Width - 35, panel.Height - height, panel.Width - 35, panel.Height - 1);
                // 绘制地感3
                e.Graphics.DrawLine(this.InductorCoil3 ? redPen3 : greenPen3, panel.Width - 25, 1, panel.Width - 25, height);
                e.Graphics.DrawLine(this.InductorCoil3 ? redPen3 : greenPen3, panel.Width - 25, panel.Height - height, panel.Width - 25, panel.Height - 1);
                // 绘制地感4
                e.Graphics.DrawLine(this.InductorCoil4 ? redPen3 : greenPen3, panel.Width - 15, 1, panel.Width - 15, height);
                e.Graphics.DrawLine(this.InductorCoil4 ? redPen3 : greenPen3, panel.Width - 15, panel.Height - height, panel.Width - 15, panel.Height - 1);

                // 上磅方向
                eDirection direction = this.CurrentDirection;
                e.Graphics.DrawString("﹥>", directionFont, direction == eDirection.Way1 ? Brushes.Red : Brushes.Lime, 2, 17);
                e.Graphics.DrawString("<﹤", directionFont, direction == eDirection.Way2 ? Brushes.Red : Brushes.Lime, panel.Width - 47, 17);
            }
            catch (Exception ex)
            {
                Log4Neter.Error("panCurrentCarNumber_Paint异常", ex);
            }
        }

        private void superGridControl_BeginEdit(object sender, DevComponents.DotNetBar.SuperGrid.GridEditEventArgs e)
        {
            // 取消进入编辑
            e.Cancel = true;
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

        //设置是否自动打印磅单
        private void chkAutoPrint_CheckedChanged(object sender, EventArgs e)
        {
            commonDAO.SetCommonAppletConfig("自动打印磅单", chkAutoPrint.Checked ? "1" : "0");
        }

        #endregion

    }
}
