using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SmokeyBook.DataAccess.Repository.IRepository;
using SmokeyBook.Models;
using SmokeyBook.Models.ViewModels;

namespace SmokeyBook.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork,IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult GetProducts()
        { 
            return View();
        }

        /*used datatalbe for the List so have to use api*/
        #region Api Calls For Getting Item List
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> products = _unitOfWork.productRepository.GetAll().ToList();
            return Json(new { data = products });
        }
        #endregion

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM();

            productVM.CategoryList = _unitOfWork.categoryRepository.GetAll().Select(x=> new SelectListItem
            {
                Text=x.Name,
                Value=x.Id.ToString()
            });
            productVM.Product = new Product();

            if (id == null || id == 0)
            {
                //create

                return View(productVM);
            }
            else
            {
                //update

                productVM.Product = _unitOfWork.productRepository.Get(x => x.Id == id);
                return View(productVM);
            }
             
        }

        [HttpPost]
       public IActionResult Upsert(ProductVM productVM,IFormFile? file)
        {
            if (ModelState.IsValid)
            {

                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                    string productPath = Path.Combine(wwwRootPath, @"images\products");

                    if (!string.IsNullOrEmpty(productVM.Product.ImageURL))
                    {
                        //delete the old image

                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageURL.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }


                    using (var fileStream=new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVM.Product.ImageURL = @"\images\products\" + fileName;

                }

                if (productVM.Product.Id == 0)
                {
                    //create
                    _unitOfWork.productRepository.Add(productVM.Product);
                }

                else
                {
                    // update
                    _unitOfWork.productRepository.Update(productVM.Product);
                }

               
                _unitOfWork.Save();
                TempData["success"] = "Category Saved / Updated successfully";
                return RedirectToAction("GetProducts");
            }

            return View();

        }

    }
}
