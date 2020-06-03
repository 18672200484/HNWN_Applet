using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities.Sys;

namespace CMCS.Common.Entities.Inf
{
	/// <summary>
	/// 存样柜-存样柜信息
	/// </summary>
	[CMCS.DapperDber.Attrs.DapperBind("ifntbAutoCupBoard")]
	public class InfAutoCupBoard : EntityBase1
	{
		private string cupBoardType;
		/// <summary>
		/// 样柜类型
		/// </summary>
		public string CupBoardType
		{
			get { return cupBoardType; }
			set { cupBoardType = value; }
		}

		private string cupBoardCode;
		/// <summary>
		/// 样柜编码
		/// </summary>
		public string CupBoardCode
		{
			get { return cupBoardCode; }
			set { cupBoardCode = value; }
		}

		private string cupBoardDes;
		/// <summary>
		/// 样柜描述
		/// </summary>
		public string CupBoardDes
		{
			get { return cupBoardDes; }
			set { cupBoardDes = value; }
		}

		private int state;
		/// <summary>
		/// 状态
		/// </summary>
		public int State
		{
			get { return state; }
			set { state = value; }
		}

		private string isValid;
		/// <summary>
		/// 是否有效
		/// </summary>
		public string IsValid
		{
			get { return isValid; }
			set { isValid = value; }
		}

		private string pkid;
		/// <summary>
		/// 
		/// </summary>
		public string PKID
		{
			get { return pkid; }
			set { pkid = value; }
		}

		private string parentPKID;
		/// <summary>
		/// 
		/// </summary>
		public string ParentPKID
		{
			get { return parentPKID; }
			set { parentPKID = value; }
		}

		private DateTime saveTime;
		/// <summary>
		/// 存样时间
		/// </summary>
		public DateTime SaveTime
		{
			get { return saveTime; }
			set { saveTime = value; }
		}

		private int row;
		/// <summary>
		/// 行
		/// </summary>
		public int RowNumber
		{
			get { return row; }
			set { row = value; }
		}

		private int cell;
		/// <summary>
		/// 列
		/// </summary>
		public int CellNumber
		{
			get { return cell; }
			set { cell = value; }
		}

		private string cupNumber;
		/// <summary>
		/// 柜门
		/// </summary>
		public string CupNumber
		{
			get { return cupNumber; }
			set { cupNumber = value; }
		}

	}
}
