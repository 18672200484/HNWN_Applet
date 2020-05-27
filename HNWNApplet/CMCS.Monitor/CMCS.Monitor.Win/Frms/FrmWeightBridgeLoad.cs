using System;
using System.Collections.Generic;
using System.Windows.Forms;
//
using CMCS.Common.Entities;
using CMCS.Common;
using DevComponents.DotNetBar;
using System.Linq;
using DevComponents.DotNetBar.SuperGrid;
using DevComponents.DotNetBar.Metro;
using CMCS.Monitor.Win.Frms.Sys;
using CMCS.Common.Entities.TrainInFactory;
using CMCS.Common.Entities.BaseInfo;

namespace CMCS.Monitor.Win.Frms
{
    public partial class FrmWeightBridgeLoad : MetroForm
    {
        /// <summary>
        /// 窗体唯一标识符
        /// </summary>
        public static string UniqueKey = "FrmWeightBridgeLoad";

        public FrmWeightBridgeLoad()
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


        private void FrmWeightBridge_Load(object sender, EventArgs e)
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
            cmbEquipment.DisplayMember = "EquipmentName";
            cmbEquipment.ValueMember = "EquipmentCode";
            cmbEquipment.DataSource = Dbers.GetInstance().SelfDber.Entities<CmcsCMEquipment>("WHERE PARENTID IN (SELECT ID FROM CMCSTBCMEQUIPMENT A WHERE A.EQUIPMENTCODE in ('火车入厂动态衡','火车入厂静态衡')) ORDER BY SEQUENCE");
            cmbEquipment.SelectedIndex = 0;
        }

        public class CmcsTrainWeightRecordTemp : CmcsTrainWeightRecord
        {
            public int TrueNumber { get; set; }
        }

        public void BindData()
        {
            string tempSqlWhere = this.SqlWhere;
            List<CmcsTrainWeightRecord> list = Dbers.GetInstance().SelfDber.ExecutePager<CmcsTrainWeightRecord>(PageSize, CurrentIndex, tempSqlWhere + " order by OrderNumber desc");

            GetTotalCount(tempSqlWhere);
            superGridControl1.PrimaryGrid.DataSource = list;
            PagerControlStatue();

            lblPagerInfo.Text = string.Format("共 {0} 条记录，每页 {1} 条，共 {2} 页，当前第 {3} 页", TotalCount, PageSize, PageCount, CurrentIndex + 1);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.SqlWhere = string.Empty;

            CmcsCMEquipment cMEquipment = cmbEquipment.SelectedItem as CmcsCMEquipment;
            if (cMEquipment != null) SqlWhere += " and MachineCode='" + cMEquipment.EquipmentCode + "' ";

            if (!String.IsNullOrEmpty((String)dateTimeInput1.Text))
            {
                SqlWhere += " and GrossTime>= to_date('" + dateTimeInput1.Value.ToString("yyyy-MM-dd HH:mm:ss") + "','yyyy-mm-dd hh24:mi:ss') ";
            }
            if (!String.IsNullOrEmpty((String)dateTimeInput2.Text))
            {
                SqlWhere += " and GrossTime<= to_date('" + dateTimeInput2.Value.ToString("yyyy-MM-dd HH:mm:ss") + "','yyyy-mm-dd hh24:mi:ss') ";
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
            TotalCount = Dbers.GetInstance().SelfDber.Count<CmcsTrainWeightRecord>(sqlWhere);
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

            SuperGridControl sgc = (SuperGridControl)sender;
            CmcsTrainWeightRecord entity = Dbers.GetInstance().SelfDber.Get<CmcsTrainWeightRecord>(sgc.PrimaryGrid.GetCell(e.GridCell.GridRow.Index, 13).Value.ToString());
            if (entity == null)
                return;
            String newid = sgc.PrimaryGrid.GetCell(e.GridCell.GridRow.Index, 13).Value.ToString();
            switch (sgc.PrimaryGrid.GetCell(e.GridCell.GridRow.Index, e.GridCell.ColumnIndex).NullString)
            {
                case "抓拍":
                    if (Dbers.GetInstance().SelfDber.Entities<CmcsTrainWatch>(String.Format(" where TrainWeightRecordId='{0}'", newid)).Count > 0)
                    {
                        FrmWeightBridgeLoad_Pic frm1 = new FrmWeightBridgeLoad_Pic(newid);
                        if (frm1.ShowDialog() == DialogResult.OK)
                        {
                        }
                    }
                    else
                    {
                    }
                    break;
                case "装车线":
                    if (Dbers.GetInstance().SelfDber.Entities<CmcsTrainLine>(String.Format(" where TrainWeightRecordId='{0}'", newid)).Count > 0)
                    {
                        FrmWeightBridgeLoad_Line frm = new FrmWeightBridgeLoad_Line(newid);
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                        }
                    }
                    else
                    {
                    }
                    break;
            }
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {

            FrmWeightBridgeLoadToday frm = new FrmWeightBridgeLoadToday();
            FrmMainFrame.superTabControlManager.CreateTab(frm.Text, frm.Text, frm, true);
        }

        private void superGridControl1_CellValidated(object sender, GridCellValidatedEventArgs e)
        {

        }

        private void superGridControl1_GetCellFormattedValue(object sender, GridGetCellFormattedValueEventArgs e)
        {
            if (e.GridCell.ColumnIndex == -1 || e.GridCell.GridRow.Index == -1)
                return;

            SuperGridControl sgc = (SuperGridControl)sender;
            String newid = sgc.PrimaryGrid.GetCell(e.GridCell.GridRow.Index, 13).Value.ToString();
            switch (sgc.PrimaryGrid.GetCell(e.GridCell.GridRow.Index, e.GridCell.ColumnIndex).NullString)
            {
                case "抓拍":
                    if (Dbers.GetInstance().SelfDber.Entities<CmcsTrainWatch>(String.Format(" where TrainWeightRecordId='{0}'", newid)).Count == 0)
                    {
                        sgc.PrimaryGrid.GetCell(e.GridCell.GridRow.Index, e.GridCell.ColumnIndex).Value = "";
                    }
                    break;
                case "装车线":
                    if (Dbers.GetInstance().SelfDber.Entities<CmcsTrainLine>(String.Format(" where TrainWeightRecordId='{0}'", newid)).Count == 0)
                    {
                        sgc.PrimaryGrid.GetCell(e.GridCell.GridRow.Index, e.GridCell.ColumnIndex).Value = "";
                    }
                    break;
            }

        }
    }
}
