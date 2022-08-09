using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;


using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<ProductBrand> _repoBrand;
        private readonly IGenericRepository<ProductType> _repoType;
        private readonly IGenericRepository<Product> _repoProduct;

        public ProductsController(IGenericRepository<ProductBrand> repoBrand, IGenericRepository<ProductType> repoType,
        IGenericRepository<Product> repoProduct)
        {
            _repoBrand = repoBrand;
            _repoType = repoType;
            _repoProduct = repoProduct;
        }

        [HttpGet]  
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            return Ok(await _repoProduct.ListAllAsync());
        }

       [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            return await _repoProduct.GetByIdAsync(id);
        }

        [HttpGet("Brands")]  
        public async Task<ActionResult<List<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _repoBrand.ListAllAsync());
        }

        [HttpGet("Types")]  
        public async Task<ActionResult<List<ProductType>>> GetProductTypes()
        {
            return Ok(await _repoType.ListAllAsync());
        }
    }
}