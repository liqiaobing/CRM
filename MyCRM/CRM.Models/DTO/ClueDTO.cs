using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM.Models.DTO
{
    public class ClueDTO
    {
        public int Id { get; set; }

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

        public List<DicValueDTO>? keyValuePairs { get; set; }

    }
}
