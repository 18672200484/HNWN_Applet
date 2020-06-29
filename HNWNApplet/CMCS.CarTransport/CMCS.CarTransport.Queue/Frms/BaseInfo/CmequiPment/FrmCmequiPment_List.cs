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
		/// 窗体唯一标识符
		/// </summary>
		public static string UniqueKey = "FrmFuelKind_List";

		/// <summary>
		/// 选中的实体
		/// </summary>
		public CmcsCMEquipment SelFuelKind;

		/// <summary>
		/// 当前界面操作模式
		/// </summary>
		private eEditMode CurrEditMode = eEditMode.默认;

		CommonDAO commonDAO = CommonDAO.GetInstance();

		#endregion

		public FrmCmequiPment_List()
		{
			InitializeComponent();
		}

		private void FrmFuelKind_List_Shown(object sender, EventArgs e)
		{
			InitTree();

			//01查看 02增加 03修改 04删除
			btnAdd.Visible = QueuerDAO.GetInstance().CheckPower(this.GetType().ToString(), "02", GlobalVars.LoginUser);
			btnUpdate.Visible = QueuerDAO.GetInstance().CheckPower(this.GetType().ToString(), "03", GlobalVars.LoginUser);
			btnDelete.Visible = QueuerDAO.GetInstance().CheckPower(this.GetType().ToString(), "04", GlobalVars.LoginUser);
		}

		private void InitTree()
		{
			IList<CmcsCMEquipment> rootList = Dbers.GetInstance().SelfDber.Entities<CmcsCMEquipment>();

			if (rootList.Count == 0)
			{
				//初始化根节点
				CmcsCMEquipment rootFuelKind = new CmcsCMEquipment();
				rootFuelKind.Id = "-1";
				rootFuelKind.EquipmentName = "设备管理";
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

			ProcessFromRequest(eEditMode.查看);
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
			ProcessFromRequest(eEditMode.新增);
		}

		private void btnUpdate_Click(object sender, EventArgs e)
		{
			if (this.SelFuelKind == null)
			{
				MessageBoxEx.Show("操作提示", "请先选择一个设备!", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			ProcessFromRequest(eEditMode.修改);
		}

		private void btnDelete_Click(object sender, EventArgs e)
		{
			if (this.SelFuelKind == null)
			{
				MessageBoxEx.Show("操作提示", "请先选择一个设备!", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			ProcessFromRequest(eEditMode.删除);
		}

		private void SelFuelNode()
		{
			this.SelFuelKind = (advTree1.SelectedNode.Tag as CmcsCMEquipment);
			ProcessFromRequest(eEditMode.查看);
		}

		private void ProcessFromRequest(eEditMode editMode)
		{
			if (editMode != eEditMode.查看 && SelFuelKind == null)
			{
				MessageBoxEx.Show("请先选择一个节点!", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			switch (editMode)
			{
				case eEditMode.新增:
					CurrEditMode = editMode;
					ClearFromControls();
					HelperUtil.ControlReadOnly(pnlMain, false);
					break;
				case eEditMode.修改:
					if (this.SelFuelKind.Id == "-1") { MessageBoxEx.Show("根节点不允许修改!", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
					CurrEditMode = editMode;
					InitObjectInfo();
					HelperUtil.ControlReadOnly(pnlMain, false);
					break;
				case eEditMode.查看:
					CurrEditMode = editMode;
					InitObjectInfo();
					HelperUtil.ControlReadOnly(pnlMain, true);
					break;
				case eEditMode.删除:
					CurrEditMode = editMode;
					DelTreeNode();
					ClearFromControls();
					HelperUtil.ControlReadOnly(pnlMain, true);
					break;
			}
		}

		private void DelTreeNode()
		{
			if (this.SelFuelKind.Id == "-1") { MessageBoxEx.Show("根节点不允许删除!", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
			if (MessageBoxEx.Show("确认删除该节点及子节点吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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

			if (CurrEditMode == eEditMode.新增)
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
			else if (CurrEditMode == eEditMode.修改)
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
		/// 验证页面控件值的有效合法性
		/// </summary>
		/// <returns></returns>
		private bool ValidatePage()
		{
			if (string.IsNullOrEmpty(txt_EquipMentName.Text))
			{
				MessageBoxEx.Show("设备名称不能为空!", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return false;
			}
			if (commonDAO.IsExistCMEquipMentName(txt_EquipMentName.Text, SelFuelKind.Id))
			{
				MessageBoxEx.Show("已有相同设备名称!", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return false;
			}
			if (string.IsNullOrEmpty(txt_EquipMentCode.Text))
			{
				MessageBoxEx.Show("设备编码不能为空!", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return false;
			}
			if (commonDAO.IsExistCMEquipMentCode(txt_EquipMentCode.Text, SelFuelKind.Id))
			{
				MessageBoxEx.Show("已有相同设备编码!", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return false;
			}
			if (string.IsNullOrEmpty(txt_InterfaceType.Text))
			{
				MessageBoxEx.Show("接口类型不能为空!", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return false;
			}
			return true;
		}
	}
}