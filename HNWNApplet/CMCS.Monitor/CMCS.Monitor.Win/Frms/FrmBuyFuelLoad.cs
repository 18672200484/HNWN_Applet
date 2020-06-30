using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CMCS.Common;
//
using CMCS.Common.Entities;
using CMCS.Common.Entities.BaseInfo;
using CMCS.Common.Entities.CarTransport;
using CMCS.Common.Entities.TrainInFactory;
using CMCS.Monitor.Win.Frms.Sys;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Metro;
using DevComponents.DotNetBar.SuperGrid;

namespace CMCS.Monitor.Win.Frms
{
	public partial class FrmBuyFuelLoad : MetroForm
	{
		/// <summary>
		/// 窗体唯一标识符
		/// </summary>
		public static string UniqueKey = "FrmBuyFuelLoad";

		public FrmBuyFuelLoad()
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

		List<CmcsBuyFuelTransport> list = new List<CmcsBuyFuelTransport>();

		private void FrmBuyFuel_Load(object sender, EventArgs e)
		{
			InitForm();

			btnSearch_Click(null, null);
		}

		/// <summary>
		/// 窗体初始化
		/// </summary>
		private void InitForm()
		{
			dateTimeInput1.Value = DateTime.Now.Date;
			dateTimeInput2.Value = DateTime.Now.Date.AddDays(1).AddMilliseconds(-1);

			// 加载识别设备
		}

		public void BindData()
		{
			string tempSqlWhere = this.SqlWhere;
			list = Dbers.GetInstance().SelfDber.Entities<CmcsBuyFuelTransport>(tempSqlWhere + " order by InFactoryTime desc");

			superGridControl1.PrimaryGrid.DataSource = list;
		}

		private void btnSearch_Click(object sender, EventArgs e)
		{
			this.SqlWhere = string.Empty;

			if (!String.IsNullOrEmpty(txtSupplierName.Text))
			{
				SqlWhere += " and SupplierName like '%" + txtSupplierName.Text + "%'";
			}

			if (!String.IsNullOrEmpty(txtCarNumber.Text))
			{
				SqlWhere += " and CarNumber like '%" + txtCarNumber.Text + "%'";
			}
			if (!String.IsNullOrEmpty((String)dateTimeInput1.Text))
			{
				SqlWhere += " and InFactoryTime>= to_date('" + dateTimeInput1.Value.ToString("yyyy-MM-dd HH:mm:ss") + "','yyyy-mm-dd hh24:mi:ss') ";
			}
			if (!String.IsNullOrEmpty((String)dateTimeInput2.Text))
			{
				SqlWhere += " and InFactoryTime<= to_date('" + dateTimeInput2.Value.ToString("yyyy-MM-dd HH:mm:ss") + "','yyyy-mm-dd hh24:mi:ss') ";
			}

			if (!String.IsNullOrEmpty(this.SqlWhere))
			{
				SqlWhere = " where 1=1 " + SqlWhere;
			}
			BindData();
		}

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
			if (e.GridCell.ColumnIndex == -1 || e.GridCell.GridRow.Index == -1)
				return;

			//CmcsBuyFuelTransport entity = Dbers.GetInstance().SelfDber.Get<CmcsBuyFuelTransport>(superGridControl1.PrimaryGrid.GetCell(e.GridCell.GridRow.Index, superGridControl1.PrimaryGrid.Columns["clmId"].ColumnIndex).Value.ToString());
			//if (entity == null)
			//	return;
			//switch (superGridControl1.PrimaryGrid.Columns[e.GridCell.ColumnIndex].Name)
			//{
			//	case "clmPic":
			//		//抓拍图片
			//		break;
			//}
		}

		private void superGridControl1_DataBindingComplete(object sender, GridDataBindingCompleteEventArgs e)
		{
			if (list.Count > 0)
			{
				object[] sum = new object[11] { "合计", "", "", "", "", "", "车数：" + list.Count, list.Sum(a => a.TicketWeight), list.Sum(a => a.GrossWeight), list.Sum(a => a.TareWeight), list.Sum(a => a.SuttleWeight) };

				this.superGridControl1.PrimaryGrid.Rows.Insert(superGridControl1.PrimaryGrid.Rows.Count, new DevComponents.DotNetBar.SuperGrid.GridRow(sum));
			}
		}

		private void superGridControl1_GetCellFormattedValue(object sender, GridGetCellFormattedValueEventArgs e)
		{
			if (e.GridCell.ColumnIndex == -1 || e.GridCell.GridRow.Index == -1)
				return;

			switch (superGridControl1.PrimaryGrid.Columns[e.GridCell.ColumnIndex].Name)
			{
				case "抓拍":
					//抓拍图片
					break;
			}

		}
		#endregion

		private void buttonX1_Click(object sender, EventArgs e)
		{
			FrmBuyFuelLoadToday frm = new FrmBuyFuelLoadToday();
			FrmMainFrame.superTabControlManager.CreateTab(frm.Text, frm.Text, frm, true);
		}

	}
}
