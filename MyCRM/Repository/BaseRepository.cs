using IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyCRM.Common;
using MyCRM.DataBase;
using System.Linq.Expressions;

namespace Repository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new()
    {
        protected ApplicationDbcontext _dbcontext;
        protected DbSet<TEntity> _dbset;

        public BaseRepository(ApplicationDbcontext dbcontext)
        {
            _dbcontext = dbcontext;
            _dbset = _dbcontext.Set<TEntity>();
        }

        /// <summary>
        /// 获取对应表的总行数
        /// </summary>
        /// <returns></returns>
        public async Task<int> CountAsync()
        {
            return await _dbset.CountAsync();
        }
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<bool> CreateAsync(TEntity entity)
        {
            await _dbset.AddAsync(entity);
            return await _dbcontext.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 根据id删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteAsync(int id)
        {
            var result = await _dbset.FindAsync(id);
            if(result != null)
            {
                _dbset.Remove(result);
            }
            return await _dbcontext.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public virtual async Task<List<TEntity>> PageQueryAsync(int page, int pageSize, int total)
        {
            return await _dbset.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }


        /// <summary>
        /// 计算分页total
        /// </summary>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<int> PageTotalAsync(int pageSize)
        {
            var count = await _dbset.CountAsync();
            return (count + pageSize - 1) / pageSize;
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<TEntity>> QueryAsync()
        {
            return await _dbset.ToListAsync();
        }

   

        /// <summary>
        /// 根据id获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> QueryAsync(int id)
        {
            return await _dbset.FindAsync(id);
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> QueryAsync(Expression<Func<TEntity, bool>> func)
        {
            return await _dbset.SingleOrDefaultAsync(func);
        }

        public async Task<TEntity> QueryFirstAsync(Expression<Func<TEntity, bool>> func)
        {
            return await _dbset.FirstOrDefaultAsync(func);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
            _dbset.Update(entity);
            return await _dbcontext.SaveChangesAsync() > 0;
        }

        async Task<List<TEntity>> IBaseRepository<TEntity>.QuerysAsync(Expression<Func<TEntity, bool>> func)
        {
            return await _dbset.Where(func).ToListAsync();
        }
    }
}