using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using CMCS.Common.DAO;
using CMCS.Common.Entities.CarTransport;
using CMCS.Common.Utilities;
using Microsoft.Reporting.WinForms;

namespace CMCS.CarTransport.Queue
{
    public class ReportPrint
    {
        private static ReportPrint instance;

        public static ReportPrint GetInstance()
        {
            if (instance == null)
            {
                instance = new ReportPrint();
            }

            return instance;
        }

        //声明一个Stream对象的列表用来保存报表的输出数据
        //LocalReport对象的Render方法会将报表按页输出为多个Stream对象。
        private IList<Stream> m_streams;

        //用来记录当前打印到第几页了
        private int m_currentPageIndex;

        /// <summary>
        /// 打印入厂煤磅单
        /// </summary>
        /// <param name="entity"></param>
        public void PrintBuyFuelTransport(CmcsBuyFuelTransport entity)
        {
            #region 备份
            DataTable table = new DataTable();
            table.Columns.Add("SerialNumber", typeof(string));
            table.Columns.Add("TareTime", typeof(string));
            table.Columns.Add("CarNumber", typeof(string));
            table.Columns.Add("FuelKindName", typeof(string));
            table.Columns.Add("SupplierName", typeof(string));
            table.Columns.Add("Remark", typeof(string));
            table.Columns.Add("GrossWeight", typeof(string));
            table.Columns.Add("TareWeight", typeof(string));
            table.Columns.Add("SuttleWeight", typeof(string));
            table.Columns.Add("DeductWeight", typeof(string));
            table.Columns.Add("CreateUser", typeof(string));
            table.Columns.Add("Type", typeof(string));

            table.Rows.Add(entity.SerialNumber, entity.TareTime.ToString("yyyy年MM月dd日 HH:mm:ss"), entity.CarNumber, entity.FuelKindName, entity.SupplierName, "华能渭南",
                 entity.GrossWeight, entity.TareWeight, entity.SuttleWeight, entity.DeductWeight, CommonDAO.GetInstance().GetNameByAccount(entity.CreateUser));

            LocalReport report = new LocalReport();
            //设置需要打印的报表的文件名称。
            report.ReportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TransportReport.rdlc");

            //创建要打印的数据源
            ReportDataSource source = new ReportDataSource();
            source.Name = "Transport";
            source.Value = table;

            report.DataSources.Add(source);
            ////刷新报表中的需要呈现的数据
            //report.Refresh();

            PrintDocument(source, report);

            Dispose();

            report.Dispose();

            #endregion
        }

        public void PrintGoodsTransport(CmcsGoodsTransport entity)
        {
            #region 备份
            DataTable table = new DataTable();
            table.Columns.Add("SerialNumber", typeof(string));
            table.Columns.Add("TareTime", typeof(string));
            table.Columns.Add("CarNumber", typeof(string));
            table.Columns.Add("FuelKindName", typeof(string));
            table.Columns.Add("SupplierName", typeof(string));
            table.Columns.Add("Remark", typeof(string));
            table.Columns.Add("GrossWeight", typeof(string));
            table.Columns.Add("TareWeight", typeof(string));
            table.Columns.Add("SuttleWeight", typeof(string));
            table.Columns.Add("DeductWeight", typeof(string));
            table.Columns.Add("CreateUser", typeof(string));
            table.Columns.Add("Type", typeof(string));

            decimal GrossWeight = entity.FirstWeight, TareWeight = entity.SecondWeight;
            if (entity.FirstWeight < entity.SecondWeight)
            {
                GrossWeight = entity.SecondWeight;
                TareWeight = entity.FirstWeight;
            }

            table.Rows.Add(entity.SerialNumber, entity.SecondTime.ToString("yyyy年MM月dd日 HH:mm"), entity.CarNumber, entity.GoodsTypeName, entity.SupplyUnitName, entity.ReceiveUnitName,
                 GrossWeight, TareWeight, entity.SuttleWeight, 0, CommonDAO.GetInstance().GetNameByAccount(entity.CreateUser));

            LocalReport report = new LocalReport();
            //设置需要打印的报表的文件名称。
            report.ReportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TransportReport.rdlc");

            //创建要打印的数据源
            ReportDataSource source = new ReportDataSource();
            source.Name = "Transport";
            source.Value = table;

            report.DataSources.Add(source);
            ////刷新报表中的需要呈现的数据
            //report.Refresh();

            PrintDocument(source, report);

            Dispose();

            report.Dispose();

            #endregion
        }

        private void PrintDocument(ReportDataSource source, LocalReport report)
        {
            Warning[] warnings;
            m_streams = new List<Stream>();

            //这里是设置打印的格式 边距什么的
            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>EMF</OutputFormat>" +
                //"  <PageWidth>24cm</PageWidth>" +
                //"  <PageHeight>10cm</PageHeight>" +
                //"  <MarginTop>0cm</MarginTop>" +
                //"  <MarginLeft>0.5cm</MarginLeft>" +
                //"  <MarginRight>0.5cm</MarginRight>" +
                //"  <MarginBottom>0.5cm</MarginBottom>" +
            "</DeviceInfo>";

            report.Render("Image", deviceInfo, CreateStream, out warnings);

            m_currentPageIndex = 0;

            if (m_streams == null || m_streams.Count == 0)
                return;

            //声明PrintDocument对象用于数据的打印
            PrintDocument printDoc = new PrintDocument();
            printDoc.DefaultPageSettings.PaperSize = new PaperSize("Custum", 900, 360);

            //声明PrintDocument对象的PrintPage事件，具体的打印操作需要在这个事件中处理。
            printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
            //执行打印操作，Print方法将触发PrintPage事件。
            printDoc.Print();
        }

        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            //Metafile对象用来保存EMF或WMF格式的图形，
            //我们在前面将报表的内容输出为EMF图形格式的数据流。
            m_streams[m_currentPageIndex].Position = 0;

            Metafile pageImage = new Metafile(m_streams[m_currentPageIndex]);

            //指定是否横向打印
            ev.PageSettings.Landscape = false;
            //这里的Graphics对象实际指向了打印机
            //ev.Graphics.DrawImage(pageImage, ev.PageBounds, 0, 0, ev.PageBounds.Width - 80, ev.PageBounds.Height, System.Drawing.GraphicsUnit.Millimeter);

            //此写法不受分辨率影响
            System.Drawing.Rectangle adjustedRect = new System.Drawing.Rectangle(
                ev.PageBounds.Left - (int)ev.PageSettings.HardMarginX,
                ev.PageBounds.Top - (int)ev.PageSettings.HardMarginY,
                ev.PageBounds.Width,
                ev.PageBounds.Height);
            ev.Graphics.DrawImage(pageImage, adjustedRect);

            m_streams[m_currentPageIndex].Close();
            m_currentPageIndex++;
            //设置是否需要继续打印
            ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
        }

        //用来提供Stream对象的函数，用于LocalReport对象的Render方法的第三个参数。
        private Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        {
            //如果需要将报表输出的数据保存为文件，请使用FileStream对象。
            Stream stream = new MemoryStream();
            m_streams.Add(stream);
            return stream;
        }

        public void Dispose()
        {
            if (m_streams != null)
            {
                foreach (Stream stream in m_streams)
                {
                    stream.Dispose();
                    stream.Close();
                }
                m_streams = null;
            }
        }
    }
}
