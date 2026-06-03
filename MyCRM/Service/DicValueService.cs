using IRepository;
using IService;
using MyCRM.Common.ApiResult;
using MyCRM.Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class DicValueService : BaseService<TDicValue>, IDicValueService
    {
        private readonly IDicValueRepository _dicValueRepository;

        public DicValueService(IDicValueRepository dicValueRepository) 
        { 
            base._baseRepository = dicValueRepository;
            _dicValueRepository = dicValueRepository;
        }

        
        public async Task<Dictionary<string, int>> GetDicsListAsync()
        {
            var res = await _dicValueRepository.GetDicValuesAsync();
            return res;
        }

       
    }
}
