using Microsoft.AspNetCore.Mvc;
using SmokeyBook.DataAccess.Repository;
using SmokeyBook.DataAccess.Repository.IRepository;
using SmokeyBook.Models;

namespace SmokeyBook.WebApp.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class CategoryController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        
        public IActionResult GetCategory()
        {
            List<Category> categories = _unitOfWork.categoryRepository.GetAll().ToList();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            //Create Code

            if (ModelState.IsValid)
            {
                _unitOfWork.categoryRepository.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category saved successfully";
            }

            return RedirectToAction("GetCategory");
        }

        public IActionResult Edit(int? id)
        {
            //Edit Code

            if (id == 0 || id == null)
            {
                return NotFound();
            }

            Category? category = _unitOfWork.categoryRepository.Get(x => x.Id == id);

            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.categoryRepository.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category edited successfully";
                return RedirectToAction("GetCategory");
            }
            return View();

        }

        public IActionResult Delete(int? id)
        {
            //Delete Code

            if (id == 0 || id == null)
            {
                return NotFound();
            }

            Category? category = _unitOfWork.categoryRepository.Get(x => x.Id == id);

            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _unitOfWork.categoryRepository.Get(x => x.Id == id);

            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.categoryRepository.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category removed successfully";

            return RedirectToAction("GetCategory");
        }

    }
}
