using System;
using System.Collections.Generic;

namespace MyCRM.Models
{
    /// <summary>
    /// 市场活动表
    /// </summary>
    public partial class TActivity
    {
        public TActivity()
        {
            TActivityRemarks = new HashSet<TActivityRemark>();
            TClues = new HashSet<TClue>();
        }

        /// <summary>
        /// 主键，自动增长，活动ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 活动所属人ID
        /// </summary>
        public int? OwnerId { get; set; }
        /// <summary>
        /// 活动名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 活动开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 活动结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 活动预算
        /// </summary>
        public decimal? Cost { get; set; }
        /// <summary>
        /// 活动描述
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// 活动创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 活动创建人
        /// </summary>
        public int? CreateBy { get; set; }
        /// <summary>
        /// 活动编辑时间
        /// </summary>
        public DateTime? EditTime { get; set; }
        /// <summary>
        /// 活动编辑人
        /// </summary>
        public int? EditBy { get; set; }

        public virtual TUser? CreateByNavigation { get; set; }
        public virtual TUser? EditByNavigation { get; set; }
        public virtual TUser? Owner { get; set; }
        public virtual ICollection<TActivityRemark> TActivityRemarks { get; set; }
        public virtual ICollection<TClue> TClues { get; set; }
    }
}
