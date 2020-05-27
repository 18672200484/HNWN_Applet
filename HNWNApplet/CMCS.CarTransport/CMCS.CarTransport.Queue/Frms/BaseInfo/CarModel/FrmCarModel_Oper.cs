using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CMCS.CarTransport.Queue.Core;
using CMCS.Common;
using CMCS.Common.Entities.CarTransport;
using DevComponents.DotNetBar;
using CMCS.CarTransport.Queue.Enums;

namespace CMCS.CarTransport.Queue.Frms.BaseInfo.CarModel
{
    public partial class FrmCarModel_Oper : DevComponents.DotNetBar.Metro.MetroForm
    {
        #region Var

        //ҵ��id
        string PId = String.Empty;

        //�༭ģʽ
        eEditMode EditMode = eEditMode.Ĭ��;

        CmcsCarModel CmcsCarModel;

        #endregion

        public FrmCarModel_Oper(string pId, eEditMode editMode)
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
        private void FrmCarModel_Oper_Load(object sender, EventArgs e)
        {
            label_warn.ForeColor = Color.Red;

            this.CmcsCarModel = Dbers.GetInstance().SelfDber.Get<CmcsCarModel>(this.PId);
            if (this.CmcsCarModel != null)
            {
                txt_ModelName.Text = CmcsCarModel.ModelName;
                dbi_LeftObstacle1.Value = this.CmcsCarModel.LeftObstacle1;
                dbi_LeftObstacle2.Value = this.CmcsCarModel.LeftObstacle2;
                dbi_LeftObstacle3.Value = this.CmcsCarModel.LeftObstacle3;
                dbi_LeftObstacle4.Value = this.CmcsCarModel.LeftObstacle4;
                dbi_LeftObstacle5.Value = this.CmcsCarModel.LeftObstacle5;
                dbi_LeftObstacle6.Value = this.CmcsCarModel.LeftObstacle6;
                dbi_RightObstacle1.Value = this.CmcsCarModel.RightObstacle1;
                dbi_RightObstacle2.Value = this.CmcsCarModel.RightObstacle2;
                dbi_RightObstacle3.Value = this.CmcsCarModel.RightObstacle3;
                dbi_RightObstacle4.Value = this.CmcsCarModel.RightObstacle4;
                dbi_RightObstacle5.Value = this.CmcsCarModel.RightObstacle5;
                dbi_RightObstacle6.Value = this.CmcsCarModel.RightObstacle6;
                dbi_CarriageLength.Value = this.CmcsCarModel.CarriageLength;
                dbi_CarriageWidth.Value = this.CmcsCarModel.CarriageWidth;
                dbi_CarriageBottomToFloor.Value = this.CmcsCarModel.CarriageBottomToFloor;
                txt_ReMark.Text = CmcsCarModel.ReMark;
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
            if (txt_ModelName.Text.Length == 0)
            {
                MessageBoxEx.Show("�ñ공�Ͳ���Ϊ�գ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if ((CmcsCarModel == null || CmcsCarModel.ModelName != txt_ModelName.Text) && Dbers.GetInstance().SelfDber.Entities<CmcsCarModel>(" where ModelName=:ModelName", new { ModelName = txt_ModelName.Text }).Count > 0)
            {
                MessageBoxEx.Show("�ñ공�Ͳ����ظ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (this.EditMode == eEditMode.�޸�)
            {
                if (dbi_CarriageLength.Value <= 0)
                {
                    MessageBoxEx.Show("�ó��ͳ�����Ϊ0��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (dbi_CarriageWidth.Value <= 0)
                {
                    MessageBoxEx.Show("�ó��Ϳ���Ϊ0��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (dbi_CarriageBottomToFloor.Value <= 0)
                {
                    MessageBoxEx.Show("�ó��ͳ���ײ�������߲���Ϊ0��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if ((dbi_LeftObstacle6.Value > 0 || dbi_RightObstacle6.Value > 0) && (dbi_LeftObstacle5.Value <= 0 && dbi_RightObstacle5.Value <= 0))
                {
                    MessageBoxEx.Show("������������Ϣ��������������Ϣ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if ((dbi_LeftObstacle5.Value > 0 || dbi_RightObstacle5.Value > 0) && (dbi_LeftObstacle4.Value <= 0 && dbi_RightObstacle4.Value <= 0))
                {
                    MessageBoxEx.Show("������������Ϣ��������������Ϣ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if ((dbi_LeftObstacle4.Value > 0 || dbi_RightObstacle4.Value > 0) && (dbi_LeftObstacle3.Value <= 0 && dbi_RightObstacle3.Value <= 0))
                {
                    MessageBoxEx.Show("������������Ϣ��������������Ϣ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if ((dbi_LeftObstacle3.Value > 0 || dbi_RightObstacle3.Value > 0) && (dbi_LeftObstacle2.Value <= 0 && dbi_RightObstacle2.Value <= 0))
                {
                    MessageBoxEx.Show("������������Ϣ�������������Ϣ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if ((dbi_LeftObstacle2.Value > 0 || dbi_RightObstacle2.Value > 0) && (dbi_LeftObstacle1.Value <= 0 && dbi_RightObstacle1.Value <= 0))
                {
                    MessageBoxEx.Show("�����������Ϣ����������һ��Ϣ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                CmcsCarModel.ModelName = txt_ModelName.Text;
                CmcsCarModel.LeftObstacle1 = (int)dbi_LeftObstacle1.Value;
                CmcsCarModel.LeftObstacle2 = (int)dbi_LeftObstacle2.Value;
                CmcsCarModel.LeftObstacle3 = (int)dbi_LeftObstacle3.Value;
                CmcsCarModel.LeftObstacle4 = (int)dbi_LeftObstacle4.Value;
                CmcsCarModel.LeftObstacle5 = (int)dbi_LeftObstacle5.Value;
                CmcsCarModel.LeftObstacle6 = (int)dbi_LeftObstacle6.Value;
                CmcsCarModel.RightObstacle1 = (int)dbi_RightObstacle1.Value;
                CmcsCarModel.RightObstacle2 = (int)dbi_RightObstacle2.Value;
                CmcsCarModel.RightObstacle3 = (int)dbi_RightObstacle3.Value;
                CmcsCarModel.RightObstacle4 = (int)dbi_RightObstacle4.Value;
                CmcsCarModel.RightObstacle5 = (int)dbi_RightObstacle5.Value;
                CmcsCarModel.RightObstacle6 = (int)dbi_RightObstacle6.Value;
                CmcsCarModel.CarriageLength = (int)dbi_CarriageLength.Value;
                CmcsCarModel.CarriageWidth = (int)dbi_CarriageWidth.Value;
                CmcsCarModel.CarriageBottomToFloor = (int)dbi_CarriageBottomToFloor.Value;
                CmcsCarModel.ReMark = txt_ReMark.Text;
                Dbers.GetInstance().SelfDber.Update(CmcsCarModel);
            }
            else if (this.EditMode == eEditMode.����)
            {
                CmcsCarModel = new CmcsCarModel();
                CmcsCarModel.ModelName = txt_ModelName.Text;
                CmcsCarModel.LeftObstacle1 = (int)dbi_LeftObstacle1.Value;
                CmcsCarModel.LeftObstacle2 = (int)dbi_LeftObstacle2.Value;
                CmcsCarModel.LeftObstacle3 = (int)dbi_LeftObstacle3.Value;
                CmcsCarModel.LeftObstacle4 = (int)dbi_LeftObstacle4.Value;
                CmcsCarModel.LeftObstacle5 = (int)dbi_LeftObstacle5.Value;
                CmcsCarModel.LeftObstacle6 = (int)dbi_LeftObstacle6.Value;
                CmcsCarModel.RightObstacle1 = (int)dbi_RightObstacle1.Value;
                CmcsCarModel.RightObstacle2 = (int)dbi_RightObstacle2.Value;
                CmcsCarModel.RightObstacle3 = (int)dbi_RightObstacle3.Value;
                CmcsCarModel.RightObstacle4 = (int)dbi_RightObstacle4.Value;
                CmcsCarModel.RightObstacle5 = (int)dbi_RightObstacle5.Value;
                CmcsCarModel.RightObstacle6 = (int)dbi_RightObstacle6.Value;
                CmcsCarModel.CarriageLength = (int)dbi_CarriageLength.Value;
                CmcsCarModel.CarriageWidth = (int)dbi_CarriageWidth.Value;
                CmcsCarModel.CarriageBottomToFloor = (int)dbi_CarriageBottomToFloor.Value;
                CmcsCarModel.ReMark = txt_ReMark.Text;
                Dbers.GetInstance().SelfDber.Insert(CmcsCarModel);
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