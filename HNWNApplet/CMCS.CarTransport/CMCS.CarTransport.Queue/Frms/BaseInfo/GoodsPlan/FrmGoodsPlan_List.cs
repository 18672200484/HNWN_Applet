﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CMCS.CarTransport.DAO;
using CMCS.CarTransport.Queue.Core;
using CMCS.CarTransport.Queue.Enums;
using CMCS.Common;
using CMCS.Common.Entities.BaseInfo;
using CMCS.Common.Entities.CarTransport;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Metro;
using DevComponents.DotNetBar.SuperGrid;

namespace CMCS.CarTransport.Queue.Frms.BaseInfo.GoodsPlan
{
    public partial class FrmGoodsPlan_List : MetroAppForm
    {
        /// <summary>
        /// 窗体唯一标识符
        /// </summary>
        public static string UniqueKey = "FrmGoodsPlan_List";

        /// <summary>
        /// 每页显示行数
        /// </summary>
        int PageSize = 18;

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

        public FrmGoodsPlan_List()
        {
            InitializeComponent();
        }

        private void FrmGoodsPlan_List_Load(object sender, EventArgs e)
        {
            superGridControl1.PrimaryGrid.AutoGenerateColumns = false;

            //01查看 02增加 03修改 04删除
            btnAdd.Visible = QueuerDAO.GetInstance().CheckPower(this.GetType().ToString(), "02", GlobalVars.LoginUser);
            GridColumn clmEdit = superGridControl1.PrimaryGrid.Columns["clmEdit"];
            clmEdit.Visible = QueuerDAO.GetInstance().CheckPower(this.GetType().ToString(), "03", GlobalVars.LoginUser);
            GridColumn clmDelete = superGridControl1.PrimaryGrid.Columns["clmDelete"];
            clmDelete.Visible = QueuerDAO.GetInstance().CheckPower(this.GetType().ToString(), "04", GlobalVars.LoginUser);

            btnSearch_Click(null, null);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.SqlWhere = " where 1=1";

            if (!string.IsNullOrEmpty(txt_CarNumber.Text)) this.SqlWhere += " and CarNumber like '%" + txt_CarNumber.Text + "%'";

            CurrentIndex = 0;

            BindData();
        }

        public void BindData()
        {
            string tempSqlWhere = this.SqlWhere;
            List<CmcsGoodsPlan> list = Dbers.GetInstance().SelfDber.ExecutePager<CmcsGoodsPlan>(PageSize, CurrentIndex, tempSqlWhere + " order by OperDate desc");
            superGridControl1.PrimaryGrid.DataSource = list;

            GetTotalCount(tempSqlWhere);
            PagerControlStatue();

            lblPagerInfo.Text = string.Format("共 {0} 条记录，每页 {1} 条，共 {2} 页，当前第 {3} 页", TotalCount, PageSize, PageCount, CurrentIndex + 1);
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
            TotalCount = Dbers.GetInstance().SelfDber.Count<CmcsGoodsPlan>(sqlWhere);
            if (TotalCount % PageSize != 0)
                PageCount = TotalCount / PageSize + 1;
            else
                PageCount = TotalCount / PageSize;
        }

        #endregion

        private void btnAdd_Click(object sender, EventArgs e)
        {
            FrmGoodsPlan_Oper frmEdit = new FrmGoodsPlan_Oper(Guid.NewGuid().ToString(), eEditMode.新增);
            if (frmEdit.ShowDialog() == DialogResult.OK)
            {
                BindData();
            }
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            this.SqlWhere = string.Empty;
            txt_CarNumber.Text = string.Empty;
            CurrentIndex = 0;
            BindData();
        }

        private void superGridControl1_DataBindingComplete(object sender, DevComponents.DotNetBar.SuperGrid.GridDataBindingCompleteEventArgs e)
        {
            foreach (GridRow gridRow in e.GridPanel.Rows)
            {
                CmcsGoodsPlan entity = gridRow.DataItem as CmcsGoodsPlan;
                if (entity == null) return;
            }
        }

        private void superGridControl1_BeginEdit(object sender, GridEditEventArgs e)
        {
            // 取消编辑
            e.Cancel = true;
        }

        private void superGridControl1_CellMouseDown(object sender, GridCellMouseEventArgs e)
        {
            CmcsGoodsPlan entity = Dbers.GetInstance().SelfDber.Get<CmcsGoodsPlan>(superGridControl1.PrimaryGrid.GetCell(e.GridCell.GridRow.Index, superGridControl1.PrimaryGrid.Columns["clmId"].ColumnIndex).Value.ToString());
            switch (superGridControl1.PrimaryGrid.Columns[e.GridCell.ColumnIndex].Name)
            {

                case "clmShow":
                    FrmGoodsPlan_Oper frmShow = new FrmGoodsPlan_Oper(entity.Id, eEditMode.查看);
                    if (frmShow.ShowDialog() == DialogResult.OK)
                    {
                        BindData();
                    }
                    break;
                case "clmEdit":
                    FrmGoodsPlan_Oper frmEdit = new FrmGoodsPlan_Oper(entity.Id, eEditMode.修改);
                    if (frmEdit.ShowDialog() == DialogResult.OK)
                    {
                        BindData();
                    }
                    break;
                case "clmDelete":
                    // 查询正在使用该供应商的车数 
                    if (MessageBoxEx.Show("确定要删除该配置信息？", "操作提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        try
                        {
                            Dbers.GetInstance().SelfDber.Delete<CmcsGoodsPlan>(entity.Id);
                        }
                        catch (Exception)
                        {
                            MessageBoxEx.Show("该配置信息正在使用中，禁止删除！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        BindData();
                    }
                    break;
            }
        }

        /// <summary>
        ///设置行号  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void superGridControl1_GetRowHeaderText(object sender, GridGetRowHeaderTextEventArgs e)
        {
            e.Text = (e.GridRow.RowIndex + 1).ToString();
        }

    }
}
