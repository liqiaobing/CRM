
using MyCRM.Common.ApiResult;
using MyCRM.Common.page;
using MyCRM.Models;
using MyCRM.Models.Request;

namespace IService
{
    public interface IClueService : IBaseService<TClue>
    {
        public Task<ApiResult> GetClueLsitAsync(int currentPage, int currentId);

        public Task<ApiResult> BatchImportClues(string fileName, int currentId);

        public Task<ApiResult> GetDicValueAsync(string parameter, int? id = null);

        public Task<ApiResult> CreateClueAsync(ClueRequest request, int currentId);

        public Task<ApiResult> UpdateClueAsync(CluePutRequest request, int currentId);

        public Task<ApiResult> GetClueAsync(int id);




        public Task<ApiResult> AddClueRemarkAsync(ClueRemarkRequest request, int currentId);
        public Task<ApiResult> GetClueRemarkListAsync(int id, int currentPage, int currentId);
        public Task<ApiResult> ClueRemarkDelAsync(int id, int currentId);
        public Task<ApiResult> UpdateDelClueRemarkAsync(ClueRemarkRequest request, int currentId);
        public Task<ApiResult> GetClueRemarkAsync(int id);
    }

}
