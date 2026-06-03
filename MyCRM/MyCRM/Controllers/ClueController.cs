
using MyCRM.Common.ApiResult;
using IService;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MyCRM.Common.excel;
using Service;
using MyCRM.Models.Request;
using AutoMapper;
using MyCRM.Models.DTO;
using Org.BouncyCastle.Asn1.Ocsp;

namespace MyCRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClueController : ControllerBase
    {
        private readonly IClueService _clueService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IDicValueService _dicValueService;
        private readonly IMapper _iMapper;

        public ClueController(IClueService service, IDicValueService dicValueService, IWebHostEnvironment webHostEnvironment,
            IMapper iMapper) 
        {
            _clueService = service;
            _webHostEnvironment = webHostEnvironment;
            _dicValueService = dicValueService;
            _iMapper = iMapper;
        }

        [HttpGet("Clue/{currentPage}")]
        public async Task<ActionResult<ApiResult>> ClueList(int currentPage)
        {
            var currentId = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var res = await _clueService.GetClueLsitAsync(currentPage, currentId);
            return res;
        }

        /// <summary>
        /// 处理文件上传
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>

        //public async Task<ActionResult<ApiResult>> UploadFile([FromForm] List<IFormFile> formFile)
        [HttpPost("Clue")]
        public async Task<ActionResult<ApiResult>> UploadFile(IFormFile formFile)
        {
            int currentId = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            string fileName = string.Empty;
            if (formFile == null) return ApiResultHelper.Error("请先选择文件");
            if (formFile.Length > 1024 * 1024 * 50) return ApiResultHelper.Error($"上传的文件 “{formFile.Name}” 超过50MB");
            fileName = await ProcessUploadAsync(formFile);
            if (fileName == null) return ApiResultHelper.Error("上传失败");
            var res = await _clueService.BatchImportClues(fileName, currentId);
            return res;
        }

        private async Task<string> ProcessUploadAsync(IFormFile file)
        {
            /* string ContentType { get; } //类型
            string ContentDisposition { get; } //位置
            IHeaderDictionary Headers { get; } //头
            long Length { get; }    //文件长度
            string Name { get; }    //文件名不带后缀
            string FileName { get; }//文件名带后缀
            void CopyTo(Stream target);//同步复制到指定路径
            Task CopyToAsync(Stream target, CancellationToken cancellationToken = default);//异步复制到指定路径
            Stream OpenReadStream();//打开读取流*/

            string fileName = string.Empty;
            
            //拼接存放服务位置 hostEnvironment.WebRootPath wwwroot目录 
            //string prefix = Path.Combine(_webHostEnvironment.WebRootPath, "uploads\\clue_excels");
            string prefix = Path.Combine(_webHostEnvironment.ContentRootPath, "uploads\\clue_excels");
            //文件名
            fileName = Guid.NewGuid().ToString() + file.FileName;
            //写入路径+文件名
            string savePath = Path.Combine(prefix, fileName);
            //文件夹不存在时传教
            if (!Directory.Exists(prefix)) Directory.CreateDirectory(prefix).Create();

            //写入
            using (Stream s = new FileStream(savePath, FileMode.Create))
            {
                await file.CopyToAsync(s);
            }
            return savePath;
        }


        /// <summary>
        /// 获取字典
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [HttpGet("Detail/GetSelected/{parameter}")]
        public async Task<ActionResult<ApiResult>> ClueList(string parameter)
        {
            var res = await _clueService.GetDicValueAsync(parameter);
            return res;
        }

        /// <summary>
        /// 检查手机号
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <returns></returns>
        [HttpGet("Detail/CheckPhone/{phoneNum}")]
        public async Task<ActionResult<ApiResult>> Check(string phoneNum)
        {
            var res = await _clueService.QueryFirstAsync(p => p.Phone == phoneNum);
            if(res == null) { return ApiResultHelper.Success("ok"); }
            return ApiResultHelper.Error("手机号已被注册");
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("AddClue")]
        public async Task<ActionResult<ApiResult>> AddClue([FromForm]ClueRequest request)
        {
            var currentId = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return await _clueService.CreateClueAsync(request, currentId);
        }

        /// <summary>
        /// 获取数据 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Edit/{id}")]
        public async Task<ActionResult<ApiResult>> Edit(int id)
        {
            var res = await _clueService.QueryAsync(id);
            if(res == null) return ApiResultHelper.Error("获取数据失败");
            var dto = _iMapper.Map<ClueDTO>(res);
            //获取对应的活动字典和 当前时间段有效的活动
            var dic = await _clueService.GetDicValueAsync("Activitys", dto.ActivityId);
            dto.keyValuePairs = dic.Data;
            return ApiResultHelper.Success(dto);
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Detail/{id}")]
        public async Task<ActionResult<ApiResult>> Detail(int id)
        {
            var res = await _clueService.GetClueAsync(id);
            return res;
        }

        /// <summary>
        /// 编辑线索
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("Clue")]
        public async Task<ActionResult<ApiResult>> Edit([FromForm] CluePutRequest request)
        {
            var currentId = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var res = await _clueService.UpdateClueAsync(request, currentId);
            return res;
        }

        /// <summary>
        /// 添加clueRemark notecontent
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("ClueRemark")]
        public async Task<ActionResult<ApiResult>> AddCludeRemark(ClueRemarkRequest request)
        {
            var currentId = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var res = await _clueService.AddClueRemarkAsync(request, currentId);
            return ApiResultHelper.Success(res);
        }

        /// <summary>
        /// 获取clueRemark notecontentList
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("ClueRemark/{id}/{currentPage}")]
        public async Task<ActionResult<ApiResult>> ClueRemarkList(int id, int currentPage)
        {
            var currentId = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var res = await _clueService.GetClueRemarkListAsync(id, currentPage, currentId);
            return res;
        }

        /// <summary>
        /// 更新 clueRemark
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("ClueRemark")]
        public async Task<ActionResult<ApiResult>> EditClueRemark(ClueRemarkRequest request)
        {
            var currentId = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var res = await _clueService.UpdateDelClueRemarkAsync(request, currentId);
            return res;
        }

        /// <summary>
        /// 软删除 clueRemark
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("ClueRemark/{id}")]
        public async Task<ActionResult<ApiResult>> DelClueRemark(int id)
        {
            var currentId = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var res = await _clueService.ClueRemarkDelAsync(id, currentId);
            return res;
        }

        /// <summary>
        /// 获取notecontent
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("ClueRemarkEdit/{id}")]
        public async Task<ActionResult<ApiResult>> GetClueRemark(int id)
        {
            return await _clueService.GetClueRemarkAsync(id);
        }
    }
}
