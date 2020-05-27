
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
    [CMCS.DapperDber.Attrs.DapperBind("SYSSMTBCODECONTENT")]
    public class CodeContent : EntityBase1
    {
        private string _KindId;
        /// <summary>
        /// 主编码ID
        /// </summary>
        public string KindId { get { return _KindId; } set { _KindId = value; } }

        private string _Code;
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get { return _Code; } set { _Code = value; } }

        private string _Content;
        /// <summary>
        /// 编码内容
        /// </summary>
        public string Content { get { return _Content; } set { _Content = value; } }

        private int _CodeOrder;
        /// <summary>
        /// 顺序
        /// </summary>
        public int CodeOrder { get { return _CodeOrder; } set { _CodeOrder = value; } }
    }
}
