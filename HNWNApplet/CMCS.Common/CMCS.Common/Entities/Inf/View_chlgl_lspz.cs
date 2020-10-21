using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMCS.Common.Entities.Inf
{
    /// <summary>
    /// 更换挂车记录
    /// </summary>
    [Serializable]
    [CMCS.DapperDber.Attrs.DapperBind("View_chlgl_lspz")]
    public class View_chlgl_lspz
    {
        /// <summary>
        /// 车辆ID
        /// </summary>
        [DapperDber.Attrs.DapperPrimaryKey]
        public decimal Chlgl_id { get; set; }
        /// <summary>
        /// 车号
        /// </summary>
        public string Chph { get; set; }
        /// <summary>
        /// 皮重
        /// </summary>
        public decimal Lspz { get; set; }
        /// <summary>
        /// 更改原因
        /// </summary>
        public string Xgyy { get; set; }
        /// <summary>
        /// 删除标识 
        /// </summary>
        public string Is_del { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public decimal Update_by { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime Update_date { get; set; }
    }
}
