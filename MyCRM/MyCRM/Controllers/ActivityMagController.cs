using AutoMapper;
using Common.Md5;
using IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using MyCRM.Common;
using MyCRM.Common.ApiResult;
using MyCRM.Models.DTO;
using MyCRM.Models.Request;
using System.Security.Claims;

namespace MyCRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ActivityMagController : ControllerBase
    {
        private readonly IActivityService _activityService;
        private readonly IMapper _imapper;
        private readonly IOptions<SystemItem> _itemCfg;


        public ActivityMagController(IOptions<SystemItem> options, IActivityService activityService, IMapper imapper)
        {
            _itemCfg = options;
            _activityService = activityService;
            _imapper = imapper;
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
  /*      [HttpGet("Activity/{page}")]
        public async Task<ActionResult<ApiResult>> PageUser(int page)
        {
            var result = await _activityService.PageQueryApiAsync<TActivityDTO>(page);
            return result;
        }*/

        /// <summary>
        /// 获取负责人和对应的id 
        /// </summary>
        /// <returns></returns>
        [HttpGet("Activity/GetOwners")]
        public async Task<ActionResult<ApiResult>> GetOwners()
        {
            //var result = await _activityService.GetActivityOwnerAsync();
            var result = await _activityService.GetActivityOwnerAsync_V2();
            return result;
        }

        /// <summary>
        /// 活动的分页和条件查询
        /// </summary>
        /// <param name="current"></param>
        /// <param name="name"></param>
        /// <param name="ownerId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="createTime"></param>
        /// <param name="cost"></param>
        /// <returns></returns>
        [HttpGet("Activity/ConditionQuery")]
        public async Task<ActionResult<ApiResult>> ConditionQuery(int current, string? name, int? ownerId, string? startTime, string? endTime, string? createTime, double? cost)
        {
            var currentId = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            //var result = await _activityService.ConditionQueryAsync(current, name, ownerId, startTime, endTime, createTime, cost);
            //var result = await _activityService.ConditionQuery_Async(current, name, ownerId, startTime, endTime, createTime, cost);
            var result = await _activityService.ConditionQueryFilterAsync(current, currentId, name, ownerId, startTime, endTime, createTime, cost);
            return result;
        }

        /// <summary>
        /// 创建活动
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Activity")]
        public async Task<ActionResult<ApiResult>> CreateActivity([FromForm]TActivityRequest request)
        {
            int? createBy = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (createBy == null) return ApiResultHelper.Error("添加人信息异常");
            var result = await _activityService.CreateActivityAsync(request, (int)createBy);
            return result;
        }

        /// <summary>
        /// 获取指定id的活动
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Activity/ActInfo/{id}")]
        public async Task<ActionResult<ApiResult>> GetActivityInfo(int id)
        {
            
            var res = await _activityService.QueryAsync(id);
            if(res == null) return ApiResultHelper.Error("获取数据异常");
            var result = _imapper.Map<TActivityDTO>(res);
            return ApiResultHelper.Success(result);
        }

        /// <summary>
        /// 修改活动
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("Activity")]
        public async Task<ActionResult<ApiResult>> EditActivity([FromForm] TActivityRequest request)
        {
            int? createBy = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (createBy == null) return ApiResultHelper.Error("添加人信息异常");
            var result = await _activityService.UpdateActivityAsync(request, (int)createBy);
            return result;
        }

        /// <summary>
        /// 活动详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Activity/Detail/{id}")]
        public async Task<ActionResult<ApiResult>> ActDetail(int id)
        {
            var res = await _activityService.ActivityDetailAsync(id);
            return res;
        }

        /// <summary>
        /// 删除活动
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Activity/{id}")]
        public async Task<ActionResult<ApiResult>> EditActivity(int id)
        {
            var res = await _activityService.DeleteAsync(id);
            if(!res) return ApiResultHelper.Error("删除活动失败，请检查依赖");
            return ApiResultHelper.Success(res); 
        }
    }
}
