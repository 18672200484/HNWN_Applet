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
using CMCS.CarTransport.JxSampler.Core;
using CMCS.CarTransport.JxSampler.Enums;
using CMCS.CarTransport.JxSampler.Frms.Sys;
using CMCS.Common;
using CMCS.Common.DAO;
using CMCS.Common.Entities;
using CMCS.Common.Entities.CarTransport;
using CMCS.Common.Entities.Fuel;
using CMCS.Common.Entities.Inf;
using CMCS.Common.Entities.Sys;
using CMCS.Common.Enums;
using CMCS.Common.Utilities;
using DevComponents.DotNetBar;
using LED.YB14;

namespace CMCS.CarTransport.JxSampler.Frms
{
    public partial class FrmJxSampler : DevComponents.DotNetBar.Metro.MetroForm
    {
        /// <summary>
        /// ����Ψһ��ʶ��
        /// </summary>
        public static string UniqueKey = "FrmCarSampler";

        public FrmJxSampler()
        {
            InitializeComponent();
        }

        #region Vars

        CarTransportDAO carTransportDAO = CarTransportDAO.GetInstance();
        JxSamplerDAO jxSamplerDAO = JxSamplerDAO.GetInstance();
        CommonDAO commonDAO = CommonDAO.GetInstance();

        IocControler iocControler;
        /// <summary>
        /// ��������
        /// </summary>
        VoiceSpeaker voiceSpeaker = new VoiceSpeaker();

        bool autoHandMode = true;
        /// <summary>
        /// �ֶ�ģʽ=true  �ֶ�ģʽ=false
        /// </summary>
        public bool AutoHandMode
        {
            get { return autoHandMode; }
            set
            {
                autoHandMode = value;

                btnSendSamplingPlan.Visible = !value;
                btnSelectAutotruck.Visible = !value;
                btnReset.Visible = !value;
            }
        }

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

        private CmcsBuyFuelTransport currentBuyFuelTransport;
        /// <summary>
        /// ��ǰ�����¼
        /// </summary>
        public CmcsBuyFuelTransport CurrentBuyFuelTransport
        {
            get { return currentBuyFuelTransport; }
            set
            {
                currentBuyFuelTransport = value;

                if (value != null)
                {
                    commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.��ǰ�����¼Id.ToString(), value.Id);

                    txtSupplierName.Text = value.SupplierName;
                    txtMineName.Text = value.MineName;
                    txtTicketWeight.Text = value.TicketWeight.ToString();
                    txtTransportCompanyName.Text = value.TransportCompanyName;
                    txtFuelKindName.Text = value.FuelKindName;
                }
                else
                {
                    commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.��ǰ�����¼Id.ToString(), string.Empty);

                    txtSupplierName.ResetText();
                    txtMineName.ResetText();
                    txtTransportCompanyName.ResetText();
                    txtFuelKindName.ResetText();
                    txtTicketWeight.ResetText();
                }
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

                if (value != null)
                {
                    commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.��ǰ��Id.ToString(), value.Id);
                    commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.��ǰ����.ToString(), value.CarNumber);

                    CmcsEPCCard ePCCard = Dbers.GetInstance().SelfDber.Get<CmcsEPCCard>(value.EPCCardId);
                    if (ePCCard != null) txtTagId.Text = ePCCard.TagId;

                    txtCarNumber.Text = value.CarNumber;
                    panCurrentCarNumber.Text = value.CarNumber;

                    dbi_CarriageLength.Value = value.CarriageLength;
                    dbi_CarriageWidth.Value = value.CarriageWidth;
                    dbi_CarriageBottomToFloor.Value = value.CarriageBottomToFloor;
                    dbi_LeftObstacle1.Value = value.LeftObstacle1;
                    dbi_LeftObstacle2.Value = value.LeftObstacle2;
                    dbi_LeftObstacle3.Value = value.LeftObstacle3;
                    dbi_LeftObstacle4.Value = value.LeftObstacle4;
                    dbi_LeftObstacle5.Value = value.LeftObstacle5;
                    dbi_LeftObstacle6.Value = value.LeftObstacle6;
                }
                else
                {
                    commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.��ǰ��Id.ToString(), string.Empty);
                    commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.��ǰ����.ToString(), string.Empty);

                    dbi_CarriageLength.Value = 0;
                    dbi_CarriageWidth.Value = 0;
                    dbi_CarriageBottomToFloor.Value = 0;
                    dbi_LeftObstacle1.Value = 0;
                    dbi_LeftObstacle2.Value = 0;
                    dbi_LeftObstacle3.Value = 0;
                    dbi_LeftObstacle4.Value = 0;
                    dbi_LeftObstacle5.Value = 0;
                    dbi_LeftObstacle6.Value = 0;

                    txtTagId.ResetText();
                    txtCarNumber.ResetText();
                    panCurrentCarNumber.ResetText();
                }
            }
        }

        private InfQCJXCYSampleCMD currentSampleCMD;
        /// <summary>
        /// ��ǰ��������
        /// </summary>
        public InfQCJXCYSampleCMD CurrentSampleCMD
        {
            get { return currentSampleCMD; }
            set { currentSampleCMD = value; }
        }

        private eEquInfSamplerSystemStatus samplerSystemStatus;
        /// <summary>
        /// ������ϵͳ״̬
        /// </summary>
        public eEquInfSamplerSystemStatus SamplerSystemStatus
        {
            get { return samplerSystemStatus; }
            set
            {
                samplerSystemStatus = value;

                if (value == eEquInfSamplerSystemStatus.��������)
                    slightSamplerStatus.LightColor = EquipmentStatusColors.BeReady;
                else if (value == eEquInfSamplerSystemStatus.�������� || value == eEquInfSamplerSystemStatus.����ж��)
                    slightSamplerStatus.LightColor = EquipmentStatusColors.Working;
                else if (value == eEquInfSamplerSystemStatus.��������)
                    slightSamplerStatus.LightColor = EquipmentStatusColors.Breakdown;
            }
        }

        /// <summary>
        /// �������豸����
        /// </summary>
        public string SamplerMachineCode;
        /// <summary>
        /// �������豸����
        /// </summary>
        public string SamplerMachineName;

        #endregion

        /// <summary>
        /// �����ʼ��
        /// </summary>
        private void InitForm()
        {
            FrmDebugConsole.GetInstance();

            // �������豸����
            this.SamplerMachineCode = commonDAO.GetAppletConfigString("�������豸����");
            this.SamplerMachineName = commonDAO.GetMachineNameByCode(this.SamplerMachineCode);

            // Ĭ���Զ�
            sbtnChangeAutoHandMode.Value = true;

            // ���ó���Զ�̿�������
            commonDAO.ResetAppRemoteControlCmd(CommonAppConfig.GetInstance().AppIdentifier);

            btnRefresh_Click(null, null);
        }

        private void FrmCarSampler_Load(object sender, EventArgs e)
        {

        }

        private void FrmCarSampler_Shown(object sender, EventArgs e)
        {
            InitHardware();

            InitForm();
        }

        private void FrmCarSampler_FormClosing(object sender, FormClosingEventArgs e)
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
        /// ǰ������
        /// </summary>
        void FrontGateUp()
        {
            this.iocControler.Gate2Up();
            this.iocControler.GreenLight1();
        }

        /// <summary>
        /// ǰ������
        /// </summary>
        void FrontGateDown()
        {
            this.iocControler.Gate2Down();
            this.iocControler.RedLight1();
        }

        /// <summary>
        /// ������
        /// </summary>
        void BackGateUp()
        {
            this.iocControler.Gate1Up();
            this.iocControler.GreenLight1();
        }

        /// <summary>
        /// �󷽽���
        /// </summary>
        void BackGateDown()
        {
            this.iocControler.Gate1Down();
            this.iocControler.RedLight1();
        }

        #endregion

        #region ������

        void Rwer1_OnScanError(Exception ex)
        {
            Log4Neter.Error("������1", ex);
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

        #region LED���ƿ�

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

                slightLED1.LightColor = (_LED1ConnectStatus ? Color.Green : Color.Red);

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
        /// ����LED��̬����
        /// </summary>
        /// <param name="value1">��һ������</param>
        /// <param name="value2">�ڶ�������</param>
        private void UpdateLedShow(string value1 = "", string value2 = "")
        {
            FrmDebugConsole.GetInstance().Output("����LED1:|" + value1 + "|" + value2 + "|");

            if (!this.LED1ConnectStatus) return;
            if (this.LED1PrevLedFileContent == value1 + value2) return;

            string ledContent = GenerateFillLedContent12(value1) + GenerateFillLedContent12(value2);

            File.WriteAllText(this.LED1TempFile, ledContent, Encoding.UTF8);

            if (LED1m_bSendBusy == false)
            {
                LED1m_bSendBusy = true;

                //int nResult = YB14DynamicAreaLeder.SendDynamicAreaInfoCommand(this.LED1nScreenNo, this.LED1DYArea_ID);
                //if (nResult != YB14DynamicAreaLeder.RETURN_NOERROR) Log4Neter.Error("����LED��̬����", new Exception(YB14DynamicAreaLeder.GetErrorMessage("SendDynamicAreaInfoCommand", nResult)));

                LED1m_bSendBusy = false;
            }

            this.LED1PrevLedFileContent = value1 + value2;
        }

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

                // IO������
                //Hardwarer.Iocer.OnReceived += new IOC.JMDM20DIOV2.JMDM20DIOV2Iocer.ReceivedEventHandler(Iocer_Received);
                //Hardwarer.Iocer.OnStatusChange += new IOC.JMDM20DIOV2.JMDM20DIOV2Iocer.StatusChangeHandler(Iocer_StatusChange);
                success = Hardwarer.Iocer.OpenCom(commonDAO.GetAppletConfigInt32("IO������_����"), commonDAO.GetAppletConfigInt32("IO������_������"), commonDAO.GetAppletConfigInt32("IO������_����λ"), (StopBits)commonDAO.GetAppletConfigInt32("IO������_ֹͣλ"), (Parity)commonDAO.GetAppletConfigInt32("IO������_У��λ"));
                if (!success) MessageBoxEx.Show("IO����������ʧ�ܣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.iocControler = new IocControler(Hardwarer.Iocer);

                // ������1
                Hardwarer.Rwer1.StartWith = commonDAO.GetAppletConfigString("������_��ǩ����");
                Hardwarer.Rwer1.OnStatusChange += new RW.LZR12.Lzr12Rwer.StatusChangeHandler(Rwer1_OnStatusChange);
                Hardwarer.Rwer1.OnScanError += new RW.LZR12.Lzr12Rwer.ScanErrorEventHandler(Rwer1_OnScanError);
                success = Hardwarer.Rwer1.OpenCom(commonDAO.GetAppletConfigInt32("������1_����"));
                if (!success) MessageBoxEx.Show("������1����ʧ�ܣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                #region LED���ƿ�

                string led1SocketIP = commonDAO.GetAppletConfigString("LED��ʾ��1_IP��ַ");
                if (!string.IsNullOrEmpty(led1SocketIP))
                {
                    if (CommonUtil.PingReplyTest(led1SocketIP))
                    {
                        int nResult = YB14DynamicAreaLeder.AddScreen(YB14DynamicAreaLeder.CONTROLLER_BX_5E1, this.LED1nScreenNo, YB14DynamicAreaLeder.SEND_MODE_NETWORK, 96, 32, 1, 1, "", 0, led1SocketIP, 5005, "");
                        if (nResult == YB14DynamicAreaLeder.RETURN_NOERROR)
                        {
                            nResult = YB14DynamicAreaLeder.AddScreenDynamicArea(this.LED1nScreenNo, this.LED1DYArea_ID, 0, 10, 1, "", 0, 0, 0, 96, 32, 255, 0, 255, 7, 6, 1);
                            if (nResult == YB14DynamicAreaLeder.RETURN_NOERROR)
                            {
                                nResult = YB14DynamicAreaLeder.AddScreenDynamicAreaFile(this.LED1nScreenNo, this.LED1DYArea_ID, this.LED1TempFile, 0, "����", 12, 0, 120, 1, 3, 0);
                                if (nResult == YB14DynamicAreaLeder.RETURN_NOERROR)
                                {
                                    // ��ʼ���ɹ�
                                    this.LED1ConnectStatus = true;
                                }
                                else
                                {
                                    this.LED1ConnectStatus = false;
                                    Log4Neter.Error("��ʼ��LED1���ƿ�", new Exception(YB14DynamicAreaLeder.GetErrorMessage("AddScreenDynamicAreaFile", nResult)));
                                    MessageBoxEx.Show("LED1���ƿ�����ʧ�ܣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                            else
                            {
                                this.LED1ConnectStatus = false;
                                Log4Neter.Error("��ʼ��LED1���ƿ�", new Exception(YB14DynamicAreaLeder.GetErrorMessage("AddScreenDynamicArea", nResult)));
                                MessageBoxEx.Show("LED1���ƿ�����ʧ�ܣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        else
                        {
                            this.LED1ConnectStatus = false;
                            Log4Neter.Error("��ʼ��LED1���ƿ�", new Exception(YB14DynamicAreaLeder.GetErrorMessage("AddScreen", nResult)));
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

                #endregion

                //��������
                voiceSpeaker.SetVoice(commonDAO.GetAppletConfigInt32("����"), commonDAO.GetAppletConfigInt32("����"), commonDAO.GetAppletConfigString("������"));

                timer1.Enabled = true;
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
                if (this.LED1ConnectStatus)
                {
                    YB14DynamicAreaLeder.SendDeleteDynamicAreasCommand(this.LED1nScreenNo, 1, "");
                    YB14DynamicAreaLeder.DeleteScreen(this.LED1nScreenNo);
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

                        if (this.InductorCoil1)
                        {
                            // ����������ظ����źţ������������߳���ʶ��

                            List<string> tags = Hardwarer.Rwer1.ScanTags();
                            if (tags.Count > 0) passCarQueuer.Enqueue(tags[0]);
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
                            //// ��ʽ��������ʶ��ı�ǩ�����ҳ�����Ϣ
                            this.CurrentAutotruck = carTransportDAO.GetAutotruckByTagId(this.CurrentImperfectCar.Voucher);

                        if (this.CurrentAutotruck != null)
                        {
                            UpdateLedShow(this.CurrentAutotruck.CarNumber + "�����ɹ�");
                            this.voiceSpeaker.Speak(this.CurrentAutotruck.CarNumber + " �����ɹ�", 1, false);

                            if (this.CurrentAutotruck.IsUse == 1)
                            {
                                if (this.CurrentAutotruck.CarriageLength > 0 && this.CurrentAutotruck.CarriageWidth > 0 && this.CurrentAutotruck.CarriageBottomToFloor > 0)
                                {
                                    // δ��������¼
                                    CmcsUnFinishTransport unFinishTransport = carTransportDAO.GetUnFinishTransportByAutotruckId(this.CurrentAutotruck.Id, eCarType.�볧ú.ToString());
                                    if (unFinishTransport != null)
                                    {
                                        this.CurrentBuyFuelTransport = carTransportDAO.GetBuyFuelTransportById(unFinishTransport.TransportId);
                                        if (this.CurrentBuyFuelTransport != null)
                                        {
                                            // �ж�·������
                                            string nextPlace;
                                            if (carTransportDAO.CheckNextTruckInFactoryWay(this.CurrentAutotruck.CarType, this.CurrentBuyFuelTransport.StepName, "����", CommonAppConfig.GetInstance().AppIdentifier, out nextPlace))
                                            {
                                                BackGateUp();

                                                btnSendSamplingPlan.Enabled = true;

                                                this.CurrentFlowFlag = eFlowFlag.���ͼƻ�;

                                                UpdateLedShow(this.CurrentAutotruck.CarNumber, "�������³�");
                                                this.voiceSpeaker.Speak(this.CurrentAutotruck.CarNumber + " �������³�", 1, false);

                                            }
                                            else
                                            {
                                                UpdateLedShow("·�ߴ���", "��ֹͨ��");
                                                this.voiceSpeaker.Speak("·�ߴ��� ��ֹͨ�� " + (!string.IsNullOrEmpty(nextPlace) ? "��ǰ��" + nextPlace : ""), 1, false);

                                                timer1.Interval = 20000;
                                            }
                                        }
                                        else
                                        {
                                            commonDAO.SelfDber.Delete<CmcsUnFinishTransport>(unFinishTransport.Id);
                                        }
                                    }
                                    else
                                    {
                                        this.UpdateLedShow(this.CurrentAutotruck.CarNumber, "δ�Ŷ�");
                                        this.voiceSpeaker.Speak("���ƺ� " + this.CurrentAutotruck.CarNumber + " δ�ҵ��ŶӼ�¼", 1, false);
                                        timer1.Interval = 20000;
                                    }
                                }
                                else
                                {
                                    this.UpdateLedShow(this.CurrentAutotruck.CarNumber, "����δ����");
                                    this.voiceSpeaker.Speak("���ƺ� " + this.CurrentAutotruck.CarNumber + " ����δ����", 1, false);

                                    timer1.Interval = 20000;
                                }
                            }
                            else
                            {
                                UpdateLedShow(this.CurrentAutotruck.CarNumber, "��ͣ��");
                                this.voiceSpeaker.Speak("���ƺ� " + this.CurrentAutotruck.CarNumber + " ��ͣ�ã���ֹͨ��", 1, false);

                                timer1.Interval = 20000;
                            }
                        }
                        else
                        {
                            UpdateLedShow(this.CurrentImperfectCar.Voucher, "δ�Ǽ�");

                            // ��ʽһ������ʶ��
                            this.voiceSpeaker.Speak("���ƺ� " + this.CurrentImperfectCar.Voucher + " δ�Ǽǣ���ֹͨ��", 1, false);
                            //// ��ʽ����ˢ����ʽ
                            //this.voiceSpeaker.Speak("����δ�Ǽǣ���ֹͨ��", 1, false);

                            timer1.Interval = 20000;
                        }

                        #endregion
                        break;

                    case eFlowFlag.���ͼƻ�:
                        #region

                        if (this.SamplerSystemStatus == eEquInfSamplerSystemStatus.��������)
                        {
                            CmcsRCSampling sampling = carTransportDAO.GetRCSamplingById(this.CurrentBuyFuelTransport.SamplingId);
                            if (sampling != null)
                            {
                                txtSampleCode.Text = sampling.SampleCode;

                                this.CurrentSampleCMD = new InfQCJXCYSampleCMD()
                                {
                                    MachineCode = this.SamplerMachineCode,
                                    CarNumber = this.CurrentBuyFuelTransport.CarNumber,
                                    InFactoryBatchId = this.CurrentBuyFuelTransport.InFactoryBatchId,
                                    SampleCode = sampling.SampleCode,
                                    Mt = 0,
                                    // ����Ԥ��
                                    TicketWeight = 0,
                                    // ����Ԥ��
                                    CarCount = 0,
                                    // ����������������߼�����
                                    PointCount = 3,
                                    CarriageLength = this.CurrentAutotruck.CarriageLength,
                                    CarriageWidth = this.CurrentAutotruck.CarriageWidth,
                                    CarriageBottomToFloor = this.CurrentAutotruck.CarriageBottomToFloor,
                                    Obstacle1 = this.CurrentAutotruck.LeftObstacle1.ToString(),
                                    Obstacle2 = this.CurrentAutotruck.LeftObstacle2.ToString(),
                                    Obstacle3 = this.CurrentAutotruck.LeftObstacle3.ToString(),
                                    Obstacle4 = this.CurrentAutotruck.LeftObstacle4.ToString(),
                                    Obstacle5 = this.CurrentAutotruck.LeftObstacle5.ToString(),
                                    Obstacle6 = this.CurrentAutotruck.LeftObstacle6.ToString(),
                                    ResultCode = eEquInfCmdResultCode.Ĭ��.ToString(),
                                    DataFlag = 0
                                };

                                // ���Ͳ����ƻ�
                                if (commonDAO.SelfDber.Insert<InfQCJXCYSampleCMD>(CurrentSampleCMD) > 0)
                                    this.CurrentFlowFlag = eFlowFlag.�ȴ�����;
                            }
                            else
                            {
                                this.UpdateLedShow("δ�ҵ���������Ϣ");
                                this.voiceSpeaker.Speak("δ�ҵ���������Ϣ������ϵ����Ա", 1, false);

                                timer1.Interval = 5000;
                            }
                        }
                        else
                        {
                            this.UpdateLedShow("������δ����");
                            this.voiceSpeaker.Speak("������δ����", 1, false);

                            timer1.Interval = 5000;
                        }

                        #endregion
                        break;

                    case eFlowFlag.�ȴ�����:
                        #region

                        // �жϲ����Ƿ����
                        InfQCJXCYSampleCMD qCJXCYSampleCMD = commonDAO.SelfDber.Get<InfQCJXCYSampleCMD>(this.CurrentSampleCMD.Id);
                        if (qCJXCYSampleCMD.ResultCode == eEquInfCmdResultCode.�ɹ�.ToString())
                        {
                            if (jxSamplerDAO.SaveBuyFuelTransport(this.CurrentBuyFuelTransport.Id, DateTime.Now, CommonAppConfig.GetInstance().AppIdentifier))
                            {
                                FrontGateUp();

                                this.UpdateLedShow("�������", " ���뿪");
                                this.voiceSpeaker.Speak("������ϣ����뿪", 1, false);

                                this.CurrentFlowFlag = eFlowFlag.�ȴ��뿪;
                            }
                        }

                        // ����������
                        timer1.Interval = 4000;

                        #endregion
                        break;

                    case eFlowFlag.�ȴ��뿪:

                        BackGateUp();

                        // �������򼰵�բ �ظ����ź�
                        ResetBuyFuel();

                        // ����������
                        timer1.Interval = 4000;

                        break;
                }

                //// ���еظ����ź�ʱ����
                //if (this.AutoHandMode && !this.InductorCoil1 && !this.InductorCoil2 && !this.InductorCoil3 && !this.InductorCoil4 && this.CurrentFlowFlag != eFlowFlag.�ȴ�����
                //    && this.CurrentImperfectCar != null) ResetBuyFuel();

                RefreshEquStatus();
            }
            catch (Exception ex)
            {
                Log4Neter.Error("timer1_Tick", ex);
            }
            finally
            {
                timer1.Start();
            }

            timer1.Start();
        }

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

        /// <summary>
        /// �л��ֶ�/�Զ�ģʽ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sbtnChangeAutoHandMode_ValueChanged(object sender, EventArgs e)
        {
            this.AutoHandMode = sbtnChangeAutoHandMode.Value;
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
        }

        #endregion

        #region �볧úҵ��

        /// <summary>
        /// �����볧ú�����¼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSendSamplingPlan_Click(object sender, EventArgs e)
        {
            if (SendSamplingPlan()) MessageBoxEx.Show("����ʧ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// �����볧ú�����¼
        /// </summary>
        /// <returns></returns>
        bool SendSamplingPlan()
        {
            if (this.CurrentBuyFuelTransport == null)
            {
                MessageBoxEx.Show("��ѡ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            this.CurrentFlowFlag = eFlowFlag.���ͼƻ�;

            return false;
        }

        /// <summary>
        /// �����볧ú�����¼
        /// </summary>
        void ResetBuyFuel()
        {
            this.CurrentFlowFlag = eFlowFlag.�ȴ�����;

            this.CurrentAutotruck = null;
            this.CurrentBuyFuelTransport = null;

            txtTagId.ResetText();
            txtSampleCode.ResetText();

            btnSendSamplingPlan.Enabled = false;

            FrontGateDown();
            BackGateDown();

            UpdateLedShow("  �ȴ�����");

            // �������
            this.CurrentImperfectCar = null;
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetBuyFuel();
        }

        /// <summary>
        /// ��ȡδ��ɵ��볧ú��¼
        /// </summary>
        void LoadTodayUnFinishBuyFuelTransport()
        {
            superGridControl1.PrimaryGrid.DataSource = jxSamplerDAO.GetUnFinishBuyFuelTransport();
        }

        /// <summary>
        /// ��ȡָ����������ɵ��볧ú��¼
        /// </summary>
        void LoadTodayFinishBuyFuelTransport()
        {
            superGridControl2.PrimaryGrid.DataSource = jxSamplerDAO.GetFinishedBuyFuelTransport(DateTime.Now.Date, DateTime.Now.Date.AddDays(1));
        }

        #endregion

        #region ������Ϣ

        /// <summary>
        /// ѡ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectAutotruck_Click(object sender, EventArgs e)
        {
            FrmUnFinishTransport_Select frm = new FrmUnFinishTransport_Select("where CarType='" + eCarType.�볧ú.ToString() + "' order by CreateDate desc");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                passCarQueuer.Enqueue(frm.Output.CarNumber);
                this.CurrentFlowFlag = eFlowFlag.��֤����;
            }
        }

        #endregion

        #region ����

        Pen redPen3 = new Pen(Color.Red, 3);
        Pen greenPen3 = new Pen(Color.Lime, 3);

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

                int height = 12;

                // ���Ƶظ�1
                e.Graphics.DrawLine(this.InductorCoil1 ? redPen3 : greenPen3, 15, 1, 15, height);
                e.Graphics.DrawLine(this.InductorCoil1 ? redPen3 : greenPen3, 15, panel.Height - height, 15, panel.Height - 1);
                // ���Ƶظ�2
                e.Graphics.DrawLine(this.InductorCoil2 ? redPen3 : greenPen3, 25, 1, 25, height);
                e.Graphics.DrawLine(this.InductorCoil2 ? redPen3 : greenPen3, 25, panel.Height - height, 25, panel.Height - 1);
                // ���Ƶظ�3
                e.Graphics.DrawLine(this.InductorCoil3 ? redPen3 : greenPen3, 406, 1, 406, height);
                e.Graphics.DrawLine(this.InductorCoil3 ? redPen3 : greenPen3, 406, panel.Height - height, 406, panel.Height - 1);
                // ���Ƶظ�4
                e.Graphics.DrawLine(this.InductorCoil4 ? redPen3 : greenPen3, panel.Width - 15, 1, panel.Width - 15, height);
                e.Graphics.DrawLine(this.InductorCoil4 ? redPen3 : greenPen3, panel.Width - 15, panel.Height - height, panel.Width - 15, panel.Height - 1);
            }
            catch (Exception ex)
            {
                Log4Neter.Error("panCurrentCarNumber_Paint�쳣", ex);
            }
        }

        private void superGridControl_BeginEdit(object sender, DevComponents.DotNetBar.SuperGrid.GridEditEventArgs e)
        {
            // ȡ������༭
            e.Cancel = true;
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

        /// <summary>
        /// ���²�����״̬
        /// </summary>
        private void RefreshEquStatus()
        {
            string systemStatus = commonDAO.GetSignalDataValue(this.SamplerMachineCode, eSignalDataName.ϵͳ.ToString());
            eEquInfSamplerSystemStatus result;
            if (Enum.TryParse(systemStatus, out result)) SamplerSystemStatus = result;
        }

        #endregion
    }
}
