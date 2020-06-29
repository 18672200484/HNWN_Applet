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

namespace CMCS.CarTransport.Queue.Frms.BaseInfo.CmequiPment
{
	public partial class FrmCmequiPment_List : DevComponents.DotNetBar.Metro.MetroForm
	{
		#region Var

		/// <summary>
		/// ����Ψһ��ʶ��
		/// </summary>
		public static string UniqueKey = "FrmFuelKind_List";

		/// <summary>
		/// ѡ�е�ʵ��
		/// </summary>
		public CmcsCMEquipment SelFuelKind;

		/// <summary>
		/// ��ǰ�������ģʽ
		/// </summary>
		private eEditMode CurrEditMode = eEditMode.Ĭ��;

		CommonDAO commonDAO = CommonDAO.GetInstance();

		#endregion

		public FrmCmequiPment_List()
		{
			InitializeComponent();
		}

		private void FrmFuelKind_List_Shown(object sender, EventArgs e)
		{
			InitTree();

			//01�鿴 02���� 03�޸� 04ɾ��
			btnAdd.Visible = QueuerDAO.GetInstance().CheckPower(this.GetType().ToString(), "02", GlobalVars.LoginUser);
			btnUpdate.Visible = QueuerDAO.GetInstance().CheckPower(this.GetType().ToString(), "03", GlobalVars.LoginUser);
			btnDelete.Visible = QueuerDAO.GetInstance().CheckPower(this.GetType().ToString(), "04", GlobalVars.LoginUser);
		}

		private void InitTree()
		{
			IList<CmcsCMEquipment> rootList = Dbers.GetInstance().SelfDber.Entities<CmcsCMEquipment>();

			if (rootList.Count == 0)
			{
				//��ʼ�����ڵ�
				CmcsCMEquipment rootFuelKind = new CmcsCMEquipment();
				rootFuelKind.Id = "-1";
				rootFuelKind.EquipmentName = "�豸����";
				rootFuelKind.EquipmentCode = "00";
				rootFuelKind.NodeCode = "00";
				rootFuelKind.Sequence = 0;
				Dbers.GetInstance().SelfDber.Insert<CmcsCMEquipment>(rootFuelKind);
			}

			advTree1.Nodes.Clear();

			CmcsCMEquipment rootEntity = Dbers.GetInstance().SelfDber.Get<CmcsCMEquipment>("-1");
			DevComponents.AdvTree.Node rootNode = CreateNode(rootEntity);

			LoadData(rootEntity, rootNode);

			advTree1.Nodes.Add(rootNode);

			ProcessFromRequest(eEditMode.�鿴);
		}

		void LoadData(CmcsCMEquipment entity, DevComponents.AdvTree.Node node)
		{
			if (entity == null || node == null) return;

			foreach (CmcsCMEquipment item in Dbers.GetInstance().SelfDber.Entities<CmcsCMEquipment>("where ParentId=:ParentId order by Sequence asc", new { ParentId = entity.Id }))
			{
				DevComponents.AdvTree.Node newNode = CreateNode(item);
				node.Nodes.Add(newNode);
				LoadData(item, newNode);
			}
		}

		DevComponents.AdvTree.Node CreateNode(CmcsCMEquipment entity)
		{
			DevComponents.AdvTree.Node node = new DevComponents.AdvTree.Node(entity.EquipmentName);
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
			if (this.SelFuelKind == null)
			{
				MessageBoxEx.Show("������ʾ", "����ѡ��һ���豸!", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			ProcessFromRequest(eEditMode.�޸�);
		}

		private void btnDelete_Click(object sender, EventArgs e)
		{
			if (this.SelFuelKind == null)
			{
				MessageBoxEx.Show("������ʾ", "����ѡ��һ���豸!", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			ProcessFromRequest(eEditMode.ɾ��);
		}

		private void SelFuelNode()
		{
			this.SelFuelKind = (advTree1.SelectedNode.Tag as CmcsCMEquipment);
			ProcessFromRequest(eEditMode.�鿴);
		}

		private void ProcessFromRequest(eEditMode editMode)
		{
			if (editMode != eEditMode.�鿴 && SelFuelKind == null)
			{
				MessageBoxEx.Show("����ѡ��һ���ڵ�!", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			switch (editMode)
			{
				case eEditMode.����:
					CurrEditMode = editMode;
					ClearFromControls();
					HelperUtil.ControlReadOnly(pnlMain, false);
					break;
				case eEditMode.�޸�:
					if (this.SelFuelKind.Id == "-1") { MessageBoxEx.Show("���ڵ㲻�����޸�!", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
					CurrEditMode = editMode;
					InitObjectInfo();
					HelperUtil.ControlReadOnly(pnlMain, false);
					break;
				case eEditMode.�鿴:
					CurrEditMode = editMode;
					InitObjectInfo();
					HelperUtil.ControlReadOnly(pnlMain, true);
					break;
				case eEditMode.ɾ��:
					CurrEditMode = editMode;
					DelTreeNode();
					ClearFromControls();
					HelperUtil.ControlReadOnly(pnlMain, true);
					break;
			}
		}

		private void DelTreeNode()
		{
			if (this.SelFuelKind.Id == "-1") { MessageBoxEx.Show("���ڵ㲻����ɾ��!", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
			if (MessageBoxEx.Show("ȷ��ɾ���ýڵ㼰�ӽڵ���", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				Dbers.GetInstance().SelfDber.DeleteBySQL<CmcsCMEquipment>("where Id=:Id or parentId=:Id", new { Id = SelFuelKind.Id });
			}
			InitTree();
		}

		private void InitObjectInfo()
		{
			if (this.SelFuelKind == null) return;
			txt_EquipMentName.Text = this.SelFuelKind.EquipmentName;
			txt_EquipMentCode.Text = this.SelFuelKind.EquipmentCode;
			txt_InterfaceType.Text = this.SelFuelKind.InterfaceType;
			dbi_Sequence.Text = this.SelFuelKind.Sequence.ToString();
			txt_SampleMachine.Text = this.SelFuelKind.SampleMachine;
		}

		private void ClearFromControls()
		{
			txt_EquipMentName.Text = string.Empty;
			txt_EquipMentCode.Text = string.Empty;
			txt_InterfaceType.Text = string.Empty;
			dbi_Sequence.Value = 0;
			txt_SampleMachine.Text = string.Empty;
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			if (!ValidatePage()) return;

			if (CurrEditMode == eEditMode.����)
			{
				if (this.SelFuelKind == null) return;
				CmcsCMEquipment entity = new CmcsCMEquipment();
				entity.EquipmentName = txt_EquipMentName.Text;
				entity.EquipmentCode = txt_EquipMentCode.Text;
				entity.NodeCode = commonDAO.GetCMEquipMentNewChildCode(this.SelFuelKind.NodeCode);
				entity.Sequence = dbi_Sequence.Value;
				entity.Parentid = this.SelFuelKind.Id;
				entity.InterfaceType = txt_InterfaceType.Text;
				entity.SampleMachine = txt_SampleMachine.Text;
				Dbers.GetInstance().SelfDber.Insert<CmcsCMEquipment>(entity);
			}
			else if (CurrEditMode == eEditMode.�޸�)
			{
				if (this.SelFuelKind == null) return;
				this.SelFuelKind.EquipmentName = txt_EquipMentName.Text;
				this.SelFuelKind.EquipmentCode = txt_EquipMentCode.Text;
				this.SelFuelKind.Sequence = dbi_Sequence.Value;
				this.SelFuelKind.InterfaceType = txt_InterfaceType.Text;
				this.SelFuelKind.SampleMachine = txt_SampleMachine.Text;
				Dbers.GetInstance().SelfDber.Update<CmcsCMEquipment>(this.SelFuelKind);
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
			if (string.IsNullOrEmpty(txt_EquipMentName.Text))
			{
				MessageBoxEx.Show("�豸���Ʋ���Ϊ��!", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return false;
			}
			if (commonDAO.IsExistCMEquipMentName(txt_EquipMentName.Text, SelFuelKind.Id))
			{
				MessageBoxEx.Show("������ͬ�豸����!", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return false;
			}
			if (string.IsNullOrEmpty(txt_EquipMentCode.Text))
			{
				MessageBoxEx.Show("�豸���벻��Ϊ��!", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return false;
			}
			if (commonDAO.IsExistCMEquipMentCode(txt_EquipMentCode.Text, SelFuelKind.Id))
			{
				MessageBoxEx.Show("������ͬ�豸����!", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return false;
			}
			if (string.IsNullOrEmpty(txt_InterfaceType.Text))
			{
				MessageBoxEx.Show("�ӿ����Ͳ���Ϊ��!", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return false;
			}
			return true;
		}
	}
}