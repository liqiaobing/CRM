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
using MyCRM.Common.ApiResult;

namespace Repository
{
    public class ClueRepository : BaseRepository<TClue>, IClueRepository
    {
        private ApplicationDbcontext _dbcontext;
        private readonly IOptions<SystemItem> _options;
        private readonly DbContextOptions<ApplicationDbcontext> _dbOptions;

        public ClueRepository(ApplicationDbcontext dbcontext, IOptions<SystemItem> options) :base(dbcontext)
        {
            _dbcontext = dbcontext;
            _options = options;
            // _dbOptions = dbOptions;, DbContextOptions<ApplicationDbcontext> dbOptions
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="delList">int类型集合存放id</param>
        /// <returns>0为失败 大于0同时等于传入的id数量则为成功</returns>
        public async Task<int> BatchDeleteAsync(List<int> delList)
        {
            var result = 0;
            List<TUser> users = new List<TUser>();
            foreach (int del in delList) 
            {
                TUser user = await _dbcontext.TUsers.FindAsync(del);
                if (user == null) continue; 
                users.Add(user);
            }
            if(users.Count > 0) 
            {
                try
                {
                    _dbcontext.TUsers.RemoveRange(users);
                }catch(Exception ) 
                { 
                    return 0;
                }
                result = await _dbcontext.SaveChangesAsync();
            }
            return result;
        }

        /// <summary>
        /// 返回活动负责人列表 优化
        /// </summary>
        /// <returns></returns>
        public async Task<dynamic> GetActivityOwnerAsync_V2()
        {
            var res = await _dbcontext.TActivities.Include(u => u.Owner).Select(n => new { ownerName =  n.Owner.Name, ownerId =  n.Owner.Id })
                .Distinct().ToListAsync();
            return res;
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="activityTiem"></param>
        /// <param name="OwnerId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<PageConfig> GetActivitysAsync(int current , DateTime defaultTime, DateTime startTime, DateTime endTime, DateTime activityTiem, int? oId, 
                string? name, decimal? cost = null)
        {

            //拼接原始
            IQueryable<TActivity> actList = _dbcontext.TActivities.Include(u => u.Owner);

            if (startTime != defaultTime && endTime != defaultTime)
            {
                actList = actList.Where(a => a.StartTime >= startTime && a.EndTime <= endTime);
            }
            if(activityTiem != defaultTime)
            {
                actList = actList.Where(a => a.CreateTime == activityTiem);
            }
            if (oId != null)
            {
                actList = actList.Where(a => oId == a.OwnerId);
            }
            if (name != null)
            {
                actList = actList.Where(a => a.Name == name);
            }
            if(cost != null)
            {
                actList = actList.Where(a => a.Cost == cost);
            }
            int total = actList.Count();
            var res = await actList.Skip((current - 1) * 10).Take(10).ToListAsync();
            
            return new PageConfig { Data=res, Total = total};
        }

        /// <summary>
        /// 条件查询 改善
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<PageConfig> GetActivitiesAsync(TActivityRequest activity, int currentPage, int pageSize)
        {
            //拼接原始
            IQueryable<TActivity> actList = _dbcontext.TActivities.Include(u => u.Owner);

            if (activity.StartTime != null && activity.EndTime != null)
            {
                actList = actList.Where(a => a.StartTime >= activity.StartTime && a.EndTime <= activity.EndTime);
            }
            if (activity.CreateTime != null)
            {
                actList = actList.Where(a => a.CreateTime == activity.CreateTime);
            }
            if (activity.OwnerId != null)
            {
                actList = actList.Where(a => activity.OwnerId == a.OwnerId);
            }
            if (activity.Name != null)
            {
                actList = actList.Where(a => a.Name == activity.Name);
            }
            if (activity.Cost != null)
            {
                actList = actList.Where(a => a.Cost == activity.Cost);
            }
            int total = actList.Count();
            var res = await actList.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PageConfig { Data = res, Total = total };
        }


        /// <summary>
        /// 条件查询 改善 加权限过滤
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<PageConfig> GetActivitiesFilterAsync(TActivityRequest activity, int currentPage, int pageSize, int currentId)
        {
            //拼接原始
            IQueryable<TActivity> actList = _dbcontext.TActivities.Include(u => u.Owner);

            if (activity.StartTime != null && activity.EndTime != null)
            {
                actList = actList.Where(a => a.StartTime >= activity.StartTime && a.EndTime <= activity.EndTime);
            }
            if (activity.CreateTime != null)
            {
                actList = actList.Where(a => a.CreateTime == activity.CreateTime);
            }
            if (activity.OwnerId != null)
            {
                actList = actList.Where(a => activity.OwnerId == a.OwnerId);
            }
            if (activity.Name != null)
            {
                actList = actList.Where(a => a.Name == activity.Name);
            }
            if (activity.Cost != null)
            {
                actList = actList.Where(a => a.Cost == activity.Cost);
            }
            if(currentId != _options.Value.Admin)
            {
                actList = actList.Where(a => a.Owner.Id == currentId);
            }
            int total = actList.Count();
            var res = await actList.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PageConfig { Data = res, Total = total };
        }

        /// <summary>
        /// 线索分页集合
        /// </summary>
        /// <returns></returns>
        public async Task<PageConfig> GetClueLsitAsync(int currentPage, int pageSize,int currentId)
        {
            IQueryable<TClue> query = _dbcontext.TClues.Include(u => u.Owner).Include(a => a.Activity).Include(a => a.AppellationNavigation)
                .Include(n => n.NeedLoanNavigation).Include(i => i.IntentionStateNavigation).Include(i => i.IntentionProductNavigation).
                Include(s => s.StateNavigation).Include(s => s.SourceNavigation);
            if(currentId != _options.Value.Admin)
            {
                query = query.Where(c => c.OwnerId == currentId);
            }
            int total = await query.CountAsync();
            var result = await query.Skip((currentPage - 1) * pageSize).Take(pageSize).Select(s => new
            {
                Id = s.Id,
                Owner = s.Owner.Name,
                Activity = s.Activity.Name,
                FullName = s.FullName,
                Appellation = s.AppellationNavigation.TypeValue,
                Phone = s.Phone,
                Weixin = s.Weixin,
                NeedLoan = s.NeedLoanNavigation.TypeValue,
                IntentionState = s.IntentionStateNavigation.TypeValue,
                IntentionProduct = s.IntentionProductNavigation.Name,
                State = s.StateNavigation.TypeValue,
                Source = s.SourceNavigation.TypeValue,
                NextContactTime = s.NextContactTime,
            }).ToListAsync();
            return new PageConfig { Total = total, Data = result };
        }

        /// <summary>
        /// 批量创建
        /// </summary>
        /// <param name="clues"></param>
        /// <returns></returns>
        public async Task<bool> BatchCreateAsync(List<TClue> clues)
        {
            int res = 0;
           /* using (var context = new ApplicationDbcontext(_dbOptions))
            {
               
            }*/
            foreach (var item in clues)
            {
                await _dbcontext.TClues.AddAsync(item);
            }
            res = await _dbcontext.SaveChangesAsync();
            return res > 0 ? true : false;
        }

        /// <summary>
        /// 获取clue线索详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<dynamic> GetClueDetailAsync(int id)
        {
            IQueryable<TClue> query = _dbcontext.TClues.Include(u => u.Owner).Include(a => a.Activity).Include(a => a.AppellationNavigation)
               .Include(n => n.NeedLoanNavigation).Include(i => i.IntentionStateNavigation).Include(i => i.IntentionProductNavigation).
               Include(s => s.StateNavigation).Include(s => s.SourceNavigation);
            var res = await query.Where(p => p.Id == id).Select(s => new
            {
                Id = s.Id,
                Owner = s.Owner.Name,
                Activity = s.Activity.Name,
                FullName = s.FullName,
                Appellation = s.AppellationNavigation.TypeValue,
                Phone = s.Phone,
                Weixin = s.Weixin,
                Email = s.Email,
                Age = s.Age,
                Qq = s.Qq,
                Job = s.Job,
                YearIncome = s.YearIncome,
                Address = s.Address,
                NeedLoan = s.NeedLoanNavigation.TypeValue,
                IntentionState = s.IntentionStateNavigation.TypeValue,
                IntentionProduct = s.IntentionProductNavigation.Name,
                description = s.Description,
                State = s.StateNavigation.TypeValue,
                Source = s.SourceNavigation.TypeValue,
                NextContactTime = s.NextContactTime,
                StateId = s.State,
            }).ToListAsync();
            return res;
        }

        /// <summary>
        /// 创建线索备注
        /// </summary>
        /// <param name="clueRemark"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> CreateClueRemarkAsync(TClueRemark clueRemark)
        {
            await _dbcontext.TClueRemarks.AddAsync(clueRemark);
            var res = await _dbcontext.SaveChangesAsync();
            return res > 0;
        }

        /// <summary>
        /// 对应的线索备注列表
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentId"></param>
        /// <returns></returns>
        public async Task<PageConfig> GetClueRemarkListAsync(int id, int currentPage, int pageSize, int currentId)
        {
            IQueryable<TClueRemark> query = _dbcontext.TClueRemarks.Include(p => p.NoteWayNavigation).Include(p => p.CreateByNavigation)
                .Include(p => p.EditByNavigation)
                .Where(p => p.ClueId == id);
            if(currentId != _options.Value.Admin)
            {
                query = query.Where(p => p.CreateBy == currentId);
            }
            int total = await query.CountAsync();
            var res = await query.OrderByDescending(p => p.CreateTime).Select(p => new
            {
                id = p.Id,
                noteWay = p.NoteWayNavigation.TypeValue,
                noteContent = p.NoteContent,
                createTime = p.CreateTime,
                createBy = p.CreateByNavigation.Name,
                editTime = p.EditTime,
                editBy = p.EditByNavigation.Name,
            }).Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PageConfig { Data = res, Total = total };
        }

        /// <summary>
        /// clueRemark Get
        /// </summary>
        /// <param name="clue"></param>
        /// <returns></returns>
        public async Task<TClueRemark> QueryClueRemarkAsync(int id)
        {
            var clue = await _dbcontext.TClueRemarks.FirstOrDefaultAsync(p => p.Id == id);
            return clue;
        }
        /// <summary>
        /// clueRemark update
        /// </summary>
        /// <param name="clue"></param>
        /// <returns></returns>
        public async Task<bool> UpdateClueRemarkAsync(TClueRemark clue)
        {
            _dbcontext.TClueRemarks.Update(clue);
            return await _dbcontext.SaveChangesAsync() > 0;
        }

    
    }
}
