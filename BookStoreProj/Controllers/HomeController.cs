using BookStore.Models;
using BookStore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStoreProj.Controllers
{
    public class HomeController : Controller
    {
        CategoryService categoryService=new CategoryService();
        BookService bookService=new BookService();  
        public ActionResult Index()
        {
            var categories=categoryService.GetCategories();
            return View(categories);
        }
        public ActionResult CountDown()
        {
            var statics=bookService.GetCountDown();
            return PartialView("_countDown",statics);
        }
        public ActionResult Service()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult GetBooksByCategory(int categoryId)
        {
            List<BookModel> books = bookService.GetBooksByCategoryId(categoryId).ToList();
            return Json(books, JsonRequestBehavior.AllowGet);
        }
    }
}