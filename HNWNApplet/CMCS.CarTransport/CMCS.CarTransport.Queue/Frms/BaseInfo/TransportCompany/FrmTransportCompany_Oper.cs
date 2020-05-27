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
using CMCS.Common.Entities;
using CMCS.Common.Entities.BaseInfo;
using CMCS.Common.Entities.CarTransport;
using DevComponents.DotNetBar;

namespace CMCS.CarTransport.Queue.Frms.BaseInfo.TransportCompany
{
    public partial class FrmTransportCompany_Oper : DevComponents.DotNetBar.Metro.MetroForm
    {
        #region Var

        //ҵ��id
        string PId = String.Empty;

        //�༭ģʽ
        eEditMode EditMode = eEditMode.Ĭ��;

        CmcsTransportCompany CmcsTransportCompany;

        #endregion

        public FrmTransportCompany_Oper(string pId, eEditMode editMode)
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
        private void FrmTransportCompany_Oper_Load(object sender, EventArgs e)
        {
            this.CmcsTransportCompany = Dbers.GetInstance().SelfDber.Get<CmcsTransportCompany>(this.PId);
            if (this.CmcsTransportCompany != null)
            {
                txt_Code.Text = CmcsTransportCompany.Code;
                txt_Name.Text = CmcsTransportCompany.Name;
                chb_IsUse.Checked = (CmcsTransportCompany.IsStop == 0);
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
            if (txt_Name.Text.Length == 0)
            {
                MessageBoxEx.Show("�õ�λ���Ʋ����ظ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (txt_Code.Text.Length == 0)
            {
                MessageBoxEx.Show("�õ�λ��Ų���Ϊ�գ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if ((CmcsTransportCompany == null || CmcsTransportCompany.Name != txt_Name.Text) && Dbers.GetInstance().SelfDber.Entities<CmcsTransportCompany>(" where Name=:Name", new { Name = txt_Name.Text }).Count > 0)
            {
                MessageBoxEx.Show("�õ�λ���Ʋ����ظ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if ((CmcsTransportCompany == null || CmcsTransportCompany.Code != txt_Code.Text) && Dbers.GetInstance().SelfDber.Entities<CmcsTransportCompany>(" where Code=:Code", new { Code = txt_Code.Text }).Count > 0)
            {
                MessageBoxEx.Show("�õ�λ��Ų���Ϊ�գ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (this.EditMode == eEditMode.�޸�)
            {
                CmcsTransportCompany.Name = txt_Name.Text;
                CmcsTransportCompany.Code = txt_Code.Text;
                CmcsTransportCompany.IsStop = (chb_IsUse.Checked ? 0 : 1);
                Dbers.GetInstance().SelfDber.Update(CmcsTransportCompany);
            }
            else if (this.EditMode == eEditMode.����)
            {
                CmcsTransportCompany = new CmcsTransportCompany();
                CmcsTransportCompany.Name = txt_Name.Text;
                CmcsTransportCompany.Code = txt_Code.Text;
                CmcsTransportCompany.IsStop = (chb_IsUse.Checked ? 0 : 1);
                Dbers.GetInstance().SelfDber.Insert(CmcsTransportCompany);
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