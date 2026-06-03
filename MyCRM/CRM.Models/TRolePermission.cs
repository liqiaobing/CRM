using System;
using System.Collections.Generic;

namespace MyCRM.Models
{
    /// <summary>
    /// 角色权限关系表
    /// </summary>
    public partial class TRolePermission
    {
        public int Id { get; set; }
        public int? RoleId { get; set; }
        public int? PermissionId { get; set; }

        public virtual TPermission? Permission { get; set; }
        public virtual TRole? Role { get; set; }
    }
}
