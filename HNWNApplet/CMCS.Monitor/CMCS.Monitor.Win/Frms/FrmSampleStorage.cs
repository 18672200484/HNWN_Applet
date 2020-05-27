using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CMCS.Common;
using CMCS.Common.DAO;
using CMCS.Common.DapperDber_etc;
using CMCS.Common.Entities.Fuel;
using CMCS.Monitor.Win.CefGlue;
using CMCS.Monitor.Win.Core;
using CMCS.Monitor.Win.Html;
using CMCS.Monitor.Win.UserControls;
using CMCS.Monitor.Win.Utilities;
using DevComponents.DotNetBar;
using Xilium.CefGlue;
using Xilium.CefGlue.WindowsForms;

namespace CMCS.Monitor.Win.Frms
{
    public partial class FrmSampleStorage : DevComponents.DotNetBar.Metro.MetroForm
    {
        /// <summary>
        /// ����Ψһ��ʶ��
        /// </summary>
        public static string UniqueKey = "FrmSampleStorage";

        CommonDAO commonDAO = CommonDAO.GetInstance();
        MonitorCommon monitorCommon = MonitorCommon.GetInstance();
        CefWebBrowserEx cefWebBrowser = new CefWebBrowserEx();
        string LastCell = string.Empty;

        public FrmSampleStorage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// �����ʼ��
        /// </summary>
        private void FormInit()
        {
#if DEBUG
            gboxTest.Visible = true;
#else
            gboxTest.Visible = false;
#endif
            cefWebBrowser.StartUrl = SelfVars.Url_SampleStorage;
            cefWebBrowser.Dock = DockStyle.Fill;
            cefWebBrowser.WebClient = new HomePageCefWebClient(cefWebBrowser);
            cefWebBrowser.LoadEnd += new EventHandler<LoadEndEventArgs>(cefWebBrowser_LoadEnd);
            panWebBrower.Controls.Add(cefWebBrowser);
        }

        void cefWebBrowser_LoadEnd(object sender, LoadEndEventArgs e)
        {
            timer1.Enabled = true;

            RequestData();
        }

        private void FrmSampleStorage_Load(object sender, EventArgs e)
        {
            FormInit();
        }

        /// <summary>
        /// ���� - ˢ��ҳ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            cefWebBrowser.Browser.Reload();
        }

        /// <summary>
        /// ���� - ����ˢ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRequestData_Click(object sender, EventArgs e)
        {
            RequestData();
        }

        /// <summary>
        /// ��������
        /// </summary>
        void RequestData()
        {
            string cell = SelfVars.CurrentSelectedCell;
            List<HtmlDataItem> datas = new List<HtmlDataItem>();

            datas.Clear();

            IList<FuelSampleCell> cellList = commonDAO.SelfDber.Entities<FuelSampleCell>(String.Format("where BiaoShi='{0}' and IsDeleted=0", cell)).ToList();
            decimal useCount = GetUseCount();
            decimal emptyCount = cellList.Count - useCount;
            decimal usePercent = cellList.Count > 0 ? Math.Round(useCount / cellList.Count, 2, MidpointRounding.AwayFromZero) : 0;
            decimal overCount = GetOverCount();
            decimal overPercent = useCount > 0 ? Math.Round(overCount / useCount, 2, MidpointRounding.AwayFromZero) : 0;


            datas.Add(new HtmlDataItem("�ܲ�λ", cellList.Count.ToString(), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("�Ѵ��λ", useCount.ToString(), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("δ���λ", emptyCount.ToString(), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("���ڲ�λ", overCount.ToString(), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("������", (usePercent * 100).ToString("F2") + "%", eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("������", (overPercent * 100).ToString("F2") + "%", eHtmlDataItemType.svg_text));

            IList<RowResult> rowResult = new List<RowResult>();
            if (cellList.Count > 0)
            {
                int maxRow = cellList.OrderByDescending(a => a.Rowss).First().Rowss;
                int maxCell = cellList.OrderByDescending(a => a.Cell).First().Cell;
                for (int i = maxRow; i > 0; i--)
                {
                    RowResult rowEntity = new RowResult();
                    rowEntity.RowName = String.Format("�� {0} ��", i);
                    rowEntity.CellList = new List<CellResult>();
                    for (int j = 1; j <= maxCell; j++)
                    {
                        CellResult cellEntity = new CellResult();
                        FuelSampleCell sampleCell = cellList.Where(a => a.Rowss == i && a.Cell == j).FirstOrDefault();
                        if (sampleCell == null)
                            cellEntity.CellFlag = -1;
                        else
                        {
                            cellEntity.CellCode = sampleCell.CellCode;
                            //���ű�ʶ��-1û��һ��0�չ�1�Ѵ�ţ�2��������3ͣ��
                            if (sampleCell.IsValid == 1) cellEntity.CellFlag = 3;
                            else if (commonDAO.SelfDber.Count<FuelSampleInOut>(String.Format(" where isdeleted=0 and statue = 0 and samplecellid='{0}' and overtime < to_date('{1}','yyyy-MM-dd HH24:mi:ss')", sampleCell.Id
                                , DateTime.Now)) > 0)
                                cellEntity.CellFlag = 2;
                            else if ((commonDAO.SelfDber.Count<FuelSampleInOut>(String.Format(" where isdeleted=0 and statue = 0 and samplecellid='{0}' and overtime >= to_date('{1}','yyyy-MM-dd HH24:mi:ss')", sampleCell.Id
                                , DateTime.Now)) > 0))
                                cellEntity.CellFlag = 1;
                            else
                                cellEntity.CellFlag = 0;
                        }
                        rowEntity.CellList.Add(cellEntity);
                    }
                    rowResult.Add(rowEntity);
                }
            }

            datas.Add(new HtmlDataItem("������Ϣ", Newtonsoft.Json.JsonConvert.SerializeObject(rowResult), eHtmlDataItemType.json_data));


            // ��Ӹ���...

            // ���͵�ҳ��
            cefWebBrowser.Browser.GetMainFrame().ExecuteJavaScript("requestData(" + Newtonsoft.Json.JsonConvert.SerializeObject(datas) + ");", "", 0);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // ���治�ɼ�ʱ��ֹͣ��������
            if (!this.Visible) return;

            if (this.LastCell != SelfVars.CurrentSelectedCell)  //ѡ��������б仯ʱ��������
            {
                RequestData();
                this.LastCell = SelfVars.CurrentSelectedCell;
            }
            else if (DateTime.Now.Second % 5 == 0)  //5�����һ��
                RequestData();

        }

        #region ��������
        private Decimal ParseDecimal(object obj)
        {
            Decimal result = 0m;
            if (obj != null)
                Decimal.TryParse(obj.ToString(), out result);
            return result;
        }

        private decimal GetUseCount()
        {
            String sql = String.Format(@"select count(a.samplecellid) as useCount
  from fultbsampleinout a
  left join FULTBSampleCELL b on a.samplecellid = b.id
 where a.isdeleted = 0
   and a.statue = 0
   and b.biaoshi = '{0}'
   and b.isdeleted = 0
 group by a.samplecellid", SelfVars.CurrentSelectedCell);
            DataTable dt = commonDAO.SelfDber.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
                return ParseDecimal(dt.Rows[0][0]);
            else return 0;
        }

        private decimal GetOverCount()
        {
            String sql = String.Format(@"select count(a.samplecellid) as overCount
  from fultbsampleinout a
  left join FULTBSampleCELL b on a.samplecellid = b.id
 where a.isdeleted = 0
   and a.statue = 0
   and b.biaoshi = '{0}'
   and b.isdeleted = 0
   and a.overtime < to_date('{1}','yyyy-MM-dd HH24:mi:ss')
 group by a.samplecellid", SelfVars.CurrentSelectedCell, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            DataTable dt = commonDAO.SelfDber.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
                return ParseDecimal(dt.Rows[0][0]);
            else return 0;
        }
        #endregion
    }

    public class RowResult
    {
        /// <summary>
        /// �ڼ���
        /// </summary>
        public virtual String RowName { get; set; }
        public virtual IList<CellResult> CellList { get; set; }
    }
    public class CellResult
    {
        /// <summary>
        /// ���ű��
        /// </summary>
        public virtual String CellCode { get; set; }
        /// <summary>
        /// ���ű�ʶ��-1û��һ��0�չ�1�Ѵ�ţ�2��������3ͣ��
        /// </summary>
        public virtual Int32 CellFlag { get; set; }
    }
}