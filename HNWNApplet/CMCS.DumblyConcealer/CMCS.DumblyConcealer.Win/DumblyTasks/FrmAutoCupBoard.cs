using System;
using System.Windows.Forms;
//
using CMCS.Common;
using CMCS.Common.DAO;
using CMCS.Common.Utilities;
using CMCS.DumblyConcealer.Enums;
using CMCS.DumblyConcealer.Tasks.AssayDevice;
using CMCS.DumblyConcealer.Tasks.AutoMaker;
using CMCS.DumblyConcealer.Win.Core;

namespace CMCS.DumblyConcealer.Win.DumblyTasks
{
	public partial class FrmAutoCupBoard : TaskForm
	{
		RTxtOutputer rTxtOutputer;

		TaskSimpleScheduler taskSimpleScheduler = new TaskSimpleScheduler();

		public FrmAutoCupBoard()
		{
			InitializeComponent();
		}

		private void FrmAutoMaker_NCGM_Load(object sender, EventArgs e)
		{
			this.Text = "全自动存样柜接口业务";

			this.rTxtOutputer = new RTxtOutputer(rtxtOutput);

			ExecuteAllTask();

		}

		/// <summary>
		/// 执行所有任务
		/// </summary>
		void ExecuteAllTask()
		{
			taskSimpleScheduler.StartNewTask("全自动存样柜-快速同步", () =>
			{
				EquAutoCupboardDAO.GetInstance().SyncCYGInfo(this.rTxtOutputer.Output);
			}, 50000, OutputError);
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
		/// 窗体关闭后
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void FrmAutoMaker_NCGM_FormClosed(object sender, FormClosedEventArgs e)
		{
			// 注意：必须取消任务
			this.taskSimpleScheduler.Cancal();
		}
	}
}
