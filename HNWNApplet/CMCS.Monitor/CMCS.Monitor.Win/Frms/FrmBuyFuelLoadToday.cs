using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using CMCS.Common;
//
using CMCS.Common.Entities;
using CMCS.Common.Entities.CarTransport;
using CMCS.Common.Entities.TrainInFactory;
using CMCS.Common.Utilities;
using CMCS.Monitor.Win.Frms.Sys;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Metro;
using DevComponents.DotNetBar.SuperGrid;

namespace CMCS.Monitor.Win.Frms
{
	public partial class FrmBuyFuelLoadToday : MetroForm
	{
		/// <summary>
		/// 窗体唯一标识符
		/// </summary>
		public static string UniqueKey = "FrmBuyFuelLoadToday";

		public FrmBuyFuelLoadToday()
		{
			InitializeComponent();
		}

		/// <summary>
		/// 每页显示行数
		/// </summary>
		int PageSize = 28;

		/// <summary>
		/// 总页数
		/// </summary>
		int PageCount = 0;

		/// <summary>
		/// 总记录数
		/// </summary>
		int TotalCount = 0;

		/// <summary>
		/// 当前页索引
		/// </summary>
		int CurrentIndex = 0;

		string SqlWhere = string.Empty;
		string Id;

		List<CmcsBuyFuelTransport> list = new List<CmcsBuyFuelTransport>();

		private void FrmBuyFuel_Load(object sender, EventArgs e)
		{
			InitForm();
			BindData();
		}

		/// <summary>
		/// 窗体初始化
		/// </summary>
		private void InitForm()
		{
		}

		public class CmcsTrainWeightRecordTemp : CmcsTrainWeightRecord
		{
			public int TrueNumber { get; set; }
		}

		public void BindData()
		{
			string tempSqlWhere = this.SqlWhere;
			list = Dbers.GetInstance().SelfDber.Entities<CmcsBuyFuelTransport>(" where InFactoryTime>=to_date('" + DateTime.Now.ToString("yyyy-MM-dd") + "','yyyy-mm-dd') order by InFactoryTime desc");

			superGridControl1.PrimaryGrid.DataSource = list;
			if (list.Count > 0)
			{
				Id = list[0].Id;
			}

		}

		private void btnSearch_Click(object sender, EventArgs e)
		{
			FrmBuyFuelLoad frm = new FrmBuyFuelLoad();
			FrmMainFrame.superTabControlManager.CreateTab(frm.Text, frm.Text, frm, true);
		}

		#region Pager

		private void btnPagerCommand_Click(object sender, EventArgs e)
		{
			BindData();
		}

		private void GetTotalCount(string sqlWhere)
		{
			TotalCount = Dbers.GetInstance().SelfDber.Count<CmcsBuyFuelTransport>(sqlWhere);
			if (TotalCount % PageSize != 0)
				PageCount = TotalCount / PageSize + 1;
			else
				PageCount = TotalCount / PageSize;
		}
		#endregion

		#region SuperGridControl

		private void superGridControl1_GetRowHeaderText(object sender, DevComponents.DotNetBar.SuperGrid.GridGetRowHeaderTextEventArgs e)
		{
			e.Text = ((this.CurrentIndex * this.PageSize) + e.GridRow.RowIndex + 1).ToString();
		}

		private void superGridControl1_BeginEdit(object sender, GridEditEventArgs e)
		{
			// 取消编辑
			e.Cancel = true;
		}


		private void superGridControl1_CellMouseDown(object sender, GridCellMouseEventArgs e)
		{
			if (e.GridCell.GridRow.Index == -1)
				return;
			e.GridCell.GridRow.IsSelected = true;
			SuperGridControl supergridcontrol = (SuperGridControl)sender;
			Id = supergridcontrol.GetCell(e.GridCell.GridRow.RowIndex, 11).Value.ToString();
		}

		private void superGridControl1_DataBindingComplete(object sender, GridDataBindingCompleteEventArgs e)
		{
			if (list.Count > 0)
			{
				object[] sum = new object[10] { "合计", "", "", "", "", "车数：" + list.Count, list.Sum(a => a.TicketWeight), list.Sum(a => a.GrossWeight), list.Sum(a => a.TareWeight), list.Sum(a => a.SuttleWeight) };

				this.superGridControl1.PrimaryGrid.Rows.Insert(superGridControl1.PrimaryGrid.Rows.Count, new DevComponents.DotNetBar.SuperGrid.GridRow(sum));
			}
		}

		#endregion

		List<CmcsTrainWatch> newcmcstrainwatchs;
		int SelectedIndex = 0;
		private void superGridControl1_RowHeaderClick(object sender, GridRowHeaderClickEventArgs e)
		{
			if (e.GridRow.Index == -1)
				return;
			e.GridRow.IsSelected = true;
			SuperGridControl supergridcontrol = (SuperGridControl)sender;
			Id = supergridcontrol.GetCell(e.GridRow.RowIndex, 11).Value.ToString();
		}

		private void BtnClick(object sender, EventArgs e)
		{
			if (newcmcstrainwatchs != null)
			{
				if (((ButtonX)sender).Text == "下一张")
				{
					if (SelectedIndex == newcmcstrainwatchs.Count - 1)
					{
						SelectedIndex = 0;
					}
					else
					{
						SelectedIndex++;
					}
				}
				else if ((((ButtonX)sender).Text == "上一张"))
				{
					if (SelectedIndex == 0)
					{
						SelectedIndex = newcmcstrainwatchs.Count - 1;
					}
					else
					{
						SelectedIndex--;
					}
				}
			}
		}


		private void FrmBuyFuelLoadToday_Shown(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// 刷新
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnRefresh_Click(object sender, EventArgs e)
		{
			BindData();
		}
	}
}
