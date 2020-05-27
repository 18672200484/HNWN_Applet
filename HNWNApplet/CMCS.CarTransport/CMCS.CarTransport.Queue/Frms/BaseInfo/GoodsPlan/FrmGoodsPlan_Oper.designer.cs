namespace CMCS.CarTransport.Queue.Frms.BaseInfo.GoodsPlan
{
    partial class FrmGoodsPlan_Oper
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.btnSubmit = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.panelEx2 = new DevComponents.DotNetBar.PanelEx();
            this.txtRemark_Goods = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnSelectReceive_Goods = new DevComponents.DotNetBar.ButtonX();
            this.btnSelectGoodsType_Goods = new DevComponents.DotNetBar.ButtonX();
            this.btnbtnSelectSupply_Goods = new DevComponents.DotNetBar.ButtonX();
            this.btnSelectAutotruck_Goods = new DevComponents.DotNetBar.ButtonX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.labelX18 = new DevComponents.DotNetBar.LabelX();
            this.labelX19 = new DevComponents.DotNetBar.LabelX();
            this.labelX20 = new DevComponents.DotNetBar.LabelX();
            this.txtCarNumber_Goods = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX22 = new DevComponents.DotNetBar.LabelX();
            this.txtSupplyUnitName_Goods = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtReceiveUnitName_Goods = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtGoodsTypeName_Goods = new DevComponents.DotNetBar.Controls.TextBoxX();
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(804, 315);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.btnSubmit);
            this.panelEx1.Controls.Add(this.btnCancel);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(3, 278);
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
            this.panelEx2.Controls.Add(this.txtRemark_Goods);
            this.panelEx2.Controls.Add(this.btnSelectReceive_Goods);
            this.panelEx2.Controls.Add(this.btnSelectGoodsType_Goods);
            this.panelEx2.Controls.Add(this.btnbtnSelectSupply_Goods);
            this.panelEx2.Controls.Add(this.btnSelectAutotruck_Goods);
            this.panelEx2.Controls.Add(this.labelX5);
            this.panelEx2.Controls.Add(this.labelX18);
            this.panelEx2.Controls.Add(this.labelX19);
            this.panelEx2.Controls.Add(this.labelX20);
            this.panelEx2.Controls.Add(this.txtCarNumber_Goods);
            this.panelEx2.Controls.Add(this.labelX22);
            this.panelEx2.Controls.Add(this.txtSupplyUnitName_Goods);
            this.panelEx2.Controls.Add(this.txtReceiveUnitName_Goods);
            this.panelEx2.Controls.Add(this.txtGoodsTypeName_Goods);
            this.panelEx2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx2.Location = new System.Drawing.Point(3, 3);
            this.panelEx2.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.panelEx2.Name = "panelEx2";
            this.panelEx2.Size = new System.Drawing.Size(798, 272);
            this.panelEx2.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx2.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx2.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx2.Style.GradientAngle = 90;
            this.panelEx2.TabIndex = 1;
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
            this.txtRemark_Goods.Location = new System.Drawing.Point(131, 162);
            this.txtRemark_Goods.Multiline = true;
            this.txtRemark_Goods.Name = "txtRemark_Goods";
            this.txtRemark_Goods.Size = new System.Drawing.Size(613, 52);
            this.txtRemark_Goods.TabIndex = 46;
            this.txtRemark_Goods.TabStop = false;
            // 
            // btnSelectReceive_Goods
            // 
            this.btnSelectReceive_Goods.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSelectReceive_Goods.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.btnSelectReceive_Goods.Location = new System.Drawing.Point(719, 110);
            this.btnSelectReceive_Goods.Name = "btnSelectReceive_Goods";
            this.btnSelectReceive_Goods.Size = new System.Drawing.Size(25, 25);
            this.btnSelectReceive_Goods.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSelectReceive_Goods.TabIndex = 40;
            this.btnSelectReceive_Goods.Text = "选";
            this.btnSelectReceive_Goods.Click += new System.EventHandler(this.btnSelectReceive_Goods_Click);
            // 
            // btnSelectGoodsType_Goods
            // 
            this.btnSelectGoodsType_Goods.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSelectGoodsType_Goods.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.btnSelectGoodsType_Goods.Location = new System.Drawing.Point(719, 60);
            this.btnSelectGoodsType_Goods.Name = "btnSelectGoodsType_Goods";
            this.btnSelectGoodsType_Goods.Size = new System.Drawing.Size(25, 25);
            this.btnSelectGoodsType_Goods.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSelectGoodsType_Goods.TabIndex = 42;
            this.btnSelectGoodsType_Goods.Text = "选";
            this.btnSelectGoodsType_Goods.Click += new System.EventHandler(this.btnSelectGoodsType_Goods_Click);
            // 
            // btnbtnSelectSupply_Goods
            // 
            this.btnbtnSelectSupply_Goods.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnbtnSelectSupply_Goods.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.btnbtnSelectSupply_Goods.Location = new System.Drawing.Point(344, 112);
            this.btnbtnSelectSupply_Goods.Name = "btnbtnSelectSupply_Goods";
            this.btnbtnSelectSupply_Goods.Size = new System.Drawing.Size(25, 25);
            this.btnbtnSelectSupply_Goods.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnbtnSelectSupply_Goods.TabIndex = 39;
            this.btnbtnSelectSupply_Goods.Text = "选";
            this.btnbtnSelectSupply_Goods.Click += new System.EventHandler(this.btnbtnSelectSupply_Goods_Click);
            // 
            // btnSelectAutotruck_Goods
            // 
            this.btnSelectAutotruck_Goods.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSelectAutotruck_Goods.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.btnSelectAutotruck_Goods.Location = new System.Drawing.Point(344, 60);
            this.btnSelectAutotruck_Goods.Name = "btnSelectAutotruck_Goods";
            this.btnSelectAutotruck_Goods.Size = new System.Drawing.Size(25, 25);
            this.btnSelectAutotruck_Goods.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSelectAutotruck_Goods.TabIndex = 37;
            this.btnSelectAutotruck_Goods.Text = "选";
            this.btnSelectAutotruck_Goods.Click += new System.EventHandler(this.btnSelectAutotruck_Goods_Click);
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
            this.labelX5.Location = new System.Drawing.Point(85, 163);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(41, 26);
            this.labelX5.TabIndex = 45;
            this.labelX5.Text = "备注";
            // 
            // labelX18
            // 
            this.labelX18.AutoSize = true;
            // 
            // 
            // 
            this.labelX18.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX18.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.labelX18.ForeColor = System.Drawing.Color.White;
            this.labelX18.Location = new System.Drawing.Point(427, 59);
            this.labelX18.Name = "labelX18";
            this.labelX18.Size = new System.Drawing.Size(74, 26);
            this.labelX18.TabIndex = 44;
            this.labelX18.Text = "物资类型";
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
            this.labelX19.Location = new System.Drawing.Point(427, 109);
            this.labelX19.Name = "labelX19";
            this.labelX19.Size = new System.Drawing.Size(74, 26);
            this.labelX19.TabIndex = 43;
            this.labelX19.Text = "收货单位";
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
            this.labelX20.Location = new System.Drawing.Point(50, 111);
            this.labelX20.Name = "labelX20";
            this.labelX20.Size = new System.Drawing.Size(74, 26);
            this.labelX20.TabIndex = 41;
            this.labelX20.Text = "供货单位";
            // 
            // txtCarNumber_Goods
            // 
            this.txtCarNumber_Goods.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
            // 
            // 
            // 
            this.txtCarNumber_Goods.Border.Class = "TextBoxBorder";
            this.txtCarNumber_Goods.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtCarNumber_Goods.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtCarNumber_Goods.ForeColor = System.Drawing.Color.White;
            this.txtCarNumber_Goods.Location = new System.Drawing.Point(131, 58);
            this.txtCarNumber_Goods.Name = "txtCarNumber_Goods";
            this.txtCarNumber_Goods.ReadOnly = true;
            this.txtCarNumber_Goods.Size = new System.Drawing.Size(240, 29);
            this.txtCarNumber_Goods.TabIndex = 36;
            this.txtCarNumber_Goods.TabStop = false;
            // 
            // labelX22
            // 
            this.labelX22.AutoSize = true;
            // 
            // 
            // 
            this.labelX22.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX22.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.labelX22.ForeColor = System.Drawing.Color.White;
            this.labelX22.Location = new System.Drawing.Point(66, 59);
            this.labelX22.Name = "labelX22";
            this.labelX22.Size = new System.Drawing.Size(58, 26);
            this.labelX22.TabIndex = 38;
            this.labelX22.Text = "车牌号";
            // 
            // txtSupplyUnitName_Goods
            // 
            this.txtSupplyUnitName_Goods.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
            // 
            // 
            // 
            this.txtSupplyUnitName_Goods.Border.Class = "TextBoxBorder";
            this.txtSupplyUnitName_Goods.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtSupplyUnitName_Goods.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtSupplyUnitName_Goods.ForeColor = System.Drawing.Color.White;
            this.txtSupplyUnitName_Goods.Location = new System.Drawing.Point(131, 110);
            this.txtSupplyUnitName_Goods.Name = "txtSupplyUnitName_Goods";
            this.txtSupplyUnitName_Goods.ReadOnly = true;
            this.txtSupplyUnitName_Goods.Size = new System.Drawing.Size(240, 29);
            this.txtSupplyUnitName_Goods.TabIndex = 47;
            this.txtSupplyUnitName_Goods.TabStop = false;
            // 
            // txtReceiveUnitName_Goods
            // 
            this.txtReceiveUnitName_Goods.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
            // 
            // 
            // 
            this.txtReceiveUnitName_Goods.Border.Class = "TextBoxBorder";
            this.txtReceiveUnitName_Goods.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtReceiveUnitName_Goods.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtReceiveUnitName_Goods.ForeColor = System.Drawing.Color.White;
            this.txtReceiveUnitName_Goods.Location = new System.Drawing.Point(506, 108);
            this.txtReceiveUnitName_Goods.Name = "txtReceiveUnitName_Goods";
            this.txtReceiveUnitName_Goods.ReadOnly = true;
            this.txtReceiveUnitName_Goods.Size = new System.Drawing.Size(240, 29);
            this.txtReceiveUnitName_Goods.TabIndex = 48;
            this.txtReceiveUnitName_Goods.TabStop = false;
            // 
            // txtGoodsTypeName_Goods
            // 
            this.txtGoodsTypeName_Goods.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
            // 
            // 
            // 
            this.txtGoodsTypeName_Goods.Border.Class = "TextBoxBorder";
            this.txtGoodsTypeName_Goods.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtGoodsTypeName_Goods.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtGoodsTypeName_Goods.ForeColor = System.Drawing.Color.White;
            this.txtGoodsTypeName_Goods.Location = new System.Drawing.Point(506, 58);
            this.txtGoodsTypeName_Goods.Name = "txtGoodsTypeName_Goods";
            this.txtGoodsTypeName_Goods.ReadOnly = true;
            this.txtGoodsTypeName_Goods.Size = new System.Drawing.Size(240, 29);
            this.txtGoodsTypeName_Goods.TabIndex = 49;
            this.txtGoodsTypeName_Goods.TabStop = false;
            // 
            // FrmGoodsPlan_Oper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 315);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.Name = "FrmGoodsPlan_Oper";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "其他物资计划管理详情";
            this.Load += new System.EventHandler(this.FrmGoodsPlan_Oper_Load);
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
        private DevComponents.DotNetBar.ButtonX btnSelectReceive_Goods;
        private DevComponents.DotNetBar.ButtonX btnSelectGoodsType_Goods;
        private DevComponents.DotNetBar.ButtonX btnbtnSelectSupply_Goods;
        private DevComponents.DotNetBar.ButtonX btnSelectAutotruck_Goods;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.LabelX labelX18;
        private DevComponents.DotNetBar.LabelX labelX19;
        private DevComponents.DotNetBar.LabelX labelX20;
        private DevComponents.DotNetBar.Controls.TextBoxX txtCarNumber_Goods;
        private DevComponents.DotNetBar.LabelX labelX22;
        private DevComponents.DotNetBar.Controls.TextBoxX txtSupplyUnitName_Goods;
        private DevComponents.DotNetBar.Controls.TextBoxX txtReceiveUnitName_Goods;
        private DevComponents.DotNetBar.Controls.TextBoxX txtGoodsTypeName_Goods;
    }
}