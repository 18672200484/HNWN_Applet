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
using CMCS.Common.Entities;
using CMCS.Common.Entities.Inf;
using CMCS.Common.Enums;
using CMCS.Common.Utilities;
using CMCS.Monitor.DAO;
using CMCS.Monitor.Win.Core;
using CMCS.Monitor.Win.Html;
using CMCS.Monitor.Win.UserControls;
using DevComponents.DotNetBar.Metro;
using Xilium.CefGlue;
using Xilium.CefGlue.WindowsForms;

namespace CMCS.Monitor.Win.Frms
{
    public partial class FrmAutoMaker : MetroForm
    {
        /// <summary>
        /// 窗体唯一标识符
        /// </summary>
        public static string UniqueKey = "FrmAutoMakerCSKY";
        public static string Device = string.Empty;

        public string[] strSignal = new string[] { 
            "圆盘给料机正转",
            "圆盘给料机反转",
            "锤破电机", 
            "旋转刮料电机", 
            "六毫米圆盘缩分器", 
            "对辊破碎机",
            "三毫米圆盘缩分器",
            "三毫米二分振动器",
            "粉碎机",
            "零毫米二分振动器",
        };

        public string[] strSignal2 = new string[] { 
            
            "Z型提升机", 
            "三毫米存查样输送皮带",
            "一号桶",
            "二号桶",
            "三号桶",
            "四号桶",
            "五号桶",
            "六号桶",
            "七号桶",
            "八号桶",
            "入料斗",
            "烘干机A",
            "烘干机B",
            "烘干机C",
            "提升斗",
            "三毫米二分振动器",
            "三毫米清洗样给料机",
            "三毫米分析样给料机",
            "粉碎机",
            "真空上料机",
            "零毫米二分振动器",
            "零毫米分析样给料机",
            "零毫米存查样给料机"
        };

        CefWebBrowserEx cefWebBrowser = new CefWebBrowserEx();

        public FrmAutoMaker()
        {
            InitializeComponent();
        }

       

        /// <summary>
        /// 窗体初始化
        /// </summary>
        private void FormInit()
        {

#if DEBUG
            gboxTest.Visible = true;
#else
            gboxTest.Visible = false;
#endif
            cefWebBrowser.StartUrl = SelfVars.Url_AutoMaker;
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

        private void FrmAutoMakerCSKY_Load(object sender, EventArgs e)
        {
            FormInit();
        }

        /// <summary>
        /// 请求数据
        /// </summary>
        void RequestData()
        {
            try
            {
               
                CommonDAO commonDAO = CommonDAO.GetInstance();
                AutoMakerDAO automakerDAO = AutoMakerDAO.GetInstance();

                string value = string.Empty, machineCode = string.Empty;
                List<HtmlDataItem> datas = new List<HtmlDataItem>();
                List<InfEquInfHitch> equInfHitchs = new List<InfEquInfHitch>();
                datas.Clear();
                Device = string.IsNullOrEmpty(Device) ? "#1全自动制样机" : Device;
                datas.Add(new HtmlDataItem("全自动制样机", Device, eHtmlDataItemType.svg_text));

                Creatadatas(commonDAO, GlobalVars.MachineCode_QZDZYJ_1, datas);

                // 发送到页面
                if (datas != null)
                {
                    cefWebBrowser.Browser.GetMainFrame().ExecuteJavaScript("requestData(" + Newtonsoft.Json.JsonConvert.SerializeObject(datas) + ");", "", 0);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void Creatadatas(CommonDAO commonDAO, string machineCode, List<HtmlDataItem> datas)
        {
            #region 全自动制样机
            //制样信息
            string value = "";
            datas.Add(new HtmlDataItem(machineCode + "Z型提升机", GetColor(commonDAO.GetSignalDataValue(machineCode, "Z型提升机")), eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem(machineCode + "三毫米清洗样给料机", GetColor(commonDAO.GetSignalDataValue(machineCode, "三毫米清洗样给料机")), eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem(machineCode + "三毫米分析样给料机", GetColor(commonDAO.GetSignalDataValue(machineCode, "三毫米分析样给料机")), eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem(machineCode + "真空上料机", GetColor(commonDAO.GetSignalDataValue(machineCode, "真空上料机")), eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem(machineCode + "零毫米存查样给料机", GetColor(commonDAO.GetSignalDataValue(machineCode, "零毫米存查样给料机")), eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem(machineCode + "零毫米分析样给料机", GetColor(commonDAO.GetSignalDataValue(machineCode, "零毫米分析样给料机")), eHtmlDataItemType.svg_color));

            datas.Add(new HtmlDataItem(machineCode + "0.2毫米存查样瓶", GetColor(commonDAO.GetSignalDataValue(machineCode, "零毫米存查样给料机")), eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem(machineCode + "0.2毫米分析样瓶", GetColor(commonDAO.GetSignalDataValue(machineCode, "零毫米分析样给料机")), eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem(machineCode + "6毫米分析样瓶", GetColor(commonDAO.GetSignalDataValue(machineCode, "六毫米圆盘缩分器")), eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem(machineCode + "3毫米存查样瓶", GetColor(commonDAO.GetSignalDataValue(machineCode, "三毫米圆盘缩分器")), eHtmlDataItemType.svg_color));

            datas.Add(new HtmlDataItem(machineCode + "一号桶", GetColor(commonDAO.GetSignalDataValue(machineCode, "一号桶")), eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem(machineCode + "二号桶", GetColor(commonDAO.GetSignalDataValue(machineCode, "二号桶")), eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem(machineCode + "三号桶", GetColor(commonDAO.GetSignalDataValue(machineCode, "三号桶")), eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem(machineCode + "四号桶", GetColor(commonDAO.GetSignalDataValue(machineCode, "四号桶")), eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem(machineCode + "五号桶", GetColor(commonDAO.GetSignalDataValue(machineCode, "五号桶")), eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem(machineCode + "六号桶", GetColor(commonDAO.GetSignalDataValue(machineCode, "六号桶")), eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem(machineCode + "七号桶", GetColor(commonDAO.GetSignalDataValue(machineCode, "七号桶")), eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem(machineCode + "八号桶", GetColor(commonDAO.GetSignalDataValue(machineCode, "八号桶")), eHtmlDataItemType.svg_color));

            datas.Add(new HtmlDataItem(machineCode + "入料斗", GetColor(commonDAO.GetSignalDataValue(machineCode, "入料斗")), eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem(machineCode + "烘干机A", GetColor(commonDAO.GetSignalDataValue(machineCode, "烘干机A")), eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem(machineCode + "烘干机B", GetColor(commonDAO.GetSignalDataValue(machineCode, "烘干机B")), eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem(machineCode + "烘干机C", GetColor(commonDAO.GetSignalDataValue(machineCode, "烘干机C")), eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem(machineCode + "提升斗", GetColor(commonDAO.GetSignalDataValue(machineCode, "提升斗")), eHtmlDataItemType.svg_color));

            datas.Add(new HtmlDataItem(machineCode + "三毫米存查样输送皮带stroke", GetColor(commonDAO.GetSignalDataValue(machineCode, "三毫米存查样输送皮带")), eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem(machineCode + "弃料皮带stroke", GetColor(commonDAO.GetSignalDataValue(machineCode, "弃料皮带")), eHtmlDataItemType.svg_color));

            var up = commonDAO.GetSignalDataValue(machineCode, "上料提升机上升");
            var down = commonDAO.GetSignalDataValue(machineCode, "上料提升机下降");
            datas.Add(new HtmlDataItem(machineCode + "上料提升机上升", ConvertBoolean(up), eHtmlDataItemType.svg_visible));
            datas.Add(new HtmlDataItem(machineCode + "上料提升机下降", ConvertBoolean(down), eHtmlDataItemType.svg_visible));
            datas.Add(new HtmlDataItem(machineCode + "上料提升机", up == "1" || down == "1" ? GetColor("1") : GetColor("0"), eHtmlDataItemType.svg_color));

            value = commonDAO.GetSignalDataValue(machineCode, eSignalDataName.系统.ToString());
            if (value.Contains("运行"))
                datas.Add(new HtmlDataItem(machineCode + "制样机_系统", ColorTranslator.ToHtml(EquipmentStatusColors.Working), eHtmlDataItemType.svg_color));
            else if (value.Contains("就绪"))
                datas.Add(new HtmlDataItem(machineCode + "制样机_系统", ColorTranslator.ToHtml(EquipmentStatusColors.BeReady), eHtmlDataItemType.svg_color));
            else if (value.Contains("故障"))
                datas.Add(new HtmlDataItem(machineCode + "制样机_系统", ColorTranslator.ToHtml(EquipmentStatusColors.Breakdown), eHtmlDataItemType.svg_color));
            else
                datas.Add(new HtmlDataItem(machineCode + "制样机_系统", ColorTranslator.ToHtml(EquipmentStatusColors.Forbidden), eHtmlDataItemType.svg_color));

            #region #1全水分析仪
            value = commonDAO.GetSignalDataValue(GlobalVars.DeviceCode_QSFXY_1, eSignalDataName.系统.ToString());
            if (value.Contains("运行"))
            {
                datas.Add(new HtmlDataItem("txt_211566376", ColorTranslator.ToHtml(EquipmentStatusColors.Working),
                    eHtmlDataItemType.svg_color));
                datas.Add(new HtmlDataItem("txt_211567288", ColorTranslator.ToHtml(EquipmentStatusColors.Working),
                eHtmlDataItemType.svg_color));
                datas.Add(new HtmlDataItem("txt_211567096", ColorTranslator.ToHtml(EquipmentStatusColors.Working),
                eHtmlDataItemType.svg_color));
                datas.Add(new HtmlDataItem("txt_211567408", ColorTranslator.ToHtml(EquipmentStatusColors.Working),
                eHtmlDataItemType.svg_color));
                datas.Add(new HtmlDataItem("txt_211567216", ColorTranslator.ToHtml(EquipmentStatusColors.Working),
                eHtmlDataItemType.svg_color));
                datas.Add(new HtmlDataItem("txt_211566736", ColorTranslator.ToHtml(EquipmentStatusColors.Working),
                eHtmlDataItemType.svg_color));
                datas.Add(new HtmlDataItem("txt_211567696", ColorTranslator.ToHtml(EquipmentStatusColors.Working),
                eHtmlDataItemType.svg_color));
                datas.Add(new HtmlDataItem("txt_211566016", ColorTranslator.ToHtml(EquipmentStatusColors.Working),
                eHtmlDataItemType.svg_color));
            }
            else if (value.Contains("就绪"))
            {
                datas.Add(new HtmlDataItem("txt_211566376", ColorTranslator.ToHtml(EquipmentStatusColors.BeReady),
                  eHtmlDataItemType.svg_color));
                datas.Add(new HtmlDataItem("txt_211567288", ColorTranslator.ToHtml(EquipmentStatusColors.BeReady),
                eHtmlDataItemType.svg_color));
                datas.Add(new HtmlDataItem("txt_211567096", ColorTranslator.ToHtml(EquipmentStatusColors.BeReady),
                eHtmlDataItemType.svg_color));
                datas.Add(new HtmlDataItem("txt_211567408", ColorTranslator.ToHtml(EquipmentStatusColors.BeReady),
                eHtmlDataItemType.svg_color));
                datas.Add(new HtmlDataItem("txt_211567216", ColorTranslator.ToHtml(EquipmentStatusColors.BeReady),
                eHtmlDataItemType.svg_color));
                datas.Add(new HtmlDataItem("txt_211566736", ColorTranslator.ToHtml(EquipmentStatusColors.BeReady),
                eHtmlDataItemType.svg_color));
                datas.Add(new HtmlDataItem("txt_211567696", ColorTranslator.ToHtml(EquipmentStatusColors.BeReady),
                eHtmlDataItemType.svg_color));
                datas.Add(new HtmlDataItem("txt_211566016", ColorTranslator.ToHtml(EquipmentStatusColors.BeReady),
                eHtmlDataItemType.svg_color));
            }

            else if (value.Contains("故障"))
            {
                datas.Add(new HtmlDataItem("txt_211566376", ColorTranslator.ToHtml(EquipmentStatusColors.Breakdown),
                 eHtmlDataItemType.svg_color));
                datas.Add(new HtmlDataItem("txt_211567288", ColorTranslator.ToHtml(EquipmentStatusColors.Breakdown),
                eHtmlDataItemType.svg_color));
                datas.Add(new HtmlDataItem("txt_211567096", ColorTranslator.ToHtml(EquipmentStatusColors.Breakdown),
                eHtmlDataItemType.svg_color));
                datas.Add(new HtmlDataItem("txt_211567408", ColorTranslator.ToHtml(EquipmentStatusColors.Breakdown),
                eHtmlDataItemType.svg_color));
                datas.Add(new HtmlDataItem("txt_211567216", ColorTranslator.ToHtml(EquipmentStatusColors.Breakdown),
                eHtmlDataItemType.svg_color));
                datas.Add(new HtmlDataItem("txt_211566736", ColorTranslator.ToHtml(EquipmentStatusColors.Breakdown),
                eHtmlDataItemType.svg_color));
                datas.Add(new HtmlDataItem("txt_211567696", ColorTranslator.ToHtml(EquipmentStatusColors.Breakdown),
                eHtmlDataItemType.svg_color));
                datas.Add(new HtmlDataItem("txt_211566016", ColorTranslator.ToHtml(EquipmentStatusColors.Breakdown),
                eHtmlDataItemType.svg_color));
            }
            else
            {
                datas.Add(new HtmlDataItem("txt_211566376", ColorTranslator.ToHtml(EquipmentStatusColors.Forbidden),
                eHtmlDataItemType.svg_color));
                datas.Add(new HtmlDataItem("txt_211567288", ColorTranslator.ToHtml(EquipmentStatusColors.Forbidden),
                eHtmlDataItemType.svg_color));
                datas.Add(new HtmlDataItem("txt_211567096", ColorTranslator.ToHtml(EquipmentStatusColors.Forbidden),
                eHtmlDataItemType.svg_color));
                datas.Add(new HtmlDataItem("txt_211567408", ColorTranslator.ToHtml(EquipmentStatusColors.Forbidden),
                eHtmlDataItemType.svg_color));
                datas.Add(new HtmlDataItem("txt_211567216", ColorTranslator.ToHtml(EquipmentStatusColors.Forbidden),
                eHtmlDataItemType.svg_color));
                datas.Add(new HtmlDataItem("txt_211566736", ColorTranslator.ToHtml(EquipmentStatusColors.Forbidden),
                eHtmlDataItemType.svg_color));
                datas.Add(new HtmlDataItem("txt_211567696", ColorTranslator.ToHtml(EquipmentStatusColors.Forbidden),
                eHtmlDataItemType.svg_color));
                datas.Add(new HtmlDataItem("txt_211566016", ColorTranslator.ToHtml(EquipmentStatusColors.Forbidden),
                eHtmlDataItemType.svg_color));
            }
               
            value = commonDAO.GetSignalDataValue(GlobalVars.DeviceCode_QSFXY_1, "分析数值");
            datas.Add(new HtmlDataItem("分析数值", value+"%", eHtmlDataItemType.svg_text));
            #endregion

            //信号状态
            string keys = string.Empty;
            foreach (string item in strSignal)
            {
                var scrollvalue = commonDAO.GetSignalDataValue(machineCode, item);
                if (scrollvalue == "运行" || scrollvalue == "1")
                {
                    keys += item;
                }
            }
            datas.Add(new HtmlDataItem(machineCode + "Keys", keys, eHtmlDataItemType.svg_scroll));
            #endregion
        }

        private string GetColor(string value)
        {
            string color = string.Empty;
            switch (value)
            {
                case "就绪":
                case "0":
                    color = ColorTranslator.ToHtml(EquipmentStatusColors.BeReady);
                    break;
                case "运行":
                case "1":
                    color = ColorTranslator.ToHtml(EquipmentStatusColors.Working);
                    break;
                case "故障":
                    color = ColorTranslator.ToHtml(EquipmentStatusColors.Breakdown);
                    break;
                case "停止":
                    color = ColorTranslator.ToHtml(EquipmentStatusColors.Forbidden);
                    break;
                default:
                    color = ColorTranslator.ToHtml(EquipmentStatusColors.Forbidden);
                    break;

            }
            return color;
        }

        /// <summary>
        /// 转换布尔类型状态为颜色值
        /// </summary>
        /// <param name="status">状态</param>
        /// <returns></returns>
        private string ConvertBoolean(string status)
        {
            return status.ToLower() == "1" ? "true" : "false";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // 界面不可见时，停止发送数据
            if (!this.Visible) return;

            RequestData();
        }

        /// <summary>
        /// 刷新页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            cefWebBrowser.Browser.Reload();
        }

        private void btnRequestData_Click(object sender, EventArgs e)
        {
            RequestData();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            // 发送到页面
            cefWebBrowser.Browser.GetMainFrame().ExecuteJavaScript("testColor();", "", 0);
        }
    }
}
