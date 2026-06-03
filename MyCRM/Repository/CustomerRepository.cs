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
using Microsoft.AspNetCore.SignalR;
using System.ComponentModel.DataAnnotations;
using NPOI.SS.Formula.Functions;

namespace Repository
{
    public class CustomerRepository : BaseRepository<TCustomer>, ICustomerRepository
    {
        private ApplicationDbcontext _dbcontext;
        private readonly IOptions<SystemItem> _options;
        private readonly DbContextOptions<ApplicationDbcontext> _dbOptions;

        public CustomerRepository(ApplicationDbcontext dbcontext, IOptions<SystemItem> options) :base(dbcontext)
        {
            _dbcontext = dbcontext;
            _options = options;
            // _dbOptions = dbOptions;, DbContextOptions<ApplicationDbcontext> dbOptions
        }

        /// <summary>
        /// 获取客户列表分页
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="currentId"></param>
        /// <returns></returns>
        public async Task<PageConfig> GetCstPageListAsync(int currentPage, int currentId)
        {
            IQueryable<TCustomer> query = _dbcontext.TCustomers.Include(p => p.Clue).Include(p => p.ProductNavigation);
            if(currentId != _options.Value.Admin)
            {
                query = query.Where(p => p.CreateBy == currentId);
            }
            int total = query.Count();
            var data = await query.Select(p => new
            {
                Id = p.Id,
                OwnerName = p.Clue.Owner.Name,
                ActivityName = p.Clue.Activity.Name,
                FullName = p.Clue.FullName,
                Appellation = p.Clue.AppellationNavigation.TypeValue,
                Phone = p.Clue.Phone,
                Weixin = p.Clue.Weixin,
                NeedLoan = p.Clue.NeedLoanNavigation.TypeValue,
                IntentionState = p.Clue.IntentionStateNavigation.TypeValue,
                Source = p.Clue.SourceNavigation.TypeValue,
                ProductName = p.ProductNavigation.Name,
                NextContactTime = p.NextContactTime,
                State = p.Clue.StateNavigation.TypeValue,
            }).Skip((currentPage - 1) * _options.Value.PageSize).Take(_options.Value.PageSize).ToListAsync();

            return new PageConfig { Data = data, Total = total};
        }

        /// <summary>
        /// 获取对应用户的所有客户信息
        /// </summary>
        /// <param name="currentId"></param>
        /// <returns></returns>
        public async Task<dynamic> GetCstListAsync(int currentId)
        {
            IQueryable<TCustomer> query = _dbcontext.TCustomers.Include(p => p.Clue).Include(p => p.ProductNavigation);
            if (currentId != _options.Value.Admin)
            {
                query = query.Where(p => p.CreateBy == currentId);
            }
            var data = await query.Select(p => new CustomerDTO
            {
                OwnerName = p.Clue.Owner.Name,
                ActivityName = p.Clue.Activity.Name,
                FullName = p.Clue.FullName,
                Appellation = p.Clue.AppellationNavigation.TypeValue,
                Phone = p.Clue.Phone,
                Weixin = p.Clue.Weixin,
                Qq = p.Clue.Qq,
                Email = p.Clue.Email,
                Age = p.Clue.Age,
                Job = p.Clue.Job,
                YearIncome = p.Clue.YearIncome,
                Address = p.Clue.Address,
                NeedLoan = p.Clue.NeedLoanNavigation.TypeValue,
                ProductName = p.ProductNavigation.Name,
                Source = p.Clue.SourceNavigation.TypeValue,
                Description = p.Description,
                NextContactTime = p.NextContactTime,
            }).ToListAsync();
            return data;
        }

        /// <summary>
        /// 获取对应用户的给定条件的信息
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="currentId"></param>
        /// <returns></returns>
        public async Task<dynamic> GetCstConditionListAsync(List<int> condition, int currentId)
        {
            
            IQueryable<TCustomer> query = _dbcontext.TCustomers.Include(p => p.Clue).Include(p => p.ProductNavigation);
            if (currentId != _options.Value.Admin)
            {
                query = query.Where(p => p.CreateBy == currentId);
            }

            List<CustomerDTO> dto = new List<CustomerDTO>();
            foreach(var cdt in condition)
            {
                var item = query.Where(p => p.Id == cdt);
                var data = await item.Select(p => new CustomerDTO
                {
                    OwnerName = p.Clue.Owner.Name,
                    ActivityName = p.Clue.Activity.Name,
                    FullName = p.Clue.FullName,
                    Appellation = p.Clue.AppellationNavigation.TypeValue,
                    Phone = p.Clue.Phone,
                    Weixin = p.Clue.Weixin,
                    Qq = p.Clue.Qq,
                    Email = p.Clue.Email,
                    Age = p.Clue.Age,
                    Job = p.Clue.Job,
                    YearIncome = p.Clue.YearIncome,
                    Address = p.Clue.Address,
                    NeedLoan = p.Clue.NeedLoanNavigation.TypeValue,
                    ProductName = p.ProductNavigation.Name,
                    Source = p.Clue.SourceNavigation.TypeValue,
                    Description = p.Description,
                    NextContactTime = p.NextContactTime,
                }).FirstAsync();
                dto.Add(data);
            }

            return dto;
        }

        
    }
}
