using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CMCS.CarTransport.DAO;
using CMCS.CarTransport.Queue.Core;
using CMCS.CarTransport.Queue.Enums;
using CMCS.CarTransport.Queue.Frms.BaseInfo.CarModel;
using CMCS.CarTransport.Queue.Frms.BaseInfo.EPCCard;
using CMCS.CarTransport.Queue.Utilities;
using CMCS.Common;
using CMCS.Common.Entities.CarTransport;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using DevComponents.Editors;

namespace CMCS.CarTransport.Queue.Frms.BaseInfo.Autotruck
{
    public partial class FrmAutotruck_Oper : DevComponents.DotNetBar.Metro.MetroForm
    {
        #region Var

        //ҵ��id
        string PId = String.Empty;

        //�༭ģʽ
        eEditMode EditMode = eEditMode.Ĭ��;

        CmcsAutotruck CmcsAutotruck;

        CmcsEPCCard cmcsEPCCard;
        /// <summary>
        /// ��ǰEPC��ǩ��
        /// </summary>
        public CmcsEPCCard CmcsEPCCard
        {
            get { return cmcsEPCCard; }
            set
            {
                cmcsEPCCard = value;

                if (value != null)
                    txt_EPCCardNumber.Text = cmcsEPCCard.CardNumber;
            }
        }

        #endregion

        public FrmAutotruck_Oper(string pId, eEditMode editMode)
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
        private void FrmAutotruck_Oper_Load(object sender, EventArgs e)
        {
            label_warn.ForeColor = Color.Red;

            cmb_CarType.Items.Add("�볧ú");
            cmb_CarType.Items.Add("��������");
            cmb_CarType.SelectedIndex = 0;

            this.CmcsAutotruck = Dbers.GetInstance().SelfDber.Get<CmcsAutotruck>(this.PId);
            if (this.CmcsAutotruck != null)
            {
                txt_CarNumber.Text = CmcsAutotruck.CarNumber;
                cmb_CarType.SelectedItem = CmcsAutotruck.CarType;
                txt_Driver.Text = CmcsAutotruck.Driver;
                txt_CellPhoneNumber.Text = CmcsAutotruck.CellPhoneNumber;
                chb_IsUse.Checked = (CmcsAutotruck.IsUse == 1);
                dbi_LeftObstacle1.Value = CmcsAutotruck.LeftObstacle1;
                dbi_LeftObstacle2.Value = CmcsAutotruck.LeftObstacle2;
                dbi_LeftObstacle3.Value = CmcsAutotruck.LeftObstacle3;
                dbi_LeftObstacle4.Value = CmcsAutotruck.LeftObstacle4;
                dbi_LeftObstacle5.Value = CmcsAutotruck.LeftObstacle5;
                dbi_LeftObstacle6.Value = CmcsAutotruck.LeftObstacle6;
                dbi_CarriageLength.Value = CmcsAutotruck.CarriageLength;
                dbi_CarriageWidth.Value = CmcsAutotruck.CarriageWidth;
                dbi_CarriageBottomToFloor.Value = CmcsAutotruck.CarriageBottomToFloor;
                txt_ReMark.Text = CmcsAutotruck.ReMark;
                //this.CmcsEPCCard = Dbers.GetInstance().SelfDber.Get<CmcsEPCCard>(CmcsAutotruck.EPCCardId);
                //txt_EPCCardNumber.Text = cmcsEPCCard.CardNumber;
                txt_EPCCardNumber.Text = CmcsAutotruck.EPCCardId;
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
            txt_CarNumber.Text = txt_CarNumber.Text.ToUpper();
            if (string.IsNullOrWhiteSpace(txt_CarNumber.Text))
            {
                MessageBoxEx.Show("�ó��ƺŲ���Ϊ�գ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if ((CmcsAutotruck == null || CmcsAutotruck.CarNumber != txt_CarNumber.Text) && Dbers.GetInstance().SelfDber.Entities<CmcsAutotruck>(" where CarNumber=:CarNumber", new { CarNumber = txt_CarNumber.Text }).Count > 0)
            {
                MessageBoxEx.Show("�ó��ƺŲ����ظ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txt_EPCCardNumber.Text))
            {
                MessageBoxEx.Show("��ǩ�Ų���Ϊ�գ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Dbers.GetInstance().SelfDber.Entities<CmcsAutotruck>(" where EPCCardId=:EPCCardId and Id!=:Id", new { EPCCardId = txt_EPCCardNumber.Text, Id = PId }).Count > 0)
            {
                MessageBoxEx.Show("��ǩ�Ų�����ͬ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cmb_CarType.SelectedItem.ToString() == "�볧ú")
            {
                if (dbi_CarriageLength.Value <= 0)
                {
                    MessageBoxEx.Show("�ó�����Ϊ0��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (dbi_CarriageWidth.Value <= 0)
                {
                    MessageBoxEx.Show("�ÿ���Ϊ0��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (dbi_CarriageBottomToFloor.Value <= 0)
                {
                    MessageBoxEx.Show("�ó���ײ�������߲���Ϊ0��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (dbi_LeftObstacle6.Value > 0 && dbi_LeftObstacle5.Value <= 0)
                {
                    MessageBoxEx.Show("������������Ϣ��������������Ϣ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (dbi_LeftObstacle5.Value > 0 && dbi_LeftObstacle4.Value <= 0)
                {
                    MessageBoxEx.Show("������������Ϣ��������������Ϣ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (dbi_LeftObstacle4.Value > 0 && dbi_LeftObstacle3.Value <= 0)
                {
                    MessageBoxEx.Show("������������Ϣ��������������Ϣ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (dbi_LeftObstacle3.Value > 0 && dbi_LeftObstacle2.Value <= 0)
                {
                    MessageBoxEx.Show("������������Ϣ�������������Ϣ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (dbi_LeftObstacle2.Value > 0 && dbi_LeftObstacle1.Value <= 0)
                {
                    MessageBoxEx.Show("�����������Ϣ����������һ��Ϣ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (dbi_CarriageLength.Value < 2000 || dbi_CarriageLength.Value > 20000 ||
                    dbi_CarriageWidth.Value < 1000 || dbi_CarriageWidth.Value > 5000 ||
                    dbi_CarriageBottomToFloor.Value < 300 || dbi_CarriageBottomToFloor.Value > 3000)
                {
                    MessageBoxEx.Show("�����������쳣�����飡", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            if (this.EditMode == eEditMode.�޸�)
            {
                CmcsAutotruck.CarNumber = txt_CarNumber.Text;
                CmcsAutotruck.CarType = cmb_CarType.SelectedItem.ToString();
                CmcsAutotruck.Driver = txt_Driver.Text;
                CmcsAutotruck.CellPhoneNumber = txt_CellPhoneNumber.Text;
                CmcsAutotruck.IsUse = (chb_IsUse.Checked ? 1 : 0);
                CmcsAutotruck.LeftObstacle1 = (int)dbi_LeftObstacle1.Value;
                CmcsAutotruck.LeftObstacle2 = (int)dbi_LeftObstacle2.Value;
                CmcsAutotruck.LeftObstacle3 = (int)dbi_LeftObstacle3.Value;
                CmcsAutotruck.LeftObstacle4 = (int)dbi_LeftObstacle4.Value;
                CmcsAutotruck.LeftObstacle5 = (int)dbi_LeftObstacle5.Value;
                CmcsAutotruck.LeftObstacle6 = (int)dbi_LeftObstacle6.Value;
                CmcsAutotruck.CarriageLength = (int)dbi_CarriageLength.Value;
                CmcsAutotruck.CarriageWidth = (int)dbi_CarriageWidth.Value;
                CmcsAutotruck.CarriageBottomToFloor = (int)dbi_CarriageBottomToFloor.Value;
                CmcsAutotruck.ReMark = txt_ReMark.Text;
                CmcsAutotruck.EPCCardId = txt_EPCCardNumber.Text;
                Dbers.GetInstance().SelfDber.Update(CmcsAutotruck);
            }
            else if (this.EditMode == eEditMode.����)
            {
                CmcsAutotruck = new CmcsAutotruck();
                CmcsAutotruck.CarNumber = txt_CarNumber.Text;
                CmcsAutotruck.CarType = cmb_CarType.SelectedItem.ToString();
                CmcsAutotruck.Driver = txt_Driver.Text;
                CmcsAutotruck.CellPhoneNumber = txt_CellPhoneNumber.Text;
                CmcsAutotruck.IsUse = (chb_IsUse.Checked ? 1 : 0);
                CmcsAutotruck.LeftObstacle1 = (int)dbi_LeftObstacle1.Value;
                CmcsAutotruck.LeftObstacle2 = (int)dbi_LeftObstacle2.Value;
                CmcsAutotruck.LeftObstacle3 = (int)dbi_LeftObstacle3.Value;
                CmcsAutotruck.LeftObstacle4 = (int)dbi_LeftObstacle4.Value;
                CmcsAutotruck.LeftObstacle5 = (int)dbi_LeftObstacle5.Value;
                CmcsAutotruck.LeftObstacle6 = (int)dbi_LeftObstacle6.Value;
                CmcsAutotruck.CarriageLength = (int)dbi_CarriageLength.Value;
                CmcsAutotruck.CarriageWidth = (int)dbi_CarriageWidth.Value;
                CmcsAutotruck.CarriageBottomToFloor = (int)dbi_CarriageBottomToFloor.Value;
                CmcsAutotruck.ReMark = txt_ReMark.Text;
                CmcsAutotruck.EPCCardId = txt_EPCCardNumber.Text;
                Dbers.GetInstance().SelfDber.Insert(CmcsAutotruck);
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

        private void btnSelectedCarModel_Click(object sender, EventArgs e)
        {
            FrmCarModel_Select Frm = new FrmCarModel_Select(string.Empty);
            Frm.ShowDialog();
            if (Frm.DialogResult == DialogResult.OK)
            {
                CmcsCarModel item = Frm.Output;
                dbi_LeftObstacle1.Value = item.LeftObstacle1;
                dbi_LeftObstacle2.Value = item.LeftObstacle2;
                dbi_LeftObstacle3.Value = item.LeftObstacle3;
                dbi_LeftObstacle4.Value = item.LeftObstacle4;
                dbi_LeftObstacle5.Value = item.LeftObstacle5;
                dbi_LeftObstacle6.Value = item.LeftObstacle6;
                dbi_CarriageLength.Value = item.CarriageLength;
                dbi_CarriageWidth.Value = item.CarriageWidth;
                dbi_CarriageBottomToFloor.Value = item.CarriageBottomToFloor;
            }
        }

        private void btnSelectEPCCardNumber_Click(object sender, EventArgs e)
        {
            FrmEPCCard_Select Frm = new FrmEPCCard_Select(string.Empty);
            Frm.ShowDialog();
            if (Frm.DialogResult == DialogResult.OK)
            {
                cmcsEPCCard = Frm.Output;
                txt_EPCCardNumber.Text = cmcsEPCCard.CardNumber;
            }
        }

        /// <summary>
        /// ����ʡ�ݼ�ư�ť
        /// </summary>
        private void CreateProvinceAbbreviationButton()
        {
            flpanProvinceAbbreviation.Controls.Clear();

            foreach (CmcsProvinceAbbreviation provinceAbbreviation in CarTransportDAO.GetInstance().GetProvinceAbbreviationsInOrder())
            {
                ButtonX btnProvinceAbbreviation = new ButtonX();
                btnProvinceAbbreviation.Text = provinceAbbreviation.PaName;
                btnProvinceAbbreviation.Style = eDotNetBarStyle.Metro;
                btnProvinceAbbreviation.Font = new Font("΢���ź�", 10.8f, FontStyle.Bold);
                btnProvinceAbbreviation.Size = new Size(26, 26);
                btnProvinceAbbreviation.Margin = new System.Windows.Forms.Padding(3);
                btnProvinceAbbreviation.Click += BtnProvinceAbbreviation_Click;

                flpanProvinceAbbreviation.Controls.Add(btnProvinceAbbreviation);
            }
        }

        /// <summary>
        /// ���ʡ�ݼ�ư�ť
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnProvinceAbbreviation_Click(object sender, EventArgs e)
        {
            ButtonX btnProvinceAbbreviation = sender as ButtonX;
            if (btnProvinceAbbreviation != null) CarTransportDAO.GetInstance().AddProvinceAbbreviationUseCount(btnProvinceAbbreviation.Text);

            txt_CarNumber.Text = txt_CarNumber.Text.Insert(0, btnProvinceAbbreviation.Text);
            txt_CarNumber.CloseDropDown();

            txt_CarNumber.Focus();
            txt_CarNumber.Select(txt_CarNumber.Text.Length, 0);
        }

        private void txt_CarNumber_ButtonDropDownClick(object sender, CancelEventArgs e)
        {
            CreateProvinceAbbreviationButton();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            try
            {
                List<string> tags = Hardwarer.Rwer2.ScanTags();
                if (tags.Count > 0)
                {
                    string tagId = tags[0];
                    this.CmcsEPCCard = Dbers.GetInstance().SelfDber.Entity<CmcsEPCCard>("where TagId=:TagId", new { TagId = tagId });
                }
            }
            catch { }
            timer1.Start();
        }
    }
}