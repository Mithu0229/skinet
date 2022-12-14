using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<ProductBrand> _repoBrand;
        private readonly IGenericRepository<ProductType> _repoType;
        private readonly IGenericRepository<Product> _repoProduct;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<ProductBrand> repoBrand, IGenericRepository<ProductType> repoType,
        IGenericRepository<Product> repoProduct,IMapper mapper)
        {
            _repoBrand = repoBrand;
            _repoType = repoType;
            _repoProduct = repoProduct;
            _mapper = mapper;
        }

        [Cached(600)]
        [HttpGet]  
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams productParams)
        {
            var spec= new ProductWithTypesAndBrandsSpecification(productParams);
            var countSpec= new ProductWithFiltersForCountSpecification(productParams);
            var totalItems= await _repoProduct.CountAsync(countSpec);
            
            var products=await _repoProduct.ListAsync(spec);
            var data =_mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductToReturnDto>>(products);


            return Ok(new Pagination<ProductToReturnDto>(productParams.PageSize,productParams.PageIndex,totalItems,data));
        }

        [Cached(600)]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
               var spec= new ProductWithTypesAndBrandsSpecification(id);

             var product=  await _repoProduct.GetEntityWithSpec(spec);
             if(product ==null){return NotFound(new ApiResponse(404));}
             return _mapper.Map<Product,ProductToReturnDto>(product);
        }

        [Cached(600)]
        [HttpGet("Brands")]  
        public async Task<ActionResult<List<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _repoBrand.ListAllAsync());
        }

        [Cached(600)]
        [HttpGet("Types")]  
        public async Task<ActionResult<List<ProductType>>> GetProductTypes()
        {
            return Ok(await _repoType.ListAllAsync());
        }
    }
}