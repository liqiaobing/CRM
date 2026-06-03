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
    public class RemarkRepository : BaseRepository<TActivityRemark>, IRemarkRepository
    {
        private ApplicationDbcontext _dbcontext;
        private readonly IOptions<SystemItem> _options;

        public RemarkRepository(ApplicationDbcontext dbcontext, IOptions<SystemItem> opt):base(dbcontext)
        {
            _dbcontext = dbcontext;
            _options = opt;
        }

        /// <summary>
        /// 配置筛选条件了
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="currentId"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public async Task<PageConfig> QueryNoteContentAsync(int activityId, int currentId, int currentPage)
        {
            IQueryable<TActivityRemark> query = _dbcontext.TActivityRemarks.Include(p => p.Activity)
                .Include(c => c.CreateByNavigation).Include(e => e.EditByNavigation);
        
            if (currentId == _options.Value.Admin)
            {
                query = query.Where(a => a.ActivityId == activityId);
            }
            else
            {
                query = query.Where(a => a.ActivityId == activityId && a.CreateBy == currentId);
            }
        
            int total = query.Count();
            var result = await query.Skip((currentPage - 1) * _options.Value.PageSize).Take(_options.Value.PageSize).Select(p => new
            {
                Id = p.Id,
                NoteContent = p.NoteContent,
                CreateTime = p.CreateTime,
                CreateByName = p.CreateByNavigation.Name,
                EditTime = p.EditTime,
                EditByName = p.EditByNavigation.Name,
            }).ToListAsync();
            return new PageConfig { Data = result, Total = total };
        }
    }
}
