
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities.Sys; 

namespace CMCS.Common.Entities.iEAA
{
    /// <summary>
    /// 编码管理
    /// </summary>
    [CMCS.DapperDber.Attrs.DapperBind("SYSSMTBCODEKIND")]
    public class CodeKind : EntityBase1
    {
        private string _Kind;
        /// <summary>
        /// 编码名称
        /// </summary>
        public string Kind { get { return _Kind; } set { _Kind = value; } }
    }
}
