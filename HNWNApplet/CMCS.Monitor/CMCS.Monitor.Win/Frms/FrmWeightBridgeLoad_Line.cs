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
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;
using CMCS.Common.Entities.TrainInFactory;

namespace CMCS.Monitor.Win.Frms
{
    public partial class FrmWeightBridgeLoad_Line : MetroForm
    {
        public FrmWeightBridgeLoad_Line()
        {
            InitializeComponent();
        }
        String Id;
        public FrmWeightBridgeLoad_Line(String pId)
        {
            Id = pId;
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


        private void FrmWeightBridgeLoad_Line_Load(object sender, EventArgs e)
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
                    while (chart1.Titles.Count == 0)
                    {
                        chart1.Titles.Add(new Title());
                    }
                    chart1.Titles[0].Text = "平均值：" + (sums / nums).ToString("F2");
                    chart1.Titles[0].Visible = true;
                   
                }
            }
        }

        public class CmcsTrainWeightRecordTemp : CmcsTrainWeightRecord
        {
            public int TrueNumber { get; set; }
        }

        public void BindData()
        {
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region Pager

        private void btnPagerCommand_Click(object sender, EventArgs e)
        {
            ButtonX btn = sender as ButtonX;
            switch (btn.CommandParameter.ToString())
            {
                case "First":
                    CurrentIndex = 0;
                    break;
                case "Previous":
                    CurrentIndex = CurrentIndex - 1;
                    break;
                case "Next":
                    CurrentIndex = CurrentIndex + 1;
                    break;
                case "Last":
                    CurrentIndex = PageCount - 1;
                    break;
            }

            BindData();
        }

        public void PagerControlStatue()
        {
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
    }
}
