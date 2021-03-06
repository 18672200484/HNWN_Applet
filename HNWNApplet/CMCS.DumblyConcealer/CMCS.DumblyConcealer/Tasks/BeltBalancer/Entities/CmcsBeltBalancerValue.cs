﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities.Sys;
using CMCS.DapperDber.Attrs;

namespace CMCS.DumblyConcealer.Tasks.BeltBalancer.Entities
{
    /// <summary>
    /// 皮带秤实时值
    /// </summary>
    [DapperBind("CMCStbBELTBALANCERVALUE")]
    public class CmcsBeltBalancerValue : EntityBase1
    {
        private string _SymbolName;
        /// <summary>
        /// 标识符
        /// </summary>
        public string SymbolName { get { return _SymbolName; } set { _SymbolName = value; } }

        private string _TagName;
        /// <summary>
        /// 标签名
        /// </summary>
        public string TagName { get { return _TagName; } set { _TagName = value; } }

        /// <summary>
        /// 标签值
        /// </summary>
        public string TagValue { get; set; }

        private DateTime _TagTime;
        /// <summary>
        /// OPC时间
        /// </summary>
        public DateTime TagTime { get { return _TagTime; } set { _TagTime = value; } }
    }
}
