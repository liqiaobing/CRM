using System;
using System.Collections.Generic;

namespace MyCRM.Models
{
    /// <summary>
    /// 市场活动备注表
    /// </summary>
    public partial class TActivityRemark
    {
        /// <summary>
        /// 主键，自动增长，活动备注ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 活动ID
        /// </summary>
        public int? ActivityId { get; set; }
        /// <summary>
        /// 备注内容
        /// </summary>
        public string? NoteContent { get; set; }
        /// <summary>
        /// 备注创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 备注创建人
        /// </summary>
        public int? CreateBy { get; set; }
        /// <summary>
        /// 备注编辑时间
        /// </summary>
        public DateTime? EditTime { get; set; }
        /// <summary>
        /// 备注编辑人
        /// </summary>
        public int? EditBy { get; set; }
        /// <summary>
        /// 删除状态（0正常，1删除）
        /// </summary>
        public int? Deleted { get; set; }

        public virtual TActivity? Activity { get; set; }
        public virtual TUser? CreateByNavigation { get; set; }
        public virtual TUser? EditByNavigation { get; set; }
    }
}
