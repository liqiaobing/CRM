using System;
using System.Collections.Generic;

namespace MyCRM.Models
{
    /// <summary>
    /// 交易历史记录表
    /// </summary>
    public partial class TTranHistory
    {
        /// <summary>
        /// 主键，自动增长，交易记录ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 交易ID
        /// </summary>
        public int? TranId { get; set; }
        /// <summary>
        /// 交易阶段
        /// </summary>
        public int? Stage { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public decimal? Money { get; set; }
        /// <summary>
        /// 交易预计成交时间
        /// </summary>
        public DateTime? ExpectedDate { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public int? CreateBy { get; set; }

        public virtual TUser? CreateByNavigation { get; set; }
        public virtual TDicValue? StageNavigation { get; set; }
        public virtual TTran? Tran { get; set; }
    }
}
