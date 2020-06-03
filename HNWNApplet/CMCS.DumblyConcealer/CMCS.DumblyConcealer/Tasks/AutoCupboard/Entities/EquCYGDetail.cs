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
	[CMCS.DapperDber.Attrs.DapperBind("VIEW_RLGL_YGGL_DETAIL")]
	public class EquCYGDetail
	{
		[DapperPrimaryKey]
		public int YGId { get; set; }

		/// <summary>
		/// 存样时间
		/// </summary>
		public DateTime CYSJ { get; set; }

		/// <summary>
		/// 438正常；439 弃样
		/// </summary>
		public int ZT { get; set; }

	}
}
