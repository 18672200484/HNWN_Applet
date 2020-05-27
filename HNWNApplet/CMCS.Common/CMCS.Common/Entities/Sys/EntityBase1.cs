using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using CMCS.DapperDber.Attrs;

namespace CMCS.Common.Entities.Sys
{
    [Serializable]
    public class EntityBase1
    {
        public EntityBase1()
        {
            this.Id = Guid.NewGuid().ToString();
            this.CreateDate = DateTime.Now;
            this.CreateUser = GlobalVars.LoginUser == null ? "" : GlobalVars.LoginUser.UserAccount;
            this.OperDate = this.CreateDate;
            this.OperUser = this.CreateUser;
            this.CreateUserId = "-2";
            this.CreateUserDeptId = "-1";
            this.CreateUserDeptCode = "00";
        }

        [DapperPrimaryKey]
        public string Id { get; set; }
        public DateTime OperDate { get; set; }
        public string OperUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public string CreateUserId { get; set; }
        public string CreateUserDeptId { get; set; }
        public string CreateUserDeptCode { get; set; }
    }
}
