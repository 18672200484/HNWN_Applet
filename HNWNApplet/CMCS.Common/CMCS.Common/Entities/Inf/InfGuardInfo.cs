using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities.Sys;

namespace CMCS.Common.Entities.Inf
{
    /// <summary>
    /// 第三方设备接口 - 门禁记录表
    /// </summary>
    [CMCS.DapperDber.Attrs.DapperBind("inftbguardinfo")]
    public class InfGuardInfo : EntityBase1
    {
        private string _UserId;
        private string _UserName;
        private int _InOut;
        private string _DoorNo;
        private string _DoorName;
        private DateTime _ReadCardTime;

        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }

        /// <summary>
        /// 进出状态
        /// </summary>
        public int InOut
        {
            get { return _InOut; }
            set { _InOut = value; }
        }

        /// <summary>
        /// 门号
        /// </summary>
        public string DoorNo
        {
            get { return _DoorNo; }
            set { _DoorNo = value; }
        }

        /// <summary>
        /// 门名称
        /// </summary>
        public string DoorName
        {
            get { return _DoorName; }
            set { _DoorName = value; }
        }

        /// <summary>
        /// 刷卡时间(进门时间)
        /// </summary>
        public DateTime ReadCardTime
        {
            get { return _ReadCardTime; }
            set { _ReadCardTime = value; }
        }
    }
}
