using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM.Models.DTO
{
    public class TActivityRemakeRequest
    {
        /// <summary>
        /// 市场活动备注
        /// </summary>

        /// <summary>
        /// 主键，自动增长，活动备注ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 活动ID
        /// </summary>
        public int? ActivityId { get; set; }
        /// <summary>
        /// 备注内容
        /// </summary>
        public string? NoteContent { get; set; }
        /// <summary>
        /// 备注创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 备注创建人
        /// </summary>
        public int? CreateBy { get; set; }
        /// <summary>
        /// 备注编辑时间
        /// </summary>
        public DateTime? EditTime { get; set; }
        /// <summary>
        /// 备注编辑人
        /// </summary>
        public int? EditBy { get; set; }
        /// <summary>
        /// 删除状态（0正常，1删除）
        /// </summary>
        public int? Deleted { get; set; }
    }
}
