using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities.Sys;
using System.ComponentModel; 

namespace CMCS.Common.Entities.BaseInfo
{
    /// <summary>
    /// 供应商分配采样机表
    /// </summary>
    [Description("供应商分配采样机")]
    [CMCS.DapperDber.Attrs.DapperBind("CmcsTbSupplierAssignSampler")]
    public class CmcsSupplierAssignSampler : EntityBase1
    {
        private string _SupplierName;
        /// <summary>
        /// 供煤单位
        /// </summary>
        [Description("供煤单位")]
        public virtual string SupplierName { get { return _SupplierName; } set { _SupplierName = value; } }

        private string _SupplierId;
        /// <summary>
        /// 供煤单位
        /// </summary>
        public virtual string SupplierId { get { return _SupplierId; } set { _SupplierId = value; } }

        private string _Sampler;
        /// <summary>
        /// 采样机编号
        /// </summary>
        [Description("采样机编号")]
        public virtual string Sampler { get { return _Sampler; } set { _Sampler = value; } }

        private string _Remark;
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get { return _Remark; } set { _Remark = value; } }

    }
}
