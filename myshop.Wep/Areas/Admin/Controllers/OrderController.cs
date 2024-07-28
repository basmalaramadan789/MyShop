using Microsoft.AspNetCore.Mvc;
using myshop.Enteties.Models;
using myshop.Enteties.Repositories;
using myshop.Enteties.ViewModel;

namespace myshop.Wep.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
             
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetData()
        {
            IEnumerable<OrderHeader> orderHeaders;
            orderHeaders = _unitOfWork.orderHeader.GetAll(Includeword: "ApplicationUser");
            return Json(new { data = orderHeaders });

        }
        public IActionResult Details(int orderid)
        {
            OrderVM orderVM = new OrderVM()
            {
                OrderHeader = _unitOfWork.orderHeader.GetOne(u => u.Id == orderid, Includeword: "ApplicationUser"),
                OrderDetails = _unitOfWork.orderDetails.GetAll(u => u.OrderId == orderid,Includeword:"Product")

            };
            return View(orderVM);

        }
    }
}
