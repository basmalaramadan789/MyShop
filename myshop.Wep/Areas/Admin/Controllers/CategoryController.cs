using Microsoft.AspNetCore.Mvc;
using myshop.Enteties.Models;
using myshop.DataAccess;
using myshop.Enteties.Repositories;


namespace myshop.Wep.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        public IActionResult Index()
        {
            var categories = _unitOfWork.Category.GetAll();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                // _context.categories.Add(category);
                //_context.SaveChanges();
                _unitOfWork.Category.AddOne(category);
                _unitOfWork.Complete();
                TempData["Create"] = "Data Has Added Successfully";
                return RedirectToAction("Index");
            }
            return View(category);

        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                NotFound();
            }
            //var category = _context.categories.Find(id);
            var category = _unitOfWork.Category.GetOne(x => x.Id == id);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                //_context.categories.Update(category);
                _unitOfWork.Category.Update(category);
                // _context.SaveChanges();
                //_unitOfWork.Dispose();
                _unitOfWork.Complete();
                TempData["Update"] = "Data Has Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(category);
        }
        
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                NotFound();
            }
            var category = _unitOfWork.Category.GetOne(x => x.Id == id);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCategory(int? id)
        {
            var category = _unitOfWork.Category.GetOne(x => x.Id == id);
            if (category == null)
            {
                NotFound();
            }
            //_context .categories.Remove(category);
            //_context .SaveChanges();
            _unitOfWork.Category.Remove(category);
            _unitOfWork.Complete();
            TempData["Delete"] = "Data Has Deleted Successfully";
            return RedirectToAction("Index");
        }





    }
}
