using Assos_Business.Repository.IRepository;
using Assos_Common;
using Assos_DataAccess;
using Assos_DataAccess.Data;
using Assos_DataAccess.ViewModel;
using AssosModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Assos_Business.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public OrderRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

       

        public async Task<OrderDTO> Create(OrderDTO objDTO)
        {
            try
            {
                var obj = _mapper.Map<OrderDTO, Order>(objDTO);
                _context.OrderHeaders.Add(obj.OrderHeader);
                await _context.SaveChangesAsync();
                foreach (var details in obj.OrderDetails)
                {
                    details.OrderHeaderId = obj.OrderHeader.Id;
                }
                _context.OrderDetails.AddRange(obj.OrderDetails);

                await _context.SaveChangesAsync();
                return new OrderDTO()
                {
                    OrderHeader = _mapper.Map<OrderHeader, OrderHeaderDTO>(obj.OrderHeader),
                    OrderDetails = _mapper.Map<IEnumerable<OrderDetail>, IEnumerable<OrderDetailDTO>>(obj.OrderDetails).ToList(),
                };


            }
            catch (Exception ex)
            {

                throw ex;
            }
            return objDTO;
        }

        public async Task<int> Delete(int id)
        {
            var objHeader = await _context.OrderHeaders.FirstOrDefaultAsync(u => u.Id == id);
            if (objHeader != null)
            {
                IEnumerable<OrderDetail> objDetail = _context.OrderDetails.Where(u => u.OrderHeaderId == id);
                _context.OrderDetails.RemoveRange(objDetail);
                return _context.SaveChanges();
            }
            return 0;
        }

        public async Task<OrderDTO> Get(int id)
        {
            Order order = new()
            {
                OrderHeader = _context.OrderHeaders.FirstOrDefault(u => u.Id == id),
                OrderDetails = _context.OrderDetails.Where(u => u.OrderHeaderId == id),
            };
            if (order != null)
            {
                return _mapper.Map<Order, OrderDTO>(order);
            }
            return new OrderDTO();
        }

        public async Task<IEnumerable<OrderDTO>> GetAll(string? userId = null, string? status = null)
        {
            List<Order> OrderFromDb = new List<Order>();
            IEnumerable<OrderHeader> orderHeaderList = _context.OrderHeaders;
            IEnumerable<OrderDetail> orderDetailList = _context.OrderDetails;
            foreach (OrderHeader header in orderHeaderList)
            {
                Order order = new()
                {
                    OrderHeader = header,
                    OrderDetails = orderDetailList.Where(u => u.OrderHeaderId == header.Id)
                };
                OrderFromDb.Add(order);
            }
            return _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDTO>>(OrderFromDb);
        }


        public async Task<OrderHeaderDTO> MarkPaymentSuccessful(int id,string paymentIntentId)
        {
            var data = await _context.OrderHeaders.FindAsync(id);
            if (data == null)
            {
                return new OrderHeaderDTO();
            }
            if (data.Status==SD.Status_Pending)
            {
                data.PaymentIntentId = paymentIntentId;
                data.Status = SD.Status_Confirmed;
                await _context.SaveChangesAsync();
                return _mapper.Map<OrderHeader, OrderHeaderDTO>(data);
            }
            return new OrderHeaderDTO();
        }

        public async Task<OrderHeaderDTO> UpdateHeader(OrderHeaderDTO objDTO)
        {
            if (objDTO != null)
            {
                var orderHeaderFromDb = _context.OrderHeaders.FirstOrDefault(u => u.Id == objDTO.Id);
                orderHeaderFromDb.Name = objDTO.Name;
                orderHeaderFromDb.PhoneNumber = objDTO.PhoneNumber;
                orderHeaderFromDb.Carrier = objDTO.Carrier;
                orderHeaderFromDb.Tracking = objDTO.Tracking;
                orderHeaderFromDb.StreetAddress = objDTO.StreetAddress;
                orderHeaderFromDb.City = objDTO.City;
                orderHeaderFromDb.State = objDTO.State;
                orderHeaderFromDb.PostalCode = objDTO.PostalCode;
                orderHeaderFromDb.Status = objDTO.Status;

                await _context.SaveChangesAsync();
                return _mapper.Map<OrderHeader, OrderHeaderDTO>(orderHeaderFromDb);
            }
            return new OrderHeaderDTO();
        }

        public async Task<bool> UpdateOrderStatus(int orderId, string status)
        {
            var data = await _context.OrderHeaders.FindAsync(orderId);
            if (data == null)
            {
                return false;
            }
            data.Status = status;
            if (status == SD.Status_Shipped)
            {
                data.ShippingDate = DateTime.Now;
            }
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<OrderHeaderDTO> CancelOrder(int id)
        {
            var orderHeader = await _context.OrderHeaders.FindAsync(id);
            if (orderHeader == null)
            {
                return new OrderHeaderDTO();
            }

            if (orderHeader.Status == SD.Status_Pending)
            {
                orderHeader.Status = SD.Status_Cancelled;
                await _context.SaveChangesAsync();
            }
            if (orderHeader.Status == SD.Status_Confirmed)
            {
                //refund
                var options = new Stripe.RefundCreateOptions
                {
                    Reason = Stripe.RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderHeader.PaymentIntentId
                };

                var service = new Stripe.RefundService();
                Stripe.Refund refund = service.Create(options);

                orderHeader.Status = SD.Status_Refunded;
                await _context.SaveChangesAsync();
            }

            return _mapper.Map<OrderHeader, OrderHeaderDTO>(orderHeader);
        }

    }
}