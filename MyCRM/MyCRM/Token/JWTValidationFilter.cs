
using IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using MyCRM.Models;
using System.Security.Claims;
namespace MyCRM.Token
{
    //自定义验证JWTVersion的过滤器
    public class JWTValidationFilter : IAsyncActionFilter
    {
        //缓存使用
        //private IMemoryCache memCache;
        //对应的Service服务
        private IUserService _userService;

        public JWTValidationFilter(IUserService userService)
        {
            //this.memCache = memCache;
            _userService = userService;
        }

        /* public JWTValidationFilter(IMemoryCache memCache, IAutherInfoService autherInfoService)
         {
             this.memCache = memCache;
             _autherInfoService = autherInfoService;
         }*/

        /// <summary>
        /// 验证JWTVersion
        /// </summary>
        /// <param name="context">此处为http中的上下文 浏览器发过来的</param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //控制器描述 
            ControllerActionDescriptor? ctrlActionDesc =
                context.ActionDescriptor as ControllerActionDescriptor;
            //描述为空返回
            if (ctrlActionDesc == null)
            {
                await next(); //不next 后续的controller不执行 
                return;
            }
            //描述不为空，判断是否有自定义的注解，该注解注释的方法不检查JWTVesion
            if (ctrlActionDesc.MethodInfo.GetCustomAttributes(typeof(NotCheckJWTVesionAttribute), true).Any())
            {
                await next();
                return;
            }
            //获取http上下文中的JWTVersion键值对
            var claimJWTVersion = context.HttpContext.User.FindFirst("JWTVersion");
            if (claimJWTVersion == null)
            {
                context.Result = new ObjectResult("Token ERROR")
                {
                    StatusCode = 401
                };
                return;
            }
            //取出JWTVersion
            int jwtVersionFromClient = Convert.ToInt32(claimJWTVersion.Value);

            //在数据库中进行查询对应JWTVersion 不用每次都查数据库，可以使用缓存
            var claimUserId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            TUser user = await _userService.QueryAsync(Convert.ToInt32(claimUserId.Value));

            //对于登录接口等没有登录的，直接跳过
            if (user == null)
            {
                context.Result = new ObjectResult("Token ERROR")
                {
                    StatusCode = 401
                };
                return;
            }
            //比对JWTVersion
            if (user.Jwtversion > jwtVersionFromClient)
            {
                context.Result = new ObjectResult("Token ERROR")
                {
                    StatusCode = 401
                };
                return;
            }
            await next();
        }
    }
}
