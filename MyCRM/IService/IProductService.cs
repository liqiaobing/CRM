
using MyCRM.Common.ApiResult;
using MyCRM.Common.page;
using MyCRM.Models;
using MyCRM.Models.Request;

namespace IService
{
    public interface IProductService : IBaseService<TProduct>
    {

        public Task<Dictionary<string, int>> GetDicsListAsync();
    }

}
