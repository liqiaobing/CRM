using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM.Models.DTO
{
    public class TActivityDTO
    {
        /// 主键，自动增长，活动ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 活动所属人ID
        /// </summary>
        public int? OwnerId { get; set; }
        /// <summary>
        /// 活动名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 活动开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 活动结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 活动预算
        /// </summary>
        public decimal? Cost { get; set; }
        /// <summary>
        /// 活动描述
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// 活动创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
   
        //负责人name
        public string? OwnerName { get; set; }
    }

}
