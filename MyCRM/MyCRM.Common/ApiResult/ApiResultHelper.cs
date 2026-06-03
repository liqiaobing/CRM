using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM.Common.ApiResult
{
    public class ApiResultHelper
    {
        public static ApiResult Success(dynamic data)
        {
            return new ApiResult
            {
                Code = 200,
                Msg = "操作成功",
                Data = data,
                Total = 0,
                PageSize = 0,
            };
        }
        public static ApiResult Success(int pageTotal, dynamic data) 
        { 
            return new ApiResult
            {
                Code = 200,
                Msg = "操作成功",
                Data = data,
                Total = pageTotal,
                PageSize = 0,
            };
        }

        public static ApiResult Success(int pageSize,int total, dynamic data)
        {
            return new ApiResult
            {
                Code = 200,
                Msg = "操作成功",
                Data = data,
                Total = total,
                PageSize = pageSize,
            };
        }

        public static ApiResult Error(string msg)
        {
            return new ApiResult
            {
                Code = 500,
                Msg = msg,
                Data = null,
                Total = 0,
                PageSize = 0,
            };
        }

        public static ApiResult Error(string msg, int code)
        {
            return new ApiResult
            {
                Code = code,
                Msg = msg,
                Data = null,
                Total = 0,
                PageSize = 0,
            };
        }
    }
}
