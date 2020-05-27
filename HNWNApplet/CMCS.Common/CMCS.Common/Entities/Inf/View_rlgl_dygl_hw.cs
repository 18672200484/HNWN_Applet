using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMCS.Common.Entities.Inf
{
    /// <summary>
    /// 货物信息
    /// </summary>
    [Serializable]
    [CMCS.DapperDber.Attrs.DapperBind("view_rlgl_dygl_hw")]
    public class View_rlgl_dygl_hw
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DapperDber.Attrs.DapperPrimaryKey]
        public int Dbid { get; set; }
        /// <summary>
        /// 货物编码
        /// </summary>
        public string Productcode { get; set; }
        /// <summary>
        /// 货物名称
        /// </summary>
        public string Productname { get; set; }
        /// <summary>
        /// 对应物料(华能物料主数据编码)
        /// </summary>
        public string Mzbh { get; set; }
        /// <summary>
        /// 物料类型（811  燃煤，812  燃油，813  灰，814  渣，815  其它）
        /// </summary>
        public int Wltype { get; set; }
    }
}
