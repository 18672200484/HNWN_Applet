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
using CMCS.Common;
using CMCS.Common.DAO;
using CMCS.Common.Entities;
using CMCS.Common.Entities.BaseInfo;
using CMCS.Common.Entities.CarTransport;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.SuperGrid;

namespace CMCS.CarTransport.Queue.Frms.BaseInfo.Mine
{
    public partial class FrmMine_List : DevComponents.DotNetBar.Metro.MetroForm
    {
        /// <summary>
        /// ����Ψһ��ʶ��
        /// </summary>
        public static string UniqueKey = "FrmMine_List";
        /// <summary>
        /// ѡ�е�ʵ��
        /// </summary>
        public CmcsMine SelCmcsMine;
        /// <summary>
        /// ��ǰ�������ģʽ
        /// </summary>
        private eEditMode EditMode = eEditMode.Ĭ��;

        CommonDAO commonDAO = CommonDAO.GetInstance();

        public FrmMine_List()
        {
            InitializeComponent();
        }

        private void FrmMine_List_Shown(object sender, EventArgs e)
        {
            InitTree();

            //01�鿴 02���� 03�޸� 04ɾ��
            btnAdd.Visible = QueuerDAO.GetInstance().CheckPower(this.GetType().ToString(), "02", GlobalVars.LoginUser);
            btnUpdate.Visible = QueuerDAO.GetInstance().CheckPower(this.GetType().ToString(), "03", GlobalVars.LoginUser);
            btnDelete.Visible = QueuerDAO.GetInstance().CheckPower(this.GetType().ToString(), "04", GlobalVars.LoginUser);
        }

        private void InitTree()
        {
            IList<CmcsMine> rootList = Dbers.GetInstance().SelfDber.Entities<CmcsMine>();

            if (rootList.Count == 0)
            {
                //��ʼ�����ڵ�
                CmcsMine rootFuelKind = new CmcsMine();
                rootFuelKind.Id = "-1";
                rootFuelKind.Name = "������";
                rootFuelKind.Code = "00";
                rootFuelKind.IsStop = 0;
                rootFuelKind.Sequence = 0;
                Dbers.GetInstance().SelfDber.Insert<CmcsMine>(rootFuelKind);
            }

            advTree1.Nodes.Clear();

            CmcsMine rootEntity = Dbers.GetInstance().SelfDber.Get<CmcsMine>("-1");
            DevComponents.AdvTree.Node rootNode = CreateNode(rootEntity);

            LoadData(rootEntity, rootNode);

            advTree1.Nodes.Add(rootNode);

            ProcessFromRequest(eEditMode.�鿴);
        }

        void LoadData(CmcsMine entity, DevComponents.AdvTree.Node node)
        {
            if (entity == null || node == null) return;

            foreach (CmcsMine item in Dbers.GetInstance().SelfDber.Entities<CmcsMine>("where ParentId=:ParentId order by Sequence asc", new { ParentId = entity.Id }))
            {
                DevComponents.AdvTree.Node newNode = CreateNode(item);
                node.Nodes.Add(newNode);
                LoadData(item, newNode);
            }
        }

        DevComponents.AdvTree.Node CreateNode(CmcsMine entity)
        {
            DevComponents.AdvTree.Node node = new DevComponents.AdvTree.Node(entity.Name + ((entity.IsStop == 0) ? "" : "(��Ч)"));
            node.Tag = entity;
            node.Expanded = true;
            return node;
        }

        private void advTree1_NodeClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            SelFuelNode();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ProcessFromRequest(eEditMode.����);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (this.SelCmcsMine == null)
            {
                MessageBoxEx.Show("������ʾ", "����ѡ��һ�����!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ProcessFromRequest(eEditMode.�޸�);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.SelCmcsMine == null)
            {
                MessageBoxEx.Show("������ʾ", "����ѡ��һ�����!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ProcessFromRequest(eEditMode.ɾ��);
        }

        private void SelFuelNode()
        {
            this.SelCmcsMine = (advTree1.SelectedNode.Tag as CmcsMine);
            ProcessFromRequest(eEditMode.�鿴);
        }

        private void ProcessFromRequest(eEditMode editMode)
        {
            switch (editMode)
            {
                case eEditMode.����:
                    EditMode = editMode;
                    ClearFromControls();
                    HelperUtil.ControlReadOnly(pnlMain, false);
                    break;
                case eEditMode.�޸�:
                    EditMode = editMode;
                    InitObjectInfo();
                    HelperUtil.ControlReadOnly(pnlMain, false);
                    break;
                case eEditMode.�鿴:
                    EditMode = editMode;
                    InitObjectInfo();
                    HelperUtil.ControlReadOnly(pnlMain, true);
                    break;
                case eEditMode.ɾ��:
                    EditMode = editMode;
                    DelTreeNode();
                    ClearFromControls();
                    HelperUtil.ControlReadOnly(pnlMain, true);
                    break;
            }
        }

        private void DelTreeNode()
        {
            if (this.SelCmcsMine.Id == "-1") { MessageBoxEx.Show("������ʾ", "���ڵ㲻����ɾ��!", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (MessageBoxEx.Show("ȷ��ɾ���ýڵ㼰�ӽڵ���", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Dbers.GetInstance().SelfDber.DeleteBySQL<CmcsMine>("where Id=:Id or parentId=:Id", new { Id = SelCmcsMine.Id });
            }
            InitTree();
        }

        private void InitObjectInfo()
        {
            if (this.SelCmcsMine == null) return;
            txt_Name.Text = this.SelCmcsMine.Name;
            txt_Code.Text = this.SelCmcsMine.Code;
            txt_ReMark.Text = this.SelCmcsMine.ReMark;
            dbi_Sequence.Text = this.SelCmcsMine.Sequence.ToString();
            chb_IsUse.Checked = (this.SelCmcsMine.IsStop == 0);
        }

        private void ClearFromControls()
        {
            txt_Name.Text = string.Empty;
            txt_Code.Text = string.Empty;
            txt_ReMark.Text = string.Empty;
            dbi_Sequence.Value = 0;
            chb_IsUse.Checked = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidatePage()) return;

            if (EditMode == eEditMode.����)
            {
                if (this.SelCmcsMine == null) return;
                CmcsMine entity = new CmcsMine();
                entity.Code = commonDAO.GetFuelKindNewChildCode(this.SelCmcsMine.Code);
                entity.Name = txt_Name.Text;
                entity.Sequence = dbi_Sequence.Value;
                entity.ParentId = this.SelCmcsMine.Id;
                entity.IsStop = chb_IsUse.Checked ? 0 : 1;
                Dbers.GetInstance().SelfDber.Insert<CmcsMine>(entity);
            }
            else if (EditMode == eEditMode.�޸�)
            {
                if (this.SelCmcsMine == null) return;
                this.SelCmcsMine.Name = txt_Name.Text;
                this.SelCmcsMine.Code = txt_Code.Text;
                this.SelCmcsMine.Sequence = dbi_Sequence.Value;
                this.SelCmcsMine.IsStop = chb_IsUse.Checked ? 0 : 1;
                this.SelCmcsMine.ReMark = txt_ReMark.Text;
                Dbers.GetInstance().SelfDber.Update<CmcsMine>(this.SelCmcsMine);
            }

            InitTree();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            InitTree();
        }

        /// <summary>
        /// ��֤ҳ��ؼ�ֵ����Ч�Ϸ���
        /// </summary>
        /// <returns></returns>
        private bool ValidatePage()
        {
            if (string.IsNullOrEmpty(txt_Name.Text))
            {
                MessageBoxEx.Show("������ʾ", "ú�����Ʋ���Ϊ��!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (commonDAO.IsExistMineName(txt_Name.Text, SelCmcsMine.Id))
            {
                MessageBoxEx.Show("������ʾ", "������ͬú������!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
    }
}