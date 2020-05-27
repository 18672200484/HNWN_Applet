using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities.CarTransport;
using System.Drawing;
using System.Drawing.Printing;
using CMCS.Common.Entities.BaseInfo;

namespace CMCS.Common.Utilities
{
    public class PrintWeightReport
    {
        private static PrintWeightReport instance;

        public static PrintWeightReport GetInstance(PrintDocument printDoc)
        {
            if (instance == null)
            {
                instance = new PrintWeightReport(printDoc);
            }

            return instance;
        }

        #region Vars

        Font TitleFont = new Font("宋体", 24, FontStyle.Bold, GraphicsUnit.Pixel);

        Font ContentFont = new Font("宋体", 14, FontStyle.Regular, GraphicsUnit.Pixel);

        PrintDocument _PrintDocument = null;

        CmcsBuyFuelTransport _BuyFuelTransport = null;

        int PageIndex = 1;

        Graphics gs = null;

        #endregion

        public PrintWeightReport(PrintDocument printDoc)
        {
            this._PrintDocument = printDoc;
            this._PrintDocument.DefaultPageSettings.PaperSize = new PaperSize("Custum", 850, 368);
            this._PrintDocument.OriginAtMargins = true;
            this._PrintDocument.DefaultPageSettings.Margins.Left = 10;
            this._PrintDocument.DefaultPageSettings.Margins.Right = 0;
            this._PrintDocument.DefaultPageSettings.Margins.Top = 0;
            this._PrintDocument.DefaultPageSettings.Margins.Bottom = 0;
            this._PrintDocument.PrintController = new StandardPrintController();
            this._PrintDocument.PrintPage += _PrintDocument_PrintPage;
        }

        public void Print(CmcsBuyFuelTransport buyfueltransport)
        {
            if (buyfueltransport == null) return;
            _BuyFuelTransport = buyfueltransport;
            try
            {
                this._PrintDocument.Print();
            }
            catch (Exception ex)
            {
                Log4Neter.Error("打印异常，请检查打印机！", ex);
            }
            _BuyFuelTransport = null;
        }

        private void _PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {

            Graphics g = e.Graphics;
            if (this.gs != null)
                g = this.gs;

            string SerialNumber = string.Empty,
                    CarNumber = string.Empty,
                    MineName = string.Empty,
                    SupplierName = string.Empty,
                    GrossTime = string.Empty,
                    TareTime = string.Empty,
                    TicketWeight = string.Empty,
                    GrossWeight = string.Empty,
                    TareWeight = string.Empty,
                    ProfitAndLossWeight = string.Empty,
                    SuttleWeight = string.Empty,
                    DeductWeight = string.Empty,
                    UserName = string.Empty,
                    TransportCompany = string.Empty,
                    FuelKindName = string.Empty,
                    PoundsWorse = string.Empty,
                    JingZhongChaoCha = string.Empty;

            if (this._BuyFuelTransport != null)
            {
                if (_BuyFuelTransport.SupplierId != null)
                {
                    CmcsSupplier supplier = Dbers.GetInstance().SelfDber.Get<CmcsSupplier>(_BuyFuelTransport.SupplierId);
                    if (supplier != null)
                        SupplierName = supplier.Name;
                }
                if (_BuyFuelTransport.MineId != null)
                {
                    CmcsMine mine = Dbers.GetInstance().SelfDber.Get<CmcsMine>(_BuyFuelTransport.MineId);
                    if (mine != null)
                        MineName = mine.Name;
                }
                if (_BuyFuelTransport.FuelKindId != null)
                {
                    CmcsFuelKind fuelkind = Dbers.GetInstance().SelfDber.Get<CmcsFuelKind>(_BuyFuelTransport.FuelKindId);
                    if (fuelkind != null)
                        FuelKindName = fuelkind.Name;
                }
                SerialNumber = this._BuyFuelTransport.SerialNumber;
                CarNumber = this._BuyFuelTransport.CarNumber;
                GrossTime = DisposeTime(this._BuyFuelTransport.GrossTime.ToString(), "yyyy-MM-dd HH:mm");
                TareTime = DisposeTime(this._BuyFuelTransport.TareTime.ToString(), "yyyy-MM-dd HH:mm");
                TicketWeight = this._BuyFuelTransport.TicketWeight.ToString("F2").PadLeft(6, ' ');
                GrossWeight = this._BuyFuelTransport.GrossWeight.ToString("F2").PadLeft(6, ' ');
                TareWeight = this._BuyFuelTransport.TareWeight.ToString("F2").PadLeft(6, ' ');
                SuttleWeight = this._BuyFuelTransport.SuttleWeight.ToString("F2").PadLeft(6, ' ');
                DeductWeight = this._BuyFuelTransport.DeductWeight.ToString("F2").PadLeft(6, ' ');
                TicketWeight = this._BuyFuelTransport.TicketWeight.ToString("F2").PadLeft(6, ' ');

                #region 入厂煤
                // 行间距 24 
                float TopValue = 20;
                string printValue = "";
                g.DrawString("华能渭南汽车过磅单", new Font("黑体", 18, FontStyle.Bold, GraphicsUnit.Pixel), Brushes.Black, 15, TopValue);
                TopValue += 34;

                g.DrawString("打印时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm"), ContentFont, Brushes.Black, 30, TopValue);
                TopValue += 24;

                g.DrawLine(new Pen(Color.Black, 2), 0, TopValue, 300 - 10, TopValue);
                TopValue += 15;

                g.DrawString("单    号：" + SerialNumber, ContentFont, Brushes.Black, 30, TopValue);
                TopValue += 24;

                g.DrawString("车 牌 号：" + CarNumber, ContentFont, Brushes.Black, 30, TopValue);
                TopValue += 24;

                g.DrawString("矿    点：" + MineName, ContentFont, Brushes.Black, 30, TopValue);
                TopValue += 24;

                g.DrawString("供货单位：", ContentFont, Brushes.Black, 30, TopValue);
                printValue = SupplierName != null ? SupplierName : string.Empty;

                if (printValue.Length > 11)
                {
                    g.DrawString(printValue.Substring(0, 11), ContentFont, Brushes.Black, 105, TopValue);
                    TopValue += 24;
                    g.DrawString(printValue.Substring(11, printValue.Length - 11), ContentFont, Brushes.Black, 105, TopValue);
                    TopValue += 24;
                }
                else
                {
                    g.DrawString(printValue, ContentFont, Brushes.Black, 102, TopValue);
                    TopValue += 24;
                }

                g.DrawString("货物名称：" + FuelKindName, ContentFont, Brushes.Black, 30, TopValue);
                TopValue += 24;

                g.DrawString("毛重时间：" + GrossTime, ContentFont, Brushes.Black, 30, TopValue);
                TopValue += 24;

                g.DrawString("皮重时间：" + TareTime, ContentFont, Brushes.Black, 30, TopValue);
                TopValue += 24;

                g.DrawString(string.Format("矿发净重：{0} 吨", TicketWeight), ContentFont, Brushes.Black, 30, TopValue);
                TopValue += 24;

                g.DrawString(string.Format("毛    重：{0} 吨", GrossWeight), ContentFont, Brushes.Black, 30, TopValue);
                TopValue += 24;

                g.DrawString(string.Format("皮    重：{0} 吨", TareWeight), ContentFont, Brushes.Black, 30, TopValue);
                TopValue += 24;

                g.DrawString(string.Format("净    量：{0} 吨", SuttleWeight), ContentFont, Brushes.Black, 30, TopValue);
                TopValue += 24;

                g.DrawString(string.Format("扣    吨：{0} 吨", DeductWeight), ContentFont, Brushes.Black, 30, TopValue);
                TopValue += 24;

                //g.DrawString(string.Format("验 收 量：{0} 吨", CheckWeight), ContentFont, Brushes.Black, 30, TopValue);
                //TopValue += 24;

                TopValue += 24;

                #endregion
            }
        }

        public static string DisposeTime(string dt, string format)
        {
            if (!string.IsNullOrEmpty(dt))
            {
                DateTime dti = DateTime.Parse(dt);
                if (dti > new DateTime(2000, 1, 1))
                    return dti.ToString(format);
            }
            return string.Empty;
        }

    }
}
