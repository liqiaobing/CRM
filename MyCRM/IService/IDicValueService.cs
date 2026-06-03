using MyCRM.Common.ApiResult;
using MyCRM.Models;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IService
{
    public interface IDicValueService : IBaseService<TDicValue>
    {
        public Task<Dictionary<string, int>> GetDicsListAsync();

    }
}
