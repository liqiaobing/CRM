using System;
using System.Collections.Generic;

namespace MyCRM.Models
{
    /// <summary>
    /// 用户表
    /// </summary>
    public partial class TUser
    {
        public TUser()
        {
            TActivityCreateByNavigations = new HashSet<TActivity>();
            TActivityEditByNavigations = new HashSet<TActivity>();
            TActivityOwners = new HashSet<TActivity>();
            TActivityRemarkCreateByNavigations = new HashSet<TActivityRemark>();
            TActivityRemarkEditByNavigations = new HashSet<TActivityRemark>();
            TClueCreateByNavigations = new HashSet<TClue>();
            TClueEditByNavigations = new HashSet<TClue>();
            TClueOwners = new HashSet<TClue>();
            TClueRemarkCreateByNavigations = new HashSet<TClueRemark>();
            TClueRemarkEditByNavigations = new HashSet<TClueRemark>();
            TCustomerCreateByNavigations = new HashSet<TCustomer>();
            TCustomerEditByNavigations = new HashSet<TCustomer>();
            TCustomerRemarkCreateByNavigations = new HashSet<TCustomerRemark>();
            TCustomerRemarkEditByNavigations = new HashSet<TCustomerRemark>();
            TProductCreateByNavigations = new HashSet<TProduct>();
            TProductEditByNavigations = new HashSet<TProduct>();
            TSystemInfoCreateByNavigations = new HashSet<TSystemInfo>();
            TSystemInfoEditByNavigations = new HashSet<TSystemInfo>();
            TTranCreateByNavigations = new HashSet<TTran>();
            TTranEditByNavigations = new HashSet<TTran>();
            TTranHistories = new HashSet<TTranHistory>();
            TTranRemarkCreateByNavigations = new HashSet<TTranRemark>();
            TTranRemarkEditByNavigations = new HashSet<TTranRemark>();
            TUserRoles = new HashSet<TUserRole>();
        }

        /// <summary>
        /// 主键，自动增长，用户ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 登录账号
        /// </summary>
        public string? LoginAct { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string? LoginPwd { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 用户手机
        /// </summary>
        public string? Phone { get; set; }
        /// <summary>
        /// 用户邮箱
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// 账户是否没有过期，0已过期 1正常
        /// </summary>
        public int? AccountNoExpired { get; set; }
        /// <summary>
        /// 密码是否没有过期，0已过期 1正常
        /// </summary>
        public int? CredentialsNoExpired { get; set; }
        /// <summary>
        /// 账号是否没有锁定，0已锁定 1正常
        /// </summary>
        public int? AccountNoLocked { get; set; }
        /// <summary>
        /// 账号是否启用，0禁用 1启用
        /// </summary>
        public int? AccountEnabled { get; set; }
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
        /// <summary>
        /// 最近登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }
        /// <summary>
        /// JWTVersion
        /// </summary>
        public int? Jwtversion { get; set; }

        public virtual ICollection<TActivity> TActivityCreateByNavigations { get; set; }
        public virtual ICollection<TActivity> TActivityEditByNavigations { get; set; }
        public virtual ICollection<TActivity> TActivityOwners { get; set; }
        public virtual ICollection<TActivityRemark> TActivityRemarkCreateByNavigations { get; set; }
        public virtual ICollection<TActivityRemark> TActivityRemarkEditByNavigations { get; set; }
        public virtual ICollection<TClue> TClueCreateByNavigations { get; set; }
        public virtual ICollection<TClue> TClueEditByNavigations { get; set; }
        public virtual ICollection<TClue> TClueOwners { get; set; }
        public virtual ICollection<TClueRemark> TClueRemarkCreateByNavigations { get; set; }
        public virtual ICollection<TClueRemark> TClueRemarkEditByNavigations { get; set; }
        public virtual ICollection<TCustomer> TCustomerCreateByNavigations { get; set; }
        public virtual ICollection<TCustomer> TCustomerEditByNavigations { get; set; }
        public virtual ICollection<TCustomerRemark> TCustomerRemarkCreateByNavigations { get; set; }
        public virtual ICollection<TCustomerRemark> TCustomerRemarkEditByNavigations { get; set; }
        public virtual ICollection<TProduct> TProductCreateByNavigations { get; set; }
        public virtual ICollection<TProduct> TProductEditByNavigations { get; set; }
        public virtual ICollection<TSystemInfo> TSystemInfoCreateByNavigations { get; set; }
        public virtual ICollection<TSystemInfo> TSystemInfoEditByNavigations { get; set; }
        public virtual ICollection<TTran> TTranCreateByNavigations { get; set; }
        public virtual ICollection<TTran> TTranEditByNavigations { get; set; }
        public virtual ICollection<TTranHistory> TTranHistories { get; set; }
        public virtual ICollection<TTranRemark> TTranRemarkCreateByNavigations { get; set; }
        public virtual ICollection<TTranRemark> TTranRemarkEditByNavigations { get; set; }
        public virtual ICollection<TUserRole> TUserRoles { get; set; }
    }
}
