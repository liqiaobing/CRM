
using MyCRM.Common.ApiResult;
using MyCRM.Common.page;
using MyCRM.Models;
using MyCRM.Models.DTO;
using MyCRM.Models.Request;

namespace IService
{
    public interface IUserService : IBaseService<TUser>
    {
        public Task<string> LoginAsync(TUser user);

        public Task<bool> OutLoginAsync(int id);

        public Task<TUserDTO> GetUserInfoAsync(int id);

        public Task<ApiResult> AddUserAsync(AddUserRequest addUser, int createUserId);
        public Task<ApiResult> UpdateUserAsync(EditUserRequest editUser, int editUserId);
        public Task<ApiResult> DeleteUserAsync(string delStr);
        public Task<ApiResult> GetUserListAsync(int page, int currentId);

    }
}
