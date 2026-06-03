using MyCRM.Common;
using MyCRM.Common.ApiResult;
using MyCRM.Common.page;
using MyCRM.Models;
using MyCRM.Models.DTO;
using MyCRM.Models.Request;
using System;
using System.Linq.Expressions;



namespace IRepository
{
    public interface IProductRepository : IBaseRepository<TProduct>
    {
        public Task<Dictionary<string, int>> GetDicValuesAsync();
        
    }
}
