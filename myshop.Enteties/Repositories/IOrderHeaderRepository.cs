using myshop.Enteties.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Enteties.Repositories
{
    public interface IOrderHeaderRepository:IGenericRepository<OrderHeader>
    {
        void Update(OrderHeader orderHeader);
        void Update(int OrderId, string OrderStatus, string PaymentStatus);
    }
}
