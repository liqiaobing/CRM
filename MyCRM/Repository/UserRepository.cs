using IRepository;
using MyCRM.DataBase;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MyCRM.Models;
using System.Globalization;
using Microsoft.Extensions.Options;
using MyCRM.Common;
using MyCRM.Common.page;

namespace Repository
{
    public class UserRepository : BaseRepository<TUser>, IUserRepository
    {
        private ApplicationDbcontext _dbcontext;
        private readonly IOptions<SystemItem> _option;

        public UserRepository(ApplicationDbcontext dbcontext, IOptions<SystemItem> opt) :base(dbcontext)
        {
            _dbcontext = dbcontext;
            _option = opt;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="delList">int类型集合存放id</param>
        /// <returns>0为失败 大于0同时等于传入的id数量则为成功</returns>
        public async Task<int> BatchDeleteAsync(List<int> delList)
        {
            var result = 0;
            List<TUser> users = new List<TUser>();
            foreach (int del in delList) 
            {
                TUser user = await _dbcontext.TUsers.FindAsync(del);
                if (user == null) continue; 
                users.Add(user);
            }
            if(users.Count > 0) 
            {
                try
                {
                    _dbcontext.TUsers.RemoveRange(users);
                }catch(Exception ) 
                { 
                    return 0;
                }
                result = await _dbcontext.SaveChangesAsync();
            }
            return result;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public async Task<TUser> LoginAsync(Expression<Func<TUser, bool>> func)
        {
            return await _dbcontext.TUsers.SingleOrDefaultAsync(func);
        }

        /// <summary>
        /// 过滤权限的分页查询
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentId"></param>
        /// <returns></returns>
        public async Task<PageConfig> PageQueryFilterAsync(int page, int currentId)
        {
            IQueryable<TUser> query = _dbcontext.TUsers;
            if(currentId != _option.Value.Admin)
            {
                query = query.Where(p => p.Id == currentId);
            }
            var total = query.Count();
            return new PageConfig
            {
                Data = await query.Skip((page - 1) * _option.Value.PageSize).Take(_option.Value.PageSize).ToListAsync(),
                Total = total
            };
        }

        
    }
}
