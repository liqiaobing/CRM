using IRepository;
using IService;
using System.Text;
using System.Security.Claims;
using Common.Md5;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using MyCRM.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using MyCRM.Models.DTO;
using MyCRM.Models.Request;
using MyCRM.Common.ApiResult;
using MyCRM.Common;
using MyCRM.Common.page;
using Microsoft.Extensions.Options;

namespace Service
{
    public class ActivityService : BaseService<TActivity>, IActivityService
    {
        private readonly IActivityRepository _repository;
        private readonly IMapper _imapper;
        private readonly IOptions<SystemItem> _options;
        private readonly IUserRepository _userRepository;

        public ActivityService(IActivityRepository repository, IMapper imapper, IOptions<SystemItem> options, IUserRepository userRepository)
        {
            base._baseRepository = repository;
            base._imapper = imapper;
            _repository = repository;
            _imapper = imapper;
            _options = options;
            _userRepository = userRepository;
        }

        /// <summary>
        /// 活动信息的分页加条件查询
        /// </summary>
        /// <param name="current"></param>
        /// <param name="name"></param>
        /// <param name="ownerId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="createTime"></param>
        /// <param name="cost"></param>
        /// <returns></returns>
        public async Task<ApiResult> ConditionQueryAsync(int current, string? name, int? ownerId, string? startTime, string? endTime, string? createTime, double? cost)
        {

            //三个默认时间是一样的结果
            // dt1 = new DateTime();
            DateTime defaultTime = default(DateTime);
            //DateTime t2 = DateTime.MinValue;
           
            var start = string.IsNullOrEmpty(startTime) ? defaultTime : DateTime.Parse(startTime);
            var end = string.IsNullOrEmpty(endTime) ? defaultTime : DateTime.Parse(endTime);
            var create = string.IsNullOrEmpty(createTime) ? defaultTime : DateTime.Parse(createTime);
            PageConfig res = null;
            if (cost!= null)
            {
                string strCost = cost.ToString();
                decimal decCost = decimal.Parse(strCost);
                res = await _repository.GetActivitysAsync(current, defaultTime, start, end, create, ownerId, name, decCost);
                if (res.Total == 0) return ApiResultHelper.Error("没有查询指定范围的数据"); 
            }
            else
            {
                res = await _repository.GetActivitysAsync(current, defaultTime, start, end, create, ownerId, name);
                if (res.Total == 0) return ApiResultHelper.Error("没有查询指定范围的数据"); 
            }
            var result = _imapper.Map<List<TActivityDTO>>(res.Data);
            return ApiResultHelper.Success(10, res.Total, result);
        }

        /// <summary>
        /// 活动信息的分页加条件查询
        /// </summary>
        /// <param name="current"></param>
        /// <param name="name"></param>
        /// <param name="ownerId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="createTime"></param>
        /// <param name="cost"></param>
        /// <returns></returns>
        public async Task<ApiResult> ConditionQuery_Async(int current, string? name, int? ownerId, string? startTime, string? endTime, string? createTime, double? cost)
        {
            
            //三个默认时间是一样的结果
            // dt1 = new DateTime();
            DateTime defaultTime = default(DateTime);
            //DateTime t2 = DateTime.MinValue;

            TActivityRequest activity = new TActivityRequest
            {
                Name = name,
                OwnerId = ownerId,
                StartTime = string.IsNullOrEmpty(startTime) ? null : DateTime.Parse(startTime),
                EndTime = string.IsNullOrEmpty(endTime) ? null : DateTime.Parse(endTime),
                Cost = cost == null ? null : decimal.Parse(cost.ToString()),
                CreateTime = string.IsNullOrEmpty(createTime) ? null : DateTime.Parse(createTime),
            };

            PageConfig res = await _repository.GetActivitiesAsync(activity, current, _options.Value.PageSize);
            //if (res.Total == 0) return ApiResultHelper.Error("没有查询指定范围的数据");
            var result = _imapper.Map<List<TActivityDTO>>(res.Data);
            return ApiResultHelper.Success(10, res.Total, result);
        }

        /// <summary>
        /// 优化条件查询 加数据权限
        /// </summary>
        /// <param name="current"></param>
        /// <param name="currentId"></param>
        /// <param name="name"></param>
        /// <param name="ownerId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="createTime"></param>
        /// <param name="cost"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResult> ConditionQueryFilterAsync(int current, int currentId, string? name, int? ownerId, string? startTime, string? endTime, 
            string? createTime, double? cost)
        {
            TActivityRequest activity = new TActivityRequest
            {
                Name = name,
                OwnerId = ownerId,
                StartTime = string.IsNullOrEmpty(startTime) ? null : DateTime.Parse(startTime),
                EndTime = string.IsNullOrEmpty(endTime) ? null : DateTime.Parse(endTime),
                Cost = cost == null ? null : decimal.Parse(cost.ToString()),
                CreateTime = string.IsNullOrEmpty(createTime) ? null : DateTime.Parse(createTime),
            };

            PageConfig res = await _repository.GetActivitiesFilterAsync(activity, current, _options.Value.PageSize, currentId);
            var result = _imapper.Map<List<TActivityDTO>>(res.Data);
            return ApiResultHelper.Success(10, res.Total, result);
        }

        /// <summary>
        /// 添加活动
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ApiResult> CreateActivityAsync(TActivityRequest request, int createBy)
        {
            if (request == null) return ApiResultHelper.Error("提交的参数有误"); 
            var act = _imapper.Map<TActivity>(request);
            act.CreateTime = DateTime.Now;
            act.CreateBy = createBy;
            var res = await _baseRepository.CreateAsync(act);
            if(!res) ApiResultHelper.Error("服务器异常，添加活动失败");
            return ApiResultHelper.Success(res);
        }

        public async Task<ApiResult> UpdateActivityAsync(TActivityRequest request, int editBy)
        {
            if (request == null && request.Id != null) return ApiResultHelper.Error("提交的参数有误");
            var activity = await _baseRepository.QueryAsync((int)request.Id);
            activity.EditTime = DateTime.Now;
            activity.EditBy = editBy;
            activity.StartTime = request.StartTime;
            activity.EndTime = request.EndTime;
            activity.Cost = request.Cost;
            activity.Description = request.Description;
            activity.Name = request.Name;
            activity.OwnerId = request.OwnerId;
            var result = await _baseRepository.UpdateAsync(activity);
            if (!result) ApiResultHelper.Error("服务器异常，添加活动失败");
            return ApiResultHelper.Success(result);
        }

        /// <summary>
        /// 获取所有的活动负责人
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResult> GetActivityOwnerAsync()
        {
            var owners = await _repository.GetActivityOwnerAsync();
            if (owners == null || owners.Count <= 0) return ApiResultHelper.Error("请先添加数据");
            var res = owners.GroupBy(n=>n.OwnerName).ToList();
            List<OwnerDTO> result = new List<OwnerDTO>();
            foreach (var key in res)
            {
                foreach (var item in key)
                {
                    result.Add(item);
                    break;
                }
            }
            return ApiResultHelper.Success(result);
        }

        /// <summary>
        /// 获取所有的活动负责人 优化
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResult> GetActivityOwnerAsync_V2()
        {
            var owners = await _repository.GetActivityOwnerAsync_V2();
            return ApiResultHelper.Success(owners);
        }

        /// <summary>
        /// 活动详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResult> ActivityDetailAsync(int? id)
        {
            if(id == null) return ApiResultHelper.Error("指定的数据异常");
            var act = await _repository.QueryAsync((int)id);
            if(act == null) return ApiResultHelper.Error("没有找到该数据");
            var owner1 = await _userRepository.QueryAsync(act.OwnerId == null ? 0 : (int)act.OwnerId);
            var owner2 = await _userRepository.QueryAsync(act.CreateBy == null ? 0 : (int)act.CreateBy);
            var owner3 = await _userRepository.QueryAsync(act.EditBy == null ? 0 : (int)act.EditBy);
            var result = _imapper.Map<TActivityDetailDTO>(act);
            result.OwnerName = owner1 == null ? null : owner1.Name;
            result.CreateName = owner2 == null ? null : owner2.Name;
            result.EditName = owner3 == null ? null : owner3.Name;
            return ApiResultHelper.Success(result);
        }

        
    } 
}