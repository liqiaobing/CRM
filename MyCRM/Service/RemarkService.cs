using IRepository;
using IService;
using MyCRM.Models;
using AutoMapper;
using MyCRM.Common.ApiResult;
using MyCRM.Common;
using Microsoft.Extensions.Options;
using MyCRM.Models.Request;
using MyCRM.Models.DTO;

namespace Service
{
    public class RemarkService : BaseService<TActivityRemark>, IRemarkService
    {
        private readonly IMapper _imapper;
        private readonly IOptions<SystemItem> _options;
        private readonly IRemarkRepository _remarkRepository;

        public RemarkService(IRemarkRepository remarkRepository,IMapper imapper, IOptions<SystemItem> options )
        {
            base._imapper = imapper;
            _imapper = imapper;
            _options = options;
            _remarkRepository = remarkRepository;
        }


        /// <summary>
        /// 添加备注
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ApiResult> CreateActivityAsync(RemarkRequest request, int createBy)
        {
            var remark = new TActivityRemark
            {
                ActivityId = request.id,
                CreateTime = DateTime.Now,
                CreateBy = createBy,
                Deleted = 0,
                NoteContent = request.noteContent
            };
            var res = await _remarkRepository.CreateAsync(remark);
            if (!res) return ApiResultHelper.Error("添加失败");
            return ApiResultHelper.Success(res);
        }

        /// <summary>
        /// 获取id和备注内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResult> GetNoteCotentAsync(int id)
        {
            var res = await _remarkRepository.QueryAsync(id);
            Dictionary<string, dynamic> keyValuePairs = new Dictionary<string, dynamic>();
            keyValuePairs.Add("id", res.Id);
            keyValuePairs.Add("noteContent", res.NoteContent);
            return ApiResultHelper.Success(keyValuePairs);
        }

        /// <summary>
        /// 获取所有的备注数据 带条件和权限过滤
        /// </summary>
        /// <param name="actId"></param>
        /// <param name="currentId"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public async Task<ApiResult> QueryNoteContentAsync(int actId, int currentId, int currentPage)
        {
            //在数据库使用select完成了映射
            var res = await _remarkRepository.QueryNoteContentAsync(actId, currentId, currentPage);
            //var result = _imapper.Map<List<TActivityRemarkDTO>>(res.Data);
            return ApiResultHelper.Success(res.Total, res.Data);
        }

        /// <summary>
        /// 更新备注内容
        /// </summary>
        /// <param name="request"></param>
        /// <param name="EditId"></param>
        /// <returns></returns>
        public async Task<ApiResult> UpdateNoteContentAsync(RemarkRequest request, int EditId)
        {
            var remark = await _remarkRepository.QueryAsync(request.id);
            remark.NoteContent = request.noteContent;
            remark.EditTime = DateTime.Now;
            remark.EditBy = EditId;
            var res = await _remarkRepository.UpdateAsync(remark);
            return ApiResultHelper.Success(res);
        }

        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResult> DeletedAsync(int id)
        {
            var remark = await _remarkRepository.QueryAsync(id);
            remark.Deleted = 1;
            var result = await _remarkRepository.UpdateAsync(remark);
            if(!result) ApiResultHelper.Error("删除失败");
            return ApiResultHelper.Success(result); 
        }
    } 
}