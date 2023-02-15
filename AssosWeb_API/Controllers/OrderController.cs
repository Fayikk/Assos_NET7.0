using Assos_Business.Repository;
using Assos_Business.Repository.IRepository;
using AssosModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace AssosWeb_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IEmailSender _emailSender;
        public OrderController(IOrderRepository orderRepository,IEmailSender emailSender)
        {
            _orderRepository = orderRepository;
            _emailSender = emailSender;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _orderRepository.GetAll();
            return Ok(result);
        }

        [HttpGet("{orderHeaderId}")]
        public async Task<IActionResult> Get(int? orderHeaderId)
        {
            if (orderHeaderId == null || orderHeaderId == 0)
            {
                return BadRequest(new ErrorModelDTO()
                {
                    ErrorMessage = "Invalid Id",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }

            var orderHeader = await _orderRepository.Get(orderHeaderId.Value);

            if (orderHeader == null)
            {
                return BadRequest(new ErrorModelDTO()
                {
                    ErrorMessage = "Invalid Id",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            return Ok(orderHeader);
        }


        [HttpPost]
        [ActionName("paymentsuccessful")]
        public async Task<IActionResult> PaymentSuccessful([FromBody]OrderHeaderDTO orderHeaderDTO)
        {
            var service = new SessionService();
            var sessionDetails =service.Get(orderHeaderDTO.SessionId);
            if (sessionDetails.PaymentStatus=="paid")
            {
                var result = await _orderRepository.MarkPaymentSuccessful(orderHeaderDTO.Id, sessionDetails.PaymentIntentId);
                await _emailSender.SendEmailAsync(orderHeaderDTO.Email, "Assos Your Order Confirmation:", orderHeaderDTO.Name + "Your order number is : " + orderHeaderDTO.Id);
                if (result == null)
                {
                    return BadRequest(new ErrorModelDTO()
                    {
                        ErrorMessage = "Can not mark payment as successful"
                    });
                }
                return Ok(result);
            }
            return BadRequest();
        }
        [HttpPost]
        [ActionName("Create")]
        public async Task<IActionResult> Create([FromBody] StripePaymentDTO paymentDTO)
        {
            paymentDTO.Order.OrderHeader.OrderDate = DateTime.Now;
            var result = await _orderRepository.Create(paymentDTO.Order);
            return Ok(result);
        }

    }
}
