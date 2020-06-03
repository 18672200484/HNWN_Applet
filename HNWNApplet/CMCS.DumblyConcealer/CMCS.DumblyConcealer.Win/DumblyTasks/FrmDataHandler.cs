using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using CMCS.Common;
using CMCS.Common.DAO;
using CMCS.Common.Utilities;
using CMCS.DapperDber.Dbs.AccessDb;
using CMCS.DapperDber.Dbs.SqlServerDb;
using CMCS.DumblyConcealer.Enums;
using CMCS.DumblyConcealer.Tasks.CarSynchronous;
using CMCS.DumblyConcealer.Win.Core;
using HikISCApi.Core;

namespace CMCS.DumblyConcealer.Win.DumblyTasks
{
    public partial class FrmDataHandler : TaskForm
    {
        RTxtOutputer rTxtOutputer;
        RTxtOutputer rTxtOutResultputer;
        TaskSimpleScheduler taskSimpleScheduler = new TaskSimpleScheduler();
        CommonDAO commonDAO = CommonDAO.GetInstance();

        public FrmDataHandler()
        {
            InitializeComponent();
        }

        private void FrmCarSynchronous_Load(object sender, EventArgs e)
        {
            this.Text = "综合事件处理";

            this.rTxtOutputer = new RTxtOutputer(rtxtOutput);

            ExecuteAllTask();

        }

        /// <summary>
        /// 执行所有任务
        /// </summary>
        void ExecuteAllTask()
        {
            DataHandlerDAO dataHandlerDAO = DataHandlerDAO.GetInstance();
            SqlServerDapperDber qgcDapperDber = new SqlServerDapperDber(commonDAO.GetCommonAppletConfigString("全过程接口连接字符串"));

            taskSimpleScheduler.StartNewTask("同步全过程基础信息", () =>
            {
                dataHandlerDAO.SyncBaseInfoForCHLGL(this.rTxtOutputer.Output, qgcDapperDber);
                //dataHandlerDAO.SyncBaseInfoForDYJH(this.rTxtOutputer.Output, qgcDapperDber);
                dataHandlerDAO.SyncBaseInfoForGYS(this.rTxtOutputer.Output, qgcDapperDber);
                //dataHandlerDAO.SyncBaseInfoForCHYSH(this.rTxtOutputer.Output, qgcDapperDber);
                dataHandlerDAO.SyncBaseInfoForKB(this.rTxtOutputer.Output, qgcDapperDber);
                dataHandlerDAO.SyncBaseInfoForMZ(this.rTxtOutputer.Output, qgcDapperDber);
                dataHandlerDAO.SyncBaseInfoForDYJHK(this.rTxtOutputer.Output, qgcDapperDber);
                dataHandlerDAO.SyncBaseInfoForYAPFK(this.rTxtOutputer.Output, qgcDapperDber);
                dataHandlerDAO.SyncBaseInfoForQCLMYGP(this.rTxtOutputer.Output, qgcDapperDber);
                dataHandlerDAO.SyncBaseInfoForWZJHCL(this.rTxtOutputer.Output, qgcDapperDber);
                dataHandlerDAO.SyncBaseInfoForJLMZ(this.rTxtOutputer.Output, qgcDapperDber);
                dataHandlerDAO.SyncBaseInfoForJLPZ(this.rTxtOutputer.Output, qgcDapperDber);

            }, 10000, OutputError);

            taskSimpleScheduler.StartNewTask("处理集控首页数据", () =>
            {
                dataHandlerDAO.HandleHomePageData(this.rTxtOutputer.Output);

            }, 10000, OutputError);

            //初始化Api
            string ip = commonDAO.GetCommonAppletConfigString("海康平台地址");
            int port = commonDAO.GetCommonAppletConfigInt32("海康协议端口号");
            string Appkey = commonDAO.GetCommonAppletConfigString("海康Appkey");
            string Secret = commonDAO.GetCommonAppletConfigString("海康Secret");

            HttpUtillib.SetPlatformInfo(Appkey, Secret, ip, port, false);

            taskSimpleScheduler.StartNewTask("同步门禁数据", () =>
            {
                dataHandlerDAO.SyncDoorEventData(this.rTxtOutputer.Output);

            }, 60000, OutputError);
        }

        /// <summary>
        /// 输出异常信息
        /// </summary>
        /// <param name="text"></param>
        /// <param name="ex"></param>
        void OutputError(string text, Exception ex)
        {
            this.rTxtOutputer.Output(text + Environment.NewLine + ex.Message, eOutputType.Error);

            Log4Neter.Error(text, ex);
        }

        /// <summary>
        /// 输出异常信息（结果）
        /// </summary>
        /// <param name="text"></param>
        /// <param name="ex"></param>
        void OutputResultError(string text, Exception ex)
        {
            this.rTxtOutResultputer.Output(text + Environment.NewLine + ex.Message, eOutputType.Error);

            Log4Neter.Error(text, ex);
        }

        /// <summary>
        /// 窗体关闭后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmCarSynchronous_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 注意：必须取消任务
            this.taskSimpleScheduler.Cancal();
        }
    }
}
