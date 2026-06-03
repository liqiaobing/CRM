
using IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

namespace MyCRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _iMapper;

        public PermissionController(IPermissionService service, IWebHostEnvironment webHostEnvironment,
            IMapper iMapper) 
        {
            _permissionService = service;
            _webHostEnvironment = webHostEnvironment;
            _iMapper = iMapper;
        }
     
    }
}
