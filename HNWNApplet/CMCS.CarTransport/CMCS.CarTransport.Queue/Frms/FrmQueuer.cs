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
        /// ����Ψһ��ʶ��
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
        /// ��������
        /// </summary>
        VoiceSpeaker voiceSpeaker = new VoiceSpeaker();

        bool inductorCoil1 = false;
        /// <summary>
        /// �ظ�1״̬ true=���ź�  false=���ź�
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

                commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.�ظ�1�ź�.ToString(), value ? "1" : "0");
            }
        }

        int inductorCoil1Port;
        /// <summary>
        /// �ظ�1�˿�
        /// </summary>
        public int InductorCoil1Port
        {
            get { return inductorCoil1Port; }
            set { inductorCoil1Port = value; }
        }

        bool inductorCoil2 = false;
        /// <summary>
        /// �ظ�2״̬ true=���ź�  false=���ź�
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

                commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.�ظ�2�ź�.ToString(), value ? "1" : "0");
            }
        }

        int inductorCoil2Port;
        /// <summary>
        /// �ظ�2�˿�
        /// </summary>
        public int InductorCoil2Port
        {
            get { return inductorCoil2Port; }
            set { inductorCoil2Port = value; }
        }

        bool inductorCoil3 = false;
        /// <summary>
        /// �ظ�3״̬ true=���ź�  false=���ź�
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

                commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.�ظ�3�ź�.ToString(), value ? "1" : "0");
            }
        }

        int inductorCoil3Port;
        /// <summary>
        /// �ظ�3�˿�
        /// </summary>
        public int InductorCoil3Port
        {
            get { return inductorCoil3Port; }
            set { inductorCoil3Port = value; }
        }

        bool inductorCoil4 = false;
        /// <summary>
        /// �ظ�4״̬ true=���ź�  false=���ź�
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

                commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.�ظ�4�ź�.ToString(), value ? "1" : "0");
            }
        }

        int inductorCoil4Port;
        /// <summary>
        /// �ظ�4�˿�
        /// </summary>
        public int InductorCoil4Port
        {
            get { return inductorCoil4Port; }
            set { inductorCoil4Port = value; }
        }

        bool autoHandMode = true;
        /// <summary>
        /// �Զ�ģʽ=true  �ֶ�ģʽ=false
        /// </summary>
        public bool AutoHandMode
        {
            get { return autoHandMode; }
            set
            {
                autoHandMode = value;

                #region �볧ú

                btnSelectAutotruck_BuyFuel.Visible = !value;
                btnSelectSupplier_BuyFuel.Visible = !value;
                btnSelectMine_BuyFuel.Visible = !value;
                btnSelectTransportCompany_BuyFuel.Visible = !value;

                #endregion

                #region ��������

                btnSelectAutotruck_Goods.Visible = !value;
                btnbtnSelectSupply_Goods.Visible = !value;
                btnSelectReceive_Goods.Visible = !value;
                btnSelectGoodsType_Goods.Visible = !value;

                #endregion
            }
        }

        #region �볧

        public static PassCarQueuer passCarQueuer = new PassCarQueuer();

        ImperfectCar currentImperfectCar;
        /// <summary>
        /// ʶ���ѡ��ĳ���ƾ֤
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
                    panCurrentCarNumber.Text = "�ȴ�����";
            }
        }

        eFlowFlag currentFlowFlag = eFlowFlag.�ȴ�����;
        /// <summary>
        /// ��ǰҵ�����̱�ʶ
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
        /// ��ǰ��
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
                    commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.��ǰ��Id.ToString(), value.Id);
                    commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.��ǰ����.ToString(), value.CarNumber);

                    CmcsEPCCard ePCCard = Dbers.GetInstance().SelfDber.Get<CmcsEPCCard>(value.EPCCardId);
                    if (value.CarType == eCarType.�볧ú.ToString())
                    {
                        if (ePCCard != null) txtTagId_BuyFuel.Text = ePCCard.TagId;

                        txtCarNumber_BuyFuel.Text = value.CarNumber;
                        txtTagId_BuyFuel.Text = value.EPCCardId;

                        superTabControlMain.SelectedTab = superTabItem_BuyFuel;
                    }
                    else if (value.CarType == eCarType.����ú.ToString())
                    {
                        if (ePCCard != null) txtTagId_SaleFuel.Text = ePCCard.TagId;

                        txtCarNumber_SaleFuel.Text = value.CarNumber;
                        superTabControlMain.SelectedTab = superTabItem_SaleFuel;
                    }
                    else if (value.CarType == eCarType.��������.ToString())
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
                    commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.��ǰ��Id.ToString(), string.Empty);
                    commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.��ǰ����.ToString(), string.Empty);

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
        /// ��ǰ���˼ƻ���Ϣ
        /// </summary>
        public ZkBizResult CurrentZkBizResult { get; set; }

        /// <summary>
        /// ��ǰȫ����������úԤ������Ϣ
        /// </summary>
        public View_rlgl_cygl_qclmygp CurrentQCLMYGP { get; set; }

        /// <summary>
        /// ��ǰ���������
        /// </summary>
        public string CurrentSampler { get; set; }

        /// <summary>
        /// �Ƿ�����֤������
        /// </summary>
        bool IsPaperWorkPass = false;

        /// <summary>
        /// ����ͨ������
        /// </summary>
        int SampleWayCount = 0;

        /// <summary>
        /// ���ò���ͨ������
        /// </summary>
        bool isSampleWayCount = false;

        /// <summary>
        /// �����ܳ���
        /// </summary>
        int FactoryCount = 0;

        /// <summary>
        /// ���ó����ܳ���
        /// </summary>
        bool isFactoryCount = false;

        #endregion

        #region ����

        public static PassCarQueuer passCarQueuerOut = new PassCarQueuer();

        ImperfectCar currentImperfectCarOut;
        /// <summary>
        /// ʶ���ѡ��ĳ���ƾ֤
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
                    panCurrentOutCarNumber.Text = "�ȴ�����";
            }
        }

        eFlowFlag currentFlowFlagOut = eFlowFlag.�ȴ�����;
        /// <summary>
        /// ��ǰҵ�����̱�ʶ
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
        /// ��ǰ��
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
        /// ��ǰ�볧ú�����¼
        /// </summary>
        public CmcsBuyFuelTransport CurrentBuyFuelTransportOut
        {
            get { return currentBuyFuelTransportOut; }
            set { currentBuyFuelTransportOut = value; }
        }

        CmcsGoodsTransport currentGoodsTransportOut;
        /// <summary>
        /// ��ǰ�������������¼
        /// </summary>
        public CmcsGoodsTransport CurrentGoodsTransportOut
        {
            get { return currentGoodsTransportOut; }
            set { currentGoodsTransportOut = value; }
        }

        #endregion

        #endregion

        /// <summary>
        /// �����ʼ��
        /// </summary>
        private void InitForm()
        {
            FrmDebugConsole.GetInstance();

            //�п�ӿڵ�ַ
            commService = new CommService(commonDAO.GetCommonAppletConfigString("�пؽӿ����ӵ�ַ"));

            //��ʼ����Ƶ
            CameraSDK.InitSDK(commonDAO.GetCommonAppletConfigString("����ƽ̨��ַ"), commonDAO.GetCommonAppletConfigInt32("����Э��˿ں�"), commonDAO.GetCommonAppletConfigString("����Appkey"), commonDAO.GetCommonAppletConfigString("����Secret"));

            // Ĭ���Զ�
            sbtnChangeAutoHandMode.Value = true;

            // ���ó���Զ�̿�������
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
            // ж���豸
            UnloadHardware();
        }

        #region �豸���

        #region IO������

        void Iocer_StatusChange(bool status)
        {
            // ����IO������״̬ 
            InvokeEx(() =>
            {
                slightIOC.LightColor = (status ? Color.Green : Color.Red);

                commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.IO������_����״̬.ToString(), status ? "1" : "0");
            });
        }

        /// <summary>
        /// IO��������������ʱ����
        /// </summary>
        /// <param name="receiveValue"></param>
        void Iocer_Received(int[] receiveValue)
        {
            // ���յظ�״̬  
            InvokeEx(() =>
            {
                this.InductorCoil1 = (receiveValue[this.InductorCoil1Port - 1] == 1);
                this.InductorCoil2 = (receiveValue[this.InductorCoil2Port - 1] == 1);
                this.InductorCoil3 = (receiveValue[this.InductorCoil3Port - 1] == 1);
                this.InductorCoil4 = (receiveValue[this.InductorCoil4Port - 1] == 1);
            });
        }

        /// <summary>
        /// ����ͨ��
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
        /// ���ǰ��
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

        #region �볧������

        void Rwer1_OnScanError(Exception ex)
        {
            Log4Neter.Error("�볧������", ex);
        }

        void Rwer1_OnStatusChange(bool status)
        {
            // ���ն�����1״̬ 
            InvokeEx(() =>
             {
                 slightRwer1.LightColor = (status ? Color.Green : Color.Red);

                 commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.������1_����״̬.ToString(), status ? "1" : "0");
             });
        }

        #endregion

        #region ����������

        void RwerOut_OnScanError(Exception ex)
        {
            Log4Neter.Error("����������", ex);
        }

        void RwerOut_OnStatusChange(bool status)
        {
            // ���ն�����1״̬ 
            InvokeEx(() =>
            {
                slightRwer2.LightColor = (status ? Color.Green : Color.Red);

                commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.������2_����״̬.ToString(), status ? "1" : "0");
            });
        }

        #endregion

        #region LED��ʾ��

        /// <summary>
        /// ����12�ֽڵ��ı�����
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
        /// ����LED��̬����
        /// </summary>
        /// <param name="value1">��һ������</param>
        /// <param name="value2">�ڶ�������</param>
        private void UpdateLedShow(string value1 = "", string value2 = "")
        {
            if (this.CurrentImperfectCar == null) return;

            UpdateLed1Show(value1, value2);
        }

        /// <summary>
        /// ��ʾ���Դ�����Ϣ
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        private void UpdateShowDebug(string value1 = "", string value2 = "")
        {
            FrmDebugConsole.GetInstance().Output("������Ϣ:|" + value1 + "|" + value2 + "|");
        }

        #region LED1���ƿ�

        /// <summary>
        /// LED1���ƿ�����
        /// </summary>
        int LED1nScreenNo = 1;
        /// <summary>
        /// LED1��̬�����
        /// </summary>
        int LED1DYArea_ID = 1;
        /// <summary>
        /// LED1���±�ʶ
        /// </summary>
        bool LED1m_bSendBusy = false;

        private bool _LED1ConnectStatus;
        /// <summary>
        /// LED1����״̬
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

                commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.LED��1_����״̬.ToString(), value ? "1" : "0");
            }
        }

        /// <summary>
        /// LED1��ʾ�����ı�
        /// </summary>
        string LED1TempFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Led1TempFile.txt");

        /// <summary>
        /// LED1��һ����ʾ����
        /// </summary>
        string LED1PrevLedFileContent = string.Empty;

        /// <summary>
        /// ����LED1��̬����
        /// </summary>
        /// <param name="value1">��һ������</param>
        /// <param name="value2">�ڶ�������</param>
        private void UpdateLed1Show(string value1 = "", string value2 = "")
        {
            FrmDebugConsole.GetInstance().Output("����LED1:|" + value1 + "|" + value2 + "|");

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
            Log4Neter.Error("LED��1", ex);
        }

        #endregion

        #region �豸��ʼ����ж��

        /// <summary>
        /// ��ʼ������豸
        /// </summary>
        private void InitHardware()
        {
            try
            {
                bool success = false;

                this.InductorCoil1Port = commonDAO.GetAppletConfigInt32("IO������_�ظ�1�˿�");
                this.InductorCoil2Port = commonDAO.GetAppletConfigInt32("IO������_�ظ�2�˿�");
                this.InductorCoil3Port = commonDAO.GetAppletConfigInt32("IO������_�ظ�3�˿�");
                this.InductorCoil4Port = commonDAO.GetAppletConfigInt32("IO������_�ظ�4�˿�");

                //��������
                this.IsPaperWorkPass = commonDAO.GetCommonAppletConfigString("����֤������") == "1";
                this.SampleWayCount = commonDAO.GetCommonAppletConfigInt32("����ͨ������");
                this.isSampleWayCount = (commonDAO.GetCommonAppletConfigString("���ò���ͨ������") == "1");
                this.FactoryCount = commonDAO.GetCommonAppletConfigInt32("�����ܳ���");
                this.isFactoryCount = (commonDAO.GetCommonAppletConfigString("���ó����ܳ���") == "1");

                // IO������
                //Hardwarer.Iocer.OnReceived += new IOC.JMDM20DIOV2.JMDM20DIOV2Iocer.ReceivedEventHandler(Iocer_Received);
                //Hardwarer.Iocer.OnStatusChange += new IOC.JMDM20DIOV2.JMDM20DIOV2Iocer.StatusChangeHandler(Iocer_StatusChange);
                success = Hardwarer.Iocer.OpenCom(commonDAO.GetAppletConfigInt32("IO������_����"), commonDAO.GetAppletConfigInt32("IO������_������"), commonDAO.GetAppletConfigInt32("IO������_����λ"), (StopBits)commonDAO.GetAppletConfigInt32("IO������_ֹͣλ"), (Parity)commonDAO.GetAppletConfigInt32("IO������_У��λ"));
                if (!success) MessageBoxEx.Show("IO����������ʧ�ܣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.iocControler = new IocControler(Hardwarer.Iocer);

                // ������1
                Hardwarer.Rwer1.StartWith = commonDAO.GetAppletConfigString("������_��ǩ����");
                Hardwarer.Rwer1.OnStatusChange += new RW.LZR12.Net.Lzr12Rwer.StatusChangeHandler(Rwer1_OnStatusChange);
                Hardwarer.Rwer1.OnScanError += new RW.LZR12.Net.Lzr12Rwer.ScanErrorEventHandler(Rwer1_OnScanError);
                success = Hardwarer.Rwer1.OpenCom(commonDAO.GetAppletConfigString("������1_Ip"), commonDAO.GetAppletConfigInt32("������1_�˿�"));
                if (!success) MessageBoxEx.Show("�볧����������ʧ�ܣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Hardwarer.RwerOut.StartWith = commonDAO.GetAppletConfigString("������_��ǩ����");
                Hardwarer.RwerOut.OnStatusChange += new RW.LZR12.Net.Lzr12Rwer.StatusChangeHandler(RwerOut_OnStatusChange);
                Hardwarer.RwerOut.OnScanError += new RW.LZR12.Net.Lzr12Rwer.ScanErrorEventHandler(RwerOut_OnScanError);
                success = Hardwarer.RwerOut.OpenCom(commonDAO.GetAppletConfigString("������2_Ip"), commonDAO.GetAppletConfigInt32("������2_�˿�"));
                if (!success) MessageBoxEx.Show("��������������ʧ�ܣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                #region LED���ƿ�1

                string led1SocketIP = commonDAO.GetAppletConfigString("LED��ʾ��1_IP��ַ");
                if (!string.IsNullOrEmpty(led1SocketIP))
                {
                    if (CommonUtil.PingReplyTest(led1SocketIP))
                    {
                        Hardwarer.Led1.OnScanError += new YBDynamicLeder.ScanErrorEventHandler(Led1_OnScanError);
                        if (Hardwarer.Led1.OpenLED(led1SocketIP))
                        {
                            // ��ʼ���ɹ�
                            this.LED1ConnectStatus = true;
                            UpdateLed1Show("  �ȴ�����");
                        }
                        else
                        {
                            this.LED1ConnectStatus = false;
                            Log4Neter.Error("��ʼ��LED1���ƿ�ʧ��", new Exception("�����豸"));
                            MessageBoxEx.Show("LED1���ƿ�����ʧ�ܣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        this.LED1ConnectStatus = false;
                        Log4Neter.Error("��ʼ��LED1���ƿ�����������ʧ��", new Exception("�����쳣"));
                        MessageBoxEx.Show("LED1���ƿ���������ʧ�ܣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.LED��1_����״̬.ToString(), this.LED1ConnectStatus ? "1" : "0");

                #endregion

                //��������
                voiceSpeaker.SetVoice(commonDAO.GetAppletConfigInt32("����"), commonDAO.GetAppletConfigInt32("����"), commonDAO.GetAppletConfigString("������"));

                //�볧ҵ��
                timer1.Enabled = true;

                //����ҵ��
                timer_Out.Enabled = true;
            }
            catch (Exception ex)
            {
                Log4Neter.Error("�豸��ʼ��", ex);
            }
        }

        /// <summary>
        /// ж���豸
        /// </summary>
        private void UnloadHardware()
        {
            // ע��˶δ���
            Application.DoEvents();

            try
            {
                commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.��ǰ��Id.ToString(), string.Empty);
                commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.��ǰ����.ToString(), string.Empty);
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

        #region ��բ���ư�ť

        /// <summary>
        /// ��բ1����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGate1Up_Click(object sender, EventArgs e)
        {
            if (this.iocControler != null) this.iocControler.Gate1Up();
        }

        /// <summary>
        /// ��բ1����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGate1Down_Click(object sender, EventArgs e)
        {
            if (this.iocControler != null) this.iocControler.Gate1Down();
        }

        /// <summary>
        /// ��բ2����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGate2Up_Click(object sender, EventArgs e)
        {
            if (this.iocControler != null) this.iocControler.Gate2Up();
        }

        /// <summary>
        /// ��բ2����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGate2Down_Click(object sender, EventArgs e)
        {
            if (this.iocControler != null) this.iocControler.Gate2Down();
        }

        #endregion

        #region ����ҵ��

        /// <summary>
        /// ����������ʶ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            timer1.Interval = 2000;

            try
            {
                // ִ��Զ������
                ExecAppRemoteControlCmd();

                switch (this.CurrentFlowFlag)
                {
                    case eFlowFlag.�ȴ�����:
                        #region

                        //���������
                        timer1.Interval = 1000;

                        // PassWay.Way1
                        if (Hardwarer.Rwer1.Status)
                        {
                            // ����������ظ����źţ������������߳���ʶ��

                            List<string> tags = Hardwarer.Rwer1.ScanTags();
                            if (tags.Count > 0) passCarQueuer.Enqueue(ePassWay.Way1, tags[0], true);
                        }
                        // PassWay.Way2
                        else if (Hardwarer.Rwer2.Status)
                        {
                            // ����������ظ����źţ������������߳���ʶ��

                            List<string> tags = Hardwarer.Rwer2.ScanTags();
                            if (tags.Count > 0) passCarQueuer.Enqueue(ePassWay.Way2, tags[0], true);
                        }

                        if (passCarQueuer.Count > 0) this.CurrentFlowFlag = eFlowFlag.��֤����;

                        #endregion
                        break;

                    case eFlowFlag.��֤����:
                        #region

                        // �������޳�ʱ���ȴ�����
                        if (passCarQueuer.Count == 0)
                        {
                            this.CurrentFlowFlag = eFlowFlag.�ȴ�����;
                            break;
                        }

                        this.CurrentImperfectCar = passCarQueuer.Dequeue();

                        // ��ʽһ������ʶ��ĳ��ƺŲ��ҳ�����Ϣ
                        this.CurrentAutotruck = carTransportDAO.GetAutotruckByCarNumber(this.CurrentImperfectCar.Voucher);
                        if (this.CurrentAutotruck == null)
                            // ��ʽ��������ʶ��ı�ǩ�����ҳ�����Ϣ
                            this.CurrentAutotruck = carTransportDAO.GetAutotruckByTagId(this.CurrentImperfectCar.Voucher);

                        if (this.CurrentAutotruck != null)
                        {
                            UpdateLedShow(this.CurrentAutotruck.CarNumber);

                            if (this.CurrentAutotruck.IsUse == 1)
                            {
                                //�жϳ������ա���ʻ֤����·��ҵ�ʸ�֤��Чʱ��
                                if (this.IsPaperWorkPass)
                                {
                                    if (this.CurrentAutotruck.BX_EndDate < DateTime.Now)
                                    {
                                        UpdateLedShow(this.CurrentAutotruck.CarNumber, "�����ѵ���");
                                        timer1.Interval = 8000;
                                        this.CurrentFlowFlag = eFlowFlag.�쳣����1;
                                        break;
                                    }
                                    if (this.CurrentAutotruck.Xshzh_EndDate < DateTime.Now)
                                    {
                                        UpdateLedShow(this.CurrentAutotruck.CarNumber, "��ʻ֤�ѵ���");
                                        timer1.Interval = 8000;
                                        this.CurrentFlowFlag = eFlowFlag.�쳣����1;
                                        break;
                                    }
                                    if (this.CurrentAutotruck.Zgzh_EndDate < DateTime.Now)
                                    {
                                        UpdateLedShow(this.CurrentAutotruck.CarNumber, "�ʸ�֤�ѵ���");
                                        timer1.Interval = 8000;
                                        this.CurrentFlowFlag = eFlowFlag.�쳣����1;
                                        break;
                                    }
                                }

                                // �ж��Ƿ����δ���������¼�������������û�ȷ��
                                bool hasUnFinish = false;

                                CmcsUnFinishTransport unFinishTransport = carTransportDAO.GetUnFinishTransportByAutotruckId(this.CurrentAutotruck.Id, this.CurrentAutotruck.CarType);
                                if (unFinishTransport != null)
                                {
                                    UpdateLedShow(this.CurrentAutotruck.CarNumber, "�ѽ���");
                                    timer1.Interval = 8000;
                                    this.CurrentFlowFlag = eFlowFlag.�쳣����1;
                                    break;
                                }

                                if (!hasUnFinish)
                                {
                                    if (this.CurrentAutotruck.CarType == eCarType.�볧ú.ToString())
                                    {
                                        CmcsBuyFuelTransport cmcsBuyFuelTransport = commonDAO.SelfDber.Entity<CmcsBuyFuelTransport>("where CarNumber=:CarNumber order by TareTime desc", new { CarNumber = this.CurrentAutotruck.CarNumber });
                                        if (cmcsBuyFuelTransport != null && cmcsBuyFuelTransport.TareTime.AddHours(1) > DateTime.Now)
                                        {
                                            //��ֹ�����������볧������ʶ��
                                            UpdateLedShow(this.CurrentAutotruck.CarNumber, "������ʶ��");
                                            timer1.Interval = 4000;
                                            this.CurrentFlowFlag = eFlowFlag.�쳣����1;
                                            break;
                                        }

                                        this.timer_BuyFuel_Cancel = false;

                                        if (this.AutoHandMode)
                                            this.CurrentFlowFlag = eFlowFlag.ƥ�����;
                                        else
                                            this.CurrentFlowFlag = eFlowFlag.����¼��;
                                    }
                                    else if (this.CurrentAutotruck.CarType == eCarType.����ú.ToString())
                                    {
                                        this.timer_SaleFuel_Cancel = false;
                                        this.CurrentFlowFlag = eFlowFlag.����¼��;
                                    }
                                    else if (this.CurrentAutotruck.CarType == eCarType.��������.ToString())
                                    {
                                        this.timer_Goods_Cancel = false;

                                        if (this.AutoHandMode)
                                            this.CurrentFlowFlag = eFlowFlag.ƥ�����;
                                        else
                                            this.CurrentFlowFlag = eFlowFlag.����¼��;
                                    }
                                }
                            }
                            else
                            {
                                UpdateLedShow(this.CurrentAutotruck.CarNumber, "��ͣ��");
                                this.voiceSpeaker.Speak("���ƺ� " + this.CurrentAutotruck.CarNumber + " ��ͣ�ã���ֹͨ��", 1, false);

                                timer1.Interval = 8000;
                            }
                        }
                        else
                        {
                            UpdateLedShow(this.CurrentImperfectCar.Voucher, "δ�Ǽ�");

                            // ��ʽ����ˢ����ʽ
                            this.voiceSpeaker.Speak("����δ�Ǽǣ���ֹͨ��", 1, false);

                            timer1.Interval = 8000;
                        }

                        #endregion
                        break;

                    case eFlowFlag.�쳣����1:
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
        /// ��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Stop();
            // ������ִ��һ��
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
        /// �豸״̬��ʾ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer3_Tick(object sender, EventArgs e)
        {
            try
            {
                #region ������

                this.InductorCoil1 = (Hardwarer.Iocer.ReceiveValue[this.InductorCoil1Port - 1] == 1);
                this.InductorCoil2 = (Hardwarer.Iocer.ReceiveValue[this.InductorCoil2Port - 1] == 1);
                this.InductorCoil3 = (Hardwarer.Iocer.ReceiveValue[this.InductorCoil3Port - 1] == 1);
                this.InductorCoil4 = (Hardwarer.Iocer.ReceiveValue[this.InductorCoil4Port - 1] == 1);

                if (lastIOStatus != Hardwarer.Iocer.Status)
                {
                    slightIOC.LightColor = (Hardwarer.Iocer.Status ? Color.Green : Color.Red);
                    commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.IO������_����״̬.ToString(), Hardwarer.Iocer.Status ? "1" : "0");
                }

                #endregion

                lastIOStatus = Hardwarer.Iocer.Status;
            }
            catch { }
        }

        /// <summary>
        /// �г����ڵ�ǰ��·��
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
        /// ����ú��
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
        /// ���ز�����ʽ
        /// </summary>
        void LoadSampleType(ComboBoxEx comboBoxEx)
        {
            comboBoxEx.DisplayMember = "Content";
            comboBoxEx.ValueMember = "Code";
            comboBoxEx.DataSource = commonDAO.GetCodeContentByKind("������ʽ");

            comboBoxEx.Text = eSamplingType.��е����.ToString();
        }

        /// <summary>
        /// ִ��Զ������
        /// </summary>
        void ExecAppRemoteControlCmd()
        {
            // ��ȡ���µ�����
            CmcsAppRemoteControlCmd appRemoteControlCmd = commonDAO.GetNewestAppRemoteControlCmd(CommonAppConfig.GetInstance().AppIdentifier);
            if (appRemoteControlCmd != null)
            {
                if (appRemoteControlCmd.CmdCode == "���Ƶ�բ")
                {
                    Log4Neter.Info("����Զ�����" + appRemoteControlCmd.CmdCode + "��������" + appRemoteControlCmd.Param);

                    if (appRemoteControlCmd.Param.Equals("Gate1Up", StringComparison.CurrentCultureIgnoreCase))
                        this.iocControler.Gate1Up();
                    else if (appRemoteControlCmd.Param.Equals("Gate1Down", StringComparison.CurrentCultureIgnoreCase))
                        this.iocControler.Gate1Down();
                    else if (appRemoteControlCmd.Param.Equals("Gate2Up", StringComparison.CurrentCultureIgnoreCase))
                        this.iocControler.Gate2Up();
                    else if (appRemoteControlCmd.Param.Equals("Gate2Down", StringComparison.CurrentCultureIgnoreCase))
                        this.iocControler.Gate2Down();

                    // ����ִ�н��
                    commonDAO.SetAppRemoteControlCmdResultCode(appRemoteControlCmd, eEquInfCmdResultCode.�ɹ�);
                }
            }
        }

        /// <summary>
        /// ˢ���б�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // �볧ú
            LoadTodayUnFinishBuyFuelTransport();
            LoadTodayFinishBuyFuelTransport();

            // ����ú 
            LoadTodayUnFinishSaleFuelTransport();
            LoadTodayFinishSaleFuelTransport();

            // ��������
            LoadTodayUnFinishGoodsTransport();
            LoadTodayFinishGoodsTransport();
        }

        private void sbtnChangeAutoHandMode_ValueChanged(object sender, EventArgs e)
        {
            this.AutoHandMode = sbtnChangeAutoHandMode.Value;
        }

        #endregion

        #region �볧úҵ��

        bool timer_BuyFuel_Cancel = true;

        private CmcsSupplier selectedSupplier_BuyFuel;
        /// <summary>
        /// ѡ��Ĺ�ú��λ
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
        /// ѡ������䵥λ
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
        /// ѡ��Ŀ��
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
        /// ѡ���ú��
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
        /// ѡ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectAutotruck_BuyFuel_Click(object sender, EventArgs e)
        {
            FrmAutotruck_Select frm = new FrmAutotruck_Select("and CarType='" + eCarType.�볧ú.ToString() + "' and IsUse=1 order by CarNumber asc");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                passCarQueuer.Enqueue(ePassWay.UnKnow, frm.Output.CarNumber, false);
                this.CurrentFlowFlag = eFlowFlag.��֤����;
            }
        }

        /// <summary>
        /// ѡ��ú��λ
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
        /// ѡ����
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
        /// ѡ�����䵥λ
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
        /// ѡ��ú��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbFuelName_BuyFuel_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SelectedFuelKind_BuyFuel = cmbFuelName_BuyFuel.SelectedItem as CmcsFuelKind;
        }

        /// <summary>
        /// �³��Ǽ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewAutotruck_BuyFuel_Click(object sender, EventArgs e)
        {
            // eCarType.�볧ú

            new FrmAutotruck_Oper(Guid.NewGuid().ToString(), eEditMode.����).Show();
        }

        /// <summary>
        /// ѡ���볧ú��úԤ��
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
        /// �����볧ú�����¼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveTransport_BuyFuel_Click(object sender, EventArgs e)
        {
            SaveBuyFuelTransport();
        }

        /// <summary>
        /// ���������¼
        /// </summary>
        /// <returns></returns>
        bool SaveBuyFuelTransport()
        {
            if (this.CurrentAutotruck == null)
            {
                MessageBoxEx.Show("��ѡ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(this.CurrentAutotruck.EPCCardId))
            {
                MessageBoxEx.Show("������ǩ������Ϊ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (this.SelectedFuelKind_BuyFuel == null)
            {
                MessageBoxEx.Show("��ѡ��ú��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (this.SelectedMine_BuyFuel == null)
            {
                MessageBoxEx.Show("��ѡ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (this.SelectedSupplier_BuyFuel == null)
            {
                MessageBoxEx.Show("��ѡ��ú��λ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (this.SelectedTransportCompany_BuyFuel == null)
            {
                MessageBoxEx.Show("��ѡ�����䵥λ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (txtTicketWeight_BuyFuel.Value <= 0)
            {
                MessageBoxEx.Show("��������Ч�Ŀ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            try
            {
                // �����볧ú�ŶӼ�¼��ͬʱ����������Ϣ�Լ����ƻ���������
                if (queuerDAO.JoinQueueBuyFuelTransport(this.CurrentAutotruck, this.SelectedSupplier_BuyFuel, this.SelectedMine_BuyFuel, this.SelectedTransportCompany_BuyFuel, this.SelectedFuelKind_BuyFuel, (decimal)txtTicketWeight_BuyFuel.Value, DateTime.Now, (this.CurrentQCLMYGP == null ? "" : this.CurrentQCLMYGP.Mpph), this.CurrentSampler, txtRemark_BuyFuel.Text, CommonAppConfig.GetInstance().AppIdentifier))
                {
                    #region �����ܵ��˷�����Ʊ���
                    try { ZkBizResult entity = commService.SetVerifyResult(this.CurrentAutotruck.CarNumber, true, "", "", 1); }
                    catch (Exception ex) { Log4Neter.Error("�����ܵ��˷�����Ʊ���", ex); }
                    #endregion

                    #region ��ȫ���̷�����Ʊ���
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
                    catch (Exception ex) { Log4Neter.Error("��ȫ���̷�����Ʊ���", ex); }
                    #endregion

                    btnSaveTransport_BuyFuel.Enabled = false;

                    if (this.AutoHandMode)
                        UpdateLedShow(this.CurrentAutotruck.CarNumber, "��ǰ��" + this.CurrentSampler + "������");
                    else
                        MessageBoxEx.Show(this.CurrentAutotruck.CarNumber + "�Ŷӳɹ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // ����������
                    timer_BuyFuel.Interval = 8000;
                    this.CurrentFlowFlag = eFlowFlag.�ȴ��뿪;

                    LoadTodayUnFinishBuyFuelTransport();
                    LoadTodayFinishBuyFuelTransport();

                    LetPass();

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("����ʧ��\r\n" + ex.Message, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);

                Log4Neter.Error("���������¼", ex);
            }

            return false;
        }

        /// <summary>
        /// �����볧ú�����¼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_BuyFuel_Click(object sender, EventArgs e)
        {
            ResetBuyFuel();
        }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        void ResetBuyFuel()
        {
            this.timer_BuyFuel_Cancel = true;

            this.CurrentFlowFlag = eFlowFlag.�ȴ�����;

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

            UpdateLedShow("  �ȴ�����");

            // �������
            this.CurrentImperfectCar = null;
        }

        /// <summary>
        /// �볧ú�����¼ҵ��ʱ��
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
                    case eFlowFlag.ƥ�����:
                        #region

                        try
                        {
                            //����
                            //this.CurrentZkBizResult = new ZkBizResult() { DispatchInfo = new DispatchInfo() { TaskNo = "W899191201", SendTare = 32.2 }, Message = "", Status = true };
                            this.CurrentZkBizResult = commService.GetRunningElecDisp(this.CurrentAutotruck.CarNumber, 1);
                            if (this.CurrentZkBizResult == null)
                            {
                                UpdateLedShow("���ܵ��˶�ȡʧ�ܣ�����ϵ�п�");
                                timer_BuyFuel.Interval = 8000;
                                this.CurrentFlowFlag = eFlowFlag.�쳣����2;
                                break;
                            }
                            if (!this.CurrentZkBizResult.Status)
                            {
                                UpdateLedShow("���ܵ��˶�ȡʧ�ܣ�����ϵ�п�");
                                timer_BuyFuel.Interval = 8000;
                                this.CurrentFlowFlag = eFlowFlag.�쳣����2;
                                break;
                            }

                            if (this.CurrentZkBizResult.DispatchInfo.SendTare <= 0)
                            {
                                UpdateLedShow("�󷢾���Ϊ0������ϵ�п�");
                                timer_BuyFuel.Interval = 8000;
                                this.CurrentFlowFlag = eFlowFlag.�쳣����2;
                                break;
                            }

                            //��ȡ������Ϣ
                            txtRemark_BuyFuel.Text = "���䵥λ��" + this.CurrentZkBizResult.DispatchInfo.Carrier + "������Ʒ�֣�" +
                                this.CurrentZkBizResult.DispatchInfo.Catagory + "��������λ:" + this.CurrentZkBizResult.DispatchInfo.Shipper
                                + "���󷢾���:" + this.CurrentZkBizResult.DispatchInfo.SendTare + "�����˼ƻ�����" + this.CurrentZkBizResult.DispatchInfo.TaskNo;
                            txtTicketWeight_BuyFuel.Value = CurrentZkBizResult.DispatchInfo.SendTare;

                            #region ���˼ƻ���

                            //View_dyjhk view_dyjhk = Dbers.GetInstance().SelfDber.Entity<View_dyjhk>("where Dybh=:Dybh", new { Dybh = this.CurrentZkBizResult.DispatchInfo.TaskNo });
                            //if (view_dyjhk == null)
                            //{
                            //    UpdateLedShow("���˼ƻ�����ȡʧ�ܣ����Ե�");
                            //    timer_BuyFuel.Interval = 8000;
                            //    this.CurrentFlowFlag = eFlowFlag.�쳣����2;
                            //    break;
                            //}

                            //this.SelectedFuelKind_BuyFuel = Dbers.GetInstance().SelfDber.Entity<CmcsFuelKind>("where Code=:Code", new { Code = view_dyjhk.Mzbm });
                            //this.SelectedMine_BuyFuel = Dbers.GetInstance().SelfDber.Entity<CmcsMine>("where Code=:Code", new { Code = view_dyjhk.Kbbm });
                            //this.SelectedSupplier_BuyFuel = Dbers.GetInstance().SelfDber.Entity<CmcsSupplier>("where Code=:Code", new { Code = view_dyjhk.Gysbm });
                            //this.SelectedTransportCompany_BuyFuel = Dbers.GetInstance().SelfDber.Entity<CmcsTransportCompany>("where Code=:Code", new { Code = view_dyjhk.Gysbm });
                            //this.CurrentFlowFlag = eFlowFlag.�Զ�����;
                            //break;

                            #endregion

                            #region ������úԤ����

                            this.CurrentQCLMYGP = Dbers.GetInstance().SelfDber.Entity<View_rlgl_cygl_qclmygp>("where Dybh=:Dybh and to_char(PlanTime,'YYYYMMDD')=to_char(:PlanTime,'YYYYMMDD')", new { Dybh = CurrentZkBizResult.DispatchInfo.TaskNo, PlanTime = DateTime.Now });
                            if (this.CurrentQCLMYGP == null)
                            {
                                UpdateLedShow("��úԤ������ȡʧ�ܣ����Ե�");
                                timer_BuyFuel.Interval = 8000;
                                this.CurrentFlowFlag = eFlowFlag.�쳣����2;
                                break;
                            }

                            this.SelectedFuelKind_BuyFuel = Dbers.GetInstance().SelfDber.Entity<CmcsFuelKind>("where PkId=:PkId", new { PkId = this.CurrentQCLMYGP.Mzid });
                            this.SelectedMine_BuyFuel = Dbers.GetInstance().SelfDber.Entity<CmcsMine>("where PkId=:PkId", new { PkId = this.CurrentQCLMYGP.Kbid });
                            this.SelectedSupplier_BuyFuel = Dbers.GetInstance().SelfDber.Entity<CmcsSupplier>("where PkId=:PkId", new { PkId = this.CurrentQCLMYGP.Gysid });
                            this.SelectedTransportCompany_BuyFuel = Dbers.GetInstance().SelfDber.Entity<CmcsTransportCompany>("where PkId=:PkId", new { PkId = this.CurrentQCLMYGP.Gysid });
                            this.CurrentFlowFlag = eFlowFlag.ƥ�����;

                            #endregion

                        }
                        catch (Exception ex)
                        {
                            Log4Neter.Error("��ȡ������������", ex);

                            UpdateLedShow("���ܵ��˶�ȡʧ�ܣ����Ե�" + (this.CurrentZkBizResult == null ? "" : this.CurrentZkBizResult.Message));
                            timer_BuyFuel.Interval = 8000;
                            this.CurrentFlowFlag = eFlowFlag.�쳣����2;
                            break;
                        }

                        #endregion
                        break;

                    case eFlowFlag.����¼��:
                        #region


                        #endregion
                        break;

                    case eFlowFlag.ƥ�����:
                        #region

                        try
                        {
                            string message = string.Empty;
                            string sampler = string.Empty;

                            //�жϳ��ڳ���
                            if (!queuerDAO.GetIsFactory(this.FactoryCount, this.isFactoryCount, ref message))
                            {
                                UpdateLedShow(message);
                                timer_BuyFuel.Interval = 8000;
                                this.CurrentFlowFlag = eFlowFlag.�쳣����2;
                                break;
                            }

                            //�������Ӧ��ָ���˲������Ͳ���Ҫ���������
                            if (this.SelectedSupplier_BuyFuel != null)
                            {
                                CmcsSupplierAssignSampler cmcsSupplierAssignSampler = commonDAO.SelfDber.Entity<CmcsSupplierAssignSampler>("where SupplierId=:SupplierId", new { SupplierId = this.SelectedSupplier_BuyFuel.Id });
                                if (cmcsSupplierAssignSampler != null)
                                {
                                    sampler = cmcsSupplierAssignSampler.Sampler;
                                }
                                else
                                {
                                    //�жϴ���������
                                    if (!queuerDAO.GetSamplerMachineCode(this.CurrentQCLMYGP.Cyjbm, this.SampleWayCount, this.isSampleWayCount, ref sampler, ref message))
                                    {
                                        UpdateLedShow(message);
                                        timer_BuyFuel.Interval = 8000;
                                        this.CurrentFlowFlag = eFlowFlag.�쳣����2;
                                        break;
                                    }
                                }
                            }

                            this.CurrentSampler = sampler;
                            this.CurrentFlowFlag = eFlowFlag.�Զ�����;
                        }
                        catch (Exception ex)
                        {
                            Log4Neter.Error("ƥ����������", ex);
                        }

                        #endregion
                        break;

                    case eFlowFlag.�Զ�����:
                        #region

                        //����������
                        timer_BuyFuel.Interval = 4000;

                        SaveBuyFuelTransport();

                        #endregion
                        break;

                    case eFlowFlag.�ȴ��뿪:
                        #region

                        // ��ǰ��·�ظ����ź�ʱ����
                        if (!HasCarOnCurrentWay()) ResetBuyFuel();

                        // ����������
                        timer_BuyFuel.Interval = 4000;

                        #endregion
                        break;

                    case eFlowFlag.�쳣����2:

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
        /// ��ȡδ��ɵ��볧ú��¼
        /// </summary>
        void LoadTodayUnFinishBuyFuelTransport()
        {
            superGridControl1_BuyFuel.PrimaryGrid.DataSource = queuerDAO.GetUnFinishBuyFuelTransport();
        }

        /// <summary>
        /// ��ȡָ����������ɵ��볧ú��¼
        /// </summary>
        void LoadTodayFinishBuyFuelTransport()
        {
            superGridControl2_BuyFuel.PrimaryGrid.DataSource = queuerDAO.GetFinishedBuyFuelTransport(DateTime.Now.Date, DateTime.Now.Date.AddDays(1));
        }

        /// <summary>
        /// ��ȡԤ����Ϣ
        /// </summary>
        /// <param name="lMYB">��úԤ��</param>
        void BorrowForecast_BuyFuel(CmcsLMYB lMYB)
        {
            if (lMYB == null) return;

            this.SelectedFuelKind_BuyFuel = commonDAO.SelfDber.Get<CmcsFuelKind>(lMYB.FuelKindId);
            this.SelectedMine_BuyFuel = commonDAO.SelfDber.Get<CmcsMine>(lMYB.MineId);
            this.SelectedSupplier_BuyFuel = commonDAO.SelfDber.Get<CmcsSupplier>(lMYB.SupplierId);
            this.SelectedTransportCompany_BuyFuel = commonDAO.SelfDber.Get<CmcsTransportCompany>(lMYB.TransportCompanyId);
        }

        /// <summary>
        /// ˫����ʱ���Զ���乩ú��λ��������Ϣ
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

            // ������Ч״̬
            if (e.GridCell.GridColumn.Name == "ChangeIsUse") queuerDAO.ChangeBuyFuelTransportToInvalid(entity.Id, Convert.ToBoolean(e.GridCell.Value));
        }

        private void superGridControl1_BuyFuel_DataBindingComplete(object sender, GridDataBindingCompleteEventArgs e)
        {
            foreach (GridRow gridRow in e.GridPanel.Rows)
            {
                View_BuyFuelTransport entity = gridRow.DataItem as View_BuyFuelTransport;
                if (entity == null) return;

                // �����Ч״̬
                gridRow.Cells["ChangeIsUse"].Value = Convert.ToBoolean(entity.IsUse);
            }
        }

        private void superGridControl2_BuyFuel_CellClick(object sender, GridCellClickEventArgs e)
        {
            View_BuyFuelTransport entity = e.GridCell.GridRow.DataItem as View_BuyFuelTransport;
            if (entity == null) return;

            // ������Ч״̬
            if (e.GridCell.GridColumn.Name == "ChangeIsUse") queuerDAO.ChangeBuyFuelTransportToInvalid(entity.Id, Convert.ToBoolean(e.GridCell.Value));
        }

        private void superGridControl2_BuyFuel_DataBindingComplete(object sender, GridDataBindingCompleteEventArgs e)
        {
            foreach (GridRow gridRow in e.GridPanel.Rows)
            {
                View_BuyFuelTransport entity = gridRow.DataItem as View_BuyFuelTransport;
                if (entity == null) return;

                // �����Ч״̬
                gridRow.Cells["ChangeIsUse"].Value = Convert.ToBoolean(entity.IsUse);
            }
        }

        #endregion

        #region ����úҵ��

        bool timer_SaleFuel_Cancel = true;

        List<String> StorageNames = new List<string>();

        private CmcsTransportSales selectedCmcsTransportSales;
        /// <summary>
        /// ѡ�������ú����
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
        /// ѡ��Ĺ�ú��λ
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
        /// ѡ������䵥λ
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
        /// ѡ���ú��
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
                MessageBoxEx.Show("��ѡ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (this.SelectedFuelKind_SaleFuel == null)
            {
                MessageBoxEx.Show("��ѡ��ú��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (this.SelectedSupplier_SaleFuel == null)
            {
                MessageBoxEx.Show("��ѡ��ú��λ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (this.SelectedTransportCompany_SaleFuel == null)
            {
                MessageBoxEx.Show("��ѡ�����䵥λ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            //if (this.SelectedCmcsTransportSales == null)
            //{
            //    MessageBoxEx.Show("��ѡ������ú����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return false;
            //}
            //if (StorageNames.IndexOf(cmbCoalProduct.SelectedItem.ToString()) < 0 && MessageBoxEx.Show(cmbCoalProduct.SelectedItem.ToString() + "��Ʒ�ֲ������۶����ڣ�ȷ��ѡ��", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            //{
            //    return false;
            //}
            try
            {
                // ��������ú�ŶӼ�¼
                if (queuerDAO.JoinQueueSaleFuelTransport(this.CurrentAutotruck, this.SelectedSupplier_SaleFuel, this.SelectedTransportCompany_SaleFuel, this.SelectedFuelKind_SaleFuel, DateTime.Now, txt_ReMark1.Text, CommonAppConfig.GetInstance().AppIdentifier, cmbCoalProduct.SelectedItem.ToString()))
                {
                    btnSaveTransport_BuyFuel.Enabled = false;
                    this.CurrentFlowFlag = eFlowFlag.�ȴ��뿪;

                    UpdateLedShow("�Ŷӳɹ�", "���뿪");
                    MessageBoxEx.Show("�Ŷӳɹ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadTodayUnFinishSaleFuelTransport();
                    LoadTodayFinishSaleFuelTransport();

                    LetPass();

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("����ʧ��\r\n" + ex.Message, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);

                Log4Neter.Error("���������¼", ex);
            }

            return false;
        }

        void ResetSaleFuel()
        {

            this.timer_SaleFuel_Cancel = true;

            this.CurrentFlowFlag = eFlowFlag.�ȴ�����;

            this.CurrentAutotruck = null;
            this.SelectedCmcsTransportSales = null;

            txtTagId_SaleFuel.ResetText();
            txtRemark_BuyFuel.ResetText();

            btnSaveTransport_BuyFuel.Enabled = true;

            UpdateLedShow("  �ȴ�����");

            // �������
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
            FrmAutotruck_Select frm = new FrmAutotruck_Select("and CarType='" + eCarType.����ú.ToString() + "' and IsUse=1 order by CarNumber asc");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                passCarQueuer.Enqueue(ePassWay.UnKnow, frm.Output.CarNumber, false);
                this.CurrentFlowFlag = eFlowFlag.��֤����;
            }
        }

        /// <summary>
        /// ѡ��ú��λ
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
        /// ѡ������䵥λ
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
        /// ѡ��ú��
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
                    case eFlowFlag.ƥ�����:
                        #region

                        //List<CmcsLMYB> lMYBs = queuerDAO.GetBuyFuelForecastByCarNumber(this.CurrentAutotruck.CarNumber, DateTime.Now);
                        //if (lMYBs.Count > 1)
                        //{
                        //    // ����úԤ�����ڶ���ʱ������ѡ��ȷ�Ͽ�
                        //    FrmBuyFuelForecast_Confirm frm = new FrmBuyFuelForecast_Confirm(lMYBs);
                        //    if (frm.ShowDialog() == DialogResult.OK) BorrowForecast_BuyFuel(frm.Output);
                        //}
                        //else if (lMYBs.Count == 1)
                        //{
                        //    BorrowForecast_BuyFuel(lMYBs[0]);
                        //}

                        this.CurrentFlowFlag = eFlowFlag.����¼��;

                        #endregion
                        break;

                    case eFlowFlag.����¼��:
                        #region



                        #endregion
                        break;

                    case eFlowFlag.�ȴ��뿪:
                        #region

                        // ��ǰ��·�ظ����ź�ʱ����
                        if (!HasCarOnCurrentWay()) ResetSaleFuel();

                        // ����������
                        timer_SaleFuel.Interval = 4000;

                        #endregion
                        break;
                }

                // ��ǰ��·�ظ����ź�ʱ����
                if (!HasCarOnCurrentWay() && this.CurrentFlowFlag != eFlowFlag.�ȴ����� && (this.CurrentImperfectCar != null && this.CurrentImperfectCar.IsFromDevice)) ResetSaleFuel();
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
            new FrmAutotruck_Oper(Guid.NewGuid().ToString(), eEditMode.����).Show();
        }

        #endregion

        #region ��������ҵ��

        bool timer_Goods_Cancel = true;

        private CmcsSupplier selectedSupplyUnit_Goods;
        /// <summary>
        /// ѡ��Ĺ�����λ
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
        /// ѡ����ջ���λ
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
        /// ѡ�����������
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
        /// ѡ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectAutotruck_Goods_Click(object sender, EventArgs e)
        {
            FrmAutotruck_Select frm = new FrmAutotruck_Select("and CarType='" + eCarType.��������.ToString() + "' and IsUse=1 order by CarNumber asc");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                passCarQueuer.Enqueue(ePassWay.UnKnow, frm.Output.CarNumber, false);
                this.CurrentFlowFlag = eFlowFlag.��֤����;
            }
        }

        /// <summary>
        /// ѡ�񹩻���λ
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
        /// ѡ���ջ���λ
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
        /// ѡ����������
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
        /// �³��Ǽ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewAutotruck_Goods_Click(object sender, EventArgs e)
        {
            // eCarType.�������� 

            new FrmAutotruck_Oper(Guid.NewGuid().ToString(), eEditMode.����).Show();
        }

        /// <summary>
        /// �����ŶӼ�¼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveTransport_Goods_Click(object sender, EventArgs e)
        {
            SaveGoodsTransport();
        }

        /// <summary>
        /// ���������¼
        /// </summary>
        /// <returns></returns>
        bool SaveGoodsTransport()
        {
            if (this.CurrentAutotruck == null)
            {
                MessageBoxEx.Show("��ѡ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(this.CurrentAutotruck.EPCCardId))
            {
                MessageBoxEx.Show("������ǩ������Ϊ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (this.SelectedSupplyUnit_Goods == null)
            {
                MessageBoxEx.Show("��ѡ�񹩻���λ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (this.SelectedReceiveUnit_Goods == null)
            {
                MessageBoxEx.Show("��ѡ���ջ���λ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (this.SelectedGoodsType_Goods == null)
            {
                MessageBoxEx.Show("��ѡ����������", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            try
            {
                // �����ŶӼ�¼
                if (queuerDAO.JoinQueueGoodsTransport(this.CurrentAutotruck, this.SelectedSupplyUnit_Goods, this.SelectedReceiveUnit_Goods, this.SelectedGoodsType_Goods, DateTime.Now, txtRemark_Goods.Text, CommonAppConfig.GetInstance().AppIdentifier))
                {
                    btnSaveTransport_Goods.Enabled = false;

                    if (this.AutoHandMode)
                        UpdateLedShow(this.CurrentAutotruck.CarNumber, "�����");
                    else
                        MessageBoxEx.Show(this.CurrentAutotruck.CarNumber + "�Ŷӳɹ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.CurrentFlowFlag = eFlowFlag.�ȴ��뿪;

                    LoadTodayUnFinishGoodsTransport();
                    LoadTodayFinishGoodsTransport();

                    LetPass();

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("����ʧ��\r\n" + ex.Message, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);

                Log4Neter.Error("���������¼", ex);
            }

            return false;
        }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        void ResetGoods()
        {
            this.timer_Goods_Cancel = true;

            this.CurrentFlowFlag = eFlowFlag.�ȴ�����;

            this.CurrentAutotruck = null;
            this.SelectedSupplyUnit_Goods = null;
            this.SelectedReceiveUnit_Goods = null;
            this.SelectedGoodsType_Goods = null;

            txtTagId_Goods.ResetText();
            txtRemark_Goods.ResetText();

            btnSaveTransport_Goods.Enabled = true;

            UpdateLedShow("  �ȴ�����");

            // �������
            this.CurrentImperfectCar = null;
        }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Goods_Click(object sender, EventArgs e)
        {
            ResetGoods();
        }

        /// <summary>
        /// �������������¼ҵ��ʱ��
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

                    case eFlowFlag.ƥ�����:
                        #region

                        CmcsGoodsPlan cmcsGoodsPlan = Dbers.GetInstance().SelfDber.Entity<CmcsGoodsPlan>("where CarNumber=:CarNumber", new { CarNumber = this.CurrentAutotruck.CarNumber });
                        if (cmcsGoodsPlan == null)
                        {
                            UpdateLedShow(this.CurrentAutotruck.CarNumber + "δ�ҵ����ʼƻ�������ϵ����Ա");
                            timer_Goods.Interval = 8000;
                            this.CurrentFlowFlag = eFlowFlag.�쳣����2;
                            break;
                        }

                        this.SelectedSupplyUnit_Goods = Dbers.GetInstance().SelfDber.Get<CmcsSupplier>(cmcsGoodsPlan.SupplyUnitId);
                        this.SelectedReceiveUnit_Goods = Dbers.GetInstance().SelfDber.Get<CmcsSupplier>(cmcsGoodsPlan.ReceiveUnitId);
                        this.SelectedGoodsType_Goods = Dbers.GetInstance().SelfDber.Get<CmcsGoodsType>(cmcsGoodsPlan.GoodsTypeId);
                        this.CurrentFlowFlag = eFlowFlag.�Զ�����;

                        #endregion
                        break;

                    case eFlowFlag.����¼��:
                        #region



                        #endregion
                        break;

                    case eFlowFlag.�Զ�����:
                        #region

                        //����������
                        timer_Goods.Interval = 4000;

                        SaveGoodsTransport();

                        #endregion
                        break;

                    case eFlowFlag.�ȴ��뿪:
                        #region

                        // ��ǰ��·�ظ����ź�ʱ����
                        if (!HasCarOnCurrentWay()) ResetGoods();

                        #endregion
                        break;

                    case eFlowFlag.�쳣����2:

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
        /// ��ȡδ��ɵ��������ʼ�¼
        /// </summary>
        void LoadTodayUnFinishGoodsTransport()
        {
            superGridControl1_Goods.PrimaryGrid.DataSource = queuerDAO.GetUnFinishGoodsTransport();
        }

        /// <summary>
        /// ��ȡָ����������ɵ��������ʼ�¼
        /// </summary>
        void LoadTodayFinishGoodsTransport()
        {
            superGridControl2_Goods.PrimaryGrid.DataSource = queuerDAO.GetFinishedGoodsTransport(DateTime.Now.Date, DateTime.Now.Date.AddDays(1));
        }

        /// <summary>
        /// ˫����ʱ���Զ����¼����Ϣ
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

            // ������Ч״̬
            if (e.GridCell.GridColumn.Name == "ChangeIsUse") queuerDAO.ChangeGoodsTransportToInvalid(entity.Id, Convert.ToBoolean(e.GridCell.Value));
        }

        private void superGridControl1_Goods_DataBindingComplete(object sender, GridDataBindingCompleteEventArgs e)
        {
            foreach (GridRow gridRow in e.GridPanel.Rows)
            {
                CmcsGoodsTransport entity = gridRow.DataItem as CmcsGoodsTransport;
                if (entity == null) return;

                // �����Ч״̬
                gridRow.Cells["ChangeIsUse"].Value = Convert.ToBoolean(entity.IsUse);
            }
        }

        private void superGridControl2_Goods_CellClick(object sender, GridCellClickEventArgs e)
        {
            CmcsGoodsTransport entity = e.GridCell.GridRow.DataItem as CmcsGoodsTransport;
            if (entity == null) return;

            // ������Ч״̬
            if (e.GridCell.GridColumn.Name == "ChangeIsUse") queuerDAO.ChangeGoodsTransportToInvalid(entity.Id, Convert.ToBoolean(e.GridCell.Value));
        }

        private void superGridControl2_Goods_DataBindingComplete(object sender, GridDataBindingCompleteEventArgs e)
        {
            foreach (GridRow gridRow in e.GridPanel.Rows)
            {
                CmcsGoodsTransport entity = gridRow.DataItem as CmcsGoodsTransport;
                if (entity == null) return;

                // �����Ч״̬
                gridRow.Cells["ChangeIsUse"].Value = Convert.ToBoolean(entity.IsUse);
            }
        }

        #endregion

        #region ����ҵ��

        /// <summary>
        /// ����ҵ��
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
                    case eFlowFlag.�ȴ�����:
                        #region

                        //���������
                        timer_Out.Interval = 500;

                        //List<string> tags = Hardwarer.RwerOut.ScanTags();
                        List<string> tags = new List<string>() { "BBAAC4DCCEBCC4CF00000461" };
                        if (tags.Count > 0) passCarQueuerOut.Enqueue(ePassWay.Way1, tags[0], true);

                        if (passCarQueuerOut.Count > 0) this.CurrentFlowFlagOut = eFlowFlag.ʶ����;

                        #endregion
                        break;

                    case eFlowFlag.ʶ����:
                        #region

                        // �������޳�ʱ���ȴ�����
                        if (passCarQueuerOut.Count == 0)
                        {
                            UpdateShowDebug("  �ȴ�����");
                            this.CurrentFlowFlagOut = eFlowFlag.�ȴ�����;
                            break;
                        }

                        this.CurrentImperfectCarOut = passCarQueuerOut.Dequeue();

                        // ��ʽһ������ʶ��ĳ��ƺŲ��ҳ�����Ϣ
                        this.CurrentAutotruckOut = carTransportDAO.GetAutotruckByCarNumber(this.CurrentImperfectCarOut.Voucher);

                        if (this.CurrentAutotruckOut == null)
                            // ��ʽ��������ʶ��ı�ǩ�����ҳ�����Ϣ
                            this.CurrentAutotruckOut = carTransportDAO.GetAutotruckByTagId(this.CurrentImperfectCarOut.Voucher);

                        if (this.CurrentAutotruckOut != null)
                        {
                            if (this.CurrentAutotruckOut.IsUse == 1)
                            {
                                if (this.CurrentAutotruckOut.CarType == eCarType.�볧ú.ToString())
                                    this.CurrentFlowFlagOut = eFlowFlag.�볧ú��֤����;

                                else if (this.CurrentAutotruckOut.CarType == eCarType.��������.ToString())
                                    this.CurrentFlowFlagOut = eFlowFlag.����������֤����;
                            }
                            else
                            {
                                UpdateShowDebug(this.CurrentAutotruckOut.CarNumber, "��ͣ��");

                                timer_Out.Interval = 8000;
                            }
                        }
                        else
                        {
                            UpdateShowDebug(this.CurrentImperfectCarOut.Voucher, "δ�Ǽ�");

                            timer_Out.Interval = 8000;
                        }

                        #endregion
                        break;

                    case eFlowFlag.�볧ú��֤����:
                        #region

                        this.CurrentBuyFuelTransportOut = commonDAO.SelfDber.Entity<CmcsBuyFuelTransport>("where AutotruckId=:AutotruckId order by InFactoryTime desc", new { AutotruckId = this.CurrentAutotruckOut.Id });
                        if (this.CurrentBuyFuelTransportOut != null)
                        {
                            if (this.CurrentBuyFuelTransportOut.StepName != eTruckInFactoryStep.����.ToString())
                            {
                                // �ж�·������
                                string nextPlace;
                                if (carTransportDAO.CheckNextTruckInFactoryWay(this.CurrentAutotruckOut.CarType, this.CurrentBuyFuelTransportOut.StepName, eTruckInFactoryStep.����.ToString(), CommonAppConfig.GetInstance().AppIdentifier, out nextPlace))
                                {
                                    if (this.CurrentBuyFuelTransportOut.SuttleWeight > 0)
                                    {
                                        // �Զ�ģʽ
                                        if (!SaveBuyFuelTransportOut())
                                        {
                                            UpdateLedShow(this.CurrentAutotruckOut.CarNumber, "����ʧ��");

                                            timer_Out.Interval = 4000;
                                        }
                                    }
                                    else
                                    {
                                        UpdateShowDebug(this.CurrentAutotruckOut.CarNumber, "����δ���");
                                        this.CurrentFlowFlagOut = eFlowFlag.�쳣����2;

                                        timer_Out.Interval = 8000;
                                    }
                                }
                                else
                                {
                                    UpdateShowDebug("·�ߴ���", "��ֹͨ��");
                                    this.CurrentFlowFlagOut = eFlowFlag.�쳣����2;

                                    timer_Out.Interval = 8000;
                                }
                            }
                            else
                            {
                                UpdateShowDebug(this.CurrentAutotruckOut.CarNumber, "���뿪");
                                this.CurrentFlowFlagOut = eFlowFlag.�ȴ��뿪;

                                timer_Out.Interval = 2000;
                            }
                        }
                        else
                        {
                            UpdateShowDebug(this.CurrentAutotruckOut.CarNumber, "δ�ҵ������¼");
                            this.CurrentFlowFlagOut = eFlowFlag.�쳣����2;

                            timer_Out.Interval = 8000;
                        }

                        #endregion
                        break;

                    case eFlowFlag.����������֤����:
                        #region

                        this.CurrentGoodsTransportOut = commonDAO.SelfDber.Entity<CmcsGoodsTransport>("where AutotruckId=:AutotruckId order by InFactoryTime desc", new { AutotruckId = this.CurrentAutotruckOut.Id });
                        if (this.CurrentGoodsTransportOut != null)
                        {
                            if (this.CurrentGoodsTransportOut.StepName != eTruckInFactoryStep.����.ToString())
                            {
                                // �ж�·������
                                string nextPlace;
                                if (carTransportDAO.CheckNextTruckInFactoryWay(this.CurrentAutotruckOut.CarType, this.CurrentGoodsTransportOut.StepName, "����", CommonAppConfig.GetInstance().AppIdentifier, out nextPlace))
                                {
                                    if (this.CurrentGoodsTransportOut.SuttleWeight > 0)
                                    {
                                        // �Զ�ģʽ
                                        if (!SaveGoodsTransportOut())
                                        {
                                            UpdateShowDebug(this.CurrentAutotruckOut.CarNumber, "����ʧ��");

                                            timer_Out.Interval = 8000;
                                        }
                                    }
                                    else
                                    {
                                        UpdateShowDebug(this.CurrentAutotruckOut.CarNumber, "����δ���");

                                        timer_Out.Interval = 8000;
                                    }
                                }
                                else
                                {
                                    UpdateShowDebug("·�ߴ���", "��ֹͨ��");

                                    timer_Out.Interval = 8000;
                                }
                            }
                        }
                        else
                        {
                            UpdateShowDebug(this.CurrentAutotruckOut.CarNumber, "δ�ҵ������¼");
                            this.CurrentFlowFlagOut = eFlowFlag.�쳣����2;

                            timer_Out.Interval = 8000;
                        }

                        #endregion
                        break;

                    case eFlowFlag.�ȴ��뿪:
                        #region

                        // ��ǰ��·�ظ����ź�ʱ����
                        ResetOut();

                        // ����������
                        timer_Out.Interval = 4000;

                        #endregion
                        break;

                    case eFlowFlag.�쳣����2:

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
        /// ������������¼
        /// </summary>
        /// <returns></returns>
        bool SaveBuyFuelTransportOut()
        {
            if (this.CurrentBuyFuelTransportOut == null) return false;

            try
            {
                if (outerDAO.SaveBuyFuelTransport(this.CurrentBuyFuelTransportOut.Id, DateTime.Now))
                {
                    // ��ӡ����

                    this.CurrentFlowFlagOut = eFlowFlag.�ȴ��뿪;

                    UpdateShowDebug(this.CurrentAutotruckOut.CarNumber + "�����ɹ�", "���뿪");

                    LetPass();

                    return true;
                }
            }
            catch (Exception ex)
            {
                Log4Neter.Error("�볧ú�������������¼", ex);
            }

            return false;
        }

        /// <summary>
        /// ������������¼
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

                    this.CurrentFlowFlagOut = eFlowFlag.�ȴ��뿪;

                    UpdateShowDebug(this.CurrentAutotruckOut.CarNumber + "�����ɹ�", "���뿪");

                    LetPass();

                    return true;
                }
            }
            catch (Exception ex)
            {
                Log4Neter.Error("�������ʳ������������¼", ex);
            }

            return false;
        }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        void ResetOut()
        {
            this.CurrentFlowFlagOut = eFlowFlag.�ȴ�����;

            this.CurrentAutotruckOut = null;

            UpdateShowDebug("  �ȴ�����");

            // �������
            this.CurrentImperfectCarOut = null;
        }

        #endregion

        #region ��������

        Pen redPen3 = new Pen(Color.Red, 3);
        Pen greenPen3 = new Pen(Color.Lime, 3);
        Pen greenPen1 = new Pen(Color.Lime, 1);

        /// <summary>
        /// ��ǰ����������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panCurrentCarNumber_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                PanelEx panel = sender as PanelEx;

                // ���Ƶظ�1
                e.Graphics.DrawLine(this.InductorCoil1 ? redPen3 : greenPen3, 15, 10, 15, 30);
                // ���Ƶظ�2                                                               
                e.Graphics.DrawLine(this.InductorCoil2 ? redPen3 : greenPen3, 25, 10, 25, 30);
                // ���Ʒָ���
                e.Graphics.DrawLine(greenPen1, 5, 34, 35, 34);
                // ���Ƶظ�3
                e.Graphics.DrawLine(this.InductorCoil3 ? redPen3 : greenPen3, 15, 38, 15, 58);
                // ���Ƶظ�4                                                               
                e.Graphics.DrawLine(this.InductorCoil4 ? redPen3 : greenPen3, 25, 38, 25, 58);
            }
            catch (Exception ex)
            {
                Log4Neter.Error("panCurrentCarNumber_Paint�쳣", ex);
            }
        }

        private void superGridControl_BeginEdit(object sender, DevComponents.DotNetBar.SuperGrid.GridEditEventArgs e)
        {
            if (e.GridCell.GridColumn.DataPropertyName != "IsUse")
            {
                // ȡ������༭
                e.Cancel = true;
            }
        }

        /// <summary>
        /// �����к�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void superGridControl_GetRowHeaderText(object sender, DevComponents.DotNetBar.SuperGrid.GridGetRowHeaderTextEventArgs e)
        {
            e.Text = (e.GridRow.RowIndex + 1).ToString();
        }

        /// <summary>
        /// Invoke��װ
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
