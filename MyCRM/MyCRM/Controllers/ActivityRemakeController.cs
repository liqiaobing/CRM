using MyCRM.Common.ApiResult;
using IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using System.Security.Claims;
using MyCRM.Models.Request;
using MyCRM.Models;
using Newtonsoft.Json.Linq;

namespace MyCRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ActivityRemakeController : ControllerBase
    {
        private readonly IRemarkService _remarkService;
        public ActivityRemakeController(IRemarkService remarkService) 
        {
            _remarkService = remarkService;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Remake")]
        public async Task<ActionResult<ApiResult>> AddNoteContent(RemarkRequest request)
        {
            var createBy = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var result = await _remarkService.CreateActivityAsync(request, createBy);
            return result;
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("Remake")]
        public async Task<ActionResult<ApiResult>> EditNoteContent(RemarkRequest request)
        {
            var editBy = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var result = await _remarkService.UpdateNoteContentAsync(request, editBy);
            return result;
        }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("Remake/{id}")]
        public async Task<ActionResult<ApiResult>> DeleteNoteContent(int id)
        {
            var res = await _remarkService.DeletedAsync(id);
            return res;
        }

        /// <summary>
        /// 获取备注列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet("Remake/{id}/{page}")]
        public async Task<ActionResult<ApiResult>> QueryNoteContent(int id, int page)
        {
            var createBy = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var result = await _remarkService.QueryNoteContentAsync(id, createBy, page);
            return result;
        }
        /// <summary>
        /// 获取单条备注
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Remake/{id}")]
        public async Task<ActionResult<ApiResult>> GetNoteContent(int id)
        {
            var result = await _remarkService.GetNoteCotentAsync(id);
            return result;
        }

    }
}
