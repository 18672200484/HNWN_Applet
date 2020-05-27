using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities.Sys;

namespace CMCS.Common.Entities.BaseInfo
{
    /// <summary>
    /// 基础信息-煤种
    /// </summary>
    [CMCS.DapperDber.Attrs.DapperBind("fultbfuelkind")]
    public class CmcsFuelKind : EntityBase1
    {
        private string _Code;
        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return _Code; }
            set { _Code = value; }
        }

        private string _Name;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        /// <summary>
        /// 对应物料(华能物料主数据编码)
        /// </summary>
        public string Mzbh { get; set; }

        /// <summary>
        /// 物料类型（811  燃煤，812  燃油，813  灰，814  渣，815  其它）
        /// </summary>
        public int WLType { get; set; }

        private string _ParentId;
        /// <summary>
        /// 父Id
        /// </summary>
        public string ParentId
        {
            get { return _ParentId; }
            set { _ParentId = value; }
        }

        public int Sequence { get; set; }

        public string ReMark { get; set; }

        public int IsStop { get; set; }

        /// <summary>
        /// 全过程主键id
        /// </summary>
        public int PkId { get; set; }
    }
}
