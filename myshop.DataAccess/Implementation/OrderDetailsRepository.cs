﻿using myshop.Enteties.Models;
using myshop.Enteties.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace myshop.DataAccess.Implementation
{
    public class OrderDetailsRepository : GenericRepository<OrderDetails>, IOrderDetailsRepository
	{
        private readonly ApplicationDbContext _context;

        public OrderDetailsRepository(ApplicationDbContext context) : base(context)
        {
            
                _context = context;
         
        }

		public void Update(OrderDetails orderDetails)
		{
			_context.OrderDetails.Update(orderDetails);
		}
	}
}

