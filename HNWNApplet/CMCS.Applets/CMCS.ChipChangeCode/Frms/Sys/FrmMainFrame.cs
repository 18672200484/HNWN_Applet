using System;
using System.Windows.Forms;
using CMCS.ChipChangeCode.Frms;
using CMCS.ChipChangeCode.Utilities;
using CMCS.Common;
using CMCS.Common.DAO;
using CMCS.Common.Enums;
//
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Metro;

namespace CMCS.ChipChangeCode.Frms.Sys
{
    public partial class FrmMainFrame : MetroForm
    {
        public static SuperTabControlManager superTabControlManager;

        public FrmMainFrame()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblVersion.Text = new AU.Updater().Version;

            this.superTabControl1.Tabs.Clear();
            FrmMainFrame.superTabControlManager = new SuperTabControlManager(this.superTabControl1);

            OpenWeighCheck();

            this.ShowInTaskbar = false;
            this.WindowState = FormWindowState.Minimized;//使当前窗体最小化
            notifyIcon1.Visible = true;//使最下滑的图标可见
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (MessageBoxEx.Show("确认退出系统？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
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

        private void timer_CurrentTime_Tick(object sender, EventArgs e)
        {
            lblCurrentTime.Text = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss");
        }

        #region 打开/切换可视主界面

        #region 弹出窗体

        /// <summary>
        /// 打开称重校验界面
        /// </summary>
        public void OpenWeighCheck()
        {
            string uniqueKey = FrmMakeCheck.UniqueKey;

            if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
            {
                FrmMakeCheck frm = new FrmMakeCheck();
                FrmMainFrame.superTabControlManager.CreateTab(frm.Text, uniqueKey, frm, false, false);
            }
            else
                FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
        }

        #endregion

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized) //判断当前窗体的状态是否为最小化
            {
                //this.ShowInTaskbar = true;
                this.WindowState = FormWindowState.Normal;//将当前窗体状态恢复为正常
                notifyIcon1.Visible = false;//将notifyIcon图标隐藏
            }
        }

        #endregion

        private void FrmMainFrame_Resize(object sender, EventArgs e)
        {
            //如果当前状态的状态为最小化，则显示状态栏的程序托盘  
            if (this.WindowState == FormWindowState.Minimized)
            {
                //不在Window任务栏中显示  
                //this.ShowInTaskbar = false;
                //使图标在状态栏中显示  
                this.notifyIcon1.Visible = true;
            }
        }
    }
}
