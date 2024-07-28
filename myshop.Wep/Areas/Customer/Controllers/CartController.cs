using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using myshop.Enteties.Models;
using myshop.Enteties.Repositories;
using myshop.Enteties.ViewModel;
using myshop.Utilities;
//using Stripe.BillingPortal;
using Stripe.Checkout;
using System.Security.Claims;


namespace myshop.Wep.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
        }
        public ShoppingCartVM ShoppingCartVM { get; set; }
        
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartVM = new ShoppingCartVM()
            {
                CartsList = _unitOfWork.shoppingCart.GetAll(u => u.ApplicationUserId == claim.Value,Includeword:"Product")
            };
            foreach(var item in ShoppingCartVM.CartsList)
            {
                ShoppingCartVM.TotalCarts += (item.Count * item.Product.Price);

            }




            return View(ShoppingCartVM);
        }

        public IActionResult Plus(int CardId)
        {
            var shoppingCart = _unitOfWork.shoppingCart.GetOne(x=>x.Id == CardId);
            _unitOfWork.shoppingCart.IncreaseCount(shoppingCart, 1);
            _unitOfWork.Complete();
            return RedirectToAction("Index");
        }
		public IActionResult Minus(int CardId)
		{
			var shoppingCart = _unitOfWork.shoppingCart.GetOne(x => x.Id == CardId);
            if(shoppingCart.Count<=1 )
            {
                _unitOfWork.shoppingCart.Remove(shoppingCart);
				_unitOfWork.Complete();
				return RedirectToAction("Index","Home");
			}
            else
            {
				_unitOfWork.shoppingCart.DecreaseCount(shoppingCart, 1);
			}
			
			_unitOfWork.Complete();
			return RedirectToAction("Index");
		}
		public IActionResult Remove(int CardId)
		{
			var shoppingCart = _unitOfWork.shoppingCart.GetOne(x => x.Id == CardId);
            _unitOfWork.shoppingCart.Remove(shoppingCart); 
			_unitOfWork.Complete();
			return RedirectToAction("Index");
		}

        [HttpGet]
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM()
            {
                CartsList = _unitOfWork.shoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, Includeword: "Product"),
                OrderHeader = new()
            };

            ShoppingCartVM.OrderHeader.applicationUser = _unitOfWork.ApplicationUser.GetOne(x => x.Id == claim.Value);

            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.applicationUser.Name;
            ShoppingCartVM.OrderHeader.Address = ShoppingCartVM.OrderHeader.applicationUser.Address;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.applicationUser.City;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.applicationUser.PhoneNumber;

            foreach (var item in ShoppingCartVM.CartsList)
            {
                ShoppingCartVM.OrderHeader.TotalPrice += (item.Count * item.Product.Price);
            }

            return View(ShoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public IActionResult POSTSummary(ShoppingCartVM ShoppingCartVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM.CartsList = _unitOfWork.shoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, Includeword: "Product");


            ShoppingCartVM.OrderHeader.OrderStatus = SD.Pending;
            ShoppingCartVM.OrderHeader.PaymentStatus = SD.Pending;
            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = claim.Value;


            foreach (var item in ShoppingCartVM.CartsList)
            {
                ShoppingCartVM.OrderHeader.TotalPrice += (item.Count * item.Product.Price);
            }
            _unitOfWork.orderHeader.AddOne(ShoppingCartVM.OrderHeader);
        
            _unitOfWork.Complete();

            foreach (var item in ShoppingCartVM.CartsList)
            {
                OrderDetails orderDetail = new OrderDetails()
                {
                    ProductId = item.ProductId,
                    OrderId = ShoppingCartVM.OrderHeader.Id,
                    Price = item.Product.Price,
                    Count = item.Count
                };

                _unitOfWork.orderDetails.AddOne(orderDetail);
                _unitOfWork.Complete();
            }

            var domain = "https://localhost:7020/";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),

                Mode = "payment",
                SuccessUrl = domain + $"customer/cart/orderconfirmation?id={ShoppingCartVM.OrderHeader.Id}",
                CancelUrl = domain + $"customer/cart/index",
            };

            foreach (var item in ShoppingCartVM.CartsList)
            {
                var sessionlineoption = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Product.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name,
                        },
                    },
                    Quantity = item.Count,
                };
                options.LineItems.Add(sessionlineoption);
            }


            var service = new SessionService();
            Session session = service.Create(options);
            ShoppingCartVM.OrderHeader.SessionId = session.Id;

            _unitOfWork.Complete();

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);

            //_unitOfWork.ShoppingCart.RemoveRange(ShoppingCartVM.CartsList);
            //         _unitOfWork.Complete();
            //         return RedirectToAction("Index","Home");

        }
        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderheader=_unitOfWork.orderHeader.GetOne(x=>x.Id==id);
            var service = new SessionService();
            Session session = service.Get(orderheader.SessionId);
            if (session.PaymentStatus.ToLower() == "paid")
            {
                _unitOfWork.orderHeader.Update(id, SD.Approve, SD.Approve);
                _unitOfWork.Complete();    
            }
            List<ShoppingCart> shoppingCarts = _unitOfWork.shoppingCart.GetAll(u=>u.ApplicationUserId==orderheader.ApplicationUserId).ToList();
            _unitOfWork.shoppingCart.RemoveRange(shoppingCarts);
            _unitOfWork.Complete();
            return View(id);

           
        }







    }
}
