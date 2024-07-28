﻿using myshop.Enteties.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Enteties.ViewModel
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> CartsList { get; set; }
        public decimal TotalCarts {  get; set; }
        public OrderHeader OrderHeader { get; set; }
        public OrderDetails OrderDetails { get; set; }
    }
}
