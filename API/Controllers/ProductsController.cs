using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;


using Core.Entities;
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
        private readonly StoreContext _context;

        public ProductsController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet]  
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            return Ok(await _context.Products.ToListAsync());
        }

       [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product=await _context.Products.FirstOrDefaultAsync(x=>x.Id==id);
            if(product !=null){
                return product;
            }
            return NotFound("No product found");
        }
    }
}