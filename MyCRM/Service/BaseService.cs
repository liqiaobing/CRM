using AutoMapper;
using IRepository;
using IService;
using MyCRM.Common.ApiResult;
using MyCRM.Models.DTO;
using System.Linq.Expressions;

namespace Service
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class, new()
    {
        //从子类构造函数传入
        protected IBaseRepository<TEntity> _baseRepository;
        protected IMapper _imapper;

        public virtual Task<bool> CreateAsync(TEntity entity)
        {
            return _baseRepository.CreateAsync(entity);
        }

        public virtual Task<bool> DeleteAsync(int id)
        {
            return (_baseRepository.DeleteAsync(id));
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public virtual Task<List<TEntity>> PageQueryAsync(int page, int pageSize, int total)
        {
            return _baseRepository.PageQueryAsync(page, pageSize, total);
        }

        public Task<int> PageTotalAsync(int pageSize)
        {
            return _baseRepository.PageTotalAsync(pageSize);
        }

        public virtual Task<List<TEntity>> QueryAsync()
        {
            return _baseRepository.QueryAsync();
        }

        public virtual Task<TEntity> QueryAsync(int id)
        {
            return _baseRepository.QueryAsync(id);
        }

        public virtual Task<TEntity> QueryAsync(Expression<Func<TEntity, bool>> func)
        {
            return _baseRepository.QueryAsync(func);
        }

        public virtual Task<bool> UpdateAsync(TEntity entity)
        {
            return _baseRepository.UpdateAsync(entity);
        }
        public Task<int> CountAsync()
        {
           return _baseRepository.CountAsync();
        }

        public async Task<ApiResult> PageQueryApiAsync<T>(int page)
        {
            int pageSize = 10;
            var users = await _baseRepository.PageQueryAsync(page, pageSize, 0);
            if (users.Count == 0)
            {
                return ApiResultHelper.Error("数据不存在");
            }
            //var pageTotal = await _UserService.PageTotalAsync(pageSize);
            var total = await _baseRepository.CountAsync();
            var dto = _imapper.Map<List<T>>(users);
            return ApiResultHelper.Success(pageSize, total, dto);
        }

        public Task<List<TEntity>> QuerysAsync(Expression<Func<TEntity, bool>> func)
        {
            return _baseRepository.QuerysAsync(func);
        }

        public Task<TEntity> QueryFirstAsync(Expression<Func<TEntity, bool>> func)
        {
            return _baseRepository.QueryFirstAsync(func);
        }
    }
}