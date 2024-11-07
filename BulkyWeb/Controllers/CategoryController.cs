using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;
        public CategoryController(ICategoryRepository db) 
        { 
            _categoryRepo = db;        
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _categoryRepo.GetAll().ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The display order cannot exactly match the Name.");
            }
            if (ModelState.IsValid)
            {
                _categoryRepo.Add(obj);
                _categoryRepo.Save();

                TempData["success"] = "Category Created Successfully!";
            }            
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            if(id==null || id==0)
            {
                return NotFound();
            }
            Category? categoryfromDb = _categoryRepo.Get(u=> u.Id ==id);
            //Category? categoryfromDb1 = _db.Categories.FirstOrDefault(u=> u.Id==id);
            //Category? categoryfromDb2 = _db.Categories.Where(u => u.Id==id).FirstOrDefault();
            if(categoryfromDb == null)
            {
                return NotFound();
            }
            return View(categoryfromDb);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _categoryRepo.Update(obj);
                _categoryRepo.Save();
                TempData["success"] = "Category Updated Successfully!";
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryfromDb = _categoryRepo.Get(u => u.Id == id);

            if (categoryfromDb == null)
            {
                return NotFound();
            }
            return View(categoryfromDb);
        }

        [HttpPost,ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _categoryRepo.Get(u => u.Id == id);

            if (obj == null)
            {
                return NotFound();
            }
            _categoryRepo.Remove(obj);
            _categoryRepo.Save();

            TempData["success"] = "Category Deleted Successfully!";
            return RedirectToAction("Index");
        }
    }
}
