using System;
using System.Collections.Generic;
using System.Windows.Forms;
//
using CMCS.Common.Entities;
using CMCS.Common;
using DevComponents.DotNetBar;
using System.Linq;
using DevComponents.DotNetBar.SuperGrid;
using DevComponents.DotNetBar.Metro;
using System.Drawing;
using CMCS.Common.Entities.TrainInFactory;
using CMCS.Common.Utilities;

namespace CMCS.Monitor.Win.Frms
{
    public partial class FrmWeightBridgeLoad_Pic : MetroForm
    {
        public FrmWeightBridgeLoad_Pic()
        {
            InitializeComponent();
        }
        String Id;
        public FrmWeightBridgeLoad_Pic(String pId)
        {
            InitializeComponent();
            Id = pId;
        }
        /// <summary>
        /// 每页显示行数
        /// </summary>
        int PageSize = 28;

        /// <summary>
        /// 总页数
        /// </summary>
        int PageCount = 0;

        /// <summary>
        /// 总记录数
        /// </summary>
        int TotalCount = 0;

        /// <summary>
        /// 当前页索引
        /// </summary>
        int CurrentIndex = 0;

        string SqlWhere = string.Empty;

        List<CmcsTrainWatch> newcmcstrainwatchs;


        private void FrmWeightBridgeLoad_Pic_Load(object sender, EventArgs e)
        {
            InitForm();

        }

        /// <summary>
        /// 窗体初始化
        /// </summary>
        private void InitForm()
        {
            if (!String.IsNullOrEmpty(Id))
            {
                List<CmcsTrainWatch> cmcstrainwatchs = Dbers.GetInstance().SelfDber.Entities<CmcsTrainWatch>(String.Format(" where TrainWeightRecordId='{0}'", Id));
                if (cmcstrainwatchs != null && cmcstrainwatchs.Count > 0)
                {
                    newcmcstrainwatchs = cmcstrainwatchs;
                    foreach (var item in cmcstrainwatchs)
                    {
                        ddlCatchType.Items.Add(item.CatchType);
                    }
                    ddlCatchType.SelectedIndex = 0;
                }
            }
        }

        public class CmcsTrainWeightRecordTemp : CmcsTrainWeightRecord
        {
            public int TrueNumber { get; set; }
        }

        public void BindData()
        {
            this.Close();
        }

        #region Pager
        #endregion

        #region SuperGridControl
        #endregion

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void panel_Click(object sender, EventArgs e)
        {
            Panel bt = (Panel)sender;
            if (bt.Name == "panelLeft")
            {
                if (ddlCatchType.SelectedIndex == 0)
                {
                    ddlCatchType.SelectedIndex = ddlCatchType.Items.Count - 1;
                }
                else
                {
                    ddlCatchType.SelectedIndex--;
                }
            }
            else
            {
                if (ddlCatchType.SelectedIndex == ddlCatchType.Items.Count - 1)
                {
                    ddlCatchType.SelectedIndex = 0;
                }
                else
                {
                    ddlCatchType.SelectedIndex++;
                }
            }
        }

        private void ddlCatchType_SelectedValueChanged(object sender, EventArgs e)
        {
            change();
        }

        void change()
        {
            if (newcmcstrainwatchs.Count > 0)
            {
                label1.Text = "车号：" + newcmcstrainwatchs[ddlCatchType.SelectedIndex].TheTrainWeightRecord.TrainNumber + "     时间:" + newcmcstrainwatchs[ddlCatchType.SelectedIndex].CatchTime.ToString("yyyy-MM-dd HH:mm:ss");

                try
                {
                    Image image = Image.FromStream(System.Net.WebRequest.Create(newcmcstrainwatchs[ddlCatchType.SelectedIndex].CatchDest).GetResponse().GetResponseStream());
                    Image newimage = pictureBox1.Image;
                    Size _size = new Size(pictureBox1.Width, pictureBox1.Height);
                    Bitmap _image = new Bitmap(image, _size);
                    pictureBox1.Image = _image;
                    pictureBox2.Image = _image;
                }
                catch (Exception ex)
                {
                    Log4Neter.Error("获取火车入厂抓拍照片", ex);
                }
            }
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            if (pb.Name == "pictureBox1")
            {
                if (ddlCatchType.SelectedIndex == ddlCatchType.Items.Count - 1)
                {
                    ddlCatchType.SelectedIndex = 0;
                }
                else
                {
                    ddlCatchType.SelectedIndex++;
                }
            }
            else
            {
                if (ddlCatchType.SelectedIndex == 0)
                {
                    ddlCatchType.SelectedIndex = ddlCatchType.Items.Count - 1;
                }
                else
                {
                    ddlCatchType.SelectedIndex--;
                }
            }
            change();
        }

        private void FrmWeightBridgeLoad_Pic_Shown(object sender, EventArgs e)
        {
            pictureBox2.Size = new Size(pictureBox1.Size.Width / 2, pictureBox1.Size.Height);
        }
    }
}
