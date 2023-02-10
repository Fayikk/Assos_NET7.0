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


        public async Task<OrderHeaderDTO> MarkPaymentSuccessful(int id)
        {
            var data = await _context.OrderHeaders.FindAsync(id);
            if (data == null)
            {
                return new OrderHeaderDTO();
            }
            if (data.Status==SD.Status_Pending)
            {
                data.Status = SD.Status_Confirmed;
                await _context.SaveChangesAsync();
                return _mapper.Map<OrderHeader, OrderHeaderDTO>(data);
            }
            return new OrderHeaderDTO();
        }

        public async Task<OrderHeaderDTO> UpdateHeader(OrderHeaderDTO objDTO)
        {
            if (objDTO !=null)
            {
                var orderHeader = _mapper.Map<OrderHeaderDTO, OrderHeader>(objDTO);
                _context.OrderHeaders.Update(orderHeader);
                await _context.SaveChangesAsync();
                return _mapper.Map<OrderHeader, OrderHeaderDTO>(orderHeader);  
            
            }
            return objDTO; 
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
    }
}