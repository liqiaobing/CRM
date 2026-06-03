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
    public class ActivityRepository : BaseRepository<TActivity>, IActivityRepository
    {
        private ApplicationDbcontext _dbcontext;
        private readonly IOptions<SystemItem> _options;

        public ActivityRepository(ApplicationDbcontext dbcontext, IOptions<SystemItem> options):base(dbcontext)
        {
            _dbcontext = dbcontext;
            _options = options;
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
        /// 返回活动负责人列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<OwnerDTO>> GetActivityOwnerAsync()
        {
            /*  var result1 = await _dbcontext.TActivities.Include(u => u.Owner).Select(n => new { n.Owner.Name }).ToListAsync();
              List<string?> res = result1.GroupBy(n => n.Name).Select(n => n.Key).ToList();*/
            //只查询连接Activity表中的ownerid和User表中name列
            var res = await _dbcontext.TActivities.Include(u => u.Owner).Select(n => new { n.Owner.Name, n.Owner.Id })
                .Select(n => new OwnerDTO { OwnerName = n.Name, OwnerId = n.Id }).ToListAsync();
            //GroupBy(n => n.Owner.Name).Select(g => g.Key).ToListAsync();
            //var result = res.GroupBy<TActivity, res>(n => n.Name).ToList();
            return res;
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

        public override async Task<List<TActivity>> PageQueryAsync(int page, int pageSize, int total)
        {
            return await _dbcontext.TActivities.Include(u => u.Owner).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

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
    }
}
