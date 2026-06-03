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
using Microsoft.Extensions.Options;
using MyCRM.Common;

namespace Service
{
    public class UserService : BaseService<TUser>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _imapper;
        private readonly IOptions<SystemItem> _options;

        public UserService(IUserRepository userRepository, IMapper imapper, IOptions<SystemItem> opt)
        {
            base._baseRepository = userRepository;
            _userRepository = userRepository;
            _imapper = imapper;
            _options = opt;
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> OutLoginAsync(int id)
        {
            var user = await _userRepository.QueryAsync(id);
            if(user == null) { return false; }
            user.Jwtversion++;
            var result = await _userRepository.UpdateAsync(user);
            return result;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user"></param>
        /// <returns>jwtToken; null为用户名或密码不正确</returns>
        public async Task<string> LoginAsync(TUser user)
        {
            var pwd = Md5Helper.GetMD5Str(user.LoginPwd);
            var userResult = await _userRepository.QueryAsync(p => p.LoginAct == user.LoginAct && p.LoginPwd == pwd);
            if (userResult == null) return null;

            #region jwt授权代码
            if(userResult.Jwtversion == null)
            {
                userResult.Jwtversion = 0;
            }
            userResult.Jwtversion++;
            await _userRepository.UpdateAsync(userResult);
            //登录成功 开始授权
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userResult.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, userResult.Name));
            claims.Add(new Claim("AccountNoExpired", userResult.AccountNoExpired.ToString()));  //账号是否过期 1ok 0fail
            claims.Add(new Claim("AccountEnabled", userResult.AccountEnabled.ToString()));  //账号是否启用
            claims.Add(new Claim("CredentialsNoExpired", userResult.CredentialsNoExpired.ToString())); //密码是否过期
            claims.Add(new Claim("AccountNoLocked", userResult.AccountNoLocked.ToString()));    //账号是否锁定
            claims.Add(new Claim("Jwtversion", userResult.Jwtversion.ToString()));    //jwtversion
            claims.Add(new Claim("LastLoginTime", userResult.LastLoginTime.ToString()));    //最近登录时间


            //有角色就读 下面代码是Identity框架中的表
            /* 循环读取角色信息，可能一个用户有多个角色信息 ，全部写入到token中
            var roles = await userManager.GetRolesAsync(user);
            foreach (string role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            string jwtToken = BuildToken(claims, jwtOptions.Value);
            */

            //密钥  注册的密钥要和鉴权的密钥一致
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SDMC-CJAS1-SAD-11111-ASDHJVF-VFFFFF"));
            //issuer代表颁发token的web应用程序，audience是token的受理者
            var token = new JwtSecurityToken(
                issuer: "https://localhost:7257",
                audience: "http://localhost:5092",
                //issuer: "https://localhost:7257",
                //audience: "https://localhost:7075",
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddHours(1),//1个小时后过期
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
            #endregion
        }

        /// <summary>
        /// 获取登录用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TUserDTO> GetUserInfoAsync(int id)
        {
            var user = await _userRepository.QueryAsync(id);
            TUser createUser = null;
            TUser editUser = null;
            if (user.CreateBy != null && user.CreateBy > 0) 
            {
                createUser = await _userRepository.QueryAsync((int)user.CreateBy);
            }
            if (user.EditBy != null && user.EditBy > 0)
            {
                editUser = await _userRepository.QueryAsync((int)user.EditBy);
            }
            var createInfo = _imapper.Map<UserCreateEditInfoDTo>(createUser);
            var editInfo = _imapper.Map<UserCreateEditInfoDTo>(editUser);
            TUserDTO dto = _imapper.Map<TUserDTO>(user);
            dto.CreateByInfo = createInfo;
            dto.EditByInfo = editInfo;
            return dto;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="addUser"></param>
        /// <param name="createUserId"></param>
        /// <returns></returns>
        public async Task<ApiResult> AddUserAsync(AddUserRequest addUser, int createUserId)
        {
            var isEmpty = await _userRepository.QueryAsync(ur => ur.LoginAct == addUser.LoginAct || ur.Email == addUser.Email
            || ur.Phone == addUser.Phone);
            if (isEmpty != null)
            {
                if(isEmpty.Phone !=null && isEmpty.Phone == addUser.Phone)
                {
                    return ApiResultHelper.Error("手机号已被使用");
                }
                if (isEmpty.Email != null && isEmpty.Email == addUser.Email)
                {
                    return ApiResultHelper.Error("邮箱已被使用");
                }
                return ApiResultHelper.Error("账号已存在");
            }
            var tuser = _imapper.Map<TUser>(addUser);
            tuser.LoginPwd = Md5Helper.GetMD5Str(tuser.LoginPwd);
            tuser.CreateBy = createUserId;
            tuser.CreateTime = DateTime.Now;
            var result = await _userRepository.CreateAsync(tuser);
            if (!result) return ApiResultHelper.Error("服务器异常，添加用户失败");
            return ApiResultHelper.Success(result);
        }
      
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="editUser"></param>
        /// <param name="editUserId">修改操作人</param>
        /// <returns></returns>
        public async Task<ApiResult> UpdateUserAsync(EditUserRequest editUser, int editUserId)
        {
            string str = null;
            var sw = editUser.LoginPwd;
            bool s = string.IsNullOrEmpty(editUser.LoginPwd);
            if (!string.IsNullOrEmpty(editUser.LoginPwd) && editUser.LoginPwd.Length < 6)
            {
                return ApiResultHelper.Error("用户密码设置长度必须是6-18位");
            }
           
            var queryUser = await _userRepository.QueryAsync(editUser.Id);
            if(queryUser == null) return ApiResultHelper.Error("服务器异常，修改用户信息失败");
            if (!string.IsNullOrEmpty(editUser.LoginPwd))
            {
                var pwd = Md5Helper.GetMD5Str(editUser.LoginPwd);
                queryUser.LoginPwd = pwd;
            }
            queryUser.LoginAct = editUser.LoginAct;
            queryUser.Name = editUser.Name;
            queryUser.Phone = editUser.Phone;
            queryUser.Email = editUser.Email;
            queryUser.AccountEnabled = editUser.AccountEnabled;
            queryUser.AccountNoExpired = editUser.AccountNoExpired;
            queryUser.AccountNoLocked = editUser.AccountNoLocked;
            queryUser.CredentialsNoExpired = editUser.CredentialsNoExpired;
            queryUser.EditBy = editUserId;
            var result = await _userRepository.UpdateAsync(queryUser);
            if(!result) return ApiResultHelper.Error("服务器异常，修改用户信息失败");
            return ApiResultHelper.Success(result);
        }

        /// <summary>
        /// 批量删除用户
        /// </summary>
        /// <param name="delStr"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResult> DeleteUserAsync(string delStr)
        {
            if (string.IsNullOrEmpty(delStr)) return ApiResultHelper.Error("删除失败,传递的数据为null");
            var strArray = delStr.Split(',');
            List<int> idList = new List<int>();
            for (int i = 0; i < strArray.Length; i++)
            {
                idList.Add(int.Parse(strArray[i]));
            }
            var result = await _userRepository.BatchDeleteAsync(idList);
            if (result != idList.Count) return ApiResultHelper.Error("删除失败，该用户是否有依赖");
            return ApiResultHelper.Success(result);
        }

        /// <summary>
        /// 获取用户列表 分页 过滤
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResult> GetUserListAsync(int page, int currentId)
        {
            var res = await _userRepository.PageQueryFilterAsync(page,  currentId);
            return ApiResultHelper.Success(_options.Value.PageSize, res.Total, res.Data);
        }
    }
}