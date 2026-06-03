using MyCRM.Common;
using MyCRM.Common.ApiResult;
using MyCRM.Common.page;
using MyCRM.Models;
using MyCRM.Models.DTO;
using MyCRM.Models.Request;
using System;
using System.Linq.Expressions;



namespace IRepository
{
    public interface IClueRepository : IBaseRepository<TClue>
    {
        public Task<PageConfig> GetClueLsitAsync(int currentPage, int pageSize, int currentId);

        public Task<bool> BatchCreateAsync(List<TClue> clues);

        public Task<dynamic> GetClueDetailAsync(int id);

        public Task<bool> CreateClueRemarkAsync(TClueRemark clueRemark);

        public Task<PageConfig> GetClueRemarkListAsync(int id, int currentPage, int pageSize, int currentId);

        public  Task<TClueRemark> QueryClueRemarkAsync(int id);
        public  Task<bool> UpdateClueRemarkAsync(TClueRemark clue);
    }
}
