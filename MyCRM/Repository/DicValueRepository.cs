using IRepository;
using MyCRM.DataBase;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MyCRM.Models;
using System.Globalization;
using Microsoft.AspNetCore.Server.IIS.Core;
using MyCRM.Models.DTO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyCRM.Common;
using MyCRM.Common.page;
using MyCRM.Models.Request;
using Microsoft.Extensions.Hosting;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.Diagnostics;
using Microsoft.Extensions.Options;

namespace Repository
{
    public class DicValueRepository : BaseRepository<TDicValue>, IDicValueRepository
    {
        private ApplicationDbcontext _dbcontext;
        private readonly IOptions<SystemItem> _options;

        public DicValueRepository(ApplicationDbcontext dbcontext, IOptions<SystemItem> options):base(dbcontext)
        {
            _dbcontext = dbcontext;
            _options = options;
        }


        /// <summary>
        /// 获取所有的字典key value
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string, int>> GetDicValuesAsync()
        {
            var dics = await _dbcontext.TDicValues.Select(k => new { key = k.Id, value = k.TypeValue }).ToListAsync();
            Dictionary<string, int> keyValuePairs = new Dictionary<string, int>();
            foreach(var d in dics) 
            { 
                keyValuePairs.Add(d.value, d.key);
            }
            return keyValuePairs;
        }


        /// <summary>
        /// 获取所有的字典key value
        /// </summary>
        /// <returns></returns>
  /*      public async Task<Dictionary<string, int>> GetDicValuesAsync(string code)
        {
            var dics = await _dbcontext.TDicValues.Where(d => d.TypeCode == code)
                .Select(k => new { key = k.Id, value = k.TypeValue }).ToListAsync();
            Dictionary<string, int> keyValuePairs = new Dictionary<string, int>();
            foreach (var d in dics)
            {
                keyValuePairs.Add(d.value, d.key);
            }
            return keyValuePairs;
        }*/
    }
}
