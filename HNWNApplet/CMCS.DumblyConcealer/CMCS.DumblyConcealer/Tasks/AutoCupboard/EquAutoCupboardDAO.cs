using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.DumblyConcealer.Enums;
using CMCS.DumblyConcealer.Tasks.AssayDevice.Entities;
using CMCS.Common;
using CMCS.Common.Entities.AssayDevices;
using CMCS.Common.DAO;
using CMCS.Common.Entities.Inf;

namespace CMCS.DumblyConcealer.Tasks.AssayDevice
{
	public class EquAutoCupboardDAO
	{
		private static EquAutoCupboardDAO instance;

		public static EquAutoCupboardDAO GetInstance()
		{
			if (instance == null)
			{
				instance = new EquAutoCupboardDAO();
			}
			return instance;
		}

		private EquAutoCupboardDAO()
		{

		}

		CommonDAO commonDAO = CommonDAO.GetInstance();

		/// <summary>
		/// 同步存样柜信息
		/// </summary>
		/// <param name="output"></param>
		/// <returns></returns>
		public int SyncCYGInfo(Action<string, eOutputType> output)
		{
			int res = 0;
			foreach (EquCYG item in DcDbers.GetInstance().AutoCupboard_Dber.Entities<EquCYG>())
			{
				string pkid = item.YGId.ToString();
				string CupBoardType = item.YGLX;
				if (item.YGLX == "432")
					CupBoardType = "制样室样柜";
				else if (item.YGLX == "433")
					CupBoardType = "化验室样柜";
				else if (item.YGLX == "646")
					CupBoardType = "原煤样样柜";

				int State = 0;
				if (item.ZT == 435)
					State = 1;
				else if (item.ZT == 436)
					State = 0;

				int row = 0, cell = 0;
				if (item.YGBM.Length == 9)
				{
					string ms = item.WZMS.Replace("层", "").Replace("格", "");
					int number = int.Parse(ms.Remove(0, 2));
					if (CupBoardType == "原煤样样柜")
					{
						row = number % 4;
						cell = number / 4 + 1;
						if (row == 0)
						{
							row = 4;
							cell = number / 4;
						}
					}
					else
					{
						row = number % 10;
						cell = number / 10 + 1;
						if (row == 0)
						{
							row = 10;
							cell = number / 10;
						}
					}
				}
				InfAutoCupBoard entity = commonDAO.SelfDber.Entity<InfAutoCupBoard>(" where PKID=:PKID", new { PKID = pkid });
				EquCYGDetail detail = DcDbers.GetInstance().AutoCupboard_Dber.Entity<EquCYGDetail>("where YGID=@YGID and ZT=438 order by CYSJ desc", new { YGID = item.YGId });
				if (entity == null)
				{
					entity = new InfAutoCupBoard();
					entity.CupBoardCode = item.YGBM;
					entity.CupBoardType = CupBoardType;
					entity.CupBoardDes = item.WZMS;
					entity.State = State;
					entity.IsValid = item.Is_Valid;
					entity.PKID = pkid;
					entity.ParentPKID = item.Parent_YGID;
					entity.RowNumber = row;
					entity.CellNumber = cell;
					entity.CupNumber = item.Num_Url;
					if (detail != null)
						entity.SaveTime = detail.CYSJ;
					res += commonDAO.SelfDber.Insert(entity);
				}
				else
				{
					entity.CupBoardCode = item.YGBM;
					entity.CupBoardType = CupBoardType;
					entity.CupBoardDes = item.WZMS;
					entity.State = State;
					entity.IsValid = item.Is_Valid;
					entity.RowNumber = row;
					entity.CellNumber = cell;
					entity.CupNumber = item.Num_Url;
					entity.ParentPKID = item.Parent_YGID;
					if (detail != null)
						entity.SaveTime = detail.CYSJ;
					res += commonDAO.SelfDber.Update(entity);
				}
			}
			output(string.Format("同步存样柜数据 {0} 条", res), eOutputType.Normal);

			return res;
		}
	}
}
