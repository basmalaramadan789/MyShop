using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myshop.DataAccess.Implementation;
using myshop.Enteties.Models;
using myshop.Enteties.Repositories;
using myshop.Enteties.ViewModel;
using System.Security.Claims;

namespace myshop.Wep.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
             
        }
        public IActionResult Index()
        {

            var products = _unitOfWork.product.GetAll();
            return View(products);
        }

        public IActionResult Details(int id)
        {
            ShoppingCart obj = new ShoppingCart()
            {
                ProductId = id,
                Product = _unitOfWork.product.GetOne(v => v.Id == id, Includeword: "Category"),
                Count = 1,
               
            };
            return View(obj);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            //we need the userId and productID
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUserId = claim.Value;
            ShoppingCart cartobj=_unitOfWork.shoppingCart.GetOne(u=>u.ApplicationUserId== claim.Value&& u.ProductId==shoppingCart.ProductId);
            if(cartobj == null)
            {
                _unitOfWork.shoppingCart.AddOne(shoppingCart);
            }
            else
            {
                _unitOfWork.shoppingCart.IncreaseCount(cartobj,shoppingCart.Count);
            }



           
            _unitOfWork.Complete();


            return RedirectToAction("Index");
        }
    }
}
