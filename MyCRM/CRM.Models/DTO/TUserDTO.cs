using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM.Models.DTO
{
    public class TUserDTO
    {
        /// <summary>
        /// 主键，自动增长，用户ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 登录账号
        /// </summary>
        public string? LoginAct { get; set; }
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

        public UserCreateEditInfoDTo CreateByInfo { get; set; }

        /// <summary>
        /// 编辑时间
        /// </summary>
        public DateTime? EditTime { get; set; }
        /// <summary>
        /// 编辑人
        /// </summary>
        public int? EditBy { get; set; }
        public UserCreateEditInfoDTo EditByInfo { get; set; }
        /// <summary>
        /// 最近登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }
        /// <summary>
        /// JWTVersion
        /// </summary>
       
    }
}
