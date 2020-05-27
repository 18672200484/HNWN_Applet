using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using CMCS.Common.Entities;
using CMCS.DapperDber.Attrs;


namespace CMCS.Common.Entities.iEAA
{
    /// <summary>
    /// 平台用户
    /// </summary>
    [Serializable]
    [DapperBind("SYSAMTBUSER")]
    public class User
    {
        [DapperPrimaryKey]
        public string PartyId { get; set; }

        private string _UserName;
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get { return _UserName; } set { _UserName = value; } }

        private string _UserAccount;
        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserAccount { get { return _UserAccount; } set { _UserAccount = value; } }

        private string _Password;
        /// <summary>
        /// 密码
        /// </summary>
        public string MDPassword { get { return _Password; } set { _Password = value; } }

        private int _Stop;
        /// <summary>
        /// 是否启用
        /// </summary>
        public int Stop { get { return _Stop; } set { _Stop = value; } }
    }
}
