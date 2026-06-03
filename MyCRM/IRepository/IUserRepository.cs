using MyCRM.Common.page;
using MyCRM.Models;
using System.Linq.Expressions;



namespace IRepository
{
    public interface IUserRepository : IBaseRepository<TUser>
    {
        Task<TUser> LoginAsync(Expression<Func<TUser, bool>> func);

        Task<int> BatchDeleteAsync(List<int> delList);


        public Task<PageConfig> PageQueryFilterAsync(int page, int currentId);
       

    }
}
