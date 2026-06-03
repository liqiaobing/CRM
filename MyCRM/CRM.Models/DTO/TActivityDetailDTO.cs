using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM.Models.DTO
{
    public class TActivityDetailDTO : TActivityDTO
    {
        //创建人
        public string? CreateName { get; set; }
        //编辑人
        public string? EditName { get; set; }
        //编辑时间
        public DateTime? EditTime { get; set; }
        
    }
}
