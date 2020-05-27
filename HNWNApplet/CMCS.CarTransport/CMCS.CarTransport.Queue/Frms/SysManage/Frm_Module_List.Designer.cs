﻿namespace CMCS.CarTransport.Queue.Frms.SysManage
{
    partial class Frm_Module_List
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
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn13 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn14 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn15 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn16 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn17 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn18 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn19 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn20 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn21 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn22 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn23 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn24 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            this.superGridControl1 = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAll = new DevComponents.DotNetBar.ButtonX();
            this.txtUserAccount_Ser = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnAdd = new DevComponents.DotNetBar.ButtonX();
            this.btnSearch = new DevComponents.DotNetBar.ButtonX();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.superGridControl2 = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.btnInsertRes = new DevComponents.DotNetBar.ButtonX();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panelEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // superGridControl1
            // 
            this.superGridControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
            this.superGridControl1.DefaultVisualStyles.CellStyles.Default.Alignment = DevComponents.DotNetBar.SuperGrid.Style.Alignment.MiddleCenter;
            this.superGridControl1.DefaultVisualStyles.CellStyles.Default.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.superGridControl1.DefaultVisualStyles.ColumnHeaderStyles.Default.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.superGridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superGridControl1.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this.superGridControl1.ForeColor = System.Drawing.Color.White;
            this.superGridControl1.Location = new System.Drawing.Point(0, 0);
            this.superGridControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.superGridControl1.Name = "superGridControl1";
            this.superGridControl1.PrimaryGrid.AutoGenerateColumns = false;
            this.superGridControl1.PrimaryGrid.Caption.Text = "";
            gridColumn13.DataPropertyName = "Id";
            gridColumn13.Name = "clmId";
            gridColumn13.Visible = false;
            gridColumn14.CellStyles.Default.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Underline);
            gridColumn14.DefaultNewRowCellValue = "修改";
            gridColumn14.HeaderText = "  ";
            gridColumn14.InfoImageAlignment = DevComponents.DotNetBar.SuperGrid.Style.Alignment.BottomCenter;
            gridColumn14.Name = "clmEdit";
            gridColumn14.NullString = "修改";
            gridColumn14.Width = 32;
            gridColumn15.DataPropertyName = "ModuleName";
            gridColumn15.HeaderText = "模块名称";
            gridColumn15.Name = "clmModeuleName";
            gridColumn16.DataPropertyName = "Moduleno";
            gridColumn16.HeaderText = "模块编码";
            gridColumn16.Name = "clmModuleno";
            gridColumn16.Width = 80;
            gridColumn17.AutoSizeMode = DevComponents.DotNetBar.SuperGrid.ColumnAutoSizeMode.AllCells;
            gridColumn17.DataPropertyName = "ModuleDll";
            gridColumn17.HeaderText = "完整命名空间名";
            gridColumn17.MinimumWidth = 250;
            gridColumn17.Name = "clmModuleDll";
            gridColumn17.Width = 250;
            gridColumn18.DataPropertyName = "";
            gridColumn18.HeaderText = "启用";
            gridColumn18.Name = "clmStopUse";
            gridColumn18.Width = 60;
            gridColumn19.DataPropertyName = "CreateDate";
            gridColumn19.HeaderText = "创建时间";
            gridColumn19.Name = "clmCreateDate";
            gridColumn19.Width = 150;
            this.superGridControl1.PrimaryGrid.Columns.Add(gridColumn13);
            this.superGridControl1.PrimaryGrid.Columns.Add(gridColumn14);
            this.superGridControl1.PrimaryGrid.Columns.Add(gridColumn15);
            this.superGridControl1.PrimaryGrid.Columns.Add(gridColumn16);
            this.superGridControl1.PrimaryGrid.Columns.Add(gridColumn17);
            this.superGridControl1.PrimaryGrid.Columns.Add(gridColumn18);
            this.superGridControl1.PrimaryGrid.Columns.Add(gridColumn19);
            this.superGridControl1.PrimaryGrid.InitialSelection = DevComponents.DotNetBar.SuperGrid.RelativeSelection.Row;
            this.superGridControl1.PrimaryGrid.SelectionGranularity = DevComponents.DotNetBar.SuperGrid.SelectionGranularity.Row;
            this.superGridControl1.Size = new System.Drawing.Size(983, 614);
            this.superGridControl1.TabIndex = 4;
            this.superGridControl1.Text = "superGridControl1";
            this.superGridControl1.CellMouseDown += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridCellMouseEventArgs>(this.superGridControl1_CellMouseDown);
            this.superGridControl1.DataBindingComplete += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridDataBindingCompleteEventArgs>(this.superGridControl1_DataBindingComplete);
            this.superGridControl1.BeginEdit += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridEditEventArgs>(this.superGridControl1_BeginEdit);
            this.superGridControl1.GetRowHeaderText += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridGetRowHeaderTextEventArgs>(this.superGridControl1_GetRowHeaderText);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.ForeColor = System.Drawing.Color.White;
            this.splitContainer1.Location = new System.Drawing.Point(0, 1);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel1.ForeColor = System.Drawing.Color.White;
            this.splitContainer1.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(82)))), ((int)(((byte)(89)))));
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel2.ForeColor = System.Drawing.Color.White;
            this.splitContainer1.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer1.Size = new System.Drawing.Size(1385, 655);
            this.splitContainer1.SplitterDistance = 40;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 148;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.btnAll);
            this.panel1.Controls.Add(this.txtUserAccount_Ser);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.ForeColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1385, 40);
            this.panel1.TabIndex = 12;
            // 
            // btnAll
            // 
            this.btnAll.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAll.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAll.Location = new System.Drawing.Point(1317, 9);
            this.btnAll.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(64, 23);
            this.btnAll.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnAll.TabIndex = 20;
            this.btnAll.Text = "全 部";
            this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
            // 
            // txtUserAccount_Ser
            // 
            this.txtUserAccount_Ser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUserAccount_Ser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
            // 
            // 
            // 
            this.txtUserAccount_Ser.Border.Class = "TextBoxBorder";
            this.txtUserAccount_Ser.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtUserAccount_Ser.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserAccount_Ser.ForeColor = System.Drawing.Color.White;
            this.txtUserAccount_Ser.Location = new System.Drawing.Point(1065, 7);
            this.txtUserAccount_Ser.Name = "txtUserAccount_Ser";
            this.txtUserAccount_Ser.Size = new System.Drawing.Size(169, 27);
            this.txtUserAccount_Ser.TabIndex = 19;
            this.txtUserAccount_Ser.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtUserAccount_Ser.WatermarkImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtUserAccount_Ser.WatermarkText = "用户账户...";
            // 
            // btnAdd
            // 
            this.btnAdd.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.btnAdd.Location = new System.Drawing.Point(16, 9);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(64, 23);
            this.btnAdd.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnAdd.TabIndex = 18;
            this.btnAdd.Text = "新 增";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Location = new System.Drawing.Point(1246, 9);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(64, 23);
            this.btnSearch.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSearch.TabIndex = 13;
            this.btnSearch.Text = "搜 索";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.ForeColor = System.Drawing.Color.White;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer2.Panel1.Controls.Add(this.superGridControl1);
            this.splitContainer2.Panel1.ForeColor = System.Drawing.Color.White;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer2.Panel2.Controls.Add(this.superGridControl2);
            this.splitContainer2.Panel2.Controls.Add(this.panelEx1);
            this.splitContainer2.Panel2.ForeColor = System.Drawing.Color.White;
            this.splitContainer2.Panel2MinSize = 40;
            this.splitContainer2.Size = new System.Drawing.Size(1385, 614);
            this.splitContainer2.SplitterDistance = 983;
            this.splitContainer2.SplitterWidth = 1;
            this.splitContainer2.TabIndex = 0;
            // 
            // superGridControl2
            // 
            this.superGridControl2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
            this.superGridControl2.DefaultVisualStyles.CellStyles.Default.Alignment = DevComponents.DotNetBar.SuperGrid.Style.Alignment.MiddleCenter;
            this.superGridControl2.DefaultVisualStyles.CellStyles.Default.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.superGridControl2.DefaultVisualStyles.ColumnHeaderStyles.Default.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.superGridControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superGridControl2.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this.superGridControl2.ForeColor = System.Drawing.Color.White;
            this.superGridControl2.Location = new System.Drawing.Point(0, 35);
            this.superGridControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.superGridControl2.Name = "superGridControl2";
            this.superGridControl2.PrimaryGrid.AutoGenerateColumns = false;
            this.superGridControl2.PrimaryGrid.Caption.Text = "";
            gridColumn20.DataPropertyName = "Id";
            gridColumn20.Name = "clmId";
            gridColumn20.Visible = false;
            gridColumn21.CellStyles.Default.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Underline);
            gridColumn21.DefaultNewRowCellValue = "修改";
            gridColumn21.HeaderText = "  ";
            gridColumn21.InfoImageAlignment = DevComponents.DotNetBar.SuperGrid.Style.Alignment.BottomCenter;
            gridColumn21.Name = "clmEdit";
            gridColumn21.NullString = "修改";
            gridColumn21.Width = 32;
            gridColumn22.DataPropertyName = "ResName";
            gridColumn22.HeaderText = "功能名称";
            gridColumn22.Name = "clnResName";
            gridColumn23.DataPropertyName = "Resno";
            gridColumn23.HeaderText = "功能编码";
            gridColumn23.InfoImageAlignment = DevComponents.DotNetBar.SuperGrid.Style.Alignment.MiddleCenter;
            gridColumn23.Name = "clnResno";
            gridColumn23.Width = 80;
            gridColumn24.DataPropertyName = "ModuleId";
            gridColumn24.Name = "clnModuleId";
            gridColumn24.Visible = false;
            this.superGridControl2.PrimaryGrid.Columns.Add(gridColumn20);
            this.superGridControl2.PrimaryGrid.Columns.Add(gridColumn21);
            this.superGridControl2.PrimaryGrid.Columns.Add(gridColumn22);
            this.superGridControl2.PrimaryGrid.Columns.Add(gridColumn23);
            this.superGridControl2.PrimaryGrid.Columns.Add(gridColumn24);
            this.superGridControl2.PrimaryGrid.InitialSelection = DevComponents.DotNetBar.SuperGrid.RelativeSelection.Row;
            this.superGridControl2.PrimaryGrid.SelectionGranularity = DevComponents.DotNetBar.SuperGrid.SelectionGranularity.Row;
            this.superGridControl2.Size = new System.Drawing.Size(401, 579);
            this.superGridControl2.TabIndex = 7;
            this.superGridControl2.Text = "superGridControl2";
            this.superGridControl2.CellMouseDown += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridCellMouseEventArgs>(this.superGridControl2_CellMouseDown);
            this.superGridControl2.BeginEdit += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridEditEventArgs>(this.superGridControl2_BeginEdit);
            this.superGridControl2.GetRowHeaderText += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridGetRowHeaderTextEventArgs>(this.superGridControl2_GetRowHeaderText);
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.btnInsertRes);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(401, 35);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 6;
            // 
            // btnInsertRes
            // 
            this.btnInsertRes.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnInsertRes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInsertRes.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInsertRes.Location = new System.Drawing.Point(28, 6);
            this.btnInsertRes.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnInsertRes.Name = "btnInsertRes";
            this.btnInsertRes.Size = new System.Drawing.Size(64, 23);
            this.btnInsertRes.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnInsertRes.TabIndex = 14;
            this.btnInsertRes.Text = "新 增";
            this.btnInsertRes.Click += new System.EventHandler(this.btnInsertRes_Click);
            // 
            // Frm_Module_List
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
            this.ClientSize = new System.Drawing.Size(1386, 657);
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.Name = "Frm_Module_List";
            this.Text = "用户管理列表";
            this.Load += new System.EventHandler(this.Frm_Module_List_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panelEx1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnAdd;
        private DevComponents.DotNetBar.ButtonX btnSearch;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl superGridControl1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private DevComponents.DotNetBar.Controls.TextBoxX txtUserAccount_Ser;
        private DevComponents.DotNetBar.ButtonX btnAll;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.ButtonX btnInsertRes;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl superGridControl2;
    }
}