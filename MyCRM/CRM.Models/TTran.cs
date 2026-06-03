using System;
using System.Collections.Generic;

namespace MyCRM.Models
{
    /// <summary>
    /// 交易表
    /// </summary>
    public partial class TTran
    {
        public TTran()
        {
            TTranHistories = new HashSet<TTranHistory>();
            TTranRemarks = new HashSet<TTranRemark>();
        }

        /// <summary>
        /// 主键，自动增长，交易ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 交易流水号
        /// </summary>
        public string? TranNo { get; set; }
        /// <summary>
        /// 客户ID
        /// </summary>
        public int? CustomerId { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public decimal? Money { get; set; }
        /// <summary>
        /// 预计成交日期
        /// </summary>
        public DateTime? ExpectedDate { get; set; }
        /// <summary>
        /// 交易所处阶段
        /// </summary>
        public int? Stage { get; set; }
        /// <summary>
        /// 交易描述
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// 下次联系时间
        /// </summary>
        public DateTime? NextContactTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public int? CreateBy { get; set; }
        /// <summary>
        /// 编辑时间
        /// </summary>
        public DateTime? EditTime { get; set; }
        /// <summary>
        /// 编辑人
        /// </summary>
        public int? EditBy { get; set; }

        public virtual TUser? CreateByNavigation { get; set; }
        public virtual TCustomer? Customer { get; set; }
        public virtual TUser? EditByNavigation { get; set; }
        public virtual TDicValue? StageNavigation { get; set; }
        public virtual ICollection<TTranHistory> TTranHistories { get; set; }
        public virtual ICollection<TTranRemark> TTranRemarks { get; set; }
    }
}
