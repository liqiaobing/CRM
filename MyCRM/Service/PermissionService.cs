using IRepository;
using IService;
using MyCRM.Models;
using AutoMapper;
using MyCRM.Common;
using Microsoft.Extensions.Options;

namespace Service
{
    public class PermissionService : BaseService<TPermission>, IPermissionService
    {
        private readonly IMapper _imapper;
        private readonly IOptions<SystemItem> _options;
        private readonly IClueRepository _clueRepository;
        public PermissionService(IPermissionRepository clueRepository, IMapper imapper, IOptions<SystemItem> options)
        {
            base._baseRepository = clueRepository;
            base._imapper = imapper;
            _imapper = imapper;
            _options = options;
        }

       
    } 
}