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
		/// ����Ψһ��ʶ��
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
		/// �����ʼ��
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
		/// ���� - ˢ��ҳ��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnRefresh_Click(object sender, EventArgs e)
		{
			cefWebBrowser.Browser.Reload();
		}

		/// <summary>
		/// ���� - ����ˢ��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnRequestData_Click(object sender, EventArgs e)
		{
			RequestData();
		}

		/// <summary>
		/// ��������
		/// </summary>
		void RequestData()
		{
			string value = string.Empty, machineCode = string.Empty;
			List<HtmlDataItem> datas = new List<HtmlDataItem>();

			datas.Clear();

			datas.Add(new HtmlDataItem("�����볧����", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "�����볧����"), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("���ղ�������", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "���ղ�������"), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("���մ�ж����", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "���մ�ж����"), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("��;����", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "��;����"), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("���ճ�������", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "���ճ�������"), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("���ղ���Ͱ��", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "���ղ���Ͱ��"), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("������ж����", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "������ж����"), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("����������", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "����������"), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("���ս�ú��", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "���ս�ú��"), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("���մ�ú��", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "���մ�ú��"), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("���պ�ú��", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "���պ�ú��"), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("���ջ�����", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "���ջ�����"), eHtmlDataItemType.svg_text));

			#region ����������

			machineCode = GlobalVars.MachineCode_QC_JxSampler_1;
			datas.Add(new HtmlDataItem("1�Ų�����_ϵͳ", monitorCommon.ConvertMachineStatusToColor(commonDAO.GetSignalDataValue(GlobalVars.MachineCode_QCJXCYJ_1, eSignalDataName.ϵͳ.ToString())), eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("1�Ų�����_����", commonDAO.GetSignalDataValue(machineCode, eSignalDataName.��ǰ����.ToString()), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("1�Ų�����_��բ1", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.��բ1����.ToString())), eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("1�Ų�����_��բ2", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.��բ2����.ToString())), eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("1�Ų�����_�Ѳɳ���", commonDAO.GetSignalDataValue(machineCode, "�Ѳɳ���"), eHtmlDataItemType.svg_text));

			machineCode = GlobalVars.MachineCode_QC_JxSampler_2;
			datas.Add(new HtmlDataItem("2�Ų�����_ϵͳ", monitorCommon.ConvertMachineStatusToColor(commonDAO.GetSignalDataValue(GlobalVars.MachineCode_QCJXCYJ_2, eSignalDataName.ϵͳ.ToString())), eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("2�Ų�����_����", commonDAO.GetSignalDataValue(machineCode, eSignalDataName.��ǰ����.ToString()), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("2�Ų�����_��բ1", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.��բ1����.ToString())), eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("2�Ų�����_��բ2", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.��բ2����.ToString())), eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("2�Ų�����_�Ѳɳ���", commonDAO.GetSignalDataValue(machineCode, "�Ѳɳ���"), eHtmlDataItemType.svg_text));

			machineCode = GlobalVars.MachineCode_QC_JxSampler_3;
			datas.Add(new HtmlDataItem("3�Ų�����_ϵͳ", monitorCommon.ConvertMachineStatusToColor(commonDAO.GetSignalDataValue(GlobalVars.MachineCode_QCJXCYJ_3, eSignalDataName.ϵͳ.ToString())), eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("3�Ų�����_����", commonDAO.GetSignalDataValue(machineCode, eSignalDataName.��ǰ����.ToString()), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("3�Ų�����_��բ1", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.��բ1����.ToString())), eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("3�Ų�����_��բ2", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.��բ2����.ToString())), eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("3�Ų�����_�Ѳɳ���", commonDAO.GetSignalDataValue(machineCode, "�Ѳɳ���"), eHtmlDataItemType.svg_text));

			#endregion

			#region ������

			machineCode = GlobalVars.MachineCode_QC_Weighter_2;
			datas.Add(new HtmlDataItem("1���ذ�_ϵͳ", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.ϵͳ.ToString())), eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("1���ذ�_����", commonDAO.GetSignalDataValue(machineCode, eSignalDataName.��ǰ����.ToString()), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("1���ذ�_����", commonDAO.GetSignalDataValue(machineCode, eSignalDataName.�ذ��Ǳ�_ʵʱ����.ToString()) + "t", eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("1���ذ�_��բ1", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.��բ1����.ToString())), eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("1���ذ�_��բ2", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.��բ2����.ToString())), eHtmlDataItemType.svg_color));

			machineCode = GlobalVars.MachineCode_QC_Weighter_3;
			datas.Add(new HtmlDataItem("2���ذ�_ϵͳ", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.ϵͳ.ToString())), eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("2���ذ�_����", commonDAO.GetSignalDataValue(machineCode, eSignalDataName.��ǰ����.ToString()), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("2���ذ�_����", commonDAO.GetSignalDataValue(machineCode, eSignalDataName.�ذ��Ǳ�_ʵʱ����.ToString()) + "t", eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("2���ذ�_��բ1", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.��բ1����.ToString())), eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("2���ذ�_��բ2", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.��բ2����.ToString())), eHtmlDataItemType.svg_color));

			machineCode = GlobalVars.MachineCode_QC_Weighter_4;
			datas.Add(new HtmlDataItem("3���ذ�_ϵͳ", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.ϵͳ.ToString())), eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("3���ذ�_����", commonDAO.GetSignalDataValue(machineCode, eSignalDataName.��ǰ����.ToString()), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("3���ذ�_����", commonDAO.GetSignalDataValue(machineCode, eSignalDataName.�ذ��Ǳ�_ʵʱ����.ToString()) + "t", eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("3���ذ�_��բ1", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.��բ1����.ToString())), eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("3���ذ�_��բ2", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.��բ2����.ToString())), eHtmlDataItemType.svg_color));

			machineCode = GlobalVars.MachineCode_QC_Weighter_1;
			datas.Add(new HtmlDataItem("1�����_ϵͳ", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.ϵͳ.ToString())), eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("1�����_����", commonDAO.GetSignalDataValue(machineCode, eSignalDataName.��ǰ����.ToString()), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("1�����_����", commonDAO.GetSignalDataValue(machineCode, eSignalDataName.�ذ��Ǳ�_ʵʱ����.ToString()) + "t", eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("1�����_��բ1", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.��բ1����.ToString())), eHtmlDataItemType.svg_color));
			datas.Add(new HtmlDataItem("1�����_��բ2", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.��բ2����.ToString())), eHtmlDataItemType.svg_color));

			#endregion

			// ��Ӹ���...

			// ���͵�ҳ��
			cefWebBrowser.Browser.GetMainFrame().ExecuteJavaScript("requestData(" + Newtonsoft.Json.JsonConvert.SerializeObject(datas) + ");", "", 0);
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			// ���治�ɼ�ʱ��ֹͣ��������
			if (!this.Visible) return;

			RequestData();
		}

		/// <summary>
		/// ��Ӻ��̵ƿ����ź�
		/// </summary>
		/// <param name="datas"></param>
		/// <param name="machineCode"></param>
		/// <param name="signalValue"></param>
		private void AddDataItemBySignal(List<HtmlDataItem> datas, string machineCode, string signalValue)
		{
			if (commonDAO.GetSignalDataValue(machineCode, signalValue) == "1")
			{
				//���
				datas.Add(new HtmlDataItem(signalValue + "_��", "#FF0000", eHtmlDataItemType.svg_color));
				datas.Add(new HtmlDataItem(signalValue + "_��", "#CCCCCC", eHtmlDataItemType.svg_color));
			}
			else
			{
				//�̵�
				datas.Add(new HtmlDataItem(signalValue + "_��", "#CCCCCC", eHtmlDataItemType.svg_color));
				datas.Add(new HtmlDataItem(signalValue + "_��", "#00FF00", eHtmlDataItemType.svg_color));
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
				//��ƵԤ��
				SelfVars.MainFrameForm.OpenHikVideo(message.Arguments.GetString(0));

			return true;
		}

		protected override CefContextMenuHandler GetContextMenuHandler()
		{
			return new CefMenuHandler();
		}
	}
}