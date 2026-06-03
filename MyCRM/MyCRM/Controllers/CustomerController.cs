
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
using System.Web;
using System.IO;
using System.Net;
using System.Net.Http.Headers;

namespace MyCRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _iMapper;
        private readonly ICustomerService _customerService;

        public CustomerController(IWebHostEnvironment webHostEnvironment, IMapper iMapper, ICustomerService customerService) 
        {
            _webHostEnvironment = webHostEnvironment;
            _iMapper = iMapper;
            _customerService = customerService;
        }

        [HttpPost("Custom")]
        public async Task<ActionResult<ApiResult>> AddCustom(CustomRequest request)
        {
            var currentId = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var res = await _customerService.CreateCustomer(request, currentId);
            return res;
        }

        [HttpGet("GetCustomerList/{currentPage}")]
        public async Task<ActionResult<ApiResult>> GetCstList(int currentPage)
        {
            var currentId = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var res = await _customerService.GetCustomerPageListAsync(currentPage, currentId);
            return res;
        }

        /// <summary>
        /// 导出对应用户所有的客户到excel文件
        /// </summary>
        /// <returns></returns>
        [HttpGet("ImportExcelAll")]
        public async Task<FileContentResult> ImportExcelAll()
        {
            var currentId = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            string excelPath = Path.Combine(_webHostEnvironment.ContentRootPath, "excels");
            if (!Directory.Exists(excelPath))
            {
                Directory.CreateDirectory(excelPath).Create();
            }
            //var filePath = await _customerService.ImportExcelAllToFileAsync(currentId, excelPath);
            var bytes = await _customerService.ImportExcelAllToBytesAsync(currentId);

            //将字符串转为utf8给前端解码，避免中文乱码
            var fileName = HttpUtility.UrlEncode("客户信息" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx", System.Text.Encoding.UTF8);
            
            //Response.Headers.Add($"Content-Disposition", "attachment; filename={fileName}");//给请求头加文件名
            
            //开放CotentDescription给前端代码访问
            Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
            //"application/octet-stream": 这是响应的 MIME 类型，表示二进制流数据。对于下载文件，通常使用这个 MIME 类型。
            return File(bytes, "application/octet-stream", fileName);
        }

        /// <summary>
        /// 导出对应用户所选定的客户到excel文件
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [HttpGet("ImportExcel/{str}")]
        public async Task<ActionResult<ApiResult>> ImportExcel(string str)
        {
            var currentId = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var bytes = await _customerService.ImportExcelToBytesAsync(str, currentId);
            if(bytes == null) return NotFound();
            var fileName = HttpUtility.UrlEncode("客户信息" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx", System.Text.Encoding.UTF8);
            //开放
            Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
            return File(bytes, "application/octet-stream", fileName);
        }
    }
}
