using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CMCS.CarTransport.DAO;
using CMCS.Common;
using CMCS.Common.DAO;
using CMCS.Common.Entities;
using CMCS.Common.Entities.BaseInfo;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.SuperGrid;

namespace CMCS.CarTransport.Queue.Frms.BaseInfo.Mine
{
    public partial class FrmMine_Select : DevComponents.DotNetBar.Metro.MetroForm
    {
        /// <summary>
        /// 选中的实体
        /// </summary>
        public CmcsMine Output;

        /// <summary>
        /// 条件语句
        /// </summary>
        string sqlWhere;

        public FrmMine_Select(string sqlWhere)
        {
            InitializeComponent();

            this.sqlWhere = sqlWhere;

            Search(string.Empty);
        }

        private void FrmMine_Select_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Output = null;
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void txtInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (superGridControl1.PrimaryGrid.Rows.Count > 0) superGridControl1.Focus();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                Return();
            }
            else
            {
                Search(txtInput.Text.Trim());
            }
        }

        void Search(string input)
        {
            List<CmcsMine> list = CommonDAO.GetInstance().GetMineByNameOrChs(input.Trim(), sqlWhere);
            superGridControl1.PrimaryGrid.DataSource = list;
        }

        void Return()
        {
            GridRow gridRow = superGridControl1.PrimaryGrid.ActiveRow as GridRow;
            if (gridRow == null) return;

            this.Output = (gridRow.DataItem as CmcsMine);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void superGridControl1_BeginEdit(object sender, DevComponents.DotNetBar.SuperGrid.GridEditEventArgs e)
        {
            // 取消编辑模式
            e.Cancel = true;
        }

        /// <summary>
        /// 设置行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void superGridControl_GetRowHeaderText(object sender, DevComponents.DotNetBar.SuperGrid.GridGetRowHeaderTextEventArgs e)
        {
            e.Text = (e.GridRow.RowIndex + 1).ToString();
        }

        private void superGridControl1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) Return();
        }

        private void superGridControl1_CellDoubleClick(object sender, GridCellDoubleClickEventArgs e)
        {
            Return();
        } 
    }
}