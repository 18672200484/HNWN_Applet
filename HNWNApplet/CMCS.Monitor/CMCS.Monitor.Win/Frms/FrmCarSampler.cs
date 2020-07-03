using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CMCS.Common;
using CMCS.Common.DAO;
using CMCS.Common.Entities;
using CMCS.Common.Entities.CarTransport;
using CMCS.Common.Entities.Inf;
using CMCS.Common.Enums;
using CMCS.Monitor.DAO;
using CMCS.Monitor.Win.Core;
using CMCS.Monitor.Win.Html;
using CMCS.Monitor.Win.UserControls;
using CMCS.Monitor.Win.Utilities;
using DevComponents.DotNetBar.Metro;
using Xilium.CefGlue;
using Xilium.CefGlue.WindowsForms;
using CMCS.Common.Utilities;

namespace CMCS.Monitor.Win.Frms
{
	public partial class FrmCarSampler : MetroForm
	{
		/// <summary>
		/// 窗体唯一标识符
		/// </summary>
		public static string UniqueKey = "FrmCarSampler";

		CefWebBrowserEx cefWebBrowser = new CefWebBrowserEx();

		CommonDAO commonDAO = CommonDAO.GetInstance();
		MonitorCommon monitorCommon = MonitorCommon.GetInstance();
		string LastCarNumber = string.Empty;

		string currentMachineCode = GlobalVars.MachineCode_QCJXCYJ_1;
		/// <summary>
		/// 当前选中的采样机
		/// </summary>
		public string CurrentMachineCode
		{
			get { return currentMachineCode; }
			set { currentMachineCode = value; }
		}

		public FrmCarSampler()
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
			cefWebBrowser.StartUrl = SelfVars.Url_CarSampler;
			cefWebBrowser.Dock = DockStyle.Fill;
			cefWebBrowser.WebClient = new HomePageCefWebClient(cefWebBrowser);
			cefWebBrowser.LoadEnd += new EventHandler<LoadEndEventArgs>(cefWebBrowser_LoadEnd);
			panWebBrower.Controls.Add(cefWebBrowser);
		}

		void cefWebBrowser_LoadEnd(object sender, LoadEndEventArgs e)
		{
			timer1.Enabled = true;
		}

		private void FrmCarSampler_Load(object sender, EventArgs e)
		{
			FormInit();
		}

		/// <summary>
		/// 请求数据
		/// </summary>
		void RequestData()
		{
			string value = string.Empty, machineCode = string.Empty;
			List<HtmlDataItem> datas = new List<HtmlDataItem>();
			List<InfEquInfHitch> equInfHitchs = new List<InfEquInfHitch>();

			#region 汽车机械采样机 #1

			datas.Clear();
			machineCode = this.CurrentMachineCode;

			datas.Add(new HtmlDataItem("机械采样机_当前采样机", machineCode, eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("1号机械采样机_系统", monitorCommon.ConvertMachineStatusToColor(commonDAO.GetSignalDataValue(GlobalVars.MachineCode_QCJXCYJ_1, eSignalDataName.系统.ToString())), eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("2号机械采样机_系统", monitorCommon.ConvertMachineStatusToColor(commonDAO.GetSignalDataValue(GlobalVars.MachineCode_QCJXCYJ_2, eSignalDataName.系统.ToString())), eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("3号机械采样机_系统", monitorCommon.ConvertMachineStatusToColor(commonDAO.GetSignalDataValue(GlobalVars.MachineCode_QCJXCYJ_3, eSignalDataName.系统.ToString())), eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("机械采样机_采样编码", commonDAO.GetSignalDataValue(machineCode, eSignalDataName.采样编码.ToString()), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("机械采样机_矿发量", commonDAO.GetSignalDataValue(machineCode, eSignalDataName.矿发量.ToString()), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("机械采样机_采样时间", commonDAO.GetSignalDataValue(machineCode, eSignalDataName.采样时间.ToString()), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("机械采样机_车牌号", commonDAO.GetSignalDataValue(machineCode, eSignalDataName.当前车号.ToString()), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("机械采样机_采样点数", commonDAO.GetSignalDataValue(machineCode, eSignalDataName.采样点数.ToString()), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("机械采样机_小车1", commonDAO.GetSignalDataValue(machineCode, "小车") == "1" ? "Red" : "url(#rect1770_1_)", eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("机械采样机_小车2", commonDAO.GetSignalDataValue(machineCode, "小车") == "1" ? "Red" : "url(#rect1752_1_)", eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("机械采样机_小车3", commonDAO.GetSignalDataValue(machineCode, "小车") == "1" ? "Red" : "url(#rect1761_1_)", eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("机械采样机_小车4", commonDAO.GetSignalDataValue(machineCode, "小车") == "1" ? "Red" : "url(#rect1716_1_)", eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("机械采样机_小车5", commonDAO.GetSignalDataValue(machineCode, "小车") == "1" ? "Red" : "url(#rect1725_1_)", eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("机械采样机_小车6", commonDAO.GetSignalDataValue(machineCode, "小车") == "1" ? "Red" : "url(#rect1734_1_)", eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("机械采样机_小车7", commonDAO.GetSignalDataValue(machineCode, "小车") == "1" ? "Red" : "url(#polygon1743_1_)", eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("机械采样机_接料斗1", commonDAO.GetSignalDataValue(machineCode, "接料斗") == "1" ? "Red" : "url(#_164344952_2_)", eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("机械采样机_接料斗2", commonDAO.GetSignalDataValue(machineCode, "接料斗") == "1" ? "Red" : "url(#_130855712_2_)", eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("机械采样机_接料斗3", commonDAO.GetSignalDataValue(machineCode, "接料斗") == "1" ? "Red" : "url(#_164355560_2_)", eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("机械采样机_接料斗4", commonDAO.GetSignalDataValue(machineCode, "接料斗") == "1" ? "Red" : "url(#_164351936_2_)", eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("机械采样机_除铁给料皮带", commonDAO.GetSignalDataValue(machineCode, "给料皮带") == "1" ? "Red" : "#808080", eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("机械采样机_溜煤管", commonDAO.GetSignalDataValue(machineCode, "溜煤管") == "1" ? "Red" : "url(#polygon984_1_)", eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("机械采样机_锤式破碎机1", commonDAO.GetSignalDataValue(machineCode, "锤式破碎机") == "1" ? "Red" : "url(#_125277864-0_2_)", eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("机械采样机_锤式破碎机2", commonDAO.GetSignalDataValue(machineCode, "锤式破碎机") == "1" ? "Red" : "url(#_164348960_2_)", eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("机械采样机_锤式破碎机3", commonDAO.GetSignalDataValue(machineCode, "锤式破碎机") == "1" ? "Red" : "url(#_130854176_2_)", eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("机械采样机_锤式破碎机4", commonDAO.GetSignalDataValue(machineCode, "锤式破碎机") == "1" ? "Red" : "url(#_130859336-4_2_)", eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("机械采样机_锤式破碎机5", commonDAO.GetSignalDataValue(machineCode, "锤式破碎机") == "1" ? "Red" : "url(#_164347592_2_)", eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("机械采样机_锤式破碎机6", commonDAO.GetSignalDataValue(machineCode, "锤式破碎机") == "1" ? "Red" : "url(#_164343680_2_)", eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("机械采样机_锤式破碎机7", commonDAO.GetSignalDataValue(machineCode, "锤式破碎机") == "1" ? "Red" : "url(#_164356088_2_)", eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("机械采样机_缩分皮带", commonDAO.GetSignalDataValue(machineCode, "缩分皮带") == "1" ? "Red" : "#808080", eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("机械采样机_弃料斗", commonDAO.GetSignalDataValue(machineCode, "弃料斗") == "1" ? "Red" : "url(#_164344952-8_1_)", eHtmlDataItemType.svg_color));

			string carNumber = commonDAO.GetSignalDataValue(machineCode, "车牌号");
			if (string.IsNullOrEmpty(carNumber))
			{
				datas.Add(new HtmlDataItem("Car1", "false", eHtmlDataItemType.svg_visible));
				this.LastCarNumber = string.Empty;
			}
			else
			{
				if (this.LastCarNumber != carNumber)
				{
					CmcsAutotruck autotruck = CommonDAO.GetInstance().GetAutotruckByCarNumber(carNumber);
					if (autotruck != null)
					{
						if (PreviewCarCarriage(autotruck))
							datas.Add(new HtmlDataItem("Car1", "true", eHtmlDataItemType.svg_visible));
						else
							datas.Add(new HtmlDataItem("Car1", "false", eHtmlDataItemType.svg_visible));
					}
					else
						datas.Add(new HtmlDataItem("Car1", "false", eHtmlDataItemType.svg_visible));

					this.LastCarNumber = carNumber;
				}
			}

			// 集样罐   
			List<InfEquInfSampleBarrel> barrels1 = MonitorDAO.GetInstance().GetEquInfSampleBarrels(machineCode);
			datas.Add(new HtmlDataItem("采样机1_集样罐", Newtonsoft.Json.JsonConvert.SerializeObject(barrels1.Select(a => new { BarrelNumber = a.BarrelNumber, IsCurrent = a.IsCurrent, BarrelStatus = a.BarrelStatus, SampleCount = a.SampleCount })), eHtmlDataItemType.json_data));
			#endregion

			// 发送到页面
			cefWebBrowser.Browser.GetMainFrame().ExecuteJavaScript("requestData(" + Newtonsoft.Json.JsonConvert.SerializeObject(datas) + ");", "", 0);
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			// 界面不可见时，停止发送数据
			if (!this.Visible) return;

			RequestData();
		}

		/// <summary>
		/// 刷新页面
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnRefresh_Click(object sender, EventArgs e)
		{
			cefWebBrowser.Browser.Reload();
		}

		private void btnRequestData_Click(object sender, EventArgs e)
		{
			RequestData();
		}

		private void buttonX1_Click(object sender, EventArgs e)
		{
			// 发送到页面
			cefWebBrowser.Browser.GetMainFrame().ExecuteJavaScript("testColor();", "", 0);

			//CmcsAutotruck autotruck = CommonDAO.GetInstance().GetAutotruckByCarNumber("鄂ATM168");
			//Bitmap res = new Bitmap(CMCS.Monitor.Win.Properties.Resources.Autotruck);
			//PreviewCarBmp carBmp = new PreviewCarBmp(autotruck);
			//Bitmap bmp = carBmp.GetPreviewBitmap(res, 249, 130);
			//bmp.Save("Autotruck.bmp");
		}

		/// <summary>
		/// 预览车辆拉筋信息图
		/// </summary>
		/// <param name="autotruck"></param>
		private bool PreviewCarCarriage(CmcsAutotruck autotruck)
		{
			if (autotruck != null && autotruck.CarriageLength > 0 && autotruck.CarriageWidth > 0)
			{
				Bitmap res = new Bitmap(CMCS.Monitor.Win.Properties.Resources.Autotruck);
				PreviewCarBmp carBmp = new PreviewCarBmp(autotruck);
				Bitmap bmp = carBmp.GetPreviewBitmap(res, 249, 130);
				bmp.Save("Autotruck_1.bmp");
				return true;
			}
			return false;
		}

	}
}
