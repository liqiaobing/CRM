using System;
using System.Collections.Generic;

namespace MyCRM.Models
{
    /// <summary>
    /// 客户跟踪记录表
    /// </summary>
    public partial class TCustomerRemark
    {
        /// <summary>
        /// 主键，自动增长，客户备注ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 客户ID
        /// </summary>
        public int? CustomerId { get; set; }
        /// <summary>
        /// 跟踪方式
        /// </summary>
        public int? NoteWay { get; set; }
        /// <summary>
        /// 跟踪内容
        /// </summary>
        public string? NoteContent { get; set; }
        /// <summary>
        /// 跟踪人
        /// </summary>
        public int? CreateBy { get; set; }
        /// <summary>
        /// 跟踪时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 编辑时间
        /// </summary>
        public DateTime? EditTime { get; set; }
        /// <summary>
        /// 编辑人
        /// </summary>
        public int? EditBy { get; set; }
        /// <summary>
        /// 删除状态（0正常，1删除）
        /// </summary>
        public int? Deleted { get; set; }

        public virtual TUser? CreateByNavigation { get; set; }
        public virtual TCustomer? Customer { get; set; }
        public virtual TUser? EditByNavigation { get; set; }
        public virtual TDicValue? NoteWayNavigation { get; set; }
    }
}
