using Microsoft.AspNetCore.Mvc;
using myshop.Enteties.Models;
using myshop.DataAccess;
using myshop.Enteties.Repositories;
using myshop.Enteties.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace myshop.Wep.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment; 
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {

            return View();
        }
        public IActionResult GetData()
        {
            var products = _unitOfWork.product.GetAll(Includeword: "Category");
            return Json(new { data = products });

        }
        //public IActionResult Index()
        //{
        //    var Products = _unitOfWork.product.GetAll(Includeword: "Category");
        //    return View(Products);
        //}





        [HttpGet]
        public IActionResult Create()
        {
            ProductVm productVm = new ProductVm()
            {
                Product = new Product(),
                CategoryList =_unitOfWork.Category.GetAll().Select(x=>new SelectListItem
                {
                    Text = x.Name,
                    Value=x.Id.ToString()

                })

            };

            return View(productVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create(ProductVm ProductVm,IFormFile Upload)
        {
            string RootPath=_webHostEnvironment.WebRootPath;
            if(Upload != null)
            {
                string fileNmae=Guid.NewGuid().ToString();
                var upload=Path.Combine(RootPath, @"Images\Product");
                var ext=Path.GetExtension(Upload.FileName);
                using (var filestream = new FileStream(Path.Combine(upload, fileNmae + ext), FileMode.Create))
                {
                    Upload.CopyTo(filestream);
                }
                ProductVm.Product.Image = @"Images\Product\"+ fileNmae + ext;
            }

            if (ModelState.IsValid)
            {
                // _context.categories.Add(Product);
                //_context.SaveChanges();
                _unitOfWork.product.AddOne(ProductVm.Product);
                _unitOfWork.Complete();
                TempData["Create"] = "Data Has Added Successfully";
                return RedirectToAction("Index");
            }
            return View(ProductVm.Product);

        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                NotFound();
            }
            //var Product = _context.categories.Find(id);
           
            ProductVm productVm = new ProductVm()
            {
                Product = _unitOfWork.product.GetOne(x => x.Id == id),
                CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()

                })

            };
            return View(productVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductVm Productvm, IFormFile? Upload)
        {
            if (ModelState.IsValid)
            {
                string RootPath = _webHostEnvironment.WebRootPath;
                if (Upload != null)
                {
                    string fileNmae = Guid.NewGuid().ToString();
                    var upload = Path.Combine(RootPath, @"Images\Product");
                    var ext = Path.GetExtension(Upload.FileName);
                    if(Productvm.Product.Image != null)
                    {
                        var oldimg = Path.Combine(RootPath, Productvm.Product.Image.TrimStart('\\'));
                        if (System.IO.File.Exists(oldimg))
                        {
                            System.IO.File.Delete(oldimg);
                        }
                    }
                    using (var filestream = new FileStream(Path.Combine(upload, fileNmae + ext), FileMode.Create))
                    {
                        Upload.CopyTo(filestream);
                    }
                    Productvm.Product.Image = @"Images\Product\" + fileNmae + ext;
                }


                //_context.categories.Update(Product);
                _unitOfWork.product.Update(Productvm.Product);
                // _context.SaveChanges();
                //_unitOfWork.Dispose();
                _unitOfWork.Complete();
                TempData["Update"] = "Data Has Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(Productvm.Product);
        }
        //[HttpGet]
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        NotFound();
        //    }
        //    var Product = _unitOfWork.product.GetOne(x => x.Id == id);
        //    return View(Product);
        //}

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productIndb = _unitOfWork.product.GetOne(x => x.Id == id);
            if (productIndb == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
            _unitOfWork.product.Remove(productIndb);
            var oldimg = Path.Combine(_webHostEnvironment.WebRootPath, productIndb.Image.TrimStart('\\'));
            if (System.IO.File.Exists(oldimg))
            {
                System.IO.File.Delete(oldimg);
            }
            _unitOfWork.Complete();
            return Json(new { success = true, message = "file has been Deleted" });
        }




    }
}
