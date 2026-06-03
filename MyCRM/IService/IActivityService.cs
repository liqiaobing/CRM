
using MyCRM.Common.ApiResult;
using MyCRM.Common.page;
using MyCRM.Models;
using MyCRM.Models.Request;

namespace IService
{
    public interface IActivityService : IBaseService<TActivity>
    {   
        public Task<ApiResult> GetActivityOwnerAsync();
        public Task<ApiResult> GetActivityOwnerAsync_V2();
        public Task<ApiResult> ConditionQueryAsync(int current, string? name, int? ownerId, string? startTime, string? endTime, string? createTime, double? cost);
        public Task<ApiResult> ConditionQuery_Async(int current, string? name, int? ownerId, string? startTime, string? endTime, string? createTime, double? cost);
        public Task<ApiResult> ConditionQueryFilterAsync(int current, int currentId, string? name, int? ownerId, string? startTime, string? endTime, string? createTime, double? cost);
        public Task<ApiResult> CreateActivityAsync(TActivityRequest request, int createBy);
        public Task<ApiResult> UpdateActivityAsync(TActivityRequest request, int editBy);
        public Task<ApiResult> ActivityDetailAsync(int? id);
        
    }

}
