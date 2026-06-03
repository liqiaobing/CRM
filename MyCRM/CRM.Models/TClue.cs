using System;
using System.Collections.Generic;

namespace MyCRM.Models
{
    /// <summary>
    /// 线索表
    /// </summary>
    public partial class TClue
    {
        public TClue()
        {
            TClueRemarks = new HashSet<TClueRemark>();
            TCustomers = new HashSet<TCustomer>();
        }

        /// <summary>
        /// 主键，自动增长，线索ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 线索所属人ID
        /// </summary>
        public int? OwnerId { get; set; }
        /// <summary>
        /// 活动ID
        /// </summary>
        public int? ActivityId { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string? FullName { get; set; }
        /// <summary>
        /// 称呼
        /// </summary>
        public int? Appellation { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string? Phone { get; set; }
        /// <summary>
        /// 微信号
        /// </summary>
        public string? Weixin { get; set; }
        /// <summary>
        /// QQ号
        /// </summary>
        public string? Qq { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int? Age { get; set; }
        /// <summary>
        /// 职业
        /// </summary>
        public string? Job { get; set; }
        /// <summary>
        /// 年收入
        /// </summary>
        public decimal? YearIncome { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string? Address { get; set; }
        /// <summary>
        /// 是否需要贷款（0不需要，1需要）
        /// </summary>
        public int? NeedLoan { get; set; }
        /// <summary>
        /// 意向状态
        /// </summary>
        public int? IntentionState { get; set; }
        /// <summary>
        /// 意向产品
        /// </summary>
        public int? IntentionProduct { get; set; }
        /// <summary>
        /// 线索状态
        /// </summary>
        public int? State { get; set; }
        /// <summary>
        /// 线索来源
        /// </summary>
        public int? Source { get; set; }
        /// <summary>
        /// 线索描述
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

        public virtual TActivity? Activity { get; set; }
        public virtual TDicValue? AppellationNavigation { get; set; }
        public virtual TUser? CreateByNavigation { get; set; }
        public virtual TUser? EditByNavigation { get; set; }
        public virtual TProduct? IntentionProductNavigation { get; set; }
        public virtual TDicValue? IntentionStateNavigation { get; set; }
        public virtual TDicValue? NeedLoanNavigation { get; set; }
        public virtual TUser? Owner { get; set; }
        public virtual TDicValue? SourceNavigation { get; set; }
        public virtual TDicValue? StateNavigation { get; set; }
        public virtual ICollection<TClueRemark> TClueRemarks { get; set; }
        public virtual ICollection<TCustomer> TCustomers { get; set; }
    }
}
