
using MyCRM.Common.ApiResult;
using MyCRM.Common.page;
using MyCRM.Models;
using MyCRM.Models.Request;

namespace IService
{
    public interface IRemarkService : IBaseService<TActivityRemark>
    {   
        public Task<ApiResult> CreateActivityAsync(RemarkRequest request, int currentId);
        public Task<ApiResult> QueryNoteContentAsync(int actId, int currentId, int currentPage);
        public Task<ApiResult> GetNoteCotentAsync(int id);
        public Task<ApiResult> UpdateNoteContentAsync(RemarkRequest request, int EditId);
        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ApiResult> DeletedAsync(int id);
    }

}
