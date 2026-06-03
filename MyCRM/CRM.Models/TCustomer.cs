using System;
using System.Collections.Generic;

namespace MyCRM.Models
{
    /// <summary>
    /// 客户表
    /// </summary>
    public partial class TCustomer
    {
        public TCustomer()
        {
            TCustomerRemarks = new HashSet<TCustomerRemark>();
            TTrans = new HashSet<TTran>();
        }

        /// <summary>
        /// 主键，自动增长，客户ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 线索ID
        /// </summary>
        public int? ClueId { get; set; }
        /// <summary>
        /// 选购产品
        /// </summary>
        public int? Product { get; set; }
        /// <summary>
        /// 客户描述
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

        public virtual TClue? Clue { get; set; }
        public virtual TUser? CreateByNavigation { get; set; }
        public virtual TUser? EditByNavigation { get; set; }
        public virtual TProduct? ProductNavigation { get; set; }
        public virtual ICollection<TCustomerRemark> TCustomerRemarks { get; set; }
        public virtual ICollection<TTran> TTrans { get; set; }
    }
}
