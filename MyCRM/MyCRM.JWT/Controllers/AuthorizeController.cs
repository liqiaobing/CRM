using Common.ApiResult;
using IService;
using Microsoft.AspNetCore.Mvc;
using MyCRM.Models;
using MyCRM.Models.Request;

namespace MyCRM.JWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthorizeController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("Login")]
        public async Task<ActionResult<ApiResult>> Login(LoginRequest login)
        {
            var user = new TUser { LoginAct = login.loginAct, LoginPwd = login.loginPwd };
            string jwtToken = await _userService.LoginAsync(user);
            if (jwtToken == null) return ApiResultHelper.Error("用户名或密码错误");
            return ApiResultHelper.Success(jwtToken);
        }

    }

    public record TestUser(string userName, string password);
}
