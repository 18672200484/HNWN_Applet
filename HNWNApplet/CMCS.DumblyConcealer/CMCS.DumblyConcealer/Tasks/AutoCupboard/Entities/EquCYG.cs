using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities;
using CMCS.DapperDber.Attrs;

namespace CMCS.DumblyConcealer.Tasks.AssayDevice.Entities
{
	/// <summary>
	/// 存样柜  接口
	/// </summary>
	[CMCS.DapperDber.Attrs.DapperBind("VIEW_RLGL_YGGL")]
	public class EquCYG
	{
		[DapperPrimaryKey]
		public int YGId { get; set; }

		/// <summary>
		/// 样柜类型 432 制样室样柜；433 化验室样柜；646 原煤样样柜
		/// </summary>
		public string YGLX { get; set; }
		/// <summary>
		/// 样柜编码
		/// </summary>
		public string YGBM { get; set; }

		/// <summary>
		/// 位置描述
		/// </summary>
		public string WZMS { get; set; }

		/// <summary>
		/// 状态 435存样； 436 空置
		/// </summary>
		public int ZT { get; set; }
		/// <summary>
		/// 是否有效
		/// </summary>
		public string Is_Valid { get; set; }

		/// <summary>
		/// 父节点ID
		/// </summary>
		public string Parent_YGID { get; set; }

		/// <summary>
		/// IP
		/// </summary>
		public string IP_Url { get; set; }

		/// <summary>
		/// 箱号
		/// </summary>
		public string Num_Url { get; set; }
	}
}
