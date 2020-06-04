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
using CMCS.Common.DapperDber_etc;
using CMCS.Common.Entities.Fuel;
using CMCS.Common.Entities.Inf;
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
	public partial class FrmSampleStorage : DevComponents.DotNetBar.Metro.MetroForm
	{
		/// <summary>
		/// ����Ψһ��ʶ��
		/// </summary>
		public static string UniqueKey = "FrmSampleStorage";

		CommonDAO commonDAO = CommonDAO.GetInstance();
		MonitorCommon monitorCommon = MonitorCommon.GetInstance();
		CefWebBrowserEx cefWebBrowser = new CefWebBrowserEx();
		string LastCell = string.Empty;

		public FrmSampleStorage()
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
			cefWebBrowser.StartUrl = SelfVars.Url_SampleStorage;
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

		private void FrmSampleStorage_Load(object sender, EventArgs e)
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
			string cell = SelfVars.CurrentSelectedCell;
			List<HtmlDataItem> datas = new List<HtmlDataItem>();

			datas.Clear();
			int overday = commonDAO.GetCommonAppletConfigInt32("������������");
			IList<InfAutoCupBoard> cellList = commonDAO.SelfDber.Entities<InfAutoCupBoard>("where CupBoardType=:CupBoardType and ParentPKID is not null", new { CupBoardType = cell });
			decimal useCount = cellList.Count(a => a.State == 1);
			decimal emptyCount = cellList.Count - useCount;
			decimal usePercent = cellList.Count > 0 ? Math.Round(useCount / cellList.Count, 2, MidpointRounding.AwayFromZero) : 0;
			decimal overCount = cellList.Count(a => a.State == 1 && a.SaveTime < DateTime.Now.AddDays(-overday));
			decimal overPercent = useCount > 0 ? Math.Round(overCount / useCount, 2, MidpointRounding.AwayFromZero) : 0;


			datas.Add(new HtmlDataItem("�ܲ�λ", cellList.Count.ToString(), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("�Ѵ��λ", useCount.ToString(), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("δ���λ", emptyCount.ToString(), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("���ڲ�λ", overCount.ToString(), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("������", (usePercent * 100).ToString("F2") + "%", eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("������", (overPercent * 100).ToString("F2") + "%", eHtmlDataItemType.svg_text));

			IList<RowResult> rowResult = new List<RowResult>();
			if (cellList.Count > 0)
			{
				if (cell == "ԭú������")
				{
					int maxRow = cellList.OrderByDescending(a => a.RowNumber).First().RowNumber;
					int maxCell = cellList.OrderByDescending(a => a.CellNumber).First().CellNumber;
					for (int i = 1; i <= maxRow; i++)
					{
						RowResult rowEntity = new RowResult();
						rowEntity.RowName = String.Format("�� {0} ��", i);
						rowEntity.CellList = new List<CellResult>();
						for (int j = 1; j <= maxCell; j++)
						{
							CellResult cellEntity = new CellResult();
							InfAutoCupBoard sampleCell = cellList.Where(a => a.RowNumber == i && a.CellNumber == j).FirstOrDefault();
							if (sampleCell == null)
								cellEntity.CellFlag = -1;
							else
							{
								cellEntity.CellNumber = sampleCell.CupBoardDes.Replace("��", "").Replace("��", "").Remove(0, 2);
								//���ű�ʶ��-1û��һ��0�չ�1�Ѵ�ţ�2��������3ͣ��
								if (sampleCell.IsValid == "0") cellEntity.CellFlag = 3;
								else if (sampleCell.State == 1 && sampleCell.SaveTime < DateTime.Now.AddDays(-overday))
									cellEntity.CellFlag = 2;
								else if (sampleCell.State == 1)
									cellEntity.CellFlag = 1;
								else
									cellEntity.CellFlag = 0;
							}
							rowEntity.CellList.Add(cellEntity);
						}
						rowResult.Add(rowEntity);
					}
				}
				else
				{
					int maxRow = 5;
					int maxCell = 4;
					for (int i = 1; i <= maxRow; i++)
					{
						RowResult rowEntity = new RowResult();
						rowEntity.RowName = String.Format("�� {0} ��", i);
						rowEntity.CellList = new List<CellResult>();
						for (int j = 1; j <= maxCell; j++)
						{
							CellResult cellEntity = new CellResult();

							cellEntity.CellNumber = string.Format("{0}��{1}��", i, j);
							string cellCode = i.ToString().PadLeft(2, '0') + "G" + j.ToString().PadLeft(2, '0');
							cellEntity.CellCode = cellCode;
							IList<InfAutoCupBoard> sampleCell = cellList.Where(a => a.CupBoardCode.Contains(cellCode)).ToList();

							//���ű�ʶ��-1û��һ��0�չ�1�Ѵ�ţ�2��������3ͣ��
							if (cellList.Count(a => a.State == 0 && a.CupBoardCode.Contains(cellCode)) == 60)
								cellEntity.CellFlag = 0;
							else if (cellList.Count(a => a.State == 1 && a.CupBoardCode.Contains(cellCode)) > 0)
								cellEntity.CellFlag = 1;
							else if (cellList.Count(a => a.State == 1 && a.SaveTime < DateTime.Now.AddDays(-overday) && a.CupBoardCode.Contains(cellCode)) > 0)
								cellEntity.CellFlag = 2;
							else if (cellList.Count(a => a.IsValid == "0" && a.CupBoardCode.Contains(cellCode)) == 60)
								cellEntity.CellFlag = 3;
							rowEntity.CellList.Add(cellEntity);
						}
						rowResult.Add(rowEntity);
					}
				}
			}

			datas.Add(new HtmlDataItem("������Ϣ", Newtonsoft.Json.JsonConvert.SerializeObject(rowResult), eHtmlDataItemType.json_data));


			// ��Ӹ���...

			// ���͵�ҳ��
			cefWebBrowser.Browser.GetMainFrame().ExecuteJavaScript("requestData(" + Newtonsoft.Json.JsonConvert.SerializeObject(datas) + ");", "", 0);
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			// ���治�ɼ�ʱ��ֹͣ��������
			if (!this.Visible) return;

			if (this.LastCell != SelfVars.CurrentSelectedCell)  //ѡ��������б仯ʱ��������
			{
				RequestData();
				this.LastCell = SelfVars.CurrentSelectedCell;
			}
			else if (DateTime.Now.Second % 5 == 0)  //5�����һ��
				RequestData();

		}

		#region ��������
		private Decimal ParseDecimal(object obj)
		{
			Decimal result = 0m;
			if (obj != null)
				Decimal.TryParse(obj.ToString(), out result);
			return result;
		}
		#endregion
	}

	/// <summary>
	/// ��
	/// </summary>
	public class RowResult
	{
		/// <summary>
		/// �ڼ���
		/// </summary>
		public virtual String RowName { get; set; }
		public virtual IList<CellResult> CellList { get; set; }
	}

	/// <summary>
	/// ��
	/// </summary>
	public class CellResult
	{
		/// <summary>
		/// ���ű��
		/// </summary>
		public virtual String CellNumber { get; set; }
		/// <summary>
		/// ���ű�ʶ��-1û��һ��0�չ�1�Ѵ�ţ�2��������3ͣ��
		/// </summary>
		public virtual Int32 CellFlag { get; set; }

		/// <summary>
		/// ���ű���
		/// </summary>
		public virtual string CellCode { get; set; }

		///// <summary>
		///// ���� �ٲ��� �������� �������
		///// </summary>
		//public virtual IList<CellResult> CellList { get; set; }
	}
}