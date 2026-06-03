using MyCRM.Common.ApiResult;
using MyCRM.Models.DTO;
using System.Linq.Expressions;

namespace IService
{
    public interface IBaseService<TEntity> where TEntity : class, new()
    {
        Task<bool> CreateAsync(TEntity entity);
        Task<bool> UpdateAsync(TEntity entity);
        Task<bool> DeleteAsync(int id);
        Task<List<TEntity>> QueryAsync();
        Task<TEntity> QueryAsync(int id);
        Task<TEntity> QueryAsync(Expression<Func<TEntity, bool>> func);
        Task<List<TEntity>> QuerysAsync(Expression<Func<TEntity, bool>> func);
        Task<List<TEntity>> PageQueryAsync(int page, int pageSize, int total);
        Task<ApiResult> PageQueryApiAsync<T>(int page);
        Task<int> PageTotalAsync(int pageSize); 
        Task<int> CountAsync();
        Task<TEntity> QueryFirstAsync(Expression<Func<TEntity, bool>> func);
    }
}