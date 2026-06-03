
using MyCRM.Common.ApiResult;
using MyCRM.Common.page;
using MyCRM.Models;
using MyCRM.Models.DTO;
using MyCRM.Models.Request;

namespace IService
{
    public interface ICustomerService : IBaseService<TCustomer>
    {
        public Task<ApiResult> CreateCustomer(CustomRequest customer, int currentId);

        public Task<ApiResult> GetCustomerPageListAsync(int currentPage, int currentId);

        public Task<List<CustomerDTO>> GetCustomerListAsync(int currentId);
        Task<List<CustomerDTO>> GetConditionCustomerListAsync(List<int> condition, int currentId);
        public Task<string> ImportExcelAllToFileAsync(int currentId, string pathPrefix);
        public Task<byte[]> ImportExcelAllToBytesAsync(int currentId); 
        public Task<byte[]> ImportExcelToBytesAsync(string str,int currentId);
    }

}
