using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CMCS.ChipChangeCode.Enums;
using CMCS.ChipChangeCode.Frms;
using CMCS.Common;
using CMCS.Common.DAO;
using CMCS.Common.Entities;
using CMCS.Common.Entities.Fuel;
using CMCS.Common.Enums;
using CMCS.Common.Utilities;
using CMCS.Forms.UserControls;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Metro;

namespace CMCS.ChipChangeCode.Frms
{
    public partial class FrmMakeCheck : MetroForm
    {
        RW.HFReader.HFReaderRwer Rwer = new RW.HFReader.HFReaderRwer();

        public FrmMakeCheck()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体唯一标识符
        /// </summary>
        public static string UniqueKey = "FrmMakeCheck";

        #region Vars

        CodePrinter _CodePrinter = null;

        eFlowFlag currentFlowFlag = eFlowFlag.等待扫码;
        /// <summary>
        /// 当前流程标识
        /// </summary>
        public eFlowFlag CurrentFlowFlag
        {
            get { return currentFlowFlag; }
            set
            {
                currentFlowFlag = value;
                lblCurrentFlowFlag.Text = value.ToString();

                switch (value)
                {
                    case eFlowFlag.等待扫码:
                        txtInputMakeCode.ResetText();
                        txtInputMakeCode.Focus();
                        break;
                    case eFlowFlag.打印二维码:
                        txtInputMakeCode.ButtonCustom.Enabled = true;
                        txtInputMakeCode.Focus();
                        break;
                }
            }
        }

        string resMessage = string.Empty;

        #endregion

        public void InitFrom()
        {
            this._CodePrinter = new CodePrinter(printDocument1);
        }

        private void FrmMakeCheck_Load(object sender, EventArgs e)
        {
            //初始化
            InitFrom();
            //初始化设备
            InitHardware();
        }

        private void FrmMakeCheck_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnloadHardware();
        }

        #region 设备初始化与卸载

        /// <summary>
        /// 初始化外接设备
        /// </summary>
        private void InitHardware()
        {
            try
            {
                bool success = false;

                success = Rwer.OpenNetPort(ThisAppConfig.GetInstance().HFReaderIP, ThisAppConfig.GetInstance().HFReaderPort);

                if (success) ShowMessage("读卡器初始化成功", eOutputType.Important); else ShowMessage("读卡器初始化失败", eOutputType.Error);

                timer1.Enabled = true;
            }
            catch (Exception ex)
            {
                ShowMessage("设备初始化失败" + ex.Message, eOutputType.Error);
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
        }
        #endregion

        #region 称重校验业务
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            try
            {
                #region 制样称重校验
                switch (this.CurrentFlowFlag)
                {
                    case eFlowFlag.等待扫码:

                        string readCode = Rwer.Byte16ToString(Rwer.RWRead14443A(0, 2)).Replace("\0", "");
                        if (!string.IsNullOrWhiteSpace(readCode))
                        {
                            //this.CurrentFlowFlag = eFlowFlag.打印二维码;
                            //txtInputMakeCode.Text = readCode;

                            System.Threading.Thread.Sleep(20);
                            SendKeys.SendWait(readCode.ToString());
                            System.Threading.Thread.Sleep(20);
                            SendKeys.SendWait("\r");

                            Restet();
                        }

                        break;
                    case eFlowFlag.打印二维码:

                        PrintAssayCode();

                        break;
                }
                #endregion
            }
            catch (Exception ex)
            {
                ShowMessage("Timer1运行异常" + ex.Message, eOutputType.Error);
            }
            timer1.Start();
        }

        /// <summary>
        /// 重置
        /// </summary>
        private void Restet()
        {
            this.CurrentFlowFlag = eFlowFlag.等待扫码;

            txtInputMakeCode.ButtonCustom.Enabled = false;
            txtInputMakeCode.ResetText();
            rtxtMakeCheckInfo.ResetText();

            // 方便客户快速使用，获取焦点
            txtInputMakeCode.Focus();
        }

        /// <summary>
        /// 打印二维码
        /// </summary>
        private void PrintAssayCode()
        {
            this.CurrentFlowFlag = eFlowFlag.打印二维码;

            if (MessageBoxEx.Show("立刻打印二维码？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this._CodePrinter.Print(txtInputMakeCode.Text);

                Restet();
            }
            else
                Restet();
        }

        /// <summary>
        /// 制样码回车
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtInputMakeCode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(txtInputMakeCode.Text))
                {
                    this.CurrentFlowFlag = eFlowFlag.打印二维码;
                }
            }
        }

        #endregion

        #region 操作

        /// <summary>
        /// 获取焦点时清空制样码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtInputMakeCode_Enter(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtInputMakeCode.Text))
            {
                Restet();
            }
        }

        /// <summary>
        /// 打印二维码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMakeCheckCode_ButtonCustomClick(object sender, EventArgs e)
        {
            PrintAssayCode();
        }


        #endregion

        #region 其他

        private void ShowMessage(string info, eOutputType outputType)
        {
            OutputRunInfo(rtxtMakeCheckInfo, info, outputType);
        }

        /// <summary>
        /// 输出运行信息
        /// </summary>
        /// <param name="richTextBox"></param>
        /// <param name="text"></param>
        /// <param name="outputType"></param>
        private void OutputRunInfo(RichTextBoxEx richTextBox, string text, eOutputType outputType = eOutputType.Normal)
        {
            this.Invoke((EventHandler)(delegate
            {
                if (richTextBox.TextLength > 100000) richTextBox.Clear();

                text = string.Format("{0}  {1}", DateTime.Now.ToString("HH:mm:ss"), text);

                richTextBox.SelectionStart = richTextBox.TextLength;

                switch (outputType)
                {
                    case eOutputType.Normal:
                        richTextBox.SelectionColor = ColorTranslator.FromHtml("#BD86FA");
                        break;
                    case eOutputType.Important:
                        richTextBox.SelectionColor = ColorTranslator.FromHtml("#A50081");
                        break;
                    case eOutputType.Warn:
                        richTextBox.SelectionColor = ColorTranslator.FromHtml("#F9C916");
                        break;
                    case eOutputType.Error:
                        richTextBox.SelectionColor = ColorTranslator.FromHtml("#DB2606");
                        break;
                    default:
                        richTextBox.SelectionColor = Color.White;
                        break;
                }

                richTextBox.AppendText(string.Format("{0}\r", text));

                richTextBox.ScrollToCaret();

            }));
        }

        /// <summary>
        /// 输出信息类型
        /// </summary>
        public enum eOutputType
        {
            /// <summary>
            /// 普通
            /// </summary>
            [Description("#BD86FA")]
            Normal,
            /// <summary>
            /// 重要
            /// </summary>
            [Description("#A50081")]
            Important,
            /// <summary>
            /// 警告
            /// </summary>
            [Description("#F9C916")]
            Warn,
            /// <summary>
            /// 错误
            /// </summary>
            [Description("#DB2606")]
            Error
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
