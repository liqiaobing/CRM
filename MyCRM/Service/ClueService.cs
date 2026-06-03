using IRepository;
using IService;
using MyCRM.Models;
using AutoMapper;
using MyCRM.Common.ApiResult;
using MyCRM.Common;
using Microsoft.Extensions.Options;
using MyCRM.Models.Request;
using MyCRM.Models.DTO;
using MyCRM.Common.page;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using NPOI.SS.Util;
using NPOI.Util;
using Repository;
using NPOI.OpenXmlFormats;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Service
{
    public class ClueService : BaseService<TClue>, IClueService
    {
        private readonly IMapper _imapper;
        private readonly IOptions<SystemItem> _options;
        private readonly IClueRepository _clueRepository;
        private readonly IDicValueService _dicValueService;
        private readonly IProductService _productService;
        private readonly IActivityService _activityService;

        public ClueService(IClueRepository clueRepository, IMapper imapper, IDicValueService dicValueService, IProductService productService,
            IActivityService activityService, IOptions<SystemItem> options)
        {
            base._baseRepository = clueRepository;
            base._imapper = imapper;
            _imapper = imapper;
            _options = options;
            _clueRepository = clueRepository;
            _dicValueService = dicValueService;
            _productService = productService;
            _activityService = activityService;
        }

        /// <summary>
        /// 导入数据 Excel
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="currentId"></param>
        /// <returns></returns>
        /// <exception cref="RuntimeException"></exception>
        public async Task<ApiResult> BatchImportClues(string fileName, int currentId)
        {
            //操作文档的接口
            IWorkbook workbook;
            //获取扩展名
            string fileExt = Path.GetExtension(fileName).ToLower();
            //打开流
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                //XSSFWorkbook 适用XLSX格式，HSSFWorkbook 适用XLS格式
                if (fileExt == ".xlsx")
                {
                    workbook = new XSSFWorkbook(fs);
                }
                else if (fileExt == ".xls")
                {
                    workbook = new HSSFWorkbook(fs);
                }
                else
                {
                    workbook = null;
                    return ApiResultHelper.Error("请检查Excel中数据格式");
                }
                //Sheet总数量->：workbook.NumberOfSheets
                ISheet sheet = workbook.GetSheetAt(0);//获取第一页
                //获取数据库中TDicValue字典的键值对
                var dics = await _dicValueService.GetDicsListAsync();
                //获取数据中product的键值对
                var productDics = await _productService.GetDicsListAsync();
                //存储读到的数据
                List<TClue> clues = new List<TClue>();

                //数据  
                for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                {
                    TClue clue = new TClue();
                    try
                    {
                        clue.OwnerId = sheet.GetRow(i).GetCell(0).ToString() == "" ? null : int.Parse(sheet.GetRow(i).GetCell(0).ToString());
                        clue.ActivityId = sheet.GetRow(i).GetCell(1).ToString() == "" ? null : int.Parse(sheet.GetRow(i).GetCell(1).ToString());
                        clue.FullName = sheet.GetRow(i).GetCell(2).ToString();
                        clue.Appellation = sheet.GetRow(i).GetCell(3).ToString() == "" ? null : FindDicElment(sheet.GetRow(i).GetCell(3).ToString(), dics);
                        clue.Phone = sheet.GetRow(i).GetCell(4).ToString();
                        clue.Weixin = sheet.GetRow(i).GetCell(5).ToString();
                        clue.Qq = sheet.GetRow(i).GetCell(6).ToString();
                        clue.Email = sheet.GetRow(i).GetCell(7).ToString();
                        clue.Age = sheet.GetRow(i).GetCell(8).ToString() == "" ? null : int.Parse(sheet.GetRow(i).GetCell(8).ToString());
                        clue.Job = sheet.GetRow(i).GetCell(9).ToString();
                        clue.YearIncome = sheet.GetRow(i).GetCell(10).ToString() == "" ? null : decimal.Parse(sheet.GetRow(i).GetCell(10).ToString());
                        clue.Address = sheet.GetRow(i).GetCell(11).ToString();
                        clue.NeedLoan = sheet.GetRow(i).GetCell(12).ToString() == "" ? null : FindDicElment(sheet.GetRow(i).GetCell(12).ToString(), dics);
                        clue.IntentionState = sheet.GetRow(i).GetCell(13).ToString() == "" ? null : FindDicElment(sheet.GetRow(i).GetCell(13).ToString(), dics);
                        clue.IntentionProduct = sheet.GetRow(i).GetCell(14).ToString() == "" ? null : FindDicElment(sheet.GetRow(i).GetCell(14).ToString(), productDics);
                        clue.State = sheet.GetRow(i).GetCell(15).ToString() == "" ? null : FindDicElment(sheet.GetRow(i).GetCell(15).ToString(), dics);
                        clue.Source = sheet.GetRow(i).GetCell(16).ToString() == "" ? null : FindDicElment(sheet.GetRow(i).GetCell(16).ToString(), dics);
                        clue.Description = sheet.GetRow(i).GetCell(17).ToString();
                        clue.NextContactTime = sheet.GetRow(i).GetCell(18).ToString() == "" ? null : DateTime.Parse(sheet.GetRow(i).GetCell(18).DateCellValue.ToString("yyyy/MM/dd HH:mm:ss"));
                        clue.CreateTime = DateTime.Now;
                        clue.CreateBy = currentId;
                    }catch(RuntimeException ex) 
                    {
                        throw new RuntimeException("解析Excel赋值时异常");
                    }
                    clues.Add(clue);
                }
                var res = await _clueRepository.BatchCreateAsync(clues);

                return ApiResultHelper.Success("上传成功");
            }
        }
     

        /// <summary>
        /// 线索列表
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="currentId"></param>
        /// <returns></returns>
        public async Task<ApiResult> GetClueLsitAsync(int currentPage, int currentId)
        {
            var res = await _clueRepository.GetClueLsitAsync(currentPage, _options.Value.PageSize, currentId);
            return ApiResultHelper.Success(_options.Value.PageSize, res.Total, res.Data);
        }

    
        /// <summary>
        /// 获取字典中的value  id
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        static int FindDicElment(string key, Dictionary<string, int> dic)
        {
            foreach(var i  in dic.Keys)
            {
                if (i.Contains(key))
                {
                    return dic.GetValueOrDefault(i);
                }
            }
           
            return 0;
        }


        /// <summary>
        /// 加载字典类型 value
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public async Task<ApiResult> GetDicValueAsync(string parameter, int? id = null)
        {
            List<TDicValue> dics = null;
            if (parameter == "Product")
            {
                //懒得写了，直接调用之前写的过来改
                List<DicValueDTO> proDto = new List<DicValueDTO>();
                var dic = await _productService.GetDicsListAsync();
                foreach(var item in dic)
                {
                    proDto.Add(new DicValueDTO { Id = item.Value, TypeValue = item.Key });
                }
                return ApiResultHelper.Success(proDto);
            }else if (parameter == "Activity")
            {
                List<TActivity> dic = new List<TActivity>();
                DateTime nowTime = DateTime.Now;
                dic = await _activityService.QuerysAsync(p => p.StartTime <= nowTime && p.EndTime >= nowTime);
                var actDto = _imapper.Map<List<DicValueDTO>>(dic);
                return ApiResultHelper.Success(actDto);
            }else if (parameter == "Activitys")
            {
                List<TActivity> dic = new List<TActivity>();
                DateTime nowTime = DateTime.Now;
                dic = await _activityService.QuerysAsync(p => p.Id == (int)id || (p.StartTime <= nowTime && p.EndTime >= nowTime));
                var actDto = _imapper.Map<List<DicValueDTO>>(dic);
                return new ApiResult { Data = actDto };
            }
            else
            {
                dics = await _dicValueService.QuerysAsync(p => p.TypeCode == parameter);
            }
            if (dics.Count <= 0) return ApiResultHelper.Error("加载字典失败");
            var dicDto = _imapper.Map<List<DicValueDTO>>(dics);
            return ApiResultHelper.Success(dicDto);
        }

        /// <summary>
        /// 添加线索
        /// </summary>
        /// <param name="request"></param>
        /// <param name="currentId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResult> CreateClueAsync(ClueRequest request, int currentId)
        {
            if (request == null) return ApiResultHelper.Error("请求数据异常");
            var clue = _imapper.Map<TClue>(request);
            clue.CreateTime = DateTime.Now;
            clue.CreateBy = currentId;
            var res = await _clueRepository.CreateAsync(clue);
            if(!res) return ApiResultHelper.Error("添加失败");
            return ApiResultHelper.Success(res);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="request"></param>
        /// <param name="currentId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResult> UpdateClueAsync(CluePutRequest request, int currentId)
        {
            var clue = await _clueRepository.QueryAsync(request.Id);
            if(clue == null) return ApiResultHelper.Error("请求数据异常");
            clue.OwnerId = request.OwnerId;
            clue.ActivityId = request.ActivityId;
            clue.FullName = request.FullName;
            clue.Appellation = request.Appellation;
            clue.Phone = request.Phone;
            clue.Weixin = request.Weixin;
            clue.Qq = request.Qq;
            clue.Email = request.Email;
            clue.Age = request.Age;
            clue.Job = request.Job;
            clue.YearIncome = request.YearIncome;
            clue.Address = request.Address;
            clue.NeedLoan = request.NeedLoan;
            clue.IntentionState = request.IntentionState;
            clue.IntentionProduct = request.IntentionProduct;
            clue.State = request.State;
            clue.Source = request.Source;
            clue.Description = request.Description;
            clue.NextContactTime = request.NextContactTime;
            clue.EditTime = DateTime.Now;
            clue.EditBy = currentId;
            var res = await _clueRepository.UpdateAsync(clue);
            if (!res) return ApiResultHelper.Error("更新失败");
            return ApiResultHelper.Success(res);
        }

        /// <summary>
        /// 获取clue详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResult> GetClueAsync(int id)
        {
            var clueList = await _clueRepository.GetClueDetailAsync(id);
            if (clueList == null) return ApiResultHelper.Error("没有找到数据");
            return ApiResultHelper.Success(clueList);
        }
        




        /// <summary>
        /// 提交clueNoteContent
        /// </summary>
        /// <param name="request"></param>
        /// <param name="currentId"></param>
        /// <returns></returns>
        public async Task<ApiResult> AddClueRemarkAsync(ClueRemarkRequest request, int currentId)
        {
            var clueRem =_imapper.Map<TClueRemark>(request);
            clueRem.CreateBy = currentId;
            clueRem.CreateTime = DateTime.Now;
            clueRem.Deleted = 0;
            var res = await _clueRepository.CreateClueRemarkAsync(clueRem);
            if(!res) return ApiResultHelper.Error("提交失败");
            return ApiResultHelper.Success(res);
        }

        /// <summary>
        /// 获取线索备注列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="currentPage"></param>
        /// <param name="currentId"></param>
        /// <returns></returns>
        public async Task<ApiResult> GetClueRemarkListAsync(int id, int currentPage, int currentId)
        {
            var pageCfg = await _clueRepository.GetClueRemarkListAsync(id, currentPage, _options.Value.PageSize, currentId);
            return ApiResultHelper.Success(_options.Value.PageSize,pageCfg.Total,pageCfg.Data);
        }

        public async Task<ApiResult> ClueRemarkDelAsync(int id, int currentId)
        {
            var clue = await _clueRepository.QueryClueRemarkAsync(id);
            clue.EditBy = currentId;
            clue.EditTime = DateTime.Now;
            clue.Deleted = 1;
            var res = await _clueRepository.UpdateClueRemarkAsync(clue);
            if (!res) return ApiResultHelper.Error("提交失败");
            return ApiResultHelper.Success(res);
        }

        public async Task<ApiResult> UpdateDelClueRemarkAsync(ClueRemarkRequest request, int currentId)
        {
            var clue = await _clueRepository.QueryClueRemarkAsync(request.Id);
            clue.NoteContent = request.NoteContent;
            clue.NoteWay = request.NoteWay;
            clue.EditBy = currentId;
            clue.EditTime = DateTime.Now;
            var res = await _clueRepository.UpdateClueRemarkAsync(clue);
            if (!res) return ApiResultHelper.Error("提交失败");
            return ApiResultHelper.Success(res);
        }

        public async Task<ApiResult> GetClueRemarkAsync(int id)
        {
            var clue = await _clueRepository.QueryClueRemarkAsync(id);
            if (clue == null) return ApiResultHelper.Error("获取数据异常");
            Dictionary<string, dynamic> data = new Dictionary<string, dynamic>();
            data.Add("id", clue.Id);
            data.Add("noteContent", clue.NoteContent);
            data.Add("noteWay", clue.NoteWay);
            return ApiResultHelper.Success(data);
        }
    } 
}