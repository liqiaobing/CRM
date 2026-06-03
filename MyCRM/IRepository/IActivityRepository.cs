using MyCRM.Common;
using MyCRM.Common.page;
using MyCRM.Models;
using MyCRM.Models.DTO;
using MyCRM.Models.Request;
using System;
using System.Linq.Expressions;



namespace IRepository
{
    public interface IActivityRepository : IBaseRepository<TActivity>
    {
        public Task<List<OwnerDTO>> GetActivityOwnerAsync();
        public Task<dynamic> GetActivityOwnerAsync_V2();

        public Task<PageConfig> GetActivitysAsync(int current, DateTime defaultTime, 
            DateTime startTime, DateTime endTime, DateTime activityTiem, int? OwnerId = null, string? name = null, decimal? cost = null);
        public Task<PageConfig> GetActivitiesAsync(TActivityRequest activity, int currentPage, int pageSize);
        public Task<PageConfig> GetActivitiesFilterAsync(TActivityRequest activity, int currentPage, int pageSize, int currentId);

    }
}
