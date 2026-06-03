using System;
using System.Collections.Generic;

namespace MyCRM.Models
{
    /// <summary>
    /// 用户角色关系表
    /// </summary>
    public partial class TUserRole
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? RoleId { get; set; }

        public virtual TRole? Role { get; set; }
        public virtual TUser? User { get; set; }
    }
}
