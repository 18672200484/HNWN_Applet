using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CMCS.CarTransport.Queue.Core;
using CMCS.CarTransport.Queue.Enums;
using CMCS.Common;
using CMCS.Common.Entities.CarTransport;
using DevComponents.DotNetBar;

namespace CMCS.CarTransport.Queue.Frms.BaseInfo.SupplyReceive
{
    public partial class FrmSupplyReceive_Oper : DevComponents.DotNetBar.Metro.MetroForm
    {
        #region Var

        //ҵ��id
        string PId = String.Empty;

        //�༭ģʽ
        eEditMode EditMode = eEditMode.Ĭ��;

        CmcsSupplyReceive CmcsSupplyReceive;

        #endregion

        public FrmSupplyReceive_Oper(string pId, eEditMode editMode)
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
        private void FrmSupplyReceive_Oper_Load(object sender, EventArgs e)
        {
            this.CmcsSupplyReceive = Dbers.GetInstance().SelfDber.Get<CmcsSupplyReceive>(this.PId);
            if (this.CmcsSupplyReceive != null)
            {
                txt_UnitName.Text = CmcsSupplyReceive.UnitName;
                chb_IsUse.Checked = (CmcsSupplyReceive.IsValid == 1);
                txt_ReMark.Text = CmcsSupplyReceive.Remark;
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
            if (txt_UnitName.Text.Length == 0)
            {
                MessageBoxEx.Show("�ñ굥λ���Ʋ���Ϊ�գ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if ((CmcsSupplyReceive == null || CmcsSupplyReceive.UnitName != txt_UnitName.Text) && Dbers.GetInstance().SelfDber.Entities<CmcsSupplyReceive>(" where CardNumber=:CardNumber", new { CardNumber = txt_UnitName.Text }).Count > 0)
            {
                MessageBoxEx.Show("�ñ굥λ���Ʋ����ظ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (this.EditMode == eEditMode.�޸�)
            {
                CmcsSupplyReceive.UnitName = txt_UnitName.Text;
                CmcsSupplyReceive.IsValid = (chb_IsUse.Checked ? 1 : 0);
                CmcsSupplyReceive.Remark = txt_ReMark.Text;
                Dbers.GetInstance().SelfDber.Update(CmcsSupplyReceive);
            }
            else if (this.EditMode == eEditMode.����)
            {
                CmcsSupplyReceive = new CmcsSupplyReceive();
                CmcsSupplyReceive.UnitName = txt_UnitName.Text;
                CmcsSupplyReceive.IsValid = (chb_IsUse.Checked ? 1 : 0);
                CmcsSupplyReceive.Remark = txt_ReMark.Text;
                Dbers.GetInstance().SelfDber.Insert(CmcsSupplyReceive);
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
    }
}