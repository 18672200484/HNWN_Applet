using System;
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
using CMCS.CarTransport.Queue.Frms.BaseInfo.Mine;
using CMCS.CarTransport.Queue.Frms.BaseInfo.Supplier;
using CMCS.CarTransport.Queue.Frms.BaseInfo.TransportCompany;
using CMCS.Common;
using CMCS.Common.DAO;
using CMCS.Common.Entities;
using CMCS.Common.Entities.BaseInfo;
using CMCS.Common.Entities.CarTransport;
using CMCS.Common.Entities.Fuel;
using CMCS.Common.Enums;
using DevComponents.DotNetBar;

namespace CMCS.CarTransport.Queue.Frms.Transport.BuyFuelTransport
{
    public partial class FrmBuyFuelTransport_Oper : DevComponents.DotNetBar.Metro.MetroForm
    {
        #region Var

        CarTransportDAO carTransportDAO = CarTransportDAO.GetInstance();

        //ҵ��id
        string PId = String.Empty;

        //�༭ģʽ
        eEditMode EditMode = eEditMode.Ĭ��;

        CommonDAO commonDAO = CommonDAO.GetInstance();

        //��ǰ�����¼
        CmcsBuyFuelTransport CmcsBuyFuelTransport;

        CmcsTransportCompany cmcsTransportCompany;
        /// <summary>
        /// ���䵥λ
        /// </summary>
        public CmcsTransportCompany CmcsTransportCompany
        {
            get { return cmcsTransportCompany; }
            set
            {
                cmcsTransportCompany = value;

                if (value != null)
                {
                    txt_TransportCompanyName.Text = value.Name;
                }
                else
                {
                    txt_TransportCompanyName.ResetText();
                }
            }
        }

        CmcsMine cmcsMine;
        /// <summary>
        /// ���
        /// </summary>
        public CmcsMine CmcsMine
        {
            get { return cmcsMine; }
            set
            {
                cmcsMine = value;

                if (value != null)
                {
                    txt_MineName.Text = value.Name;
                }
                else
                {
                    txt_MineName.ResetText();
                }
            }
        }

        CmcsSupplier cmcsSupplier;
        /// <summary>
        /// ��Ӧ��
        /// </summary>
        public CmcsSupplier CmcsSupplier
        {
            get { return cmcsSupplier; }
            set
            {
                cmcsSupplier = value;

                if (value != null)
                {
                    txt_SupplierName.Text = value.Name;
                }
                else
                {
                    txt_SupplierName.ResetText();
                }
            }
        }

        CmcsFuelKind cmcsFuelKind;
        /// <summary>
        /// ú��
        /// </summary>
        public CmcsFuelKind CmcsFuelKind
        {
            get { return cmcsFuelKind; }
            set { cmcsFuelKind = value; }
        }

        List<CmcsBuyFuelTransportDeduct> cmcsbuyfueltransportdeducts;

        #endregion

        public FrmBuyFuelTransport_Oper(string pId, eEditMode editMode)
        {
            InitializeComponent();

            this.PId = pId;
            this.EditMode = editMode;
        }

        /// <summary>
        /// ������ذ�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmBuyFuelTransport_Oper_Load(object sender, EventArgs e)
        {
            //��ú����Ϣ
            cmbFuelName_BuyFuel.DisplayMember = "Name";
            cmbFuelName_BuyFuel.ValueMember = "Id";
            cmbFuelName_BuyFuel.DataSource = Dbers.GetInstance().SelfDber.Entities<CmcsFuelKind>("where IsStop=0 and ParentId is not null");
            cmbFuelName_BuyFuel.SelectedIndex = 0;

            BindStepName();

            this.CmcsBuyFuelTransport = Dbers.GetInstance().SelfDber.Get<CmcsBuyFuelTransport>(this.PId);
            if (this.CmcsBuyFuelTransport != null)
            {
                txt_SerialNumber.Text = CmcsBuyFuelTransport.SerialNumber;
                txt_CarNumber.Text = CmcsBuyFuelTransport.CarNumber;
                CmcsInFactoryBatch cmcsinfactorybatch = Dbers.GetInstance().SelfDber.Get<CmcsInFactoryBatch>(CmcsBuyFuelTransport.InFactoryBatchId);
                if (cmcsinfactorybatch != null)
                {
                    txt_InFactoryBatchNumber.Text = cmcsinfactorybatch.Batch;
                }
                txt_SupplierName.Text = CmcsBuyFuelTransport.SupplierName;
                txt_TransportCompanyName.Text = CmcsBuyFuelTransport.TransportCompanyName;
                txt_MineName.Text = CmcsBuyFuelTransport.MineName;
                cmbFuelName_BuyFuel.Text = CmcsBuyFuelTransport.FuelKindName;
                dbi_TicketWeight.Value = (double)CmcsBuyFuelTransport.TicketWeight;
                dbi_GrossWeight.Value = (double)CmcsBuyFuelTransport.GrossWeight;
                dbi_TareWeight.Value = (double)CmcsBuyFuelTransport.TareWeight;
                dbi_DeductWeight.Value = (double)CmcsBuyFuelTransport.DeductWeight;
                dbi_SuttleWeight.Value = (double)CmcsBuyFuelTransport.SuttleWeight;
                txt_UnloadArea.Text = CmcsBuyFuelTransport.UnLoadArea;
                //txt_InFactoryTime.Text = CmcsBuyFuelTransport.InFactoryTime.Year == 1 ? "" : CmcsBuyFuelTransport.InFactoryTime.ToString();
                //txt_SamplingTime.Text = CmcsBuyFuelTransport.SamplingTime.Year == 1 ? "" : CmcsBuyFuelTransport.SamplingTime.ToString();
                //txt_GrossTime.Text = CmcsBuyFuelTransport.GrossTime.Year == 1 ? "" : CmcsBuyFuelTransport.GrossTime.ToString();
                //txt_UploadTime.Text = CmcsBuyFuelTransport.UploadTime.Year == 1 ? "" : CmcsBuyFuelTransport.UploadTime.ToString();
                //txt_TareTime.Text = CmcsBuyFuelTransport.TareTime.Year == 1 ? "" : CmcsBuyFuelTransport.TareTime.ToString();
                //txt_OutFactoryTime.Text = CmcsBuyFuelTransport.OutFactoryTime.Year == 1 ? "" : CmcsBuyFuelTransport.OutFactoryTime.ToString();
                txt_InFactoryTime.Value = CmcsBuyFuelTransport.InFactoryTime;
                txt_SamplingTime.Value = CmcsBuyFuelTransport.SamplingTime;
                txt_GrossTime.Value = CmcsBuyFuelTransport.GrossTime;
                txt_UploadTime.Value = CmcsBuyFuelTransport.UploadTime;
                txt_TareTime.Value = CmcsBuyFuelTransport.TareTime;
                txt_OutFactoryTime.Value = CmcsBuyFuelTransport.OutFactoryTime;

                txt_Remark.Text = CmcsBuyFuelTransport.Remark;
                chb_IsFinish.Checked = (CmcsBuyFuelTransport.IsFinish == 1);
                chb_IsUse.Checked = (CmcsBuyFuelTransport.IsUse == 1);
                cmbStepName.Text = CmcsBuyFuelTransport.StepName;
                ShowDeduct(this.CmcsBuyFuelTransport.Id);
            }

            if (this.EditMode == eEditMode.�鿴)
            {
                btnSubmit.Enabled = false;
                HelperUtil.ControlReadOnly(panelEx2, true);
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (this.EditMode == eEditMode.�޸�)
            {
                CmcsBuyFuelTransport.SerialNumber = txt_SerialNumber.Text;
                CmcsBuyFuelTransport.CarNumber = txt_CarNumber.Text;
                if (cmcsSupplier != null)
                {
                    CmcsBuyFuelTransport.SupplierId = cmcsSupplier.Id;
                    CmcsBuyFuelTransport.SupplierName = cmcsSupplier.Name;
                }
                if (cmcsTransportCompany != null)
                {
                    CmcsBuyFuelTransport.TransportCompanyId = cmcsTransportCompany.Id;
                    CmcsBuyFuelTransport.TransportCompanyName = cmcsTransportCompany.Name;
                }
                if (cmcsMine != null)
                {
                    CmcsBuyFuelTransport.MineId = cmcsMine.Id;
                    CmcsBuyFuelTransport.MineName = cmcsMine.Name;
                }
                if (cmcsFuelKind != null)
                {
                    CmcsBuyFuelTransport.FuelKindId = cmcsFuelKind.Id;
                    CmcsBuyFuelTransport.FuelKindName = cmcsFuelKind.Name;
                }
                CmcsBuyFuelTransport.TicketWeight = (decimal)dbi_TicketWeight.Value;
                CmcsBuyFuelTransport.GrossWeight = (decimal)dbi_GrossWeight.Value;
                CmcsBuyFuelTransport.DeductWeight = (decimal)dbi_DeductWeight.Value;
                CmcsBuyFuelTransport.TareWeight = (decimal)dbi_TareWeight.Value;
                CmcsBuyFuelTransport.SuttleWeight = (decimal)dbi_SuttleWeight.Value;

                CmcsBuyFuelTransport.InFactoryTime = txt_InFactoryTime.Value;
                CmcsBuyFuelTransport.SamplingTime = txt_SamplingTime.Value;
                CmcsBuyFuelTransport.GrossTime = txt_GrossTime.Value;
                CmcsBuyFuelTransport.UploadTime = txt_UploadTime.Value;
                CmcsBuyFuelTransport.TareTime = txt_TareTime.Value;
                CmcsBuyFuelTransport.OutFactoryTime = txt_OutFactoryTime.Value;

                CmcsBuyFuelTransport.Remark = txt_Remark.Text;
                CmcsBuyFuelTransport.IsFinish = (chb_IsFinish.Checked ? 1 : 0);
                CmcsBuyFuelTransport.IsUse = (chb_IsUse.Checked ? 1 : 0);
                CmcsBuyFuelTransport.StepName = cmbStepName.Text;

                CmcsUnFinishTransport unfinishTransport = Dbers.GetInstance().SelfDber.Entity<CmcsUnFinishTransport>(" where TransportId= '" + CmcsBuyFuelTransport.Id + "'");

                //��Ч����δ���ʱ��Ҫ����[δ��������¼]
                if (chb_IsUse.Checked && !chb_IsFinish.Checked)
                {
                    if (unfinishTransport == null)
                    {
                        unfinishTransport = new CmcsUnFinishTransport()
                        {
                            TransportId = CmcsBuyFuelTransport.Id,
                            CarType = eCarType.�볧ú.ToString(),
                            AutotruckId = CmcsBuyFuelTransport.AutotruckId,
                            PrevPlace = CommonAppConfig.GetInstance().AppIdentifier
                        };
                        Dbers.GetInstance().SelfDber.Insert(unfinishTransport);
                    }
                }
                //��Ч���������ʱ��Ҫɾ��[δ��������¼]
                if (!chb_IsUse.Checked || chb_IsFinish.Checked)
                {
                    if (unfinishTransport != null)
                        Dbers.GetInstance().SelfDber.Delete<CmcsUnFinishTransport>(unfinishTransport.Id);
                }

                // ���������Լ����ƻ������������� 
                //CmcsInFactoryBatch inFactoryBatch = carTransportDAO.GCQCInFactoryBatchByBuyFuelTransport(CmcsBuyFuelTransport);
                //if (inFactoryBatch != null)
                //{
                //commonDAO.SaveModifyLog<CmcsBuyFuelTransport>(CmcsBuyFuelTransport, CmcsBuyFuelTransport.Id, "�볧ú�����¼", GlobalVars.LoginUser != null ? GlobalVars.LoginUser.UserName : "admin");

                Dbers.GetInstance().SelfDber.Update(CmcsBuyFuelTransport, GlobalVars.LoginUser != null ? GlobalVars.LoginUser.UserName : "admin");
                //}
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// ȡ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSupplier_Click(object sender, EventArgs e)
        {
            FrmSupplier_Select Frm = new FrmSupplier_Select(string.Empty);
            Frm.ShowDialog();
            if (Frm.DialogResult == DialogResult.OK)
            {
                this.CmcsSupplier = Frm.Output;
            }
        }

        private void btnTransportCompany_Click(object sender, EventArgs e)
        {
            FrmTransportCompany_Select Frm = new FrmTransportCompany_Select(string.Empty);
            Frm.ShowDialog();
            if (Frm.DialogResult == DialogResult.OK)
            {
                this.CmcsTransportCompany = Frm.Output;
            }
        }

        private void BtnMine_Click(object sender, EventArgs e)
        {
            FrmMine_Select Frm = new FrmMine_Select(string.Empty);
            Frm.ShowDialog();
            if (Frm.DialogResult == DialogResult.OK)
            {
                this.CmcsMine = Frm.Output;
            }
        }

        private void cmbFuelName_BuyFuel_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cmcsFuelKind = cmbFuelName_BuyFuel.SelectedItem as CmcsFuelKind;
        }

        /// <summary>
        /// �����̽ڵ�
        /// </summary>
        private void BindStepName()
        {
            cmbStepName.Items.Add(eTruckInFactoryStep.�볧.ToString());
            cmbStepName.Items.Add(eTruckInFactoryStep.����.ToString());
            cmbStepName.Items.Add(eTruckInFactoryStep.�س�.ToString());
            cmbStepName.Items.Add(eTruckInFactoryStep.�ᳵ.ToString());
            cmbStepName.Items.Add(eTruckInFactoryStep.����.ToString());
            cmbStepName.SelectedIndex = 0;
        }

        #region �۶�

        public void ShowDeduct(string newId)
        {
            cmcsbuyfueltransportdeducts = Dbers.GetInstance().SelfDber.Entities<CmcsBuyFuelTransportDeduct>(" where TransportId=:TransportId", new { TransportId = newId });
            superGridControl1.PrimaryGrid.DataSource = cmcsbuyfueltransportdeducts;

            dbi_DeductWeight.Value = (double)cmcsbuyfueltransportdeducts.Select(a => a.DeductWeight).Sum();
        }

        private void btnAddDeduct_Click(object sender, EventArgs e)
        {
            FrmBuyFuelTransportDeduct_Oper frmEdit = new FrmBuyFuelTransportDeduct_Oper(this.CmcsBuyFuelTransport.Id, eEditMode.����);
            if (frmEdit.ShowDialog() == DialogResult.OK)
            {
                ShowDeduct(this.CmcsBuyFuelTransport.Id);
            }
        }

        private void superGridControl1_CellMouseDown(object sender, DevComponents.DotNetBar.SuperGrid.GridCellMouseEventArgs e)
        {
            CmcsBuyFuelTransportDeduct entity = cmcsbuyfueltransportdeducts.Where(a => a.Id == superGridControl1.PrimaryGrid.GetCell(e.GridCell.GridRow.Index, superGridControl1.PrimaryGrid.Columns["clmId"].ColumnIndex).Value.ToString()).FirstOrDefault();

            switch (superGridControl1.PrimaryGrid.Columns[e.GridCell.ColumnIndex].Name)
            {
                case "clmShow":
                    FrmBuyFuelTransportDeduct_Oper frmShow = new FrmBuyFuelTransportDeduct_Oper(entity.Id, eEditMode.�鿴);
                    if (frmShow.ShowDialog() == DialogResult.OK)
                    {
                        ShowDeduct(this.CmcsBuyFuelTransport.Id);
                    }
                    break;
                case "clmEdit":
                    FrmBuyFuelTransportDeduct_Oper frmEdit = new FrmBuyFuelTransportDeduct_Oper(entity.Id, eEditMode.�޸�);
                    if (frmEdit.ShowDialog() == DialogResult.OK)
                    {
                        ShowDeduct(this.CmcsBuyFuelTransport.Id);
                    }
                    break;
                case "clmDelete":

                    if (MessageBoxEx.Show("ȷ��ɾ���۶ּ�¼��", "������ʾ", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == System.Windows.Forms.DialogResult.OK)
                    {
                        Dbers.GetInstance().SelfDber.Delete<CmcsBuyFuelTransportDeduct>(entity.Id);

                        ShowDeduct(this.CmcsBuyFuelTransport.Id);
                    }
                    break;
            }

        }

        private void superGridControl1_BeginEdit(object sender, DevComponents.DotNetBar.SuperGrid.GridEditEventArgs e)
        {
            // ȡ���༭
            e.Cancel = true;
        }

        #endregion
    }
}