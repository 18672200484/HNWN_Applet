namespace CMCS.CarTransport.Queue.Frms.BaseInfo.SupplierAssignSampler
{
    partial class FrmSupplierAssignSampler_Oper
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSupplierAssignSampler_Oper));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.btnSubmit = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.panelEx2 = new DevComponents.DotNetBar.PanelEx();
            this.cmbSampler = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.txtRemark_Goods = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnSelectSupply = new DevComponents.DotNetBar.ButtonX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.labelX19 = new DevComponents.DotNetBar.LabelX();
            this.labelX20 = new DevComponents.DotNetBar.LabelX();
            this.txtSupplierName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelEx1.SuspendLayout();
            this.panelEx2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(82)))), ((int)(((byte)(89)))));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panelEx1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panelEx2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.ForeColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(804, 233);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.btnSubmit);
            this.panelEx1.Controls.Add(this.btnCancel);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(3, 196);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(798, 34);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 0;
            // 
            // btnSubmit
            // 
            this.btnSubmit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSubmit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSubmit.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.btnSubmit.Location = new System.Drawing.Point(633, 6);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSubmit.TabIndex = 1;
            this.btnSubmit.Text = "保  存";
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.btnCancel.Location = new System.Drawing.Point(714, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "取  消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panelEx2
            // 
            this.panelEx2.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx2.Controls.Add(this.cmbSampler);
            this.panelEx2.Controls.Add(this.txtRemark_Goods);
            this.panelEx2.Controls.Add(this.btnSelectSupply);
            this.panelEx2.Controls.Add(this.labelX5);
            this.panelEx2.Controls.Add(this.labelX19);
            this.panelEx2.Controls.Add(this.labelX20);
            this.panelEx2.Controls.Add(this.txtSupplierName);
            this.panelEx2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx2.Location = new System.Drawing.Point(3, 3);
            this.panelEx2.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.panelEx2.Name = "panelEx2";
            this.panelEx2.Size = new System.Drawing.Size(798, 190);
            this.panelEx2.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx2.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx2.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx2.Style.GradientAngle = 90;
            this.panelEx2.TabIndex = 1;
            // 
            // cmbSampler
            // 
            this.cmbSampler.DisplayMember = "Text";
            this.cmbSampler.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbSampler.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSampler.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.cmbSampler.ForeColor = System.Drawing.Color.White;
            this.cmbSampler.FormattingEnabled = true;
            this.cmbSampler.ItemHeight = 23;
            this.cmbSampler.Location = new System.Drawing.Point(511, 41);
            this.cmbSampler.Name = "cmbSampler";
            this.cmbSampler.Size = new System.Drawing.Size(234, 29);
            this.cmbSampler.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbSampler.TabIndex = 48;
            // 
            // txtRemark_Goods
            // 
            this.txtRemark_Goods.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
            // 
            // 
            // 
            this.txtRemark_Goods.Border.Class = "TextBoxBorder";
            this.txtRemark_Goods.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtRemark_Goods.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtRemark_Goods.ForeColor = System.Drawing.Color.White;
            this.txtRemark_Goods.Location = new System.Drawing.Point(132, 93);
            this.txtRemark_Goods.Multiline = true;
            this.txtRemark_Goods.Name = "txtRemark_Goods";
            this.txtRemark_Goods.Size = new System.Drawing.Size(613, 52);
            this.txtRemark_Goods.TabIndex = 46;
            this.txtRemark_Goods.TabStop = false;
            // 
            // btnSelectSupply
            // 
            this.btnSelectSupply.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSelectSupply.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.btnSelectSupply.Location = new System.Drawing.Point(345, 43);
            this.btnSelectSupply.Name = "btnSelectSupply";
            this.btnSelectSupply.Size = new System.Drawing.Size(25, 25);
            this.btnSelectSupply.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSelectSupply.TabIndex = 39;
            this.btnSelectSupply.Text = "选";
            this.btnSelectSupply.Click += new System.EventHandler(this.btnSelectSupply_Goods_Click);
            // 
            // labelX5
            // 
            this.labelX5.AutoSize = true;
            // 
            // 
            // 
            this.labelX5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX5.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.labelX5.ForeColor = System.Drawing.Color.White;
            this.labelX5.Location = new System.Drawing.Point(86, 94);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(41, 26);
            this.labelX5.TabIndex = 45;
            this.labelX5.Text = "备注";
            // 
            // labelX19
            // 
            this.labelX19.AutoSize = true;
            // 
            // 
            // 
            this.labelX19.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX19.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.labelX19.ForeColor = System.Drawing.Color.White;
            this.labelX19.Location = new System.Drawing.Point(414, 40);
            this.labelX19.Name = "labelX19";
            this.labelX19.Size = new System.Drawing.Size(91, 26);
            this.labelX19.TabIndex = 43;
            this.labelX19.Text = "采样机编号";
            // 
            // labelX20
            // 
            this.labelX20.AutoSize = true;
            // 
            // 
            // 
            this.labelX20.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX20.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.labelX20.ForeColor = System.Drawing.Color.White;
            this.labelX20.Location = new System.Drawing.Point(51, 42);
            this.labelX20.Name = "labelX20";
            this.labelX20.Size = new System.Drawing.Size(74, 26);
            this.labelX20.TabIndex = 41;
            this.labelX20.Text = "供货单位";
            // 
            // txtSupplierName
            // 
            this.txtSupplierName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
            // 
            // 
            // 
            this.txtSupplierName.Border.Class = "TextBoxBorder";
            this.txtSupplierName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtSupplierName.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtSupplierName.ForeColor = System.Drawing.Color.White;
            this.txtSupplierName.Location = new System.Drawing.Point(132, 41);
            this.txtSupplierName.Name = "txtSupplierName";
            this.txtSupplierName.ReadOnly = true;
            this.txtSupplierName.Size = new System.Drawing.Size(240, 29);
            this.txtSupplierName.TabIndex = 47;
            this.txtSupplierName.TabStop = false;
            // 
            // FrmSupplierAssignSampler_Oper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 233);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmSupplierAssignSampler_Oper";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "供应商分配采样机详情";
            this.Load += new System.EventHandler(this.FrmSupplierAssignSampler_Oper_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panelEx1.ResumeLayout(false);
            this.panelEx2.ResumeLayout(false);
            this.panelEx2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.ButtonX btnSubmit;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.PanelEx panelEx2;
        private DevComponents.DotNetBar.Controls.TextBoxX txtRemark_Goods;
        private DevComponents.DotNetBar.ButtonX btnSelectSupply;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.LabelX labelX19;
        private DevComponents.DotNetBar.LabelX labelX20;
        private DevComponents.DotNetBar.Controls.TextBoxX txtSupplierName;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbSampler;
    }
}