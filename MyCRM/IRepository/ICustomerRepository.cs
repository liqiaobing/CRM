using MyCRM.Common;
using MyCRM.Common.page;
using MyCRM.Models;
using MyCRM.Models.DTO;
using MyCRM.Models.Request;
using System;
using System.Linq.Expressions;



namespace IRepository
{
    public interface ICustomerRepository : IBaseRepository<TCustomer>
    {
        public Task<PageConfig> GetCstPageListAsync(int currentPage, int currentId);
        public Task<dynamic> GetCstListAsync(int currentId);
        Task<dynamic> GetCstConditionListAsync(List<int> condition, int currentId);
    }
}
