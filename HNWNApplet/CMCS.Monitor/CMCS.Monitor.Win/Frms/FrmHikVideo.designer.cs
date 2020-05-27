namespace CMCS.Monitor.Win.Frms
{
    partial class FrmHikVideo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmHikVideo));
            this.panVideo1 = new DevComponents.DotNetBar.PanelEx();
            this.SuspendLayout();
            // 
            // panVideo1
            // 
            this.panVideo1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panVideo1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panVideo1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panVideo1.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panVideo1.Location = new System.Drawing.Point(0, 0);
            this.panVideo1.Name = "panVideo1";
            this.panVideo1.Size = new System.Drawing.Size(706, 474);
            this.panVideo1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panVideo1.Style.BackColor1.Color = System.Drawing.Color.Black;
            this.panVideo1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panVideo1.Style.BorderColor.Color = System.Drawing.Color.White;
            this.panVideo1.Style.ForeColor.Color = System.Drawing.Color.White;
            this.panVideo1.Style.GradientAngle = 90;
            this.panVideo1.TabIndex = 1;
            this.panVideo1.Text = "视频一";
            // 
            // FrmHikVideo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(706, 474);
            this.Controls.Add(this.panVideo1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmHikVideo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "视频预览";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmHikVideo_FormClosing);
            this.Load += new System.EventHandler(this.FrmHikVideo_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panVideo1;
    }
}