using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM.Common.ApiResult
{
    public class ApiResult
    {
        public int Code { get; set; }
        public string Msg { get; set; }
        public dynamic Data { get; set; }
        public int Total { get; set; }
        public int PageSize { get; set; }
    }
}
