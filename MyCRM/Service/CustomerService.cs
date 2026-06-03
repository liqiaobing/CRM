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
using NPOI.SS.Formula.Functions;
using NPOI.XWPF.UserModel;
using System.Collections.Generic;

namespace Service
{
    public class CustomerService : BaseService<TCustomer>, ICustomerService
    {
        private readonly IMapper _imapper;
        private readonly IOptions<SystemItem> _options;
        private readonly ICustomerRepository _customRepository;
        private readonly IClueService _clueService;

        public CustomerService(ICustomerRepository customerRepository, IClueService clueService, IMapper imapper, IOptions<SystemItem> options)
        {
            base._baseRepository = customerRepository;
            base._imapper = imapper;
            _imapper = imapper;
            _options = options;
            _customRepository = customerRepository;
            _clueService = clueService;
        }

        /// <summary>
        /// 创建客户信息 
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="currentId"></param>
        /// <returns></returns>
        public async Task<ApiResult> CreateCustomer(CustomRequest customer, int currentId)
        {
            var cst = _imapper.Map<TCustomer>(customer);
            cst.CreateBy = currentId;
            cst.CreateTime = DateTime.Now;
            var res = await _customRepository.CreateAsync(cst);
            if (!res) return ApiResultHelper.Error("转为客户失败");
            //ok后将线索表更新
            var clue = await _clueService.QueryAsync((int)cst.ClueId);
            clue.EditBy = currentId;
            clue.EditTime = DateTime.Now;
            clue.IntentionProduct = cst.Product;
            clue.State = -1;//-1代表已转客户
            res = await _clueService.UpdateAsync(clue);
            if (!res) return ApiResultHelper.Error("转为客户后修改线索失败");
            return ApiResultHelper.Success(res);
        }

           /// <summary>
           /// 客户列表分页
           /// </summary>
           /// <param name="currentPage"></param>
           /// <param name="currentId"></param>
           /// <returns></returns>
        public async Task<ApiResult> GetCustomerPageListAsync(int currentPage, int currentId)
        {
            var page = await _customRepository.GetCstPageListAsync(currentPage, currentId);
            return ApiResultHelper.Success(_options.Value.PageSize, page.Total, page.Data);
        }

        /// <summary>
        /// 查询需要导出的数据
        /// </summary>
        /// <param name="currentId"></param>
        /// <returns></returns>
        public async Task<List<CustomerDTO>> GetCustomerListAsync(int currentId)
        {
            var data = await _customRepository.GetCstListAsync(currentId);
            if (data == null) return null;
            List<CustomerDTO> list = data;
            return list;
        }

        /// <summary>
        /// 查询需要导出的部分数据
        /// </summary>
        /// <param name="currentId"></param>
        /// <returns></returns>
        public async Task<List<CustomerDTO>> GetConditionCustomerListAsync(List<int> condition, int currentId)
        {
            var data = await _customRepository.GetCstConditionListAsync(condition, currentId);
            if (data == null) return null;
            List<CustomerDTO> list = data;
            return list;
        }

        /// <summary>
        /// 导出所有的数据写入excel 返回文件路径
        /// </summary>
        /// <param name="currentId"></param>
        /// <param name="pathPrefix"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> ImportExcelAllToFileAsync(int currentId, string pathPrefix)
        {
            var list = await GetCustomerListAsync(currentId);
            string path = Path.Combine(pathPrefix, "客户信息" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx");
            var res = GenerateExcelFile(list, path);
            if (res) return path;
            throw new Exception("导出excel失败");
        }

        /// <summary>
        /// 导出所有的数据写入excel 返回字节数组
        /// </summary>
        /// <param name="currentId"></param>
        /// <param name="pathPrefix"></param>
        /// <returns></returns>
        public async Task<byte[]> ImportExcelAllToBytesAsync(int currentId)
        {
            var resList = await GetCustomerListAsync(currentId);
            if (resList == null) return null;
            var bytes = GenerateExcelToArray(resList);
            return bytes;
        }

        /// <summary>
        /// 导出部分数据 excel 返回字节数组
        /// </summary>
        /// <param name="str">前端给过来的条件字符串用来分割成数组的</param>
        /// <param name="currentId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<byte[]> ImportExcelToBytesAsync(string str, int currentId)
        {
            if (string.IsNullOrEmpty(str)) return null;
            var strArray = str.Split(',');
            List<int> idList = new List<int>();
            for (int i = 0; i < strArray.Length; i++)
            {
                idList.Add(int.Parse(strArray[i]));
            }
            var resList = await GetConditionCustomerListAsync(idList, currentId);
            if (resList == null) return null;
            var bytes = GenerateExcelToArray(resList);
            return bytes;
        }

        /// <summary>
        /// 写入数据生产Excel文件
        /// </summary>
        /// <param name="list">数据源</param>
        /// <param name="filePath">文件的路径</param>
        /// <returns></returns>
        public static bool GenerateExcelFile(List<CustomerDTO> list, string filePath)
        {
            try
            {
                //创建一个工作薄  
                IWorkbook workbook = new HSSFWorkbook();
                //获取文件后缀
                string extension = Path.GetExtension(filePath);
                //根据指定的文件格式创建对应的类
                if (extension.Equals(".xls"))
                {
                    workbook = new HSSFWorkbook();
                }
                else
                {
                    workbook = new XSSFWorkbook();
                }
                //创建一个 sheet 表
                ISheet sheet = workbook.CreateSheet();
                /*   //创建一行
                   IRow rowH = sheet.CreateRow(0);
                   //创建一个单元格
                   ICell cell = null;

                   //创建单元格样式
                   ICellStyle cellStyle = workbook.CreateCellStyle();

                   //创建格式
                   IDataFormat dataFormat = workbook.CreateDataFormat();

                   //设置为文本格式，也可以为text，即dataFormat.GetFormat("text")
                   cellStyle.DataFormat = dataFormat.GetFormat("0");*/

                /*     //设置列名
                     foreach (DataColumn col in dt.Columns)
                     {
                         //创建单元格并设置单元格内容
                         rowH.CreateCell(col.Ordinal).SetCellValue(col.Caption);

                         //设置单元格格式
                         rowH.Cells[col.Ordinal].CellStyle = cellStyle;
                     }*/
                /*   //写入数据
                for (int i = 0; i < list.Count; i++)
                {
                    //跳过第一行，第一行为列名
                    IRow row = sheet.CreateRow(i + 1);
                    cell = row.CreateCell(0);
                    cell = row.CreateCell(1);
                    cell = row.CreateCell(2);
                    cell = row.CreateCell(3);
                    cell = row.CreateCell(5);
                    cell = row.CreateCell(6);
                    cell = row.CreateCell(7);
                    cell = row.CreateCell(8);
                    cell = row.CreateCell(9);
                    cell.SetCellValue(dt.Rows[i][j].ToString());
                    cell.CellStyle = cell.CellStyle;

                  *//*  for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        cell = row.CreateCell(j);
                        cell.SetCellValue(dt.Rows[i][j].ToString());
                        cell.CellStyle = cell.CellStyle;
                    }*//*
                }*/

                //NPOI.SS.UserModel.IRow row1 = sheet.CreateRow(0);
                IRow row1 = sheet.CreateRow(0);
                row1.CreateCell(0).SetCellValue("所属人");
                row1.CreateCell(1).SetCellValue("所属活动");
                row1.CreateCell(2).SetCellValue("客户姓名");
                row1.CreateCell(3).SetCellValue("客户称呼");
                row1.CreateCell(4).SetCellValue("客户手机");
                row1.CreateCell(5).SetCellValue("客户微信");
                row1.CreateCell(6).SetCellValue("客户QQ");
                row1.CreateCell(7).SetCellValue("客户邮箱");
                row1.CreateCell(8).SetCellValue("客户客户年龄");
                row1.CreateCell(9).SetCellValue("客户职业");
                row1.CreateCell(10).SetCellValue("客户年收入");
                row1.CreateCell(11).SetCellValue("客户住址");
                row1.CreateCell(12).SetCellValue("是否贷款");
                row1.CreateCell(13).SetCellValue("客户产品");
                row1.CreateCell(14).SetCellValue("客户来源");
                row1.CreateCell(15).SetCellValue("客户描述");
                row1.CreateCell(16).SetCellValue("下次联系时间");


                //将数据逐步写入sheet1各个行
                for (int i = 0; i < list.Count; i++)
                {
                    //跳过第一行，第一行为列名
                    //NPOI.SS.UserModel.IRow rowtemp = sheet.CreateRow(i + 1);
                    IRow rowtemp = sheet.CreateRow(i + 1);
                    rowtemp.CreateCell(0).SetCellValue(list[i].OwnerName);
                    rowtemp.CreateCell(1).SetCellValue(list[i].ActivityName);
                    rowtemp.CreateCell(2).SetCellValue(list[i].FullName);
                    rowtemp.CreateCell(3).SetCellValue(list[i].Appellation);
                    rowtemp.CreateCell(4).SetCellValue(list[i].Phone);
                    rowtemp.CreateCell(5).SetCellValue(list[i].Weixin);
                    rowtemp.CreateCell(6).SetCellValue(list[i].Qq);
                    rowtemp.CreateCell(7).SetCellValue(list[i].Email);
                    //rowtemp.CreateCell(8).SetCellValue(list[i].Age);
                    rowtemp.CreateCell(8).SetCellValue(list[i].Age.ToString());
                    //rowtemp.CreateCell(8).CopyCellTo((int)list[i].Age); 
                    rowtemp.CreateCell(9).SetCellValue(list[i].Job);
                    //rowtemp.CreateCell(10).SetCellValue(list[i].YearIncome);
                    rowtemp.CreateCell(10).SetCellValue(list[i].YearIncome.ToString());
                    rowtemp.CreateCell(11).SetCellValue(list[i].Address);
                    rowtemp.CreateCell(12).SetCellValue(list[i].NeedLoan);
                    rowtemp.CreateCell(13).SetCellValue(list[i].ProductName);
                    rowtemp.CreateCell(14).SetCellValue(list[i].Source);
                    rowtemp.CreateCell(15).SetCellValue(list[i].Description);
                    //rowtemp.CreateCell(16).SetCellValue(list[i].NextContactTime);
                    //rowtemp.CreateCell(16).DateCellValue.ToOADate();
                    rowtemp.CreateCell(16).SetCellValue(list[i].NextContactTime.ToString());
                }
                //设置新建文件路径及名称
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                //创建文件
                //FileStream file = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write);
                FileStream fs = File.OpenWrite(filePath);
                workbook.Write(fs);//向打开的这个Excel文件中写入表单并保存。  
                fs.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 写入数据生成内存 excel到字节数字中
        /// </summary>
        /// <param name="list"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static byte[] GenerateExcelToArray(List<CustomerDTO> list)
        {
            try
            {
                //创建一个工作薄  
                IWorkbook workbook = new HSSFWorkbook();
                /* //获取文件后缀
                 string extension = Path.GetExtension(filePath);
                 //根据指定的文件格式创建对应的类
                 if (extension.Equals(".xls"))
                 {
                     workbook = new HSSFWorkbook();
                 }
                 else
                 {
                 }*/
                //创建一个 sheet 表

                workbook = new XSSFWorkbook();
                ISheet sheet = workbook.CreateSheet();
         
                //NPOI.SS.UserModel.IRow row1 = sheet.CreateRow(0);
                IRow row1 = sheet.CreateRow(0);
                row1.CreateCell(0).SetCellValue("所属人");
                row1.CreateCell(1).SetCellValue("所属活动");
                row1.CreateCell(2).SetCellValue("客户姓名");
                row1.CreateCell(3).SetCellValue("客户称呼");
                row1.CreateCell(4).SetCellValue("客户手机");
                row1.CreateCell(5).SetCellValue("客户微信");
                row1.CreateCell(6).SetCellValue("客户QQ");
                row1.CreateCell(7).SetCellValue("客户邮箱");
                row1.CreateCell(8).SetCellValue("客户客户年龄");
                row1.CreateCell(9).SetCellValue("客户职业");
                row1.CreateCell(10).SetCellValue("客户年收入");
                row1.CreateCell(11).SetCellValue("客户住址");
                row1.CreateCell(12).SetCellValue("是否贷款");
                row1.CreateCell(13).SetCellValue("客户产品");
                row1.CreateCell(14).SetCellValue("客户来源");
                row1.CreateCell(15).SetCellValue("客户描述");
                row1.CreateCell(16).SetCellValue("下次联系时间");


                //将数据逐步写入sheet1各个行
                for (int i = 0; i < list.Count; i++)
                {
                    //跳过第一行，第一行为列名
                    //NPOI.SS.UserModel.IRow rowtemp = sheet.CreateRow(i + 1);
                    IRow rowtemp = sheet.CreateRow(i + 1);
                    rowtemp.CreateCell(0).SetCellValue(list[i].OwnerName);
                    rowtemp.CreateCell(1).SetCellValue(list[i].ActivityName);
                    rowtemp.CreateCell(2).SetCellValue(list[i].FullName);
                    rowtemp.CreateCell(3).SetCellValue(list[i].Appellation);
                    rowtemp.CreateCell(4).SetCellValue(list[i].Phone);
                    rowtemp.CreateCell(5).SetCellValue(list[i].Weixin);
                    rowtemp.CreateCell(6).SetCellValue(list[i].Qq);
                    rowtemp.CreateCell(7).SetCellValue(list[i].Email);
                    //rowtemp.CreateCell(8).SetCellValue(list[i].Age);
                    rowtemp.CreateCell(8).SetCellValue(list[i].Age.ToString());
                    //rowtemp.CreateCell(8).CopyCellTo((int)list[i].Age); 
                    rowtemp.CreateCell(9).SetCellValue(list[i].Job);
                    //rowtemp.CreateCell(10).SetCellValue(list[i].YearIncome);
                    rowtemp.CreateCell(10).SetCellValue(list[i].YearIncome.ToString());
                    rowtemp.CreateCell(11).SetCellValue(list[i].Address);
                    rowtemp.CreateCell(12).SetCellValue(list[i].NeedLoan);
                    rowtemp.CreateCell(13).SetCellValue(list[i].ProductName);
                    rowtemp.CreateCell(14).SetCellValue(list[i].Source);
                    rowtemp.CreateCell(15).SetCellValue(list[i].Description);
                    //rowtemp.CreateCell(16).SetCellValue(list[i].NextContactTime);
                    //rowtemp.CreateCell(16).DateCellValue.ToOADate();
                    rowtemp.CreateCell(16).SetCellValue(list[i].NextContactTime.ToString());
                }
             /*   //设置新建文件路径及名称
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                //创建文件
                //FileStream file = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write);
                FileStream fs = File.OpenWrite(filePath);
                workbook.Write(fs);//向打开的这个Excel文件中写入表单并保存。  
                fs.Close();*/
                using(MemoryStream ms = new MemoryStream())
                {
                    workbook.Write(ms);
                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}