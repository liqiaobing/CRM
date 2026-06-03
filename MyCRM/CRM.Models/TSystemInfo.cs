using System;
using System.Collections.Generic;

namespace MyCRM.Models
{
    /// <summary>
    /// 系统信息表
    /// </summary>
    public partial class TSystemInfo
    {
        public int Id { get; set; }
        public string? SystemCode { get; set; }
        public string Name { get; set; } = null!;
        public string Site { get; set; } = null!;
        public string? Logo { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Keywords { get; set; } = null!;
        public string Shortcuticon { get; set; } = null!;
        public string? Tel { get; set; }
        public string? Weixin { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Version { get; set; }
        public string? CloseMsg { get; set; }
        public string? Isopen { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? EditTime { get; set; }
        public int? EditBy { get; set; }

        public virtual TUser? CreateByNavigation { get; set; }
        public virtual TUser? EditByNavigation { get; set; }
    }
}
