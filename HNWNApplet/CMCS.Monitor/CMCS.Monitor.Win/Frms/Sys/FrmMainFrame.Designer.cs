using CMCS.Monitor.Win.Core;

namespace CMCS.Monitor.Win.Frms.Sys
{
    partial class FrmMainFrame
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMainFrame));
            this.buttonItem2 = new DevComponents.DotNetBar.ButtonItem();
            this.metroStatusBar1 = new DevComponents.DotNetBar.Metro.MetroStatusBar();
            this.labelItem1 = new DevComponents.DotNetBar.LabelItem();
            this.lblVersion = new DevComponents.DotNetBar.LabelItem();
            this.labelItem2 = new DevComponents.DotNetBar.LabelItem();
            this.lblLoginUserName = new DevComponents.DotNetBar.LabelItem();
            this.buttonItem1 = new DevComponents.DotNetBar.ButtonItem();
            this.btnOpenMainPage = new DevComponents.DotNetBar.ButtonX();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.lblCurrentTime = new System.Windows.Forms.Label();
            this.btnApplicationExit = new DevComponents.DotNetBar.ButtonX();
            this.lblSystemName = new System.Windows.Forms.Label();
            this.btnCZGK = new DevComponents.DotNetBar.ButtonX();
            this.btnOpenCarSampler = new DevComponents.DotNetBar.ButtonItem();
            this.btnOpenTrainBeltSampler = new DevComponents.DotNetBar.ButtonItem();
            this.btnOpenAutoMaker = new DevComponents.DotNetBar.ButtonItem();
            this.buttonX6 = new DevComponents.DotNetBar.ButtonX();
            this.btnOpenBuyFuelLoad = new DevComponents.DotNetBar.ButtonItem();
            this.btnOpenTruckWeighter = new DevComponents.DotNetBar.ButtonItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel_Buttons = new DevComponents.DotNetBar.PanelEx();
            this.btnSampleStorage = new DevComponents.DotNetBar.ButtonX();
            this.btnOpenGuardInfo = new DevComponents.DotNetBar.ButtonX();
            this.btnOpenEquInfHitch = new DevComponents.DotNetBar.ButtonX();
            this.btnCarMonitor = new DevComponents.DotNetBar.ButtonX();
            this.btnOpenAssayManage = new DevComponents.DotNetBar.ButtonX();
            this.superTabControl1 = new DevComponents.DotNetBar.SuperTabControl();
            this.superTabControlPanel1 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.superTabItem1 = new DevComponents.DotNetBar.SuperTabItem();
            this.flpanEquipments = new System.Windows.Forms.FlowLayoutPanel();
            this.timer_CurrentTime = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.timer_EquipmentStatus = new System.Windows.Forms.Timer(this.components);
            this.timer_MsgTime = new System.Windows.Forms.Timer(this.components);
            this.panelEx1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel_Buttons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.superTabControl1)).BeginInit();
            this.superTabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonItem2
            // 
            this.buttonItem2.Name = "buttonItem2";
            // 
            // metroStatusBar1
            // 
            this.metroStatusBar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
            // 
            // 
            // 
            this.metroStatusBar1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.metroStatusBar1.ContainerControlProcessDialogKey = true;
            this.metroStatusBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.metroStatusBar1.Font = new System.Drawing.Font("Segoe UI", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.metroStatusBar1.ForeColor = System.Drawing.Color.White;
            this.metroStatusBar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.labelItem1,
            this.lblVersion,
            this.labelItem2,
            this.lblLoginUserName});
            this.metroStatusBar1.Location = new System.Drawing.Point(0, 1016);
            this.metroStatusBar1.Name = "metroStatusBar1";
            this.metroStatusBar1.Size = new System.Drawing.Size(1918, 22);
            this.metroStatusBar1.TabIndex = 5;
            this.metroStatusBar1.Text = "metroStatusBar1";
            // 
            // labelItem1
            // 
            this.labelItem1.Name = "labelItem1";
            this.labelItem1.Text = "版本：";
            // 
            // lblVersion
            // 
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Text = "1.0.0.0";
            // 
            // labelItem2
            // 
            this.labelItem2.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Far;
            this.labelItem2.Name = "labelItem2";
            this.labelItem2.Text = "登录用户：";
            // 
            // lblLoginUserName
            // 
            this.lblLoginUserName.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Far;
            this.lblLoginUserName.Name = "lblLoginUserName";
            this.lblLoginUserName.Text = "系统管理员";
            this.lblLoginUserName.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // buttonItem1
            // 
            this.buttonItem1.Name = "buttonItem1";
            this.buttonItem1.Text = "buttonItem1";
            // 
            // btnOpenMainPage
            // 
            this.btnOpenMainPage.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOpenMainPage.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnOpenMainPage.AutoExpandOnClick = true;
            this.btnOpenMainPage.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenMainPage.Location = new System.Drawing.Point(3, 1);
            this.btnOpenMainPage.Name = "btnOpenMainPage";
            this.btnOpenMainPage.Size = new System.Drawing.Size(108, 31);
            this.btnOpenMainPage.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnOpenMainPage.TabIndex = 6;
            this.btnOpenMainPage.Text = "集 控 首 页";
            this.btnOpenMainPage.Click += new System.EventHandler(this.btnOpenHomePage_Click);
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.lblCurrentTime);
            this.panelEx1.Controls.Add(this.btnApplicationExit);
            this.panelEx1.Controls.Add(this.lblSystemName);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Margin = new System.Windows.Forms.Padding(0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(1918, 60);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(82)))), ((int)(((byte)(89)))));
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 7;
            // 
            // lblCurrentTime
            // 
            this.lblCurrentTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCurrentTime.AutoSize = true;
            this.lblCurrentTime.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentTime.ForeColor = System.Drawing.Color.White;
            this.lblCurrentTime.Location = new System.Drawing.Point(1590, 14);
            this.lblCurrentTime.Name = "lblCurrentTime";
            this.lblCurrentTime.Size = new System.Drawing.Size(239, 28);
            this.lblCurrentTime.TabIndex = 2;
            this.lblCurrentTime.Text = "2017年02月14日 09:24:39";
            // 
            // btnApplicationExit
            // 
            this.btnApplicationExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnApplicationExit.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnApplicationExit.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApplicationExit.Location = new System.Drawing.Point(1835, 15);
            this.btnApplicationExit.Name = "btnApplicationExit";
            this.btnApplicationExit.Size = new System.Drawing.Size(63, 30);
            this.btnApplicationExit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnApplicationExit.TabIndex = 8;
            this.btnApplicationExit.Text = "退  出";
            this.btnApplicationExit.Click += new System.EventHandler(this.btnApplicationExit_Click);
            // 
            // lblSystemName
            // 
            this.lblSystemName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.lblSystemName.AutoSize = true;
            this.lblSystemName.Font = new System.Drawing.Font("Segoe UI", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSystemName.ForeColor = System.Drawing.Color.White;
            this.lblSystemName.Location = new System.Drawing.Point(767, 4);
            this.lblSystemName.Name = "lblSystemName";
            this.lblSystemName.Size = new System.Drawing.Size(370, 47);
            this.lblSystemName.TabIndex = 1;
            this.lblSystemName.Text = "华能渭南集中管控平台";
            // 
            // btnCZGK
            // 
            this.btnCZGK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCZGK.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnCZGK.AutoExpandOnClick = true;
            this.btnCZGK.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCZGK.Location = new System.Drawing.Point(231, 1);
            this.btnCZGK.Name = "btnCZGK";
            this.btnCZGK.Size = new System.Drawing.Size(108, 31);
            this.btnCZGK.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnCZGK.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnOpenCarSampler,
            this.btnOpenTrainBeltSampler,
            this.btnOpenAutoMaker});
            this.btnCZGK.TabIndex = 9;
            this.btnCZGK.Text = "采 制 管 控";
            // 
            // btnOpenCarSampler
            // 
            this.btnOpenCarSampler.GlobalItem = false;
            this.btnOpenCarSampler.Name = "btnOpenCarSampler";
            this.btnOpenCarSampler.Text = "汽车机械采样监控";
            this.btnOpenCarSampler.Click += new System.EventHandler(this.btnOpenCarSampler_Click);
            // 
            // btnOpenTrainBeltSampler
            // 
            this.btnOpenTrainBeltSampler.GlobalItem = false;
            this.btnOpenTrainBeltSampler.Name = "btnOpenTrainBeltSampler";
            this.btnOpenTrainBeltSampler.Text = "入炉皮带采样监控";
            this.btnOpenTrainBeltSampler.Click += new System.EventHandler(this.btnOpenTrainBeltSampler_Click);
            // 
            // btnOpenAutoMaker
            // 
            this.btnOpenAutoMaker.GlobalItem = false;
            this.btnOpenAutoMaker.Name = "btnOpenAutoMaker";
            this.btnOpenAutoMaker.Text = "全自动制样机监控";
            this.btnOpenAutoMaker.Click += new System.EventHandler(this.btnOpenAutoMaker_Click);
            // 
            // buttonX6
            // 
            this.buttonX6.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.buttonX6.AutoExpandOnClick = true;
            this.buttonX6.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonX6.Location = new System.Drawing.Point(117, 1);
            this.buttonX6.Name = "buttonX6";
            this.buttonX6.Size = new System.Drawing.Size(108, 31);
            this.buttonX6.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX6.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnOpenBuyFuelLoad,
            this.btnOpenTruckWeighter});
            this.buttonX6.TabIndex = 10;
            this.buttonX6.Text = "计 量 管 控";
            // 
            // btnOpenBuyFuelLoad
            // 
            this.btnOpenBuyFuelLoad.GlobalItem = false;
            this.btnOpenBuyFuelLoad.Name = "btnOpenBuyFuelLoad";
            this.btnOpenBuyFuelLoad.Text = "汽车入厂记录查询";
            this.btnOpenBuyFuelLoad.Click += new System.EventHandler(this.btnOpenBuyFuelLoad_Click);
            // 
            // btnOpenTruckWeighter
            // 
            this.btnOpenTruckWeighter.GlobalItem = false;
            this.btnOpenTruckWeighter.Name = "btnOpenTruckWeighter";
            this.btnOpenTruckWeighter.Text = "汽车过衡监控";
            this.btnOpenTruckWeighter.Click += new System.EventHandler(this.btnOpenTruckWeighter_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(82)))), ((int)(((byte)(89)))));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel_Buttons, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panelEx1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.superTabControl1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.flpanEquipments, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.ForeColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1918, 1016);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // panel_Buttons
            // 
            this.panel_Buttons.CanvasColor = System.Drawing.SystemColors.Control;
            this.panel_Buttons.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panel_Buttons.Controls.Add(this.btnSampleStorage);
            this.panel_Buttons.Controls.Add(this.btnOpenGuardInfo);
            this.panel_Buttons.Controls.Add(this.btnOpenEquInfHitch);
            this.panel_Buttons.Controls.Add(this.btnCarMonitor);
            this.panel_Buttons.Controls.Add(this.btnOpenAssayManage);
            this.panel_Buttons.Controls.Add(this.btnCZGK);
            this.panel_Buttons.Controls.Add(this.btnOpenMainPage);
            this.panel_Buttons.Controls.Add(this.buttonX6);
            this.panel_Buttons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Buttons.Location = new System.Drawing.Point(3, 63);
            this.panel_Buttons.Name = "panel_Buttons";
            this.panel_Buttons.Size = new System.Drawing.Size(1912, 34);
            this.panel_Buttons.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panel_Buttons.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panel_Buttons.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panel_Buttons.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panel_Buttons.Style.GradientAngle = 90;
            this.panel_Buttons.TabIndex = 0;
            // 
            // btnSampleStorage
            // 
            this.btnSampleStorage.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSampleStorage.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnSampleStorage.AutoExpandOnClick = true;
            this.btnSampleStorage.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSampleStorage.Location = new System.Drawing.Point(345, 2);
            this.btnSampleStorage.Name = "btnSampleStorage";
            this.btnSampleStorage.Size = new System.Drawing.Size(108, 31);
            this.btnSampleStorage.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSampleStorage.TabIndex = 18;
            this.btnSampleStorage.Text = "存样柜管理";
            this.btnSampleStorage.Click += new System.EventHandler(this.btnSampleStorage_Click);
            // 
            // btnOpenGuardInfo
            // 
            this.btnOpenGuardInfo.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOpenGuardInfo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnOpenGuardInfo.AutoExpandOnClick = true;
            this.btnOpenGuardInfo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenGuardInfo.Location = new System.Drawing.Point(801, 2);
            this.btnOpenGuardInfo.Name = "btnOpenGuardInfo";
            this.btnOpenGuardInfo.Size = new System.Drawing.Size(108, 31);
            this.btnOpenGuardInfo.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnOpenGuardInfo.TabIndex = 17;
            this.btnOpenGuardInfo.Text = "门禁记录";
            this.btnOpenGuardInfo.Click += new System.EventHandler(this.btnOpenGuardInfo_Click);
            // 
            // btnOpenEquInfHitch
            // 
            this.btnOpenEquInfHitch.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOpenEquInfHitch.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnOpenEquInfHitch.AutoExpandOnClick = true;
            this.btnOpenEquInfHitch.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenEquInfHitch.Location = new System.Drawing.Point(687, 2);
            this.btnOpenEquInfHitch.Name = "btnOpenEquInfHitch";
            this.btnOpenEquInfHitch.Size = new System.Drawing.Size(108, 31);
            this.btnOpenEquInfHitch.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnOpenEquInfHitch.TabIndex = 16;
            this.btnOpenEquInfHitch.Text = "设备异常";
            this.btnOpenEquInfHitch.Click += new System.EventHandler(this.btnOpenEquInfHitch_Click);
            // 
            // btnCarMonitor
            // 
            this.btnCarMonitor.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCarMonitor.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnCarMonitor.AutoExpandOnClick = true;
            this.btnCarMonitor.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCarMonitor.Location = new System.Drawing.Point(573, 2);
            this.btnCarMonitor.Name = "btnCarMonitor";
            this.btnCarMonitor.Size = new System.Drawing.Size(108, 31);
            this.btnCarMonitor.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnCarMonitor.TabIndex = 15;
            this.btnCarMonitor.Text = "汽车设备监控";
            this.btnCarMonitor.Click += new System.EventHandler(this.btnCarMonitor_Click);
            // 
            // btnOpenAssayManage
            // 
            this.btnOpenAssayManage.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOpenAssayManage.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnOpenAssayManage.AutoExpandOnClick = true;
            this.btnOpenAssayManage.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenAssayManage.Location = new System.Drawing.Point(459, 2);
            this.btnOpenAssayManage.Name = "btnOpenAssayManage";
            this.btnOpenAssayManage.Size = new System.Drawing.Size(108, 31);
            this.btnOpenAssayManage.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnOpenAssayManage.TabIndex = 13;
            this.btnOpenAssayManage.Text = "化验室管理";
            this.btnOpenAssayManage.Click += new System.EventHandler(this.btnOpenAssayManage_Click);
            // 
            // superTabControl1
            // 
            this.superTabControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
            // 
            // 
            // 
            // 
            // 
            // 
            this.superTabControl1.ControlBox.CloseBox.Name = "";
            // 
            // 
            // 
            this.superTabControl1.ControlBox.MenuBox.Name = "";
            this.superTabControl1.ControlBox.Name = "";
            this.superTabControl1.ControlBox.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.superTabControl1.ControlBox.MenuBox,
            this.superTabControl1.ControlBox.CloseBox});
            this.superTabControl1.ControlBox.Visible = false;
            this.superTabControl1.Controls.Add(this.superTabControlPanel1);
            this.superTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControl1.ForeColor = System.Drawing.Color.White;
            this.superTabControl1.Location = new System.Drawing.Point(0, 100);
            this.superTabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.superTabControl1.Name = "superTabControl1";
            this.superTabControl1.ReorderTabsEnabled = true;
            this.superTabControl1.SelectedTabFont = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.superTabControl1.SelectedTabIndex = 0;
            this.superTabControl1.Size = new System.Drawing.Size(1918, 882);
            this.superTabControl1.TabFont = new System.Drawing.Font("Segoe UI", 9F);
            this.superTabControl1.TabIndex = 9;
            this.superTabControl1.Tabs.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.superTabItem1});
            this.superTabControl1.TabsVisible = false;
            this.superTabControl1.Text = "superTabControl_Main";
            // 
            // superTabControlPanel1
            // 
            this.superTabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel1.Location = new System.Drawing.Point(0, 30);
            this.superTabControlPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.superTabControlPanel1.Name = "superTabControlPanel1";
            this.superTabControlPanel1.Size = new System.Drawing.Size(1918, 852);
            this.superTabControlPanel1.TabIndex = 1;
            this.superTabControlPanel1.TabItem = this.superTabItem1;
            // 
            // superTabItem1
            // 
            this.superTabItem1.AttachedControl = this.superTabControlPanel1;
            this.superTabItem1.GlobalItem = false;
            this.superTabItem1.Name = "superTabItem1";
            this.superTabItem1.Text = "superTabItem1";
            // 
            // flpanEquipments
            // 
            this.flpanEquipments.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(82)))), ((int)(((byte)(89)))));
            this.flpanEquipments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpanEquipments.ForeColor = System.Drawing.Color.White;
            this.flpanEquipments.Location = new System.Drawing.Point(3, 985);
            this.flpanEquipments.Name = "flpanEquipments";
            this.flpanEquipments.Size = new System.Drawing.Size(1912, 28);
            this.flpanEquipments.TabIndex = 10;
            // 
            // timer_CurrentTime
            // 
            this.timer_CurrentTime.Enabled = true;
            this.timer_CurrentTime.Interval = 1000;
            this.timer_CurrentTime.Tick += new System.EventHandler(this.timer_CurrentTime_Tick);
            // 
            // timer_EquipmentStatus
            // 
            this.timer_EquipmentStatus.Interval = 30000;
            this.timer_EquipmentStatus.Tick += new System.EventHandler(this.timer_EquipmentStatus_Tick);
            // 
            // timer_MsgTime
            // 
            this.timer_MsgTime.Enabled = true;
            this.timer_MsgTime.Interval = 1000;
            this.timer_MsgTime.Tick += new System.EventHandler(this.timer_MsgTime_Tick);
            // 
            // FrmMainFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1918, 1038);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.metroStatusBar1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1918, 1038);
            this.Name = "FrmMainFrame";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "武汉博晟燃料集中管控系统";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.panelEx1.ResumeLayout(false);
            this.panelEx1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel_Buttons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.superTabControl1)).EndInit();
            this.superTabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Metro.MetroStatusBar metroStatusBar1;
        private DevComponents.DotNetBar.LabelItem labelItem1;
        private DevComponents.DotNetBar.LabelItem lblVersion;
        private DevComponents.DotNetBar.LabelItem lblLoginUserName;
        private DevComponents.DotNetBar.ButtonItem buttonItem2;
        private DevComponents.DotNetBar.LabelItem labelItem2;
        private DevComponents.DotNetBar.ButtonItem buttonItem1;
        private DevComponents.DotNetBar.ButtonX btnOpenMainPage;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private System.Windows.Forms.Label lblSystemName;
        private System.Windows.Forms.Label lblCurrentTime;
        private DevComponents.DotNetBar.ButtonX btnApplicationExit;
        private DevComponents.DotNetBar.ButtonX btnCZGK;
        private DevComponents.DotNetBar.ButtonX buttonX6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevComponents.DotNetBar.SuperTabControl superTabControl1;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel1;
        private DevComponents.DotNetBar.SuperTabItem superTabItem1;
        private DevComponents.DotNetBar.PanelEx panel_Buttons;
        private System.Windows.Forms.Timer timer_CurrentTime;
        private System.Windows.Forms.FlowLayoutPanel flpanEquipments;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Timer timer_EquipmentStatus;
        private System.Windows.Forms.Timer timer_MsgTime;
        private DevComponents.DotNetBar.ButtonItem btnOpenCarSampler;
        private DevComponents.DotNetBar.ButtonX btnOpenAssayManage;
        private DevComponents.DotNetBar.ButtonX btnCarMonitor;
        private DevComponents.DotNetBar.ButtonItem btnOpenBuyFuelLoad;
        private DevComponents.DotNetBar.ButtonX btnOpenEquInfHitch;
        private DevComponents.DotNetBar.ButtonX btnOpenGuardInfo;
        private DevComponents.DotNetBar.ButtonItem btnOpenTruckWeighter;
        private DevComponents.DotNetBar.ButtonItem btnOpenTrainBeltSampler;
        private DevComponents.DotNetBar.ButtonItem btnOpenAutoMaker;
        private DevComponents.DotNetBar.ButtonX btnSampleStorage;

    }
}

