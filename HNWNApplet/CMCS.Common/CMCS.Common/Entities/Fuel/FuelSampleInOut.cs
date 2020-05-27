using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities.Sys;

namespace CMCS.Common.Entities.Fuel
{
    /// <summary>
    /// 存样柜样品存取记录
    /// </summary>
    [CMCS.DapperDber.Attrs.DapperBind("FULTBSAMPLEINOUT")]
    public class FuelSampleInOut : EntityBase1
    {
        /// <summary>
        /// 样品编码
        /// </summary>
        public virtual String SampleCode { get; set; }
        /// <summary>
        /// 制样方式
        /// </summary>
        public virtual String MakeStyle { get; set; }
        /// <summary>
        /// 重量(克)
        /// </summary>
        public virtual Decimal MakeWeight { get; set; }
        /// <summary>
        /// 存样人
        /// </summary>
        public virtual String SaveUserName { get; set; }
        /// <summary>
        /// 存样时间
        /// </summary>
        public virtual DateTime SaveTime { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public virtual DateTime OverTime { get; set; }
        /// <summary>
        /// 取样人
        /// </summary>
        public virtual String TakeUserName { get; set; }
        /// <summary>
        /// 取样人账号
        /// </summary>
        public virtual String TakeUserAccount { get; set; }
        /// <summary>
        /// 取样时间
        /// </summary>
        public virtual DateTime TakeTime { get; set; }
        /// <summary>
        /// 清样人
        /// </summary>
        public virtual String ClearUserName { get; set; }
        /// <summary>
        /// 清样人账号
        /// </summary>
        public virtual String ClearUserAccount { get; set; }
        /// <summary>
        /// 清样时间
        /// </summary>
        public virtual DateTime ClearTime { get; set; }
        /// <summary>
        /// 样品状态（0正常，1失效/已销样）
        /// </summary>
        public virtual Decimal Statue { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public virtual String SupplierName { get; set; }
        /// <summary>
        /// 批次编号
        /// </summary>
        public virtual String BatchNo { get; set; }
        /// <summary>
        /// 存样柜存放区域
        /// </summary>
        public virtual String SampleCellId { get; set; }
        /// <summary>
        /// 是否删除 1已删除 0未删除
        /// </summary>
        public virtual Decimal IsDeleted { get; set; }
    }
}
