using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CMCS.Common;
using CMCS.Common.DAO;
using CMCS.Common.Entities;
using CMCS.Common.Entities.BaseInfo;
using CMCS.Common.Entities.Inf;
using CMCS.Common.Entities.Sys;
using CMCS.Common.Enums;
using CMCS.Common.Utilities;
using CMCS.Forms.UserControls;
using CMCS.Monitor.Win.Core;
using CMCS.Monitor.Win.Frm.Sys;
using CMCS.Monitor.Win.Frms;
using CMCS.Monitor.Win.Frms.Sys;
using CMCS.Monitor.Win.Utilities;
//
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Metro;
using Xilium.CefGlue;

namespace CMCS.Monitor.Win.Frms.Sys
{
    public partial class FrmMainFrame : MetroForm
    {
        CommonDAO commonDAO = CommonDAO.GetInstance();

        public static SuperTabControlManager superTabControlManager;

        public FrmMainFrame()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblVersion.Text = new AU.Updater().Version;

            FrmMainFrame.superTabControlManager = new SuperTabControlManager(this.superTabControl1);
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (SelfVars.LoginUser != null) lblLoginUserName.Text = SelfVars.LoginUser.UserName;

            CommonDAO.GetInstance().ResetAllSysMessageStatus();

            // 打开集中管控首页
            btnOpenHomePage_Click(null, null);

            InitEquipmentStatus();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (MessageBoxEx.Show("确认退出系统？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    CefRuntime.Shutdown();
                    Application.Exit();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// 退出系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnApplicationExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 显示当前时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_CurrentTime_Tick(object sender, EventArgs e)
        {
            lblCurrentTime.Text = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss");
        }

        #region 打开/切换可视主界面

        #region 弹出窗体

        /// <summary>
        /// 打开集中管控首页
        /// </summary>
        public void OpenHomePage()
        {
            this.InvokeEx(() =>
            {
                string uniqueKey = FrmHomePage.UniqueKey;

                if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
                {
                    FrmHomePage Frm = new FrmHomePage();
                    FrmMainFrame.superTabControlManager.CreateTab(Frm.Text, uniqueKey, Frm, false);
                }
                else
                    FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
            });
        }

        /// <summary>
        /// 打开汽车入厂记录查询
        /// </summary>
        public void OpenBuyFuelLoadToday()
        {
            this.InvokeEx(() =>
            {
                string uniqueKey = FrmBuyFuelLoadToday.UniqueKey;

                if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
                {
                    FrmBuyFuelLoadToday item = new FrmBuyFuelLoadToday();
                    FrmMainFrame.superTabControlManager.CreateTab(item.Text, uniqueKey, item, true);
                }
                else
                    FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
            });
        }

        /// <summary>
        /// 打开汽车机械采样机监控
        /// </summary>
        public void OpenTruckMachinerySampler()
        {
            this.InvokeEx(() =>
            {
                string uniqueKey = FrmCarSampler.UniqueKey;

                if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
                {
                    FrmCarSampler frm = new FrmCarSampler();
                    FrmMainFrame.superTabControlManager.CreateTab(frm.Text, uniqueKey, frm, false);
                }
                else
                    FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
            });
        }

        /// <summary>
        /// 打开汽车过衡监控
        /// </summary>
        public void OpenTruckWeighter()
        {
            this.InvokeEx(() =>
            {
                string uniqueKey = FrmTruckWeighter.UniqueKey;

                if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
                {
                    SelfVars.TruckWeighterForm = new FrmTruckWeighter();
                    FrmMainFrame.superTabControlManager.CreateTab(SelfVars.TruckWeighterForm.Text, uniqueKey, SelfVars.TruckWeighterForm, false);
                }
                else
                    FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
            });
        }

        /// <summary>
        /// 打开汽车原煤仓监控
        /// </summary>
        public void OpenTruckOrder()
        {
            this.InvokeEx(() =>
            {
                string uniqueKey = FrmTruckOrder.UniqueKey;

                if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
                {
                    FrmTruckOrder frm = new FrmTruckOrder();
                    FrmMainFrame.superTabControlManager.CreateTab(frm.Text, uniqueKey, frm, false);
                }
                else
                    FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
            });
        }

        /// <summary>
        /// 打开汽车机械采样监控
        /// </summary>
        public void OpenCarSampler()
        {
            this.InvokeEx(() =>
            {
                string uniqueKey = FrmCarSampler.UniqueKey;

                if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
                {
                    SelfVars.CarSamplerForm = new FrmCarSampler();
                    FrmMainFrame.superTabControlManager.CreateTab(SelfVars.CarSamplerForm.Text, uniqueKey, SelfVars.CarSamplerForm, false);
                }
                else
                    FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
            });
        }

        /// <summary>
        /// 打开皮带采样机监控
        /// </summary>
        public void OpenTrainBeltSampler()
        {
            this.Invoke((Action)(() =>
            {
                string uniqueKey = FrmTrainBeltSampler.UniqueKey;

                if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
                {
                    FrmTrainBeltSampler frmTrainBeltSampler = new FrmTrainBeltSampler();
                    FrmMainFrame.superTabControlManager.CreateTab(frmTrainBeltSampler.Text, uniqueKey, frmTrainBeltSampler, false);
                }
                else
                    FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
            }));
        }

        /// <summary>
        /// 打开全自动制样机监控
        /// </summary>
        public void OpenAutoMaker()
        {
            this.Invoke((Action)(() =>
            {
                string uniqueKey = FrmAutoMaker.UniqueKey;

                if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
                {
                    FrmAutoMaker frmAutoMaker = new FrmAutoMaker();
                    FrmMainFrame.superTabControlManager.CreateTab(frmAutoMaker.Text, uniqueKey, frmAutoMaker, false);
                }
                else
                    FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
            }));
        }

        /// <summary>
        /// 打开化验室网络管理监控
        /// </summary>
        public void OpenAssayManage()
        {
            this.InvokeEx(() =>
            {
                string uniqueKey = FrmAssayManage.UniqueKey;

                if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
                {
                    FrmAssayManage frm = new FrmAssayManage();
                    FrmMainFrame.superTabControlManager.CreateTab(frm.Text, uniqueKey, frm, false);
                }
                else
                    FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
            });
        }

        /// <summary>
        /// 打开汽车监控界面
        /// </summary>
        public void OpenCarMonitor()
        {
            this.InvokeEx(() =>
            {
                string uniqueKey = FrmCarMonitor.UniqueKey;

                if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
                {
                    FrmCarMonitor frm = new FrmCarMonitor();
                    FrmMainFrame.superTabControlManager.CreateTab(frm.Text, uniqueKey, frm, false);
                }
                else
                    FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
            });
        }

        /// <summary>
        /// 打开设备异常查询
        /// </summary>
        public void OpenEquInfHitch()
        {
            string uniqueKey = FrmEquInfHitch.UniqueKey;

            if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
            {
                FrmEquInfHitch item = new FrmEquInfHitch();
                FrmMainFrame.superTabControlManager.CreateTab(item.Text, uniqueKey, item, true);
            }
            else
                FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
        }

        /// <summary>
        /// 打开门禁进出记录
        /// </summary>
        public void OpenGuardInfo()
        {
            string uniqueKey = FrmInfGuardInfo.UniqueKey;

            if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
            {
                FrmInfGuardInfo item = new FrmInfGuardInfo();
                FrmMainFrame.superTabControlManager.CreateTab(item.Text, uniqueKey, item, true);
            }
            else
                FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
        }

        /// <summary>
        /// 打开存样柜管理
        /// </summary>
        public void OpenSampleStorage()
        {
            string uniqueKey = FrmSampleStorage.UniqueKey;

            if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
            {
                FrmSampleStorage item = new FrmSampleStorage();
                FrmMainFrame.superTabControlManager.CreateTab(item.Text, uniqueKey, item, false);
            }
            else
                FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
        }

        /// <summary>
        /// 打开视频预览
        /// </summary>
        /// <param name="param"></param>
        /// <param name="videoName"></param>
        public void OpenHikVideo(string videoName)
        {
            this.BeginInvoke((Action)(() =>
            {
                if (!string.IsNullOrEmpty(videoName))
                {
                    FrmHikVideo frm = new FrmHikVideo(videoName);
                    frm.ShowDialog();
                }
                else
                    MessageBoxEx.Show("视频参数未配置");
            }));
        }

        #endregion

        /// <summary>
        /// FrmCefTester
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCefTester_Click(object sender, EventArgs e)
        {
            //this.InvokeEx(() =>
            //{
            string uniqueKey = FrmCefTester.UniqueKey;

            if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
            {
                FrmCefTester Frm = new FrmCefTester();
                FrmMainFrame.superTabControlManager.CreateTab(Frm.Text, uniqueKey, Frm, false);
            }
            else
                FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
            //});
        }

        public void SetColorTable(string controlName)
        {
            if (string.IsNullOrEmpty(controlName)) return;
            foreach (Control item in panel_Buttons.Controls)
            {
                if (item.GetType().Name != "ButtonX" && item.GetType().Name != "ButtonItem")
                    continue;
                ButtonX button = (ButtonX)item;
                if (item.Name == controlName || (button.SubItems.Count > 0 && button.SubItems.Contains(controlName)))
                    button.ColorTable = eButtonColor.Magenta;
                else
                    button.ColorTable = eButtonColor.BlueWithBackground;
            }
        }

        /// <summary>
        /// 打开集中管控首页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenHomePage_Click(object sender, EventArgs e)
        {
            ButtonX button = (ButtonX)sender;
            SetColorTable(button != null ? button.Name : "");

            OpenHomePage();
        }

        /// <summary>
        /// 打开入厂煤计量查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenBuyFuelLoad_Click(object sender, EventArgs e)
        {
            ButtonItem button = (ButtonItem)sender;
            SetColorTable(button != null ? button.Name : "");

            OpenBuyFuelLoadToday();
        }

        /// <summary>
        /// 打开汽车机械采样机界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenCarSampler_Click(object sender, EventArgs e)
        {
            ButtonItem button = (ButtonItem)sender;
            SetColorTable(button != null ? button.Name : "");

            OpenCarSampler();
        }

        /// <summary>
        /// 打开化验室网络管理界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenAssayManage_Click(object sender, EventArgs e)
        {
            ButtonX button = (ButtonX)sender;
            SetColorTable(button != null ? button.Name : "");

            OpenAssayManage();
        }

        /// <summary>
        /// 打开汽车监控界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCarMonitor_Click(object sender, EventArgs e)
        {
            ButtonX button = (ButtonX)sender;
            SetColorTable(button != null ? button.Name : "");

            OpenCarMonitor();
        }

        /// <summary>
        /// 打开设备异常查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenEquInfHitch_Click(object sender, EventArgs e)
        {
            ButtonX button = (ButtonX)sender;
            SetColorTable(button != null ? button.Name : "");

            OpenEquInfHitch();
        }

        /// <summary>
        /// 打开门禁记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenGuardInfo_Click(object sender, EventArgs e)
        {
            ButtonX button = (ButtonX)sender;
            SetColorTable(button != null ? button.Name : "");

            OpenGuardInfo();
        }

        /// <summary>
        /// 打开汽车过衡监控
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenTruckWeighter_Click(object sender, EventArgs e)
        {
            ButtonItem button = (ButtonItem)sender;
            SetColorTable(button != null ? button.Name : "");

            OpenTruckWeighter();
        }

        /// <summary>
        /// 打开皮带采样机监控
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenTrainBeltSampler_Click(object sender, EventArgs e)
        {
            ButtonItem button = (ButtonItem)sender;
            SetColorTable(button != null ? button.Name : "");

            OpenTrainBeltSampler();
        }

        /// <summary>
        /// 打开全自动制样机监控
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenAutoMaker_Click(object sender, EventArgs e)
        {
            ButtonItem button = (ButtonItem)sender;
            SetColorTable(button != null ? button.Name : "");

            OpenAutoMaker();
        }

        /// <summary>
        /// 打开存样柜管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSampleStorage_Click(object sender, EventArgs e)
        {
            ButtonX button = (ButtonX)sender;
            SetColorTable(button != null ? button.Name : "");

            OpenSampleStorage();
        }

        #endregion

        #region 设备状态监控

        /// <summary>
        /// 初始化设备状态任务
        /// </summary>
        private void InitEquipmentStatus()
        {
            timer_EquipmentStatus.Enabled = true;

            List<CmcsCMEquipment> list = commonDAO.GetChildrenMachinesByCode("机械采样机");
            list.AddRange(commonDAO.GetChildrenMachinesByCode("全自动制样机"));
            list.AddRange(commonDAO.GetChildrenMachinesByCode("皮带采样机"));

            CreateEquipmentStatus(list);

            // 更新设备状态
            RefreshEquipmentStatus();
        }

        /// <summary>
        /// 创建设备状态控件
        /// </summary>
        /// <param name="list"></param>
        private void CreateEquipmentStatus(List<CmcsCMEquipment> list)
        {
            flpanEquipments.SuspendLayout();

            foreach (CmcsCMEquipment cMEquipment in list)
            {
                UCtrlSignalLight uCtrlSignalLight = new UCtrlSignalLight()
                {
                    Anchor = AnchorStyles.Left,
                    Width = 16,
                    Height = 16,
                    Tag = cMEquipment.EquipmentCode,
                    Padding = new System.Windows.Forms.Padding(10, 0, 0, 0)
                };
                SetSystemStatusToolTip(uCtrlSignalLight);

                flpanEquipments.Controls.Add(uCtrlSignalLight);

                LabelX lblMachineName = new LabelX()
                {
                    Text = cMEquipment.EquipmentName,
                    Tag = cMEquipment.EquipmentCode,
                    AutoSize = true,
                    Anchor = AnchorStyles.Left,
                    Font = new Font("Segoe UI", 10f, FontStyle.Regular)
                };

                flpanEquipments.Controls.Add(lblMachineName);
            }

            flpanEquipments.ResumeLayout();
        }

        /// <summary>
        /// 更新设备状态
        /// </summary>
        private void RefreshEquipmentStatus()
        {
            foreach (UCtrlSignalLight uCtrlSignalLight in flpanEquipments.Controls.OfType<UCtrlSignalLight>())
            {
                if (uCtrlSignalLight.Tag == null) continue;

                string machineCode = uCtrlSignalLight.Tag.ToString();
                if (string.IsNullOrEmpty(machineCode)) continue;

                string systemStatus = CommonDAO.GetInstance().GetSignalDataValue(machineCode, eSignalDataName.系统.ToString());
                if ("|就绪待机|".Contains("|" + systemStatus + "|"))
                    uCtrlSignalLight.LightColor = EquipmentStatusColors.BeReady;
                else if ("|正在运行|正在卸样|".Contains("|" + systemStatus + "|"))
                    uCtrlSignalLight.LightColor = EquipmentStatusColors.Working;
                else if ("|发生故障|".Contains("|" + systemStatus + "|"))
                    uCtrlSignalLight.LightColor = EquipmentStatusColors.Breakdown;
                else
                    uCtrlSignalLight.LightColor = EquipmentStatusColors.Forbidden;
            }
        }

        /// <summary>
        /// 设置ToolTip提示
        /// </summary>
        private void SetSystemStatusToolTip(Control control)
        {
            this.toolTip1.SetToolTip(control, "<绿色> 就绪待机\r\n<红色> 正在运行\r\n<黄色> 发生故障");
        }

        private void timer_EquipmentStatus_Tick(object sender, EventArgs e)
        {
            // 更新设备状态
            RefreshEquipmentStatus();
        }

        #endregion

        #region 显示消息框

        /// <summary>
        /// 显示消息框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_MsgTime_Tick(object sender, EventArgs e)
        {
            timer_MsgTime.Stop();

            if (DateTime.Now.Second % 30 == 0)
                //30秒获取一次异常信息表
                ShowEquInfHitch();
            if (DateTime.Now.Second % 5 == 0)
                ShowSysMessage();

            timer_MsgTime.Start();
        }

        /// <summary>
        /// 显示设备异常消息框
        /// </summary>
        public void ShowEquInfHitch()
        {
            List<InfEquInfHitch> listResult = CommonDAO.GetInstance().GetWarnEquInfHitch();
            StringBuilder sbHitchDescribe = new StringBuilder();
            if (listResult.Count > 0)
            {
                foreach (InfEquInfHitch item in listResult)
                {
                    sbHitchDescribe.Append("<font color='red' size='2'>");
                    sbHitchDescribe.Append(item.HitchTime.ToString("HH:mm") + "   " + item.HitchDescribe + "<br>");
                    sbHitchDescribe.Append("</font>");
                    CommonDAO.GetInstance().UpdateReadEquInfHitch(item.Id);
                }
                //右下角显示
                FrmSysMsg frm_sysMsg = new FrmSysMsg(sbHitchDescribe.ToString(), false);
                frm_sysMsg.Show();
            }
        }

        /// <summary>
        /// 显示系统消息
        /// </summary>
        public void ShowSysMessage()
        {
            CmcsSysMessage entity = CommonDAO.GetInstance().GetTodayTopSysMessage();
            if (entity != null)
            {
                CommonDAO.GetInstance().ChangeSysMessageStatus(entity.Id, eSysMessageStatus.处理中);

                FrmSysMsg frmSysMsg = new FrmSysMsg(entity);
                frmSysMsg.OnMsgHandler += new FrmSysMsg.MsgHandler(frmSysMsg_OnMsgHandler);

            }
        }

        void frmSysMsg_OnMsgHandler(string msgId, string msgCode, string jsonStr, string buttonText, Form frmMsg)
        {
            switch (buttonText)
            {
                case "查看":

                    CommonDAO.GetInstance().ChangeSysMessageStatus(msgId, eSysMessageStatus.已处理);

                    switch (msgCode)
                    {
                        case "汽车桥式采样机":
                            break;
                    }

                    frmMsg.Close();
                    break;
                case "我知道了":
                    frmMsg.Close();
                    break;
                default:
                    frmMsg.Close();
                    break;
            }
        }
        #endregion

        private void buttonX1_Click(object sender, EventArgs e)
        {
            OpenTruckWeighter();
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

    }
}
