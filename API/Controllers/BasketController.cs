using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;

        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasketAsync(string basketId)
        {
            var basket = await _basketRepository.GetBasketByIdAsync(basketId);

            return Ok(basket ?? new CustomerBasket(basketId));  //retorna basket e caso seja null(client n tenha basket) cria novo baskter com id passado como param(basketId)
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasketAsync(CustomerBasket basket)
        {
            var updatedBasket = await _basketRepository.UpdateBasketAsync(basket);

            return Ok(updatedBasket);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteBasketAsync(string basketId)
        {
            return Ok(await _basketRepository.DeleteBasketAsync(basketId));
        }
    }
}
