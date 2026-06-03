using IRepository;
using MyCRM.Common.page;
using MyCRM.Models;
using System.Linq.Expressions;



namespace IRepository
{
    public interface IRemarkRepository : IBaseRepository<TActivityRemark>
    {
        public Task<PageConfig> QueryNoteContentAsync(int activityId, int currentId, int currentPage);
    }
}
