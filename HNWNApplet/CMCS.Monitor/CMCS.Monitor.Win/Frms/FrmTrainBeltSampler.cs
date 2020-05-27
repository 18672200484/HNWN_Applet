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
using CMCS.Common.Enums;
using CMCS.Monitor.DAO;
using CMCS.Monitor.Win.CefGlue;
using CMCS.Monitor.Win.Core;
using CMCS.Monitor.Win.Html;
using CMCS.Monitor.Win.UserControls;
using DevComponents.DotNetBar.Metro;
using DevComponents.DotNetBar.SuperGrid;
using Xilium.CefGlue;
//
using Xilium.CefGlue.WindowsForms;
using CMCS.Common.Entities.Inf;

namespace CMCS.Monitor.Win.Frms
{
    public partial class FrmTrainBeltSampler : MetroForm
    {
        /// <summary>
        /// 窗体唯一标识符
        /// </summary>
        public static string UniqueKey = "FrmTrainBeltSampler";

        CefWebBrowserEx cefWebBrowser = new CefWebBrowserEx();

        Dictionary<string, string> SampleCodes = new Dictionary<string, string>();
        bool tempBool = true;

        public FrmTrainBeltSampler()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体初始化
        /// </summary>
        private void FormInit()
        {
#if DEBUG
            gboxTest.Visible = true;
#else
            gboxTest.Visible = false; 
#endif

            cefWebBrowser.StartUrl = SelfVars.Url_BeltSampler;
            cefWebBrowser.Dock = DockStyle.Fill;
            cefWebBrowser.WebClient = new HomePageCefWebClient(cefWebBrowser);
            cefWebBrowser.LoadEnd += new EventHandler<LoadEndEventArgs>(cefWebBrowser_LoadEnd);
            panWebBrower.Controls.Add(cefWebBrowser);
        }

        void cefWebBrowser_LoadEnd(object sender, LoadEndEventArgs e)
        {
            timer1.Enabled = true;
            timer2.Enabled = true;
            //页面初始化完成
            //ReadConfig();
        }

        private void FrmTrainBeltSampler_Load(object sender, EventArgs e)
        {
            FormInit();
        }

        private void superGridControl_CancelEdit_BeginEdit(object sender, GridEditEventArgs e)
        {
            // 取消编辑
            e.Cancel = true;
        }

        /// <summary>
        /// 打开卸样界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenUnload_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 测试 - 刷新页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            cefWebBrowser.Browser.Reload();
        }

        /// <summary>
        /// 测试 - 数据刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRequestData_Click(object sender, EventArgs e)
        {
            RequestData();

            //ReadConfig();
        }

        /// <summary>
        /// 请求数据
        /// </summary>
        void RequestData()
        {
            CommonDAO commonDAO = CommonDAO.GetInstance();

            string value = string.Empty, machineCode = string.Empty, equInfSamplerSystemStatus = string.Empty;
            List<HtmlDataItem> datas = new List<HtmlDataItem>();

            #region 皮带采样机 #1

            datas.Clear();
            machineCode = GlobalVars.MachineCode_HCPDCYJ_1;
            equInfSamplerSystemStatus = commonDAO.GetSignalDataValue(machineCode, eSignalDataName.系统.ToString());
            datas.Add(new HtmlDataItem("#1皮采_采样编码", ConvertSignalValue(!SysStatus(equInfSamplerSystemStatus) ? "" : commonDAO.GetSignalDataValue(machineCode, "采样编码")), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("#1皮采_矿发量", ConvertSignalValue(!SysStatus(equInfSamplerSystemStatus) ? "" : commonDAO.GetSignalDataValue(machineCode, "矿发量")), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("#1皮采_开始时间", ConvertSignalValue(!SysStatus(equInfSamplerSystemStatus) ? "" : commonDAO.GetSignalDataValue(machineCode, "截取开始时间")), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("#1皮采_来煤车数", ConvertSignalValue(!SysStatus(equInfSamplerSystemStatus) ? "" : commonDAO.GetSignalDataValue(machineCode, "来煤车数")), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("#1皮采_采样次数", ConvertSignalValue(!SysStatus(equInfSamplerSystemStatus) ? "" : commonDAO.GetSignalDataValue(machineCode, "次采次数")), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("#1皮采_系统", ConvertMachineStatusToColor(equInfSamplerSystemStatus), eHtmlDataItemType.svg_color));

            // 集样罐   
            List<InfEquInfSampleBarrel> barrels1 = MonitorDAO.GetInstance().GetEquInfSampleBarrels(machineCode);
            datas.Add(new HtmlDataItem("#1皮采_集样罐", Newtonsoft.Json.JsonConvert.SerializeObject(barrels1.Select(a => new { SampleCode = a.SampleCode, BarrelNumber = a.BarrelNumber, IsCurrent = a.IsCurrent, BarrelStatus = a.BarrelStatus, SampleCount = a.SampleCount })), eHtmlDataItemType.json_data));

            value = commonDAO.GetSignalDataValue(machineCode, "采样头");
            datas.Add(new HtmlDataItem("#1皮采_采样头", value == "1" ? "Red" : "#808080", eHtmlDataItemType.svg_color));
            value = commonDAO.GetSignalDataValue(machineCode, "1A输送机");
            datas.Add(new HtmlDataItem("#1皮采_1A输送机", value == "1" ? "Red" : "#808080", eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem("#1皮采_1A输送机", value, tempBool, eHtmlDataItemType.svg_dyncolor));
            value = commonDAO.GetSignalDataValue(machineCode, "煤流检测");
            datas.Add(new HtmlDataItem("#1皮采_煤堆", value, tempBool, eHtmlDataItemType.svg_visible));
            value = commonDAO.GetSignalDataValue(machineCode, "螺旋给料机");
            datas.Add(new HtmlDataItem("#1皮采_螺旋给料机", value == "1" ? "Red" : "#c0c0c0", eHtmlDataItemType.svg_color));
            value = commonDAO.GetSignalDataValue(machineCode, "皮带机");
            datas.Add(new HtmlDataItem("#1皮采_皮带机", value == "1" ? "Red" : "#808080", eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem("#1皮采_皮带机", value, tempBool, eHtmlDataItemType.svg_dyncolor));
            datas.Add(new HtmlDataItem("#1皮采_皮带机_煤堆", value, tempBool, eHtmlDataItemType.svg_visible));
            value = commonDAO.GetSignalDataValue(machineCode, "骨架粉碎机");
            datas.Add(new HtmlDataItem("#1皮采_骨架粉碎机1", value == "1" ? "Red" : "url(#linearGradient21005)", eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem("#1皮采_骨架粉碎机2", value == "1" ? "Red" : "url(#linearGradient21007)", eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem("#1皮采_骨架粉碎机3", value == "1" ? "Red" : "url(#linearGradient21009)", eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem("#1皮采_骨架粉碎机4", value == "1" ? "Red" : "url(#linearGradient21011)", eHtmlDataItemType.svg_color));
            value = commonDAO.GetSignalDataValue(machineCode, "缩分输送机");
            datas.Add(new HtmlDataItem("#1皮采_缩分输送机", value == "1" ? "Red" : "#808080", eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem("#1皮采_缩分输送机", value, tempBool, eHtmlDataItemType.svg_dyncolor));
            datas.Add(new HtmlDataItem("#1皮采_缩分输送机_煤堆", value, tempBool, eHtmlDataItemType.svg_visible));
            value = commonDAO.GetSignalDataValue(machineCode, "缩分器");
            datas.Add(new HtmlDataItem("#1皮采_缩分器1", value == "1" ? "Red" : "url(#linearGradient21093)", eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem("#1皮采_缩分器2", value == "1" ? "Red" : "url(#linearGradient7693)", eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem("#1皮采_缩分器3", value == "1" ? "Red" : "url(#linearGradient7697)", eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem("#1皮采_缩分器4", value == "1" ? "Red" : "url(#linearGradient21081)", eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem("#1皮采_缩分器5", value == "1" ? "Red" : "url(#linearGradient7689)", eHtmlDataItemType.svg_color));
            value = commonDAO.GetSignalDataValue(machineCode, "无轴螺旋输送机");
            datas.Add(new HtmlDataItem("#1皮采_无轴螺旋输送机", value == "1" ? "Red" : "#c0c0c0", eHtmlDataItemType.svg_color));
            value = commonDAO.GetSignalDataValue(machineCode, "斗提机");
            datas.Add(new HtmlDataItem("#1皮采_斗提机", value == "1" ? "Red" : "url(#linearGradient21061)", eHtmlDataItemType.svg_color));

            // 添加更多...

            // 发送到页面
            cefWebBrowser.Browser.GetMainFrame().ExecuteJavaScript("requestData(" + Newtonsoft.Json.JsonConvert.SerializeObject(datas) + ");", "", 0);

            #endregion

            #region 皮带采样机 #2

            datas.Clear();
            machineCode = GlobalVars.MachineCode_HCPDCYJ_2;
            equInfSamplerSystemStatus = commonDAO.GetSignalDataValue(machineCode, eSignalDataName.系统.ToString());
            datas.Add(new HtmlDataItem("#2皮采_采样编码", ConvertSignalValue(!SysStatus(equInfSamplerSystemStatus) ? "" : commonDAO.GetSignalDataValue(machineCode, "采样编码")), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("#2皮采_矿发量", ConvertSignalValue(!SysStatus(equInfSamplerSystemStatus) ? "" : commonDAO.GetSignalDataValue(machineCode, "矿发量")), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("#2皮采_开始时间", ConvertSignalValue(!SysStatus(equInfSamplerSystemStatus) ? "" : commonDAO.GetSignalDataValue(machineCode, "截取开始时间")), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("#2皮采_来煤车数", ConvertSignalValue(!SysStatus(equInfSamplerSystemStatus) ? "" : commonDAO.GetSignalDataValue(machineCode, "来煤车数")), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("#2皮采_采样次数", ConvertSignalValue(!SysStatus(equInfSamplerSystemStatus) ? "" : commonDAO.GetSignalDataValue(machineCode, "次采次数")), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("#2皮采_系统", ConvertMachineStatusToColor(equInfSamplerSystemStatus), eHtmlDataItemType.svg_color));

            // 集样罐   
            List<InfEquInfSampleBarrel> barrels2 = MonitorDAO.GetInstance().GetEquInfSampleBarrels(machineCode);
            datas.Add(new HtmlDataItem("#2皮采_集样罐", Newtonsoft.Json.JsonConvert.SerializeObject(barrels2.Select(a => new { SampleCode = a.SampleCode, BarrelNumber = a.BarrelNumber, IsCurrent = a.IsCurrent, BarrelStatus = a.BarrelStatus, SampleCount = a.SampleCount })), eHtmlDataItemType.json_data));

            value = commonDAO.GetSignalDataValue(machineCode, "采样头");
            datas.Add(new HtmlDataItem("#2皮采_采样头", value == "1" ? "Red" : "#808080", eHtmlDataItemType.svg_color));
            value = commonDAO.GetSignalDataValue(machineCode, "1B输送机");
            datas.Add(new HtmlDataItem("#2皮采_1B输送机", value == "1" ? "Red" : "#808080", eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem("#2皮采_1B输送机", value, tempBool, eHtmlDataItemType.svg_dyncolor));
            value = commonDAO.GetSignalDataValue(machineCode, "煤流检测");
            datas.Add(new HtmlDataItem("#2皮采_煤堆", value, tempBool, eHtmlDataItemType.svg_visible));
            value = commonDAO.GetSignalDataValue(machineCode, "螺旋给料机");
            datas.Add(new HtmlDataItem("#2皮采_螺旋给料机", value == "1" ? "Red" : "#c0c0c0", eHtmlDataItemType.svg_color));
            value = commonDAO.GetSignalDataValue(machineCode, "皮带机");
            datas.Add(new HtmlDataItem("#2皮采_皮带机", value == "1" ? "Red" : "#808080", eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem("#2皮采_皮带机", value, tempBool, eHtmlDataItemType.svg_dyncolor));
            datas.Add(new HtmlDataItem("#2皮采_皮带机_煤堆", value, tempBool, eHtmlDataItemType.svg_visible));
            value = commonDAO.GetSignalDataValue(machineCode, "骨架粉碎机");
            datas.Add(new HtmlDataItem("#2皮采_骨架粉碎机1", value == "1" ? "Red" : "url(#linearGradient21185)", eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem("#2皮采_骨架粉碎机2", value == "1" ? "Red" : "url(#linearGradient21187)", eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem("#2皮采_骨架粉碎机3", value == "1" ? "Red" : "url(#linearGradient21189)", eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem("#2皮采_骨架粉碎机4", value == "1" ? "Red" : "url(#linearGradient21191)", eHtmlDataItemType.svg_color));
            value = commonDAO.GetSignalDataValue(machineCode, "缩分输送机");
            datas.Add(new HtmlDataItem("#2皮采_缩分输送机", value == "1" ? "Red" : "#808080", eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem("#2皮采_缩分输送机", value, tempBool, eHtmlDataItemType.svg_dyncolor));
            datas.Add(new HtmlDataItem("#2皮采_缩分输送机_煤堆", value, tempBool, eHtmlDataItemType.svg_visible));
            value = commonDAO.GetSignalDataValue(machineCode, "缩分器");
            datas.Add(new HtmlDataItem("#2皮采_缩分器1", value == "1" ? "Red" : "url(#linearGradient21273)", eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem("#2皮采_缩分器2", value == "1" ? "Red" : "url(#linearGradient8507)", eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem("#2皮采_缩分器3", value == "1" ? "Red" : "url(#linearGradient8511)", eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem("#2皮采_缩分器4", value == "1" ? "Red" : "url(#linearGradient21261)", eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem("#2皮采_缩分器5", value == "1" ? "Red" : "url(#linearGradient8503)", eHtmlDataItemType.svg_color));
            value = commonDAO.GetSignalDataValue(machineCode, "无轴螺旋输送机");
            datas.Add(new HtmlDataItem("#2皮采_无轴螺旋输送机", value == "1" ? "Red" : "#c0c0c0", eHtmlDataItemType.svg_color));
            value = commonDAO.GetSignalDataValue(machineCode, "斗提机");
            datas.Add(new HtmlDataItem("#2皮采_斗提机", value == "1" ? "Red" : "url(#linearGradient21061)", eHtmlDataItemType.svg_color));

            // 添加更多...

            // 发送到页面
            cefWebBrowser.Browser.GetMainFrame().ExecuteJavaScript("requestData(" + Newtonsoft.Json.JsonConvert.SerializeObject(datas) + ");", "", 0);

            tempBool = tempBool ? false : true;

            #endregion
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // 界面不可见时，停止发送数据
            if (!this.Visible) return;
            RequestData();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            // 发送到页面
            cefWebBrowser.Browser.GetMainFrame().ExecuteJavaScript("testColor();", "", 0);
        }

        /// <summary>
        /// 转换信号点值
        /// </summary>
        /// <param name="signalValue">信号点值</param>
        /// <param name="unit">单位</param>
        /// <returns></returns>
        private string ConvertSignalValue(string signalValue, string unit = "")
        {
            return !string.IsNullOrEmpty(signalValue) ? signalValue + unit : "--";
        }

        private bool SysStatus(string systemStatus)
        {
            if ("|就绪待机|离线|".Contains("|" + systemStatus + "|"))
                return false;
            else if ("|正在运行|正在卸样|".Contains("|" + systemStatus + "|"))
                return true;
            else if ("|发生故障|".Contains("|" + systemStatus + "|"))
                return false;
            else if ("|离线状态|".Contains("|" + systemStatus + "|"))
                return false;
            else
                return false;
        }

        /// <summary>
        /// 转换设备系统状态为颜色值
        /// </summary>
        /// <param name="systemStatus">系统状态</param>
        /// <returns></returns>
        private string ConvertMachineStatusToColor(string systemStatus)
        {
            if ("|就绪待机|离线|".Contains("|" + systemStatus + "|"))
                return ColorTranslator.ToHtml(EquipmentStatusColors.BeReady);
            else if ("|正在运行|正在卸样|".Contains("|" + systemStatus + "|"))
                return ColorTranslator.ToHtml(EquipmentStatusColors.Working);
            else if ("|发生故障|".Contains("|" + systemStatus + "|"))
                return ColorTranslator.ToHtml(EquipmentStatusColors.Breakdown);
            else if ("|离线状态|".Contains("|" + systemStatus + "|"))
                return ColorTranslator.ToHtml(EquipmentStatusColors.OffLine);
            else
                return ColorTranslator.ToHtml(EquipmentStatusColors.Forbidden);
        }
    }
}
