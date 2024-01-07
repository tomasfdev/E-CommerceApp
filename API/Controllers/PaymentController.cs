using API.Errors;
using Core.Interfaces;
using Core.Models;
using Core.Models.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private const string WebhookSecret = "whsec_1b2c010b2337025a07977562efeab73b2263ed8602b8503690a3b8a72d12822e";
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);

            if (basket is null) return BadRequest(new ApiResponse(400, "Problem with your basket"));

            return Ok(basket);
        }

        [HttpPost("webhook")]
        public async Task<ActionResult> StripeWebhook() //stripe is going to send a request/event in the request body of the request
        {
            var json = await new StreamReader(Request.Body).ReadToEndAsync();   //takes the request/event from stripe and transforms it into json formatted contents 

            var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], WebhookSecret);   //get access to the stripe event

            PaymentIntent paymentIntent;
            Order order;

            switch (stripeEvent.Type)   //checks the type of request/event sent by stripe
            {
                case "payment_intent.succeeded":
                    paymentIntent = (PaymentIntent)stripeEvent.Data.Object; //set paymentIntent
                    _logger.LogInformation("Payment succeeded: ", paymentIntent.Id);
                    order = await _paymentService.UpdateOrderPaymentSucceeded(paymentIntent.Id);    //update the order with the new status
                    _logger.LogInformation("Order updated to payment received: ", order.Id);
                    break;

                case "payment_intent.payment_failed":
                    paymentIntent = (PaymentIntent)stripeEvent.Data.Object; //set paymentIntent
                    _logger.LogInformation("Payment failed: ", paymentIntent.Id);
                    order = await _paymentService.UpdateOrderPaymentFailed(paymentIntent.Id);    //update the order with the new status
                    _logger.LogInformation("Order updated to payment failed: ", order.Id);
                    //TODO: update the order with the new status 
                    break;
            }

            return new EmptyResult();
        }
    }
}
