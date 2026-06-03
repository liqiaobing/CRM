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
    public class ProductService : BaseService<TProduct>, IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository) 
        { 
            base._baseRepository = productRepository;
            _productRepository = productRepository;
        }

        
        public async Task<Dictionary<string, int>> GetDicsListAsync()
        {
            var res = await _productRepository.GetDicValuesAsync();
            return res;
        }

       
    }
}
