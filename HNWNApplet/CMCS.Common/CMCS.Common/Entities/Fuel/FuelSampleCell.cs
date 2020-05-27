using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities.Sys;

namespace CMCS.Common.Entities.Fuel
{
    /// <summary>
    /// 存样柜存放区域表
    /// </summary>
    [CMCS.DapperDber.Attrs.DapperBind("FULTBSAMPLECELL")]
    public class FuelSampleCell : EntityBase1
    {
        /// <summary>
        /// 柜门编号
        /// </summary>
        public virtual String CellCode { get; set; }
        /// <summary>
        /// 样柜名称
        /// </summary>
        public virtual String CellName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual String Remark { get; set; }
        /// <summary>
        /// 列
        /// </summary>
        public virtual Int32 Cell { get; set; }
        /// <summary>
        /// 行
        /// </summary>
        public virtual Int32 Rowss { get; set; }
        /// <summary>
        /// 条码编号
        /// </summary>
        public virtual String BarCode { get; set; }
        /// <summary>
        /// 样柜序号
        /// </summary>
        public virtual Int32 CellOrder { get; set; }
        /// <summary>
        /// 样柜标识（0.2mm/原煤/仲裁）
        /// </summary>
        public virtual String BiaoShi { get; set; }
        /// <summary>
        /// 样柜是否有效，1：无效，0有效
        /// </summary>
        public virtual Int32 IsValid { get; set; }
        /// <summary>
        /// 数据来源
        /// </summary>
        public virtual String DataFrom { get; set; }
        /// <summary>
        /// 是否删除 1已删除 0未删除
        /// </summary>
        public virtual Int32 IsDeleted { get; set; }
    }
}
