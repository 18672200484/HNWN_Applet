using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CMCS.Common;
using CMCS.Common.DAO;
using CMCS.Common.Enums;
using CMCS.Monitor.Win.CefGlue;
using CMCS.Monitor.Win.Core;
using CMCS.Monitor.Win.Html;
using CMCS.Monitor.Win.UserControls;
using CMCS.Monitor.Win.Utilities;
using DevComponents.DotNetBar;
using Xilium.CefGlue;
using Xilium.CefGlue.WindowsForms;

namespace CMCS.Monitor.Win.Frms
{
	public partial class FrmHomePage : DevComponents.DotNetBar.Metro.MetroForm
	{
		/// <summary>
		/// 窗体唯一标识符
		/// </summary>
		public static string UniqueKey = "FrmHomePage";

		CommonDAO commonDAO = CommonDAO.GetInstance();
		MonitorCommon monitorCommon = MonitorCommon.GetInstance();

		CefWebBrowserEx cefWebBrowser = new CefWebBrowserEx();

		public FrmHomePage()
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
			cefWebBrowser.StartUrl = SelfVars.Url_HomePage;
			cefWebBrowser.Dock = DockStyle.Fill;
			cefWebBrowser.WebClient = new HomePageCefWebClient(cefWebBrowser);
			cefWebBrowser.LoadEnd += new EventHandler<LoadEndEventArgs>(cefWebBrowser_LoadEnd);
			panWebBrower.Controls.Add(cefWebBrowser);
		}

		void cefWebBrowser_LoadEnd(object sender, LoadEndEventArgs e)
		{
			timer1.Enabled = true;

			RequestData();
		}

		private void FrmHomePage_Load(object sender, EventArgs e)
		{
			FormInit();
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
		}

		/// <summary>
		/// 请求数据
		/// </summary>
		void RequestData()
		{
			string value = string.Empty, machineCode = string.Empty;
			List<HtmlDataItem> datas = new List<HtmlDataItem>();

			datas.Clear();

			datas.Add(new HtmlDataItem("今日入厂车数", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "今日入厂车数"), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("今日采样车数", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "今日采样车数"), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("今日待卸车数", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "今日待卸车数"), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("在途车数", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "在途车数"), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("今日出厂车数", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "今日出厂车数"), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("今日采样桶数", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "今日采样桶数"), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("今日已卸车数", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "今日已卸车数"), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("今日制样数", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "今日制样数"), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("今日进煤量", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "今日进煤量"), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("今日存煤量", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "今日存煤量"), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("今日耗煤量", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "今日耗煤量"), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("今日化验数", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "今日化验数"), eHtmlDataItemType.svg_text));

			#region 汽车采样机

			machineCode = GlobalVars.MachineCode_QC_JxSampler_1;
			datas.Add(new HtmlDataItem("1号采样机_系统", monitorCommon.ConvertMachineStatusToColor(commonDAO.GetSignalDataValue(GlobalVars.MachineCode_QCJXCYJ_1, eSignalDataName.系统.ToString())), eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("1号采样机_车号", commonDAO.GetSignalDataValue(machineCode, eSignalDataName.当前车号.ToString()), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("1号采样机_道闸1", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.道闸1升杆.ToString())), eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("1号采样机_道闸2", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.道闸2升杆.ToString())), eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("1号采样机_已采车数", commonDAO.GetSignalDataValue(machineCode, "已采车数"), eHtmlDataItemType.svg_text));

			machineCode = GlobalVars.MachineCode_QC_JxSampler_2;
			datas.Add(new HtmlDataItem("2号采样机_系统", monitorCommon.ConvertMachineStatusToColor(commonDAO.GetSignalDataValue(GlobalVars.MachineCode_QCJXCYJ_2, eSignalDataName.系统.ToString())), eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("2号采样机_车号", commonDAO.GetSignalDataValue(machineCode, eSignalDataName.当前车号.ToString()), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("2号采样机_道闸1", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.道闸1升杆.ToString())), eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("2号采样机_道闸2", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.道闸2升杆.ToString())), eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("2号采样机_已采车数", commonDAO.GetSignalDataValue(machineCode, "已采车数"), eHtmlDataItemType.svg_text));

			machineCode = GlobalVars.MachineCode_QC_JxSampler_3;
			datas.Add(new HtmlDataItem("3号采样机_系统", monitorCommon.ConvertMachineStatusToColor(commonDAO.GetSignalDataValue(GlobalVars.MachineCode_QCJXCYJ_3, eSignalDataName.系统.ToString())), eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("3号采样机_车号", commonDAO.GetSignalDataValue(machineCode, eSignalDataName.当前车号.ToString()), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("3号采样机_道闸1", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.道闸1升杆.ToString())), eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("3号采样机_道闸2", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.道闸2升杆.ToString())), eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("3号采样机_已采车数", commonDAO.GetSignalDataValue(machineCode, "已采车数"), eHtmlDataItemType.svg_text));

			#endregion

			#region 汽车衡

			machineCode = GlobalVars.MachineCode_QC_Weighter_2;
			datas.Add(new HtmlDataItem("1号重磅_系统", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.系统.ToString())), eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("1号重磅_车号", commonDAO.GetSignalDataValue(machineCode, eSignalDataName.当前车号.ToString()), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("1号重磅_重量", commonDAO.GetSignalDataValue(machineCode, eSignalDataName.地磅仪表_实时重量.ToString()) + "t", eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("1号重磅_道闸1", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.道闸1升杆.ToString())), eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("1号重磅_道闸2", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.道闸2升杆.ToString())), eHtmlDataItemType.svg_color));

			machineCode = GlobalVars.MachineCode_QC_Weighter_3;
			datas.Add(new HtmlDataItem("2号重磅_系统", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.系统.ToString())), eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("2号重磅_车号", commonDAO.GetSignalDataValue(machineCode, eSignalDataName.当前车号.ToString()), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("2号重磅_重量", commonDAO.GetSignalDataValue(machineCode, eSignalDataName.地磅仪表_实时重量.ToString()) + "t", eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("2号重磅_道闸1", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.道闸1升杆.ToString())), eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("2号重磅_道闸2", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.道闸2升杆.ToString())), eHtmlDataItemType.svg_color));

			machineCode = GlobalVars.MachineCode_QC_Weighter_4;
			datas.Add(new HtmlDataItem("3号重磅_系统", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.系统.ToString())), eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("3号重磅_车号", commonDAO.GetSignalDataValue(machineCode, eSignalDataName.当前车号.ToString()), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("3号重磅_重量", commonDAO.GetSignalDataValue(machineCode, eSignalDataName.地磅仪表_实时重量.ToString()) + "t", eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("3号重磅_道闸1", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.道闸1升杆.ToString())), eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("3号重磅_道闸2", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.道闸2升杆.ToString())), eHtmlDataItemType.svg_color));

			machineCode = GlobalVars.MachineCode_QC_Weighter_1;
			datas.Add(new HtmlDataItem("1号轻磅_系统", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.系统.ToString())), eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("1号轻磅_车号", commonDAO.GetSignalDataValue(machineCode, eSignalDataName.当前车号.ToString()), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("1号轻磅_重量", commonDAO.GetSignalDataValue(machineCode, eSignalDataName.地磅仪表_实时重量.ToString()) + "t", eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("1号轻磅_道闸1", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.道闸1升杆.ToString())), eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("1号轻磅_道闸2", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.道闸2升杆.ToString())), eHtmlDataItemType.svg_color));

			#endregion

			// 添加更多...

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
		/// 添加红绿灯控制信号
		/// </summary>
		/// <param name="datas"></param>
		/// <param name="machineCode"></param>
		/// <param name="signalValue"></param>
		private void AddDataItemBySignal(List<HtmlDataItem> datas, string machineCode, string signalValue)
		{
			if (commonDAO.GetSignalDataValue(machineCode, signalValue) == "1")
			{
				//红灯
				datas.Add(new HtmlDataItem(signalValue + "_红", "#FF0000", eHtmlDataItemType.svg_color));
				datas.Add(new HtmlDataItem(signalValue + "_绿", "#CCCCCC", eHtmlDataItemType.svg_color));
			}
			else
			{
				//绿灯
				datas.Add(new HtmlDataItem(signalValue + "_红", "#CCCCCC", eHtmlDataItemType.svg_color));
				datas.Add(new HtmlDataItem(signalValue + "_绿", "#00FF00", eHtmlDataItemType.svg_color));
			}
		}

	}

	public class HomePageCefWebClient : CefWebClient
	{
		CefWebBrowser cefWebBrowser;

		public HomePageCefWebClient(CefWebBrowser cefWebBrowser)
			: base(cefWebBrowser)
		{
			this.cefWebBrowser = cefWebBrowser;
		}

		protected override bool OnProcessMessageReceived(CefBrowser browser, CefProcessId sourceProcess, CefProcessMessage message)
		{
			if (message.Name == "OpenTruckWeighter")
				SelfVars.MainFrameForm.OpenTruckWeighter();
			else if (message.Name == "OpenTruckMachinerySampler")
				SelfVars.MainFrameForm.OpenTruckMachinerySampler();
			else if (message.Name == "OpenAssayManage")
				SelfVars.MainFrameForm.OpenAssayManage();
			else if (message.Name == "CarSamplerChangeSelected")
				SelfVars.CarSamplerForm.CurrentMachineCode = MonitorCommon.GetInstance().GetCarSamplerMachineCodeBySelected(message.Arguments.GetString(0));
			else if (message.Name == "TruckWeighterChangeSelected")
				SelfVars.TruckWeighterForm.CurrentMachineCode = MonitorCommon.GetInstance().GetTruckWeighterMachineCodeBySelected(message.Arguments.GetString(0));
			else if (message.Name == "ChangeSelected")
				SelfVars.CurrentSelectedCell = message.Arguments.GetString(0);
			else if (message.Name == "OpenHikVideo")
				//视频预览
				SelfVars.MainFrameForm.OpenHikVideo(message.Arguments.GetString(0));

			return true;
		}

		protected override CefContextMenuHandler GetContextMenuHandler()
		{
			return new CefMenuHandler();
		}
	}
}