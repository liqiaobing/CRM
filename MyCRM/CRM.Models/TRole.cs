using System;
using System.Collections.Generic;

namespace MyCRM.Models
{
    /// <summary>
    /// 角色表
    /// </summary>
    public partial class TRole
    {
        public TRole()
        {
            TRolePermissions = new HashSet<TRolePermission>();
            TUserRoles = new HashSet<TUserRole>();
        }

        public int Id { get; set; }
        public string? Role { get; set; }
        public string? RoleName { get; set; }

        public virtual ICollection<TRolePermission> TRolePermissions { get; set; }
        public virtual ICollection<TUserRole> TUserRoles { get; set; }
    }
}
