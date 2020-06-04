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
		/// 窗体唯一标识符
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
		/// 窗体初始化
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
			string cell = SelfVars.CurrentSelectedCell;
			List<HtmlDataItem> datas = new List<HtmlDataItem>();

			datas.Clear();
			int overday = commonDAO.GetCommonAppletConfigInt32("存样柜超期天数");
			IList<InfAutoCupBoard> cellList = commonDAO.SelfDber.Entities<InfAutoCupBoard>("where CupBoardType=:CupBoardType and ParentPKID is not null", new { CupBoardType = cell });
			decimal useCount = cellList.Count(a => a.State == 1);
			decimal emptyCount = cellList.Count - useCount;
			decimal usePercent = cellList.Count > 0 ? Math.Round(useCount / cellList.Count, 2, MidpointRounding.AwayFromZero) : 0;
			decimal overCount = cellList.Count(a => a.State == 1 && a.SaveTime < DateTime.Now.AddDays(-overday));
			decimal overPercent = useCount > 0 ? Math.Round(overCount / useCount, 2, MidpointRounding.AwayFromZero) : 0;


			datas.Add(new HtmlDataItem("总仓位", cellList.Count.ToString(), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("已存仓位", useCount.ToString(), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("未存仓位", emptyCount.ToString(), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("超期仓位", overCount.ToString(), eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("存样率", (usePercent * 100).ToString("F2") + "%", eHtmlDataItemType.svg_text));
			datas.Add(new HtmlDataItem("超期率", (overPercent * 100).ToString("F2") + "%", eHtmlDataItemType.svg_text));

			IList<RowResult> rowResult = new List<RowResult>();
			if (cellList.Count > 0)
			{
				if (cell == "原煤样样柜")
				{
					int maxRow = cellList.OrderByDescending(a => a.RowNumber).First().RowNumber;
					int maxCell = cellList.OrderByDescending(a => a.CellNumber).First().CellNumber;
					for (int i = 1; i <= maxRow; i++)
					{
						RowResult rowEntity = new RowResult();
						rowEntity.RowName = String.Format("第 {0} 层", i);
						rowEntity.CellList = new List<CellResult>();
						for (int j = 1; j <= maxCell; j++)
						{
							CellResult cellEntity = new CellResult();
							InfAutoCupBoard sampleCell = cellList.Where(a => a.RowNumber == i && a.CellNumber == j).FirstOrDefault();
							if (sampleCell == null)
								cellEntity.CellFlag = -1;
							else
							{
								cellEntity.CellNumber = sampleCell.CupBoardDes.Replace("层", "").Replace("格", "").Remove(0, 2);
								//柜门标识：-1没这一格，0空柜，1已存放，2超期样，3停用
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
						rowEntity.RowName = String.Format("第 {0} 排", i);
						rowEntity.CellList = new List<CellResult>();
						for (int j = 1; j <= maxCell; j++)
						{
							CellResult cellEntity = new CellResult();

							cellEntity.CellNumber = string.Format("{0}排{1}号", i, j);
							string cellCode = i.ToString().PadLeft(2, '0') + "G" + j.ToString().PadLeft(2, '0');
							cellEntity.CellCode = cellCode;
							IList<InfAutoCupBoard> sampleCell = cellList.Where(a => a.CupBoardCode.Contains(cellCode)).ToList();

							//柜门标识：-1没这一格，0空柜，1已存放，2超期样，3停用
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

			datas.Add(new HtmlDataItem("样柜信息", Newtonsoft.Json.JsonConvert.SerializeObject(rowResult), eHtmlDataItemType.json_data));


			// 添加更多...

			// 发送到页面
			cefWebBrowser.Browser.GetMainFrame().ExecuteJavaScript("requestData(" + Newtonsoft.Json.JsonConvert.SerializeObject(datas) + ");", "", 0);
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			// 界面不可见时，停止发送数据
			if (!this.Visible) return;

			if (this.LastCell != SelfVars.CurrentSelectedCell)  //选择的样柜有变化时立即更新
			{
				RequestData();
				this.LastCell = SelfVars.CurrentSelectedCell;
			}
			else if (DateTime.Now.Second % 5 == 0)  //5秒更新一次
				RequestData();

		}

		#region 辅助函数
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
	/// 行
	/// </summary>
	public class RowResult
	{
		/// <summary>
		/// 第几层
		/// </summary>
		public virtual String RowName { get; set; }
		public virtual IList<CellResult> CellList { get; set; }
	}

	/// <summary>
	/// 列
	/// </summary>
	public class CellResult
	{
		/// <summary>
		/// 柜门编号
		/// </summary>
		public virtual String CellNumber { get; set; }
		/// <summary>
		/// 柜门标识：-1没这一格，0空柜，1已存放，2超期样，3停用
		/// </summary>
		public virtual Int32 CellFlag { get; set; }

		/// <summary>
		/// 柜门编码
		/// </summary>
		public virtual string CellCode { get; set; }

		///// <summary>
		///// 用于 仲裁样 化验室样 多个柜子
		///// </summary>
		//public virtual IList<CellResult> CellList { get; set; }
	}
}