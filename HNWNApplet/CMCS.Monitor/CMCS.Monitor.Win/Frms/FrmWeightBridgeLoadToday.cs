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
using CMCS.Monitor.Win.Frms.Sys;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;
using CMCS.Common.Entities.TrainInFactory;
using CMCS.Common.Utilities;

namespace CMCS.Monitor.Win.Frms
{
    public partial class FrmWeightBridgeLoadToday : MetroForm
    {
        /// <summary>
        /// 窗体唯一标识符
        /// </summary>
        public static string UniqueKey = "FrmWeightBridgeLoadToday";

        public FrmWeightBridgeLoadToday()
        {
            InitializeComponent();
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
        string Id;


        private void FrmWeightBridge_Load(object sender, EventArgs e)
        {
            InitForm();
            BindData();
        }

        /// <summary>
        /// 窗体初始化
        /// </summary>
        private void InitForm()
        {
        }

        public class CmcsTrainWeightRecordTemp : CmcsTrainWeightRecord
        {
            public int TrueNumber { get; set; }
        }

        public void BindData()
        {
            string tempSqlWhere = this.SqlWhere;
            List<CmcsTrainWeightRecord> list = Dbers.GetInstance().SelfDber.Entities<CmcsTrainWeightRecord>(" where GrossTime>=to_date('" + DateTime.Now.ToString("yyyy-MM-dd") + "','yyyy-mm-dd') order by OrderNumber desc");

            superGridControl1.PrimaryGrid.DataSource = list;
            if (list.Count > 0)
            {
                Id = list[0].Id;
            }
            createinfo();

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            FrmWeightBridgeLoad frm = new FrmWeightBridgeLoad();
            FrmMainFrame.superTabControlManager.CreateTab(frm.Text, frm.Text, frm, true);
        }

        #region Pager

        private void btnPagerCommand_Click(object sender, EventArgs e)
        {
            BindData();
        }


        private void GetTotalCount(string sqlWhere)
        {
            TotalCount = Dbers.GetInstance().SelfDber.Count<CmcsTrainWeightRecord>(sqlWhere);
            if (TotalCount % PageSize != 0)
                PageCount = TotalCount / PageSize + 1;
            else
                PageCount = TotalCount / PageSize;
        }
        #endregion

        #region SuperGridControl

        private void superGridControl1_GetRowHeaderText(object sender, DevComponents.DotNetBar.SuperGrid.GridGetRowHeaderTextEventArgs e)
        {
            e.Text = ((this.CurrentIndex * this.PageSize) + e.GridRow.RowIndex + 1).ToString();
        }

        private void superGridControl1_BeginEdit(object sender, GridEditEventArgs e)
        {
            // 取消编辑
            e.Cancel = true;
        }

        #endregion


        private void superGridControl1_CellMouseDown(object sender, GridCellMouseEventArgs e)
        {
            if (e.GridCell.GridRow.Index == -1)
                return;
            e.GridCell.GridRow.IsSelected = true;
            SuperGridControl supergridcontrol = (SuperGridControl)sender;
            Id = supergridcontrol.GetCell(e.GridCell.GridRow.RowIndex, 11).Value.ToString();
            createinfo();
        }

        void createinfo()
        {

            if (true)
            {
                List<CmcsTrainLine> cmcstrainlines = Dbers.GetInstance().SelfDber.Entities<CmcsTrainLine>(String.Format(" where TrainWeightRecordId='{0}'", Id));
                if (cmcstrainlines != null && cmcstrainlines.Count > 0)
                {
                    CmcsTrainLine cmcstrainline = cmcstrainlines[0] as CmcsTrainLine;
                    string[] datas = cmcstrainline.Height.Split('|');
                    List<String> datalist = new List<String>();
                    Series s1 = new Series("default");
                    s1.ChartType = SeriesChartType.SplineArea;
                    chart1.Series[0] = (s1);
                    if (cmcstrainline.StartTime <= cmcstrainline.EndTime)
                    {
                        datalist = datas.ToList();
                    }
                    else
                    {
                        datalist = datas.Reverse().ToList();
                    }
                    int nums = 0;
                    Double sums = 0;
                    chart1.ChartAreas[0].AxisY.Maximum = 6;
                    foreach (String item in datalist)
                    {
                        Double resvalue = 0;
                        if (Double.TryParse(item, out resvalue))
                        {

                            s1.Points.Add(resvalue);
                            nums++;
                            sums += resvalue;
                            if (chart1.ChartAreas[0].AxisY.Maximum < resvalue + 1)
                            {
                                chart1.ChartAreas[0].AxisY.Maximum = resvalue + 1;
                            }
                        }
                        if (s1.Points.Count > 1)
                        {
                            s1.Points[0].AxisLabel = "车头";
                            s1.Points[s1.Points.Count - 1].AxisLabel = "车尾";
                        }
                        s1.Points[s1.Points.Count - 1].LegendToolTip = resvalue.ToString("f2");
                        chart1.ChartAreas[0].AxisX.Interval = s1.Points.Count - 1;
                        chart1.ChartAreas[0].AxisY.Interval = 1;
                    }
                    while (chart1.Legends.Count > 0)
                        chart1.Legends.RemoveAt(0);
                    //    chart1.ChartAreas[0].AxisX.IsStartedFromZero = true;
                    chart1.ChartAreas[0].AxisX.Minimum = 1;
                    chart1.ChartAreas[0].AxisX.Maximum = s1.Points.Count;
                    chart1.ChartAreas[0].BackSecondaryColor = Color.FromArgb(0x2B2E33);
                    chart1.ChartAreas[0].BackColor = Color.FromArgb(0x2B2E33);

                    chart1.BackColor = Color.FromArgb(0x2B2E33);
                    while (chart1.Titles.Count > 1)
                        chart1.Titles.RemoveAt(0);
                    while (chart1.Titles.Count == 0)
                        chart1.Titles.Add(new Title());
                    chart1.Titles[0].Text = "平均值：" + (sums / nums).ToString("F2");
                    chart1.Titles[0].Visible = true;

                }
                else
                {
                    string[] datas = "0|0".Split('|');
                    List<String> datalist = new List<String>();
                    Series s1 = new Series("default");
                    s1.ChartType = SeriesChartType.SplineArea;
                    chart1.Series[0] = (s1);
                    datalist = datas.ToList();
                    int nums = 0;
                    Double sums = 0;
                    chart1.ChartAreas[0].AxisY.Maximum = 6;
                    foreach (String item in datalist)
                    {
                        Double resvalue = 0;
                        if (Double.TryParse(item, out resvalue))
                        {

                            s1.Points.Add(resvalue);
                            nums++;
                            sums += resvalue;
                            if (chart1.ChartAreas[0].AxisY.Maximum < resvalue + 1)
                            {
                                chart1.ChartAreas[0].AxisY.Maximum = resvalue + 1;
                            }
                        }
                        if (s1.Points.Count > 1)
                        {
                            s1.Points[0].AxisLabel = "车头";
                            s1.Points[s1.Points.Count - 1].AxisLabel = "车尾";
                        }
                        s1.Points[s1.Points.Count - 1].LegendToolTip = resvalue.ToString("f2");
                        chart1.ChartAreas[0].AxisX.Interval = s1.Points.Count - 1;
                        chart1.ChartAreas[0].AxisY.Interval = 1;
                    }
                    while (chart1.Legends.Count > 0)
                        chart1.Legends.RemoveAt(0);
                    //    chart1.ChartAreas[0].AxisX.IsStartedFromZero = true;
                    chart1.ChartAreas[0].AxisX.Minimum = 1;
                    chart1.ChartAreas[0].AxisX.Maximum = s1.Points.Count;
                    chart1.ChartAreas[0].BackSecondaryColor = Color.FromArgb(0x2B2E33);
                    chart1.ChartAreas[0].BackColor = Color.FromArgb(0x2B2E33);

                    chart1.BackColor = Color.FromArgb(0x2B2E33);
                    //chart1.Titles[0].Text = "平均值：" + (sums / nums).ToString("F2");
                    //chart1.Titles[0].Visible = true;

                    while (chart1.Titles.Count > 1)
                        chart1.Titles.RemoveAt(0);
                    while (chart1.Titles.Count == 0)
                        chart1.Titles.Add(new Title());
                    chart1.Titles[0].Text = "平均值：" + (sums / nums).ToString("F2");
                    chart1.Titles[0].Visible = true;
                }
                //chart1.ChartAreas[0].AxisY.LineColor = Color.White;
                //chart1.ChartAreas[0].AxisX.LineColor = Color.White;
                //chart1.ChartAreas[0].AxisY.InterlacedColor = Color.White;
                //chart1.ChartAreas[0].AxisX.InterlacedColor = Color.White;
                //chart1.ChartAreas[0].AxisY.TitleForeColor = Color.White;
                //chart1.ChartAreas[0].AxisX.TitleForeColor = Color.White;
                //chart1.ChartAreas[0].AxisY2.LineColor = Color.White;
                //chart1.ChartAreas[0].AxisX2.LineColor = Color.White;
                //chart1.ChartAreas[0].AxisY2.InterlacedColor = Color.White;
                //chart1.ChartAreas[0].AxisX2.InterlacedColor = Color.White;
                //chart1.ChartAreas[0].AxisY2.TitleForeColor = Color.White;
                //chart1.ChartAreas[0].AxisX2.TitleForeColor = Color.White;

            }
            if (true)
            {
                List<CmcsTrainWatch> cmcstrainwatchs = Dbers.GetInstance().SelfDber.Entities<CmcsTrainWatch>(String.Format(" where TrainWeightRecordId='{0}'", Id));
                if (cmcstrainwatchs != null && cmcstrainwatchs.Count > 0)
                {
                    newcmcstrainwatchs = cmcstrainwatchs;
                    if (cmcstrainwatchs != null && cmcstrainwatchs.Count > 1)
                    {
                        buttonX1.Enabled = true;
                        buttonX2.Enabled = true;
                    }
                    else
                    {
                        buttonX1.Enabled = false;
                        buttonX2.Enabled = false;
                    }
                }
                else
                {
                    newcmcstrainwatchs = null;
                    pictureBox1.Image = null;
                    buttonX1.Enabled = false;
                    buttonX2.Enabled = false;
                }

                change();
            }
        }
        List<CmcsTrainWatch> newcmcstrainwatchs;
        int SelectedIndex = 0;
        void change()
        { 
            if (newcmcstrainwatchs != null && newcmcstrainwatchs.Count != 0)
            {
                label1.Text = "车号:" + newcmcstrainwatchs[SelectedIndex].TheTrainWeightRecord.TrainNumber + " 位置:" + newcmcstrainwatchs[SelectedIndex].CatchType + " 时间:" + newcmcstrainwatchs[SelectedIndex].CatchTime.ToString("yyyy-MM-dd HH:mm:ss");

                try
                {
                    Image image = Image.FromStream(System.Net.WebRequest.Create(newcmcstrainwatchs[SelectedIndex].CatchDest).GetResponse().GetResponseStream());
                    Image newimage = pictureBox1.Image;
                    Size _size = new Size(pictureBox1.Width, pictureBox1.Height);
                    Bitmap _image = new Bitmap(image, _size);
                    pictureBox1.Image = _image;
                }
                catch (Exception ex)
                {
                    Log4Neter.Error("获取火车入厂抓拍照片", ex);
                }
            }
            else
            {
                CmcsTrainWeightRecord cmcstrainweightrecord = Dbers.GetInstance().SelfDber.Get<CmcsTrainWeightRecord>(Id);
                if (cmcstrainweightrecord != null)
                    label1.Text = "车号:" + cmcstrainweightrecord.TrainNumber + " 位置: 时间:-------------------";
                else
                    label1.Text = "车号:------- 位置: 时间:-------------------";

            }
        }
        private void superGridControl1_RowHeaderClick(object sender, GridRowHeaderClickEventArgs e)
        {
            if (e.GridRow.Index == -1)
                return;
            e.GridRow.IsSelected = true;
            SuperGridControl supergridcontrol = (SuperGridControl)sender;
            Id = supergridcontrol.GetCell(e.GridRow.RowIndex, 11).Value.ToString();
            createinfo();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (Dbers.GetInstance().SelfDber.Entities<CmcsTrainWatch>(String.Format(" where TrainWeightRecordId='{0}'", Id)).Count > 0)
            {
                FrmWeightBridgeLoad_Pic pic = new FrmWeightBridgeLoad_Pic(Id);
                pic.ShowDialog();
            }
        }

        private void BtnClick(object sender, EventArgs e)
        {
            if (newcmcstrainwatchs != null)
            {
                if (((ButtonX)sender).Text == "下一张")
                {
                    if (SelectedIndex == newcmcstrainwatchs.Count - 1)
                    {
                        SelectedIndex = 0;
                    }
                    else
                    {
                        SelectedIndex++;
                    }
                }
                else if ((((ButtonX)sender).Text == "上一张"))
                {
                    if (SelectedIndex == 0)
                    {
                        SelectedIndex = newcmcstrainwatchs.Count - 1;
                    }
                    else
                    {
                        SelectedIndex--;
                    }
                }
            }
            change();
        }


        private void FrmWeightBridgeLoadToday_Shown(object sender, EventArgs e)
        {
            change();
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            BindData();
        }
    }
}
