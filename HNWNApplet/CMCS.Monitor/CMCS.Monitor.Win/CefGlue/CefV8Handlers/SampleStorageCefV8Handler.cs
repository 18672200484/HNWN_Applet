using CMCS.Common.DAO;
using CMCS.Common.Entities.Inf;
using CMCS.Monitor.Win.Frms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xilium.CefGlue;

namespace CMCS.Monitor.Win.CefGlue.CefV8Handlers
{
	/// <summary>
	/// 存样柜管理 CefV8Handler
	/// </summary>
	public class SampleStorageCefV8Handler : CefV8Handler
	{
		protected override bool Execute(string name, CefV8Value obj, CefV8Value[] arguments, out CefV8Value returnValue, out string exception)
		{
			exception = null;
			returnValue = null;
			string paramSampler = arguments[0].GetStringValue();

			switch (name)
			{
				case "ChangeSelected":
					CefProcessMessage cefMsg = CefProcessMessage.Create("ChangeSelected");
					cefMsg.Arguments.SetSize(0);
					cefMsg.Arguments.SetString(0, paramSampler);
					CefV8Context.GetCurrentContext().GetBrowser().SendProcessMessage(CefProcessId.Browser, cefMsg);
					break;
				case "ShowYGDetail":
					int overday = CommonDAO.GetInstance().GetCommonAppletConfigInt32("存样柜超期天数");
					IList<RowResult> rowResult = new List<RowResult>();
					IList<InfAutoCupBoard> cellList = CommonDAO.GetInstance().SelfDber.Entities<InfAutoCupBoard>(string.Format("where CupBoardCode like '%{0}%' and CupBoardType='{1}' and ParentPKID is not null", paramSampler, arguments[1].GetStringValue()));
					if (cellList != null && cellList.Count > 0)
					{
						int maxRow = cellList.OrderByDescending(a => a.RowNumber).First().RowNumber;
						int maxCell = cellList.OrderByDescending(a => a.CellNumber).First().CellNumber;
						for (int i = 1; i <= maxRow; i++)
						{
							RowResult rowEntity = new RowResult();
							rowEntity.RowName = String.Format("第 {0} 层", i);
							rowEntity.CellList = new List<CellResult>();
							for (int j = 1; j <= maxCell; j++)
							{
								CellResult cellEntity = new CellResult();
								InfAutoCupBoard sampleCell = cellList.Where(a => a.RowNumber == i && a.CellNumber == j).FirstOrDefault();
								if (sampleCell == null)
									cellEntity.CellFlag = -1;
								else
								{
									cellEntity.CellNumber = sampleCell.CupBoardDes.Replace("层", "").Replace("格", "").Remove(0, 2);
									//柜门标识：-1没这一格，0空柜，1已存放，2超期样，3停用
									if (sampleCell.IsValid == "0") cellEntity.CellFlag = 3;
									else if (sampleCell.State == 1 && sampleCell.SaveTime < DateTime.Now.AddDays(-overday))
										cellEntity.CellFlag = 2;
									else if (sampleCell.State == 1)
										cellEntity.CellFlag = 1;
									else
										cellEntity.CellFlag = 0;
								}
								rowEntity.CellList.Add(cellEntity);
							}
							rowResult.Add(rowEntity);
						}
					}
					returnValue = CefV8Value.CreateString(Newtonsoft.Json.JsonConvert.SerializeObject(rowResult));
					break;
				default:
					returnValue = null;
					break;
			}

			return true;
		}
	}
}
