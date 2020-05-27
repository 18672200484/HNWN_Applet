using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using CMCS.DumblyConcealer.Win.Core;
using CMCS.Common.Utilities;
using CMCS.Common.DAO;
using CMCS.DumblyConcealer.Tasks.BeltBalancer;
using BeltBalancer.SM6000;
using CMCS.DumblyConcealer.Enums;
using CMCS.DumblyConcealer.Tasks.BeltBalancer.Entities;

namespace CMCS.DumblyConcealer.Win.DumblyTasks
{
    /// <summary>
    /// Ƥ���ӳ�������ͬ������
    /// </summary>
    public partial class FrmBeltBalancer : TaskForm
    {
        RTxtOutputer rTxtOutputer;

        TaskSimpleScheduler taskSimpleScheduler = new TaskSimpleScheduler();

        CommonDAO commonDAO = CommonDAO.GetInstance();

        EquBeltBalancerDAO balancerDAO = EquBeltBalancerDAO.GetInstance();

        /// <summary>
        /// �����1
        /// </summary>
        public static byte[] Cmd1 = new byte[] { 0x01, 0x03, 0x90, 0x00, 0x00, 0x03, 0x28, 0xCB };

        /// <summary>
        /// �����2
        /// </summary>
        public static byte[] Cmd2 = new byte[] { 0x02, 0x03, 0x90, 0x00, 0x00, 0x06, 0xE8, 0xFB };

        SM6000ABber bber1 = new SM6000ABber(Cmd1, 1000);

        SM6000ABber bber2 = new SM6000ABber(Cmd2, 1000);

        float total1 = 0; float instant1 = 0; float speed1 = 0;

        float total2 = 0; float instant2 = 0; float speed2 = 0;

        DateTime prevHistoryTime1 = DateTime.Now.AddMinutes(-10);

        DateTime prevHistoryTime2 = DateTime.Now.AddMinutes(-10);

        public FrmBeltBalancer()
        {
            InitializeComponent();
        }

        private void FrmBeltBalancer_Load(object sender, EventArgs e)
        {
            this.Text = "Ƥ���Ӽ�������";

            this.rTxtOutputer = new RTxtOutputer(rtxtOutput);

            ExecuteAllTask();
        }

        /// <summary>
        /// ִ����������
        /// </summary>
        void ExecuteAllTask()
        {
            taskSimpleScheduler.StartNewTask("��ʼ����·Ƥ����", () =>
            {
                bber1.OnDataReceived += Bber1_OnDataReceived;
                bber1.OnReceiveError += Bber1_OnReceiveError;
                if (bber1.OpenCom(commonDAO.GetAppletConfigInt32("��·Ƥ���Ӵ��ں�"), commonDAO.GetAppletConfigInt32("��·Ƥ���Ӳ�����"), 8, System.IO.Ports.StopBits.One, System.IO.Ports.Parity.None))
                    this.rTxtOutputer.Output("��·Ƥ���Ӵ��ڴ򿪳ɹ�", eOutputType.Important);
                else
                    this.rTxtOutputer.Output("��·Ƥ���Ӵ��ڴ�ʧ��", eOutputType.Error);
            });

            taskSimpleScheduler.StartNewTask("��ʼ����·Ƥ����", () =>
            {
                bber2.OnDataReceived += Bber2_OnDataReceived;
                bber2.OnReceiveError += Bber2_OnReceiveError;
                if (bber2.OpenCom(commonDAO.GetAppletConfigInt32("��·Ƥ���Ӵ��ں�"), commonDAO.GetAppletConfigInt32("��·Ƥ���Ӳ�����"), 8, System.IO.Ports.StopBits.One, System.IO.Ports.Parity.None))
                    this.rTxtOutputer.Output("��·Ƥ���Ӵ��ڴ򿪳ɹ�", eOutputType.Important);
                else
                    this.rTxtOutputer.Output("��·Ƥ���Ӵ��ڴ�ʧ��", eOutputType.Error);
            });
        }

        #region ��·Ƥ����
        private void Bber1_OnReceiveError(Exception ex)
        {
            this.rTxtOutputer.Output("��ȡ��Ƥ��������" + ex.Message, eOutputType.Error);
        }

        private void Bber1_OnDataReceived(float total, float instant, float speed)
        {
            this.InvokeEx(() =>
            {
                lblTotal1.Text = total.ToString() + " t";
                lblInstant1.Text = instant.ToString() + " t/h";
                lblSpeed1.Text = speed.ToString() + " m/s";

                this.total1 = total;
                this.instant1 = instant;
                this.speed1 = speed;

                try
                {
                    CmcsBeltBalancerValue sisTagValue = balancerDAO.IUTagValue("��Ƥ�����ۼ���", total1.ToString(), "��Ƥ����");
                    balancerDAO.IUTagValue("��Ƥ����˲ʱ����", instant1.ToString(), "��Ƥ����");
                    // ÿһ���Ӳ����ۼ�����ʷ
                    if (prevHistoryTime1.ToString("yyyyMMddHHmm") != DateTime.Now.ToString("yyyyMMddHHmm"))
                    {
                        prevHistoryTime1 = DateTime.Now;
                        balancerDAO.InsertTagValueHistory(sisTagValue);
                    }
                }
                catch (Exception ex)
                {
                    this.rTxtOutputer.Output("�����Ƥ��������ʧ��" + ex.Message, eOutputType.Error);
                }
            });
        }
        #endregion

        #region ��·Ƥ����
        private void Bber2_OnReceiveError(Exception ex)
        {
            this.rTxtOutputer.Output("��ȡ��Ƥ��������" + ex.Message, eOutputType.Error);
        }

        private void Bber2_OnDataReceived(float total, float instant, float speed)
        {
            this.InvokeEx(() =>
            {
                lblTotal2.Text = total.ToString() + " t";
                lblInstant2.Text = instant.ToString() + " t/h";
                lblSpeed2.Text = speed.ToString() + " m/s";

                this.total2 = total;
                this.instant2 = instant;
                this.speed2 = speed;

                try
                {
                    CmcsBeltBalancerValue sisTagValue = balancerDAO.IUTagValue("��Ƥ�����ۼ���", total2.ToString(), "��Ƥ����");
                    balancerDAO.IUTagValue("��Ƥ����˲ʱ����", instant2.ToString(), "��Ƥ����");

                    // ÿһ���Ӳ����ۼ�����ʷ
                    if (prevHistoryTime2.ToString("yyyyMMddHHmm") != DateTime.Now.ToString("yyyyMMddHHmm"))
                    {
                        prevHistoryTime2 = DateTime.Now;
                        balancerDAO.InsertTagValueHistory(sisTagValue);
                    }
                }
                catch (Exception ex)
                {
                    this.rTxtOutputer.Output("������Ƥ��������ʧ��" + ex.Message, eOutputType.Error);
                }
            });
        }
        #endregion

        /// <summary>
        /// ����쳣��Ϣ
        /// </summary>
        /// <param name="text"></param>
        /// <param name="ex"></param>
        void OutputError(string text, Exception ex)
        {
            this.rTxtOutputer.Output(text + Environment.NewLine + ex.Message, eOutputType.Error);

            Log4Neter.Error(text, ex);
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
        /// ����رպ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmBeltBalancer_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                bber1.CloseCom();
                bber2.CloseCom();
            }
            catch { }

            // ע�⣺����ȡ������
            this.taskSimpleScheduler.Cancal();
        }

    }
}