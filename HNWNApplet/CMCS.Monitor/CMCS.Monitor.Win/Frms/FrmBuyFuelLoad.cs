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
            List<CmcsBuyFuelTransport> list = Dbers.GetInstance().SelfDber.ExecutePager<CmcsBuyFuelTransport>(PageSize, CurrentIndex, tempSqlWhere + " order by InFactoryTime desc");

            GetTotalCount(tempSqlWhere);
            superGridControl1.PrimaryGrid.DataSource = list;
            PagerControlStatue();

            lblPagerInfo.Text = string.Format("共 {0} 条记录，每页 {1} 条，共 {2} 页，当前第 {3} 页", TotalCount, PageSize, PageCount, CurrentIndex + 1);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.SqlWhere = string.Empty;

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

        #region Pager

        private void btnPagerCommand_Click(object sender, EventArgs e)
        {
            ButtonX btn = sender as ButtonX;
            switch (btn.CommandParameter.ToString())
            {
                case "First":
                    CurrentIndex = 0;
                    break;
                case "Previous":
                    CurrentIndex = CurrentIndex - 1;
                    break;
                case "Next":
                    CurrentIndex = CurrentIndex + 1;
                    break;
                case "Last":
                    CurrentIndex = PageCount - 1;
                    break;
            }

            BindData();
        }

        public void PagerControlStatue()
        {
            if (PageCount <= 1)
            {
                btnFirst.Enabled = false;
                btnPrevious.Enabled = false;
                btnLast.Enabled = false;
                btnNext.Enabled = false;

                return;
            }

            if (CurrentIndex == 0)
            {
                // 首页
                btnFirst.Enabled = false;
                btnPrevious.Enabled = false;
                btnLast.Enabled = true;
                btnNext.Enabled = true;
            }

            if (CurrentIndex > 0 && CurrentIndex < PageCount - 1)
            {
                // 上一页/下一页
                btnFirst.Enabled = true;
                btnPrevious.Enabled = true;
                btnLast.Enabled = true;
                btnNext.Enabled = true;
            }

            if (CurrentIndex == PageCount - 1)
            {
                // 末页
                btnFirst.Enabled = true;
                btnPrevious.Enabled = true;
                btnLast.Enabled = false;
                btnNext.Enabled = false;
            }
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

        #endregion


        private void superGridControl1_CellMouseDown(object sender, GridCellMouseEventArgs e)
        {
            if (e.GridCell.ColumnIndex == -1 || e.GridCell.GridRow.Index == -1)
                return;

            CmcsBuyFuelTransport entity = Dbers.GetInstance().SelfDber.Get<CmcsBuyFuelTransport>(superGridControl1.PrimaryGrid.GetCell(e.GridCell.GridRow.Index, superGridControl1.PrimaryGrid.Columns["clmId"].ColumnIndex).Value.ToString());
            if (entity == null)
                return;
            switch (superGridControl1.PrimaryGrid.Columns[e.GridCell.ColumnIndex].Name)
            {
                case "clmPic":
                    //抓拍图片
                    break;
            }
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            FrmBuyFuelLoadToday frm = new FrmBuyFuelLoadToday();
            FrmMainFrame.superTabControlManager.CreateTab(frm.Text, frm.Text, frm, true);
        }

        private void superGridControl1_CellValidated(object sender, GridCellValidatedEventArgs e)
        {

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
    }
}
