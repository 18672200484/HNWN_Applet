﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities.Sys; 

namespace CMCS.Common.Entities.BaseInfo
{
    /// <summary>
    /// 基础信息-供应商
    /// </summary>
    [CMCS.DapperDber.Attrs.DapperBind("fultbsupplier")]
    public class CmcsSupplier : EntityBase1
    {
        /// <summary>
        /// 公司纳税号
        /// </summary>
        public String TaxregCode { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 简称
        /// </summary>
        public String ShortName { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public String Code { get; set; }

        /// <summary>
        /// 是否发货单位(1 标识是 0 表示否) 
        /// </summary>
        public string Is_fhdw { get; set; }

        /// <summary>
        /// 是否收货单位(1 标识是 0 表示否)
        /// </summary>
        public string is_shhdw { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public virtual int IsStop { get; set; }

        /// <summary>
        /// 全过程主键id
        /// </summary>
        public int PkId { get; set; }

    }
}
