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

namespace CMCS.CarTransport.Queue.Frms.BaseInfo.GoodsPlan
{
    public partial class FrmGoodsPlan_Oper : MetroForm
    {
        //业务id
        string PId = String.Empty;

        //编辑模式
        eEditMode EditMode = eEditMode.默认;

        CmcsGoodsPlan cmcsGoodsPlan;

        public FrmGoodsPlan_Oper(string pId, eEditMode editMode)
        {
            InitializeComponent();

            this.PId = pId;
            this.EditMode = editMode;
        }

        private void FrmGoodsPlan_Oper_Load(object sender, EventArgs e)
        {
            this.cmcsGoodsPlan = Dbers.GetInstance().SelfDber.Get<CmcsGoodsPlan>(this.PId);
            if (this.cmcsGoodsPlan != null)
            {
                txtCarNumber_Goods.Text = cmcsGoodsPlan.CarNumber;
                txtSupplyUnitName_Goods.Text = cmcsGoodsPlan.SupplyUnitName;
                txtReceiveUnitName_Goods.Text = cmcsGoodsPlan.ReceiveUnitName;
                txtGoodsTypeName_Goods.Text = cmcsGoodsPlan.GoodsTypeName;
                txtRemark_Goods.Text = cmcsGoodsPlan.Remark;
            }

            if (this.EditMode == eEditMode.查看)
            {
                btnSubmit.Enabled = false;
                HelperUtil.ControlReadOnly(panelEx2, true);
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCarNumber_Goods.Text))
            {
                MessageBoxEx.Show("车牌号不能为空！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtSupplyUnitName_Goods.Text))
            {
                MessageBoxEx.Show("供货单位不能为空！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtReceiveUnitName_Goods.Text))
            {
                MessageBoxEx.Show("收货单位不能为空！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtGoodsTypeName_Goods.Text))
            {
                MessageBoxEx.Show("物资类型不能为空！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if ((cmcsGoodsPlan == null || cmcsGoodsPlan.CarNumber != txtCarNumber_Goods.Text) && Dbers.GetInstance().SelfDber.Entities<CmcsGoodsPlan>(" where CarNumber=:CarNumber", new { CarNumber = txtCarNumber_Goods.Text }).Count > 0)
            {
                MessageBoxEx.Show("该车号物资计划已存在！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (this.EditMode == eEditMode.修改)
            {
                if (this.SelectedAutotruck != null)
                {
                    cmcsGoodsPlan.AutotruckId = this.SelectedAutotruck.Id;
                    cmcsGoodsPlan.CarNumber = this.SelectedAutotruck.CarNumber;
                }
                if (this.SelectedSupplyUnit_Goods != null)
                {
                    cmcsGoodsPlan.SupplyUnitId = this.SelectedSupplyUnit_Goods.Id;
                    cmcsGoodsPlan.SupplyUnitName = this.SelectedSupplyUnit_Goods.Name;
                }
                if (this.SelectedReceiveUnit_Goods != null)
                {
                    cmcsGoodsPlan.ReceiveUnitId = this.SelectedReceiveUnit_Goods.Id;
                    cmcsGoodsPlan.ReceiveUnitName = this.SelectedReceiveUnit_Goods.Name;
                }
                if (this.SelectedGoodsType_Goods != null)
                {
                    cmcsGoodsPlan.GoodsTypeId = this.SelectedGoodsType_Goods.Id;
                    cmcsGoodsPlan.GoodsTypeName = SelectedGoodsType_Goods.GoodsName;
                }
                cmcsGoodsPlan.Remark = txtRemark_Goods.Text;

                Dbers.GetInstance().SelfDber.Update(cmcsGoodsPlan, GlobalVars.LoginUser.UserName);
            }
            else if (this.EditMode == eEditMode.新增)
            {
                cmcsGoodsPlan = new CmcsGoodsPlan();
                cmcsGoodsPlan.AutotruckId = this.SelectedAutotruck.Id;
                cmcsGoodsPlan.CarNumber = this.SelectedAutotruck.CarNumber;
                cmcsGoodsPlan.SupplyUnitId = this.SelectedSupplyUnit_Goods.Id;
                cmcsGoodsPlan.SupplyUnitName = this.SelectedSupplyUnit_Goods.Name;
                cmcsGoodsPlan.ReceiveUnitId = this.SelectedReceiveUnit_Goods.Id;
                cmcsGoodsPlan.ReceiveUnitName = this.SelectedReceiveUnit_Goods.Name;
                cmcsGoodsPlan.GoodsTypeId = this.SelectedGoodsType_Goods.Id;
                cmcsGoodsPlan.GoodsTypeName = SelectedGoodsType_Goods.GoodsName;
                cmcsGoodsPlan.Remark = txtRemark_Goods.Text;
                Dbers.GetInstance().SelfDber.Insert(cmcsGoodsPlan);
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        CmcsAutotruck selectedAutotruck;
        /// <summary>
        /// 当前车
        /// </summary>
        public CmcsAutotruck SelectedAutotruck
        {
            get { return selectedAutotruck; }
            set
            {
                selectedAutotruck = value;

                if (value != null)
                {
                    txtCarNumber_Goods.Text = value.CarNumber;
                }
                else
                {
                    txtCarNumber_Goods.ResetText();
                }
            }
        }

        private CmcsSupplier selectedSupplyUnit_Goods;
        /// <summary>
        /// 选择的供货单位
        /// </summary>
        public CmcsSupplier SelectedSupplyUnit_Goods
        {
            get { return selectedSupplyUnit_Goods; }
            set
            {
                selectedSupplyUnit_Goods = value;

                if (value != null)
                {
                    txtSupplyUnitName_Goods.Text = value.Name;
                }
                else
                {
                    txtSupplyUnitName_Goods.ResetText();
                }
            }
        }

        private CmcsSupplier selectedReceiveUnit_Goods;
        /// <summary>
        /// 选择的收货单位
        /// </summary>
        public CmcsSupplier SelectedReceiveUnit_Goods
        {
            get { return selectedReceiveUnit_Goods; }
            set
            {
                selectedReceiveUnit_Goods = value;

                if (value != null)
                {
                    txtReceiveUnitName_Goods.Text = value.Name;
                }
                else
                {
                    txtReceiveUnitName_Goods.ResetText();
                }
            }
        }

        private CmcsGoodsType selectedGoodsType_Goods;
        /// <summary>
        /// 选择的物资类型
        /// </summary>
        public CmcsGoodsType SelectedGoodsType_Goods
        {
            get { return selectedGoodsType_Goods; }
            set
            {
                selectedGoodsType_Goods = value;

                if (value != null)
                {
                    txtGoodsTypeName_Goods.Text = value.GoodsName;
                }
                else
                {
                    txtGoodsTypeName_Goods.ResetText();
                }
            }
        }

        /// <summary>
        /// 选择车辆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectAutotruck_Goods_Click(object sender, EventArgs e)
        {
            FrmAutotruck_Select frm = new FrmAutotruck_Select("and CarType='" + eCarType.其他物资.ToString() + "' and IsUse=1 order by CarNumber asc");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                this.SelectedAutotruck = frm.Output;
            }
        }

        /// <summary>
        /// 选择供货单位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnbtnSelectSupply_Goods_Click(object sender, EventArgs e)
        {
            FrmSupplier_Select frm = new FrmSupplier_Select("where IsStop=0 order by Name asc");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                this.SelectedSupplyUnit_Goods = frm.Output;
            }
        }

        /// <summary>
        /// 选择收货单位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectReceive_Goods_Click(object sender, EventArgs e)
        {
            FrmSupplier_Select frm = new FrmSupplier_Select("where IsStop=0 order by Name asc");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                this.SelectedReceiveUnit_Goods = frm.Output;
            }
        }

        /// <summary>
        /// 选择物资类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectGoodsType_Goods_Click(object sender, EventArgs e)
        {
            FrmGoodsType_Select frm = new FrmGoodsType_Select();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                this.SelectedGoodsType_Goods = frm.Output;
            }
        }
    }
}
