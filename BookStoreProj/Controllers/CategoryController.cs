using BookStore.Models;
using BookStore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStoreProj.Controllers
{
    public class CategoryController : Controller
    {
        CategoryService categoryService=new CategoryService();
        public ActionResult Index()
        {
            var categories=categoryService.GetCategories();
            return View(categories);
        }
        public ActionResult AddCategory()
        {
            var model = new CategoryModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult AddCategory(CategoryModel model)
        {
            try
            {
                categoryService.AddCategory(model);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(model);
            }
        }
        public ActionResult EditCategory(int? categoryId)
        {
            try
            {
                if (categoryId != null)
                {
                    var category = categoryService.GetCategorieById(categoryId.Value);
                    if(category != null)
                    {
                        return View(category);
                    
                    }
                    throw new Exception("Not Found The Category");
                }
                throw new Exception($"The {nameof(categoryId)} can't By Null Or Empty");          
            }
            catch(Exception ex) 
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult EditCategory(CategoryModel category)
        {
            categoryService.EditCategory(category);
            return RedirectToAction(nameof (Index));    
        }
        [HttpPost]
        public void Delete(int? id) 
        {
            categoryService.DeleteCategory(id);
        }
    }
}