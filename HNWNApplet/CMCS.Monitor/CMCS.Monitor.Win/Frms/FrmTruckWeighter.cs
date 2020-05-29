using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CMCS.Common;
using CMCS.Common.DAO;
using CMCS.Common.Entities.CarTransport;
using CMCS.Common.Enums;
using CMCS.Common.Utilities;
using CMCS.Monitor.Win.Core;
using CMCS.Monitor.Win.Html;
using CMCS.Monitor.Win.UserControls;
using CMCS.Monitor.Win.Utilities;
using DevComponents.DotNetBar;
using Xilium.CefGlue.WindowsForms;

namespace CMCS.Monitor.Win.Frms
{
	public partial class FrmTruckWeighter : DevComponents.DotNetBar.Metro.MetroForm
	{
		/// <summary>
		/// ����Ψһ��ʶ��
		/// </summary>
		public static string UniqueKey = "FrmTruckWeighter";

		CommonDAO commonDAO = CommonDAO.GetInstance();
		MonitorCommon monitorCommon = MonitorCommon.GetInstance();

		CefWebBrowserEx cefWebBrowser = new CefWebBrowserEx();

		string currentMachineCode = GlobalVars.MachineCode_QC_Weighter_1;
		/// <summary>
		/// ��ǰѡ�еĺ���
		/// </summary>
		public string CurrentMachineCode
		{
			get { return currentMachineCode; }
			set { currentMachineCode = value; }
		}

		public FrmTruckWeighter()
		{
			InitializeComponent();
		}

		private void FrmTruckWeighter_Load(object sender, EventArgs e)
		{
			FormInit();
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

			cefWebBrowser.StartUrl = SelfVars.Url_TruckWeighter;
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

			machineCode = this.CurrentMachineCode;
			string[] machineCodes = new string[] { GlobalVars.MachineCode_QC_Weighter_1, GlobalVars.MachineCode_QC_Weighter_2, GlobalVars.MachineCode_QC_Weighter_3, GlobalVars.MachineCode_QC_Weighter_4 };
			for (int i = 0; i < machineCodes.Length; i++)
			{
				int machineNumber = i + 1;
				machineCode = machineCodes[i];
				datas.Add(new HtmlDataItem("#" + machineNumber + "������_ϵͳ", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.ϵͳ.ToString())), eHtmlDataItemType.svg_color));
				datas.Add(new HtmlDataItem("#" + machineNumber + "������_IO������", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.IO������_����״̬.ToString())), eHtmlDataItemType.svg_color));
				datas.Add(new HtmlDataItem("#" + machineNumber + "������_�ذ��Ǳ�", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.�ذ��Ǳ�_����״̬.ToString())), eHtmlDataItemType.svg_color));
				datas.Add(new HtmlDataItem("#" + machineNumber + "������_LED��", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.LED��1_����״̬.ToString())), eHtmlDataItemType.svg_color));
				datas.Add(new HtmlDataItem("#" + machineNumber + "������_������1", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.������1_����״̬.ToString())), eHtmlDataItemType.svg_color));
				datas.Add(new HtmlDataItem("#" + machineNumber + "������_������2", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.������2_����״̬.ToString())), eHtmlDataItemType.svg_color));
				datas.Add(new HtmlDataItem("#" + machineNumber + "������_�Ǳ�����", commonDAO.GetSignalDataValue(machineCode, eSignalDataName.�ذ��Ǳ�_ʵʱ����.ToString()) + " ��", eHtmlDataItemType.svg_text));
				datas.Add(new HtmlDataItem("#" + machineNumber + "������_�Ǳ�����", commonDAO.GetSignalDataValue(machineCode, eSignalDataName.�ذ��Ǳ�_�ȶ�.ToString()).ToLower() == "1" ? ColorTranslator.ToHtml(EquipmentStatusColors.BeReady) : ColorTranslator.ToHtml(EquipmentStatusColors.Working), eHtmlDataItemType.svg_color));
				datas.Add(new HtmlDataItem("#" + machineNumber + "������_��ǰ����", string.IsNullOrEmpty(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.��ǰ����.ToString())) ? "--" : commonDAO.GetSignalDataValue(machineCode, eSignalDataName.��ǰ����.ToString()), eHtmlDataItemType.svg_text));

				if (!string.IsNullOrWhiteSpace(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.��ǰ�����¼Id.ToString())))
				{
					CmcsBuyFuelTransport cmcsBuyFuelTransport = commonDAO.SelfDber.Get<CmcsBuyFuelTransport>(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.��ǰ�����¼Id.ToString()));
					datas.Add(new HtmlDataItem("#" + machineNumber + "������_ë��", cmcsBuyFuelTransport == null ? "0 ��" : cmcsBuyFuelTransport.GrossWeight + " ��", eHtmlDataItemType.svg_text));
					datas.Add(new HtmlDataItem("#" + machineNumber + "������_Ƥ��", cmcsBuyFuelTransport == null ? "0 ��" : cmcsBuyFuelTransport.TareWeight + " ��", eHtmlDataItemType.svg_text));
				}
				else
				{
					datas.Add(new HtmlDataItem("#" + machineNumber + "������_ë��", "0 ��", eHtmlDataItemType.svg_text));
					datas.Add(new HtmlDataItem("#" + machineNumber + "������_Ƥ��", "0 ��", eHtmlDataItemType.svg_text));
				}

				datas.Add(new HtmlDataItem("#" + machineNumber + "������_����", (!string.IsNullOrEmpty(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.��ǰ����.ToString()))).ToString(), eHtmlDataItemType.svg_visible));
				datas.Add(new HtmlDataItem("#" + machineNumber + "������_�ظ�1", commonDAO.GetSignalDataValue(machineCode, eSignalDataName.�ظ�1�ź�.ToString()).ToLower() == "1" ? ColorTranslator.ToHtml(EquipmentStatusColors.Working) : "#c0c0c0", eHtmlDataItemType.svg_color));
				datas.Add(new HtmlDataItem("#" + machineNumber + "������_�ظ�2", commonDAO.GetSignalDataValue(machineCode, eSignalDataName.�ظ�2�ź�.ToString()).ToLower() == "1" ? ColorTranslator.ToHtml(EquipmentStatusColors.Working) : "#c0c0c0", eHtmlDataItemType.svg_color));
				datas.Add(new HtmlDataItem("#" + machineNumber + "������_����1", commonDAO.GetSignalDataValue(machineCode, eSignalDataName.����1�ź�.ToString()).ToLower() == "1" ? ColorTranslator.ToHtml(EquipmentStatusColors.Working) : "#c0c0c0", eHtmlDataItemType.svg_color));
				datas.Add(new HtmlDataItem("#" + machineNumber + "������_����2", commonDAO.GetSignalDataValue(machineCode, eSignalDataName.����2�ź�.ToString()).ToLower() == "1" ? ColorTranslator.ToHtml(EquipmentStatusColors.Working) : "#c0c0c0", eHtmlDataItemType.svg_color));
				datas.Add(new HtmlDataItem("#" + machineNumber + "������_����3", commonDAO.GetSignalDataValue(machineCode, eSignalDataName.����3�ź�.ToString()).ToLower() == "1" ? ColorTranslator.ToHtml(EquipmentStatusColors.Working) : "#c0c0c0", eHtmlDataItemType.svg_color));
				datas.Add(new HtmlDataItem("#" + machineNumber + "������_��բ1", (commonDAO.GetSignalDataValue(machineCode, eSignalDataName.��բ1����.ToString()) == "0").ToString(), eHtmlDataItemType.svg_visible));
				datas.Add(new HtmlDataItem("#" + machineNumber + "������_��բ2", (commonDAO.GetSignalDataValue(machineCode, eSignalDataName.��բ2����.ToString()) == "0").ToString(), eHtmlDataItemType.svg_visible));
				datas.Add(new HtmlDataItem("#" + machineNumber + "������_����", commonDAO.GetSignalDataValue(machineCode, eSignalDataName.�ϰ�����.ToString()), eHtmlDataItemType.svg_scare));
				// ��Ӹ���...
			}
			// ���͵�ҳ��
			cefWebBrowser.Browser.GetMainFrame().ExecuteJavaScript("requestData(" + Newtonsoft.Json.JsonConvert.SerializeObject(datas) + ");", "", 0);
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			// ���治�ɼ�ʱ��ֹͣ��������
			if (!this.Visible) return;

			RequestData();
		}

		private void buttonX1_Click(object sender, EventArgs e)
		{
			// ���͵�ҳ��
			cefWebBrowser.Browser.GetMainFrame().ExecuteJavaScript("test1();", "", 0);
		}
	}
}