using System;
using System.Collections.Generic;

namespace MyCRM.Models
{
    /// <summary>
    /// 权限表
    /// </summary>
    public partial class TPermission
    {
        public TPermission()
        {
            TRolePermissions = new HashSet<TRolePermission>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Url { get; set; }
        public string? Type { get; set; }
        public int? ParentId { get; set; }
        public int? OrderNo { get; set; }
        public string? Icon { get; set; }

        public virtual ICollection<TRolePermission> TRolePermissions { get; set; }
    }
}
