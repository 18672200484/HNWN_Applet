using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CMCS.CarTransport.Queue.Core;
using CMCS.CarTransport.Queue.Enums;
using CMCS.CarTransport.Queue.Frms.BaseInfo.Autotruck;
using CMCS.CarTransport.Queue.Frms.BaseInfo.Supplier;
using CMCS.Common;
using CMCS.Common.Entities.BaseInfo;
using CMCS.Common.Entities.CarTransport;
using CMCS.Common.Enums;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Metro;

namespace CMCS.CarTransport.Queue.Frms.BaseInfo.SupplierAssignSampler
{
    public partial class FrmSupplierAssignSampler_Oper : MetroForm
    {
        //业务id
        string PId = String.Empty;

        //编辑模式
        eEditMode EditMode = eEditMode.默认;

        CmcsSupplierAssignSampler cmcsSupplierAssignSampler;

        public FrmSupplierAssignSampler_Oper(string pId, eEditMode editMode)
        {
            InitializeComponent();

            this.PId = pId;
            this.EditMode = editMode;
        }

        private void FrmSupplierAssignSampler_Oper_Load(object sender, EventArgs e)
        {
            cmbSampler.Items.Add("01");
            cmbSampler.Items.Add("02");
            cmbSampler.Items.Add("03");
            cmbSampler.SelectedIndex = 0;

            this.cmcsSupplierAssignSampler = Dbers.GetInstance().SelfDber.Get<CmcsSupplierAssignSampler>(this.PId);
            if (this.cmcsSupplierAssignSampler != null)
            {
                txtSupplierName.Text = cmcsSupplierAssignSampler.SupplierName;
                cmbSampler.SelectedItem = cmcsSupplierAssignSampler.Sampler;
                txtRemark_Goods.Text = cmcsSupplierAssignSampler.Remark;
            }

            if (this.EditMode == eEditMode.查看)
            {
                btnSubmit.Enabled = false;
                HelperUtil.ControlReadOnly(panelEx2, true);
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSupplierName.Text))
            {
                MessageBoxEx.Show("供货单位不能为空！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if ((cmcsSupplierAssignSampler == null || cmcsSupplierAssignSampler.SupplierName != txtSupplierName.Text) && Dbers.GetInstance().SelfDber.Entities<CmcsSupplierAssignSampler>(" where SupplierName=:SupplierName", new { SupplierName = txtSupplierName.Text }).Count > 0)
            {
                MessageBoxEx.Show("该供货单位指定采样机计划已存在！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (this.EditMode == eEditMode.修改)
            {
                if (this.SelectedSupplier != null)
                {
                    cmcsSupplierAssignSampler.SupplierId = this.SelectedSupplier.Id;
                    cmcsSupplierAssignSampler.SupplierName = this.SelectedSupplier.Name;
                }
                cmcsSupplierAssignSampler.Sampler = cmbSampler.SelectedItem.ToString();
                cmcsSupplierAssignSampler.Remark = txtRemark_Goods.Text;

                Dbers.GetInstance().SelfDber.Update(cmcsSupplierAssignSampler, GlobalVars.LoginUser.UserName);
            }
            else if (this.EditMode == eEditMode.新增)
            {
                cmcsSupplierAssignSampler = new CmcsSupplierAssignSampler();
                cmcsSupplierAssignSampler.SupplierId = this.SelectedSupplier.Id;
                cmcsSupplierAssignSampler.SupplierName = this.SelectedSupplier.Name;
                cmcsSupplierAssignSampler.Sampler = cmbSampler.SelectedItem.ToString();
                cmcsSupplierAssignSampler.Remark = txtRemark_Goods.Text;
                Dbers.GetInstance().SelfDber.Insert(cmcsSupplierAssignSampler);
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private CmcsSupplier selectedSupplier;
        /// <summary>
        /// 选择的供货单位
        /// </summary>
        public CmcsSupplier SelectedSupplier
        {
            get { return selectedSupplier; }
            set
            {
                selectedSupplier = value;

                if (value != null)
                {
                    txtSupplierName.Text = value.Name;
                }
                else
                {
                    txtSupplierName.ResetText();
                }
            }
        }

        /// <summary>
        /// 选择供货单位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectSupply_Goods_Click(object sender, EventArgs e)
        {
            FrmSupplier_Select frm = new FrmSupplier_Select("where IsStop=0 order by Name asc");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                this.SelectedSupplier = frm.Output;
            }
        }
    }
}
