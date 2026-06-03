using AutoMapper;
using Common.Md5;
using IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyCRM.Common.ApiResult;
using MyCRM.Models.DTO;
using MyCRM.Models.Request;
using System.Security.Claims;

namespace MyCRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserMagController : ControllerBase
    {
        private readonly IUserService _UserService;
        private readonly IMapper _imapper;

        public UserMagController(IUserService userService, IMapper imapper)
        {
            _UserService = userService;
            _imapper = imapper;
        }

        /// <summary>
        /// 自动登录
        /// </summary>
        /// <returns></returns>
        [HttpGet("Login/free")]
        public ActionResult<ApiResult> Login()
        {
            return ApiResultHelper.Success("Login/free ok");
        }

        /// <summary>
        /// 首次登录
        /// </summary>
        /// <returns></returns>
        [HttpGet("Login/userInfo")]
        public async Task<ActionResult<ApiResult>> Get()
        {
            var uid = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //if (id == 0) return ApiResultHelper.Error("重新登录", 901);
            var user = await _UserService.GetUserInfoAsync(int.Parse(uid));

            if (user == null) return ApiResultHelper.Error("查询异常，未找到用户信息！");
            return ApiResultHelper.Success(user);
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [HttpGet("OutLogin")]
        public async Task<ActionResult<ApiResult>> OutLogin()
        {
            var uid = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _UserService.OutLoginAsync(int.Parse(uid));
            if (result)
            {
                return ApiResultHelper.Success(result);
            }
            return ApiResultHelper.Error("退出登录异常");
        }

        /// <summary>
        /// 获取用户列表数据 分页
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet("Users/{page}")]
        public async Task<ActionResult<ApiResult>> PageUser(int page)
        {
            /*int pageSize = 10;
            var users = await _UserService.PageQueryAsync(page, pageSize, 0);
            if (users.Count <= 0)
            {
                return ApiResultHelper.Error("数据不存在");
            }
            //var pageTotal = await _UserService.PageTotalAsync(pageSize);
            var total = await _UserService.CountAsync();
            var usersDTO = _imapper.Map<List<TUserDTO>>(users);
            return ApiResultHelper.Success(pageSize, total, usersDTO);*/

            //优化后有过滤权限
            var currentId = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var res = await _UserService.GetUserListAsync(page, currentId);
            return res;
        }

        /// <summary>
        /// 用户详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("User/{id}")]
        public async Task<ActionResult<ApiResult>> UserDetail(int id)
        {
            var userDetial = await _UserService.GetUserInfoAsync(id);
            if (userDetial == null) return ApiResultHelper.Error("找不到用户信息", 404);
            return ApiResultHelper.Success(userDetial);
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="addUser"></param>
        /// <returns></returns>
        //[FromForm] 此注解为接收formdata的数据，不是json的
        [HttpPost("User")]
        public async Task<ActionResult<ApiResult>> AddUser([FromForm] AddUserRequest addUser)
        {
            var createUserId = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var result = await _UserService.AddUserAsync(addUser, createUserId);
            return result;
        }


        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="editUser"></param>
        /// <returns></returns>
        [HttpPut("User")]
        public async Task<ActionResult<ApiResult>> EditUser([FromForm] EditUserRequest editUser)
        {
            int? editUserid = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (editUserid == null) return ApiResultHelper.Error("修改人信息异常");
            var result = await _UserService.UpdateUserAsync(editUser, (int)editUserid);
            return result;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("User/{id}")]
        public async Task<ActionResult<ApiResult>> Delete(int id)
        {
            var result = await _UserService.DeleteAsync(id);
            if (!result) return ApiResultHelper.Error("删除用户失败");
            return ApiResultHelper.Success(result);
        }

        /// <summary>
        /// 批量删除用户
        /// </summary>
        /// <param name="delStr"></param>
        /// <returns></returns>
        [HttpDelete("UserBatchDel/{delStr}")]
        public async Task<ActionResult<ApiResult>> BatchDel(string delStr)
        {
            var result = await _UserService.DeleteUserAsync(delStr);
            return result;
        }

    }
}
