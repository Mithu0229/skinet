using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;

        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string id)
        {
           var basket= await _basketRepository.GetBasketAsync(id);
           return Ok(basket?? new CustomerBasket(id)); 
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasket customerBasket)
        {
           var basket= await _basketRepository.UpdateBasketAsync(customerBasket);
           return Ok(basket); 
        }

        [HttpDelete]
        public async Task DeleteBasketAsync(string id)
        {
           await _basketRepository.DeleteBasketAsync(id);
            
        }

        
    }
}