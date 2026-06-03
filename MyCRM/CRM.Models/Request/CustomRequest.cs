using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM.Models.Request
{
    public class CustomRequest
    {
       // https://localhost:7075/api/Clue/Detail/GetSelected/product
        /// <summary>
        /// 线索ID
        /// </summary>
        public int ClueId { get; set; }
        /// <summary>
        /// 选购产品
        /// </summary>
        public int Product { get; set; }
        /// <summary>
        /// 客户描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 下次联系时间
        /// </summary>
        public DateTime NextContactTime { get; set; }

    }
}
