using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
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
        public async Task<ActionResult<List<ProductToReturnDto>>> GetProducts()
        {
            var spec= new ProductWithTypesAndBrandsSpecification();
            var products=await _repoProduct.ListAsync(spec);
            return Ok(products.Select(product=> new ProductToReturnDto{
                Id=product.Id,
                Name=product.Name,
                Description=product.Description,
                PictureUrl=product.PictureUrl,
                Price = product.Price,
                ProductBrand=product.ProductBrand.Name,
                ProductType= product.ProductType.Name
            }).ToList());
        }

       [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
               var spec= new ProductWithTypesAndBrandsSpecification(id);

             var product=  await _repoProduct.GetEntityWithSpec(spec);
             return new ProductToReturnDto{
                Id=product.Id,
                Name=product.Name,
                Description=product.Description,
                PictureUrl=product.PictureUrl,
                Price = product.Price,
                ProductBrand=product.ProductBrand.Name,
                ProductType= product.ProductType.Name
             };
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