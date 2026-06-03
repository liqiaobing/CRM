using System;
using System.Collections.Generic;

namespace MyCRM.Models
{
    /// <summary>
    /// 字典值表
    /// </summary>
    public partial class TDicValue
    {
        public TDicValue()
        {
            TClueAppellationNavigations = new HashSet<TClue>();
            TClueIntentionStateNavigations = new HashSet<TClue>();
            TClueNeedLoanNavigations = new HashSet<TClue>();
            TClueRemarks = new HashSet<TClueRemark>();
            TClueSourceNavigations = new HashSet<TClue>();
            TClueStateNavigations = new HashSet<TClue>();
            TCustomerRemarks = new HashSet<TCustomerRemark>();
            TTranHistories = new HashSet<TTranHistory>();
            TTranRemarks = new HashSet<TTranRemark>();
            TTrans = new HashSet<TTran>();
        }

        /// <summary>
        /// 主键，自动增长，字典值ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 字典类型代码
        /// </summary>
        public string? TypeCode { get; set; }
        /// <summary>
        /// 字典值
        /// </summary>
        public string? TypeValue { get; set; }
        /// <summary>
        /// 字典值排序
        /// </summary>
        public int? Order { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        public virtual ICollection<TClue> TClueAppellationNavigations { get; set; }
        public virtual ICollection<TClue> TClueIntentionStateNavigations { get; set; }
        public virtual ICollection<TClue> TClueNeedLoanNavigations { get; set; }
        public virtual ICollection<TClueRemark> TClueRemarks { get; set; }
        public virtual ICollection<TClue> TClueSourceNavigations { get; set; }
        public virtual ICollection<TClue> TClueStateNavigations { get; set; }
        public virtual ICollection<TCustomerRemark> TCustomerRemarks { get; set; }
        public virtual ICollection<TTranHistory> TTranHistories { get; set; }
        public virtual ICollection<TTranRemark> TTranRemarks { get; set; }
        public virtual ICollection<TTran> TTrans { get; set; }
    }
}
