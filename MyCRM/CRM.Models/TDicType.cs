using System;
using System.Collections.Generic;

namespace MyCRM.Models
{
    /// <summary>
    /// 字典类型表
    /// </summary>
    public partial class TDicType
    {
        /// <summary>
        /// 主键，自动增长，字典类型ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 字典类型代码
        /// </summary>
        public string TypeCode { get; set; } = null!;
        /// <summary>
        /// 字典类型名称
        /// </summary>
        public string? TypeName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }
}
