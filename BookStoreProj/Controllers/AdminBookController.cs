using BookStore.Consts;
using BookStore.Models;
using BookStore.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace BookStoreProj.Controllers
{
    public class AdminBookController : Controller
    {
        BookService bookService = new BookService();
        CategoryService categoryService = new CategoryService();
        public ActionResult Index()
        {
            try
            {
                var books = bookService.GetBasicInfoBooks();
                return View(books);
            }
            catch(Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }
      
        public ActionResult Detailes(int bookId)
        {
            try
            {
                var book = bookService.GetBookDetailes(bookId);
                return View(book);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction(nameof(Index));  
            }
        }
        public ActionResult AddBook()
        {

            var model = new AddOrUpdateBookModel();
            try
            {
                model.CategorySelectList = GetCategorySelectList();
                return View(model);
            }
            catch(Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(model);
            }
        }
        [HttpPost]
        public ActionResult AddBook(AddOrUpdateBookModel model)
        {
            try
            {
                
                if (ModelState.IsValid)
                {
                    model.Published = DateTime.SpecifyKind(model.Published.Value, DateTimeKind.Utc);
                    var book = bookService.GetBookByTitle(model.BookTitle);
                    if(book!=null)   
                        throw new Exception($"The Book With Title:{model.BookTitle} Is Already Found");
                    model.CategorySelectList = GetCategorySelectList();
                    string fileName = "";
                    if (model.ImageFile != null && model.ImageFile.ContentLength > 0)
                    {
                        var fileExtension = Path.GetExtension(model.ImageFile.FileName)?.ToLowerInvariant();
                        if (!FileSettings.FileFormatAccept.ToLowerInvariant().Split(',').ToList().Contains(fileExtension))
                            throw new Exception($"Only file formats {FileSettings.FileFormatAccept.Remove('"')} are accepted.");
                        fileName = Path.GetFileName(model.ImageFile.FileName);
                        string filePath = Path.Combine(Server.MapPath("~/Books"), fileName);
                        model.ImageFile.SaveAs(filePath);
                    }
                    model.CoverImageUrl = fileName;
                    bookService.AddOrUpdateBook(model);
                    return RedirectToAction(nameof(Index));
                }
                model.CategorySelectList = GetCategorySelectList();
                return View(model);
            }
            catch(Exception ex)
            {
                model.CategorySelectList = GetCategorySelectList();
                ViewBag.ErrorMessage = ex.Message;
                return View(model);
            }
        }
        public ActionResult Edit(int bookId)
        {
            try
            {
                var book = bookService.GetBookDetailes(bookId);
                var model = new AddOrUpdateBookModel
                {

                    BookTitle = book.BookTitle,
                    Subject = book.Subject,
                    Author = book.Author,
                    Published = book.publishDateValue,
                    Publisher = book.PublishHouse,
                    QuantityInStore = book.QuantityInStore,
                    CoverImageUrl = book.CoverImageUrl,
                    Description = book.Description,
                    Price = book.Price,
                    CategoryId = book.Category.CategoryId.Value,
                    CategoryName = book.Category.CategoryName,

                    CategorySelectList = GetCategorySelectList()
                };
                model.CategorySelectList = GetCategorySelectList();
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        public ActionResult Edit(AddOrUpdateBookModel model)
        {
            try
            {
                model.Published = DateTime.SpecifyKind(model.Published.Value, DateTimeKind.Utc);
                if (ModelState.IsValid)
                {
                    if (bookService.GetBooksByCategoryId(0).Any(book => book.BookTitle.ToLower() == model.BookTitle.ToLower() && book.BookId!=model.BookId))
                        throw new Exception($"The Book With Title:{model.BookTitle} IS Already Found");
                    if (model.ImageFile != null && model.ImageFile.ContentLength > 0)
                    {
                        var fileExtension = Path.GetExtension(model.ImageFile.FileName)?.ToLowerInvariant();
                        if (!FileSettings.FileFormatAccept.ToLowerInvariant().Split(',').ToList().Contains(fileExtension))
                            throw new Exception($"Only file formats {FileSettings.FileFormatAccept.Remove('"')} are accepted.");
                        string fileName = Path.GetFileName(model.ImageFile.FileName);
                        string filePath = Path.Combine(Server.MapPath("~/Books"), fileName);
                        model.ImageFile.SaveAs(filePath);
                        model.CoverImageUrl = fileName;
                    }
                    bookService.AddOrUpdateBook(model);
                    return RedirectToAction(nameof(Index));
                }
                model.CategorySelectList = GetCategorySelectList();
                return View(model);
            }
            catch(Exception ex)
            {
                model.CategorySelectList = GetCategorySelectList();
                ViewBag.ErrorMessage = ex.Message;
                return View(model);
            }
        }
        [HttpPost]
        public void Delete(int id)
        {
            bookService.DeleteBook(id);   
        }
        public ActionResult RequiredBooks()
        {
            try
            {
                var books = bookService.GetRequiredBooks();
                return View(books);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        #region Private Method
        private List<SelectListItem> GetCategorySelectList() {
           return categoryService.GetCategories().Select(p => new SelectListItem
            {
                Value = p.CategoryId.ToString(),
                Text = p.CategoryName.ToString(),
            }).ToList();
        }
        #endregion
    }
}