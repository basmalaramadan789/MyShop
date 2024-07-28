using myshop.Enteties.Models;
using myshop.Enteties.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.DataAccess.Implementation
{
    public class OrderHeaderRepository : GenericRepository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderHeaderRepository(ApplicationDbContext context) : base(context)
        {
            
                _context = context;
         
        }

        

		public void Update(OrderHeader orderHeader)
		{
			_context.OrderHeaders.Update(orderHeader);
		}

		public void Update(int OrderId, string OrderStatus, string PaymentStatus)
		{
			// retrieve the order from db
          var  OrderDb=_context.OrderHeaders.FirstOrDefault(x=>x.Id==OrderId);
            if (OrderDb!=null)
            {
                OrderDb.OrderStatus = OrderStatus;
                OrderDb.PaymentDate=DateTime.Now;
                if(PaymentStatus!=null)
                {
                    OrderDb.PaymentStatus = PaymentStatus;
                }
            }
		}
	}
}

