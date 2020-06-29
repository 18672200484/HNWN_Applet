namespace CMCS.CarTransport.Queue.Frms.BaseInfo.CmequiPment
{
    partial class FrmCmequiPment_List
	{
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.advTree1 = new DevComponents.AdvTree.AdvTree();
			this.nodeConnector1 = new DevComponents.AdvTree.NodeConnector();
			this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
			this.btnAdd = new DevComponents.DotNetBar.ButtonX();
			this.btnUpdate = new DevComponents.DotNetBar.ButtonX();
			this.btnDelete = new DevComponents.DotNetBar.ButtonX();
			this.labelX6 = new DevComponents.DotNetBar.LabelX();
			this.labelX3 = new DevComponents.DotNetBar.LabelX();
			this.txt_EquipMentName = new DevComponents.DotNetBar.Controls.TextBoxX();
			this.txt_InterfaceType = new DevComponents.DotNetBar.Controls.TextBoxX();
			this.labelX8 = new DevComponents.DotNetBar.LabelX();
			this.dbi_Sequence = new DevComponents.Editors.IntegerInput();
			this.labelX1 = new DevComponents.DotNetBar.LabelX();
			this.txt_EquipMentCode = new DevComponents.DotNetBar.Controls.TextBoxX();
			this.pnlMain = new DevComponents.DotNetBar.PanelEx();
			this.btnCancel = new DevComponents.DotNetBar.ButtonX();
			this.btnSave = new DevComponents.DotNetBar.ButtonX();
			this.labelX2 = new DevComponents.DotNetBar.LabelX();
			this.txt_SampleMachine = new DevComponents.DotNetBar.Controls.TextBoxX();
			((System.ComponentModel.ISupportInitialize)(this.advTree1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dbi_Sequence)).BeginInit();
			this.pnlMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// advTree1
			// 
			this.advTree1.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
			this.advTree1.AllowDrop = true;
			this.advTree1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
			// 
			// 
			// 
			this.advTree1.BackgroundStyle.Class = "TreeBorderKey";
			this.advTree1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.advTree1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.advTree1.ForeColor = System.Drawing.Color.White;
			this.advTree1.Location = new System.Drawing.Point(12, 39);
			this.advTree1.Name = "advTree1";
			this.advTree1.NodesConnector = this.nodeConnector1;
			this.advTree1.NodeStyle = this.elementStyle1;
			this.advTree1.PathSeparator = ";";
			this.advTree1.Size = new System.Drawing.Size(428, 606);
			this.advTree1.Styles.Add(this.elementStyle1);
			this.advTree1.TabIndex = 5;
			this.advTree1.Text = "advTree1";
			this.advTree1.NodeClick += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.advTree1_NodeClick);
			// 
			// nodeConnector1
			// 
			this.nodeConnector1.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(188)))), ((int)(((byte)(204)))));
			// 
			// elementStyle1
			// 
			this.elementStyle1.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.elementStyle1.Name = "elementStyle1";
			this.elementStyle1.TextColor = System.Drawing.Color.White;
			// 
			// btnAdd
			// 
			this.btnAdd.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 11.25F);
			this.btnAdd.Location = new System.Drawing.Point(12, 11);
			this.btnAdd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(64, 23);
			this.btnAdd.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.btnAdd.TabIndex = 19;
			this.btnAdd.Text = "新 增";
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnUpdate
			// 
			this.btnUpdate.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnUpdate.Font = new System.Drawing.Font("Segoe UI", 11.25F);
			this.btnUpdate.Location = new System.Drawing.Point(82, 11);
			this.btnUpdate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.btnUpdate.Name = "btnUpdate";
			this.btnUpdate.Size = new System.Drawing.Size(64, 23);
			this.btnUpdate.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.btnUpdate.TabIndex = 20;
			this.btnUpdate.Text = "修 改";
			this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 11.25F);
			this.btnDelete.Location = new System.Drawing.Point(152, 11);
			this.btnDelete.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(64, 23);
			this.btnDelete.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.btnDelete.TabIndex = 21;
			this.btnDelete.Text = "删 除";
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// labelX6
			// 
			this.labelX6.AutoSize = true;
			this.labelX6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
			// 
			// 
			// 
			this.labelX6.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.labelX6.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelX6.ForeColor = System.Drawing.Color.White;
			this.labelX6.Location = new System.Drawing.Point(415, 30);
			this.labelX6.Name = "labelX6";
			this.labelX6.Size = new System.Drawing.Size(56, 24);
			this.labelX6.TabIndex = 228;
			this.labelX6.Text = "顺序号";
			// 
			// labelX3
			// 
			this.labelX3.AutoSize = true;
			this.labelX3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
			// 
			// 
			// 
			this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.labelX3.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelX3.ForeColor = System.Drawing.Color.White;
			this.labelX3.Location = new System.Drawing.Point(90, 31);
			this.labelX3.Name = "labelX3";
			this.labelX3.Size = new System.Drawing.Size(72, 24);
			this.labelX3.TabIndex = 227;
			this.labelX3.Text = "设备名称";
			// 
			// txt_EquipMentName
			// 
			this.txt_EquipMentName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
			// 
			// 
			// 
			this.txt_EquipMentName.Border.Class = "TextBoxBorder";
			this.txt_EquipMentName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.txt_EquipMentName.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txt_EquipMentName.ForeColor = System.Drawing.Color.White;
			this.txt_EquipMentName.Location = new System.Drawing.Point(166, 28);
			this.txt_EquipMentName.Name = "txt_EquipMentName";
			this.txt_EquipMentName.Size = new System.Drawing.Size(180, 27);
			this.txt_EquipMentName.TabIndex = 226;
			// 
			// txt_InterfaceType
			// 
			this.txt_InterfaceType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
			// 
			// 
			// 
			this.txt_InterfaceType.Border.Class = "TextBoxBorder";
			this.txt_InterfaceType.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.txt_InterfaceType.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txt_InterfaceType.ForeColor = System.Drawing.Color.White;
			this.txt_InterfaceType.Location = new System.Drawing.Point(166, 117);
			this.txt_InterfaceType.Multiline = true;
			this.txt_InterfaceType.Name = "txt_InterfaceType";
			this.txt_InterfaceType.Size = new System.Drawing.Size(489, 60);
			this.txt_InterfaceType.TabIndex = 235;
			// 
			// labelX8
			// 
			this.labelX8.AutoSize = true;
			this.labelX8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
			// 
			// 
			// 
			this.labelX8.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.labelX8.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelX8.ForeColor = System.Drawing.Color.White;
			this.labelX8.Location = new System.Drawing.Point(90, 117);
			this.labelX8.Name = "labelX8";
			this.labelX8.Size = new System.Drawing.Size(72, 24);
			this.labelX8.TabIndex = 234;
			this.labelX8.Text = "接口类型";
			// 
			// dbi_Sequence
			// 
			this.dbi_Sequence.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
			// 
			// 
			// 
			this.dbi_Sequence.BackgroundStyle.Class = "DateTimeInputBackground";
			this.dbi_Sequence.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.dbi_Sequence.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
			this.dbi_Sequence.Font = new System.Drawing.Font("Segoe UI", 11.25F);
			this.dbi_Sequence.ForeColor = System.Drawing.Color.White;
			this.dbi_Sequence.Location = new System.Drawing.Point(475, 28);
			this.dbi_Sequence.MaxValue = 100000;
			this.dbi_Sequence.MinValue = 0;
			this.dbi_Sequence.Name = "dbi_Sequence";
			this.dbi_Sequence.Size = new System.Drawing.Size(180, 27);
			this.dbi_Sequence.TabIndex = 236;
			// 
			// labelX1
			// 
			this.labelX1.AutoSize = true;
			this.labelX1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
			// 
			// 
			// 
			this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.labelX1.Font = new System.Drawing.Font("Segoe UI", 11.25F);
			this.labelX1.ForeColor = System.Drawing.Color.White;
			this.labelX1.Location = new System.Drawing.Point(90, 74);
			this.labelX1.Name = "labelX1";
			this.labelX1.Size = new System.Drawing.Size(72, 24);
			this.labelX1.TabIndex = 241;
			this.labelX1.Text = "设备编码";
			// 
			// txt_EquipMentCode
			// 
			this.txt_EquipMentCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
			// 
			// 
			// 
			this.txt_EquipMentCode.Border.Class = "TextBoxBorder";
			this.txt_EquipMentCode.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.txt_EquipMentCode.Font = new System.Drawing.Font("Segoe UI", 11.25F);
			this.txt_EquipMentCode.ForeColor = System.Drawing.Color.White;
			this.txt_EquipMentCode.Location = new System.Drawing.Point(166, 71);
			this.txt_EquipMentCode.Name = "txt_EquipMentCode";
			this.txt_EquipMentCode.ReadOnly = true;
			this.txt_EquipMentCode.Size = new System.Drawing.Size(180, 27);
			this.txt_EquipMentCode.TabIndex = 240;
			// 
			// pnlMain
			// 
			this.pnlMain.CanvasColor = System.Drawing.Color.Transparent;
			this.pnlMain.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.pnlMain.Controls.Add(this.labelX2);
			this.pnlMain.Controls.Add(this.txt_SampleMachine);
			this.pnlMain.Controls.Add(this.btnCancel);
			this.pnlMain.Controls.Add(this.btnSave);
			this.pnlMain.Controls.Add(this.txt_InterfaceType);
			this.pnlMain.Controls.Add(this.labelX1);
			this.pnlMain.Controls.Add(this.txt_EquipMentName);
			this.pnlMain.Controls.Add(this.txt_EquipMentCode);
			this.pnlMain.Controls.Add(this.labelX3);
			this.pnlMain.Controls.Add(this.dbi_Sequence);
			this.pnlMain.Controls.Add(this.labelX6);
			this.pnlMain.Controls.Add(this.labelX8);
			this.pnlMain.Location = new System.Drawing.Point(478, 39);
			this.pnlMain.Name = "pnlMain";
			this.pnlMain.Size = new System.Drawing.Size(894, 606);
			this.pnlMain.Style.Alignment = System.Drawing.StringAlignment.Center;
			this.pnlMain.Style.BackColor1.Color = System.Drawing.Color.Transparent;
			this.pnlMain.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
			this.pnlMain.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
			this.pnlMain.Style.GradientAngle = 90;
			this.pnlMain.TabIndex = 242;
			// 
			// btnCancel
			// 
			this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 11.25F);
			this.btnCancel.Location = new System.Drawing.Point(591, 283);
			this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(64, 23);
			this.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.btnCancel.TabIndex = 243;
			this.btnCancel.Text = "取 消";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnSave
			// 
			this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnSave.Font = new System.Drawing.Font("Segoe UI", 11.25F);
			this.btnSave.Location = new System.Drawing.Point(521, 283);
			this.btnSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(64, 23);
			this.btnSave.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.btnSave.TabIndex = 242;
			this.btnSave.Text = "保 存";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// labelX2
			// 
			this.labelX2.AutoSize = true;
			this.labelX2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
			// 
			// 
			// 
			this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.labelX2.Font = new System.Drawing.Font("Segoe UI", 11.25F);
			this.labelX2.ForeColor = System.Drawing.Color.White;
			this.labelX2.Location = new System.Drawing.Point(399, 74);
			this.labelX2.Name = "labelX2";
			this.labelX2.Size = new System.Drawing.Size(72, 24);
			this.labelX2.TabIndex = 245;
			this.labelX2.Text = "厂家名称";
			// 
			// txt_SampleMachine
			// 
			this.txt_SampleMachine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
			// 
			// 
			// 
			this.txt_SampleMachine.Border.Class = "TextBoxBorder";
			this.txt_SampleMachine.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.txt_SampleMachine.Font = new System.Drawing.Font("Segoe UI", 11.25F);
			this.txt_SampleMachine.ForeColor = System.Drawing.Color.White;
			this.txt_SampleMachine.Location = new System.Drawing.Point(475, 71);
			this.txt_SampleMachine.Name = "txt_SampleMachine";
			this.txt_SampleMachine.ReadOnly = true;
			this.txt_SampleMachine.Size = new System.Drawing.Size(180, 27);
			this.txt_SampleMachine.TabIndex = 244;
			// 
			// FrmCmequiPment_List
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1404, 656);
			this.Controls.Add(this.pnlMain);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.btnUpdate);
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.advTree1);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ForeColor = System.Drawing.Color.White;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(1420, 695);
			this.MinimumSize = new System.Drawing.Size(1420, 695);
			this.Name = "FrmCmequiPment_List";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "设备管理";
			this.Shown += new System.EventHandler(this.FrmFuelKind_List_Shown);
			((System.ComponentModel.ISupportInitialize)(this.advTree1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dbi_Sequence)).EndInit();
			this.pnlMain.ResumeLayout(false);
			this.pnlMain.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.AdvTree.AdvTree advTree1;
        private DevComponents.AdvTree.NodeConnector nodeConnector1;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
        private DevComponents.DotNetBar.ButtonX btnAdd;
        private DevComponents.DotNetBar.ButtonX btnUpdate;
        private DevComponents.DotNetBar.ButtonX btnDelete;
        private DevComponents.DotNetBar.LabelX labelX6;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.TextBoxX txt_EquipMentName;
        private DevComponents.DotNetBar.Controls.TextBoxX txt_InterfaceType;
        private DevComponents.DotNetBar.LabelX labelX8;
        private DevComponents.Editors.IntegerInput dbi_Sequence;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.TextBoxX txt_EquipMentCode;
        private DevComponents.DotNetBar.PanelEx pnlMain;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.ButtonX btnSave;
		private DevComponents.DotNetBar.LabelX labelX2;
		private DevComponents.DotNetBar.Controls.TextBoxX txt_SampleMachine;
	}
}