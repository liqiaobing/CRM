using System;
using System.Collections.Generic;

namespace MyCRM.Models
{
    /// <summary>
    /// 产品表
    /// </summary>
    public partial class TProduct
    {
        public TProduct()
        {
            TCustomers = new HashSet<TCustomer>();
        }

        /// <summary>
        /// 主键，自动增长，线索ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 官方指导起始价
        /// </summary>
        public decimal? GuidePriceS { get; set; }
        /// <summary>
        /// 官方指导最高价
        /// </summary>
        public decimal? GuidePriceE { get; set; }
        /// <summary>
        /// 经销商报价
        /// </summary>
        public decimal? Quotation { get; set; }
        /// <summary>
        /// 状态 0在售 1售罄
        /// </summary>
        public int? State { get; set; }
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
        public virtual TUser? EditByNavigation { get; set; }
        public virtual ICollection<TClue> TClues { get; set; }
        public virtual ICollection<TCustomer> TCustomers { get; set; }
    }
}
