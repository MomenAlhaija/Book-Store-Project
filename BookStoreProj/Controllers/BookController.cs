using BookStore.Models;
using BookStore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace BookStoreProj.Controllers
{
    public class BookController : Controller
    {
        BookService bookService=new BookService();
        CategoryService categoryService=new CategoryService();

        public ActionResult BookDetailes(int bookId)
        {
            var book=bookService.GetBookById(bookId);
            return View(book);
        }
        public ActionResult BookFilter()
        {
            var library = GetAllBooks();
            return View(library);
        }
        [HttpPost]
        public ActionResult BooksFilter(BookFilterModel filters)
        {
            var library = GetAllBooks();
            if (filters.AuthorsNames!=null)
            {
                library.Books = library.Books.Where(book => filters.AuthorsNames.Contains(book.Author)).ToList();
            }
            if(filters.CategoryIds!=null)
            {
                library.Books=library.Books.Where(book=>filters.CategoryIds.Contains(book.Category.CategoryId.Value)).ToList();
            }
            if (filters.MinPrice != null)
            {
                library.Books=library.Books.Where(book=>book.Price>=filters.MinPrice).ToList();
            }
            if (filters.MaxPrice != null)
            {
                library.Books=library.Books.Where(book=>book.Price<=filters.MaxPrice).ToList(); 
            }
            if(filters.BookTitle!=null)
            {
                library.Books=library.Books.Where(book=>book.BookTitle.ToLower().Contains(filters.BookTitle.ToLower())).ToList();
            }
            return PartialView("_Books", library.Books);
        }
        public ActionResult Rate()
        {
            return PartialView("_rate");
        }
        public ActionResult BookFilterByTitle(string bookTitle)
        {
            var library = GetAllBooks();
            if(bookTitle!=null)
                library.Books=library.Books.Where(p=>p.BookTitle.ToLower().Contains(bookTitle.ToLower())).ToList();
            return PartialView("_Books", library.Books);
        }
        [HttpPost]
        public void RateBook(int rate,int bookId)
        {
            bookService.RateBook(new RateBookModel
            {
                Rate = rate,
                BookId = bookId,
                UserId=int.Parse(Session["UserId"].ToString())
            });

        }
        [HttpGet]
        public ActionResult GetBookByCategory(int categoryId = 0)
        {
            var books=bookService.GetBooksByCategoryId(categoryId);
            return PartialView("_Books",books);
        }

        [HttpPost]
        public ActionResult CheckPurchase(int bookId)
        {
            try
            {
                if (Session["UserId"] is null)
                    throw new Exception("Please Login First");
                int? userId = int.Parse(Session["UserId"].ToString());
                bool hasPurchased = false;
                if (userId != null) 
                    hasPurchased = bookService.CheckUserPurchasedBook(bookId, userId.Value);
                else
                    throw new Exception("You Must Login First To Can Make Vote");
                return Json(new { purchased = hasPurchased  });
            }
            catch(Exception ex)
            {
                return Json(new { success = false, errorMessage = ex.Message });

            }
        }
        public ActionResult GetVotesOnBook(int bookId)
        {
            var votes = bookService.GetRatesForBook(bookId);
            return PartialView("_Votes", votes);
        } 
        #region private Method
        private LibraryModel GetAllBooks()=> new LibraryModel()
        {
            Books = bookService.GetBooksByCategoryId(0),
            Categories = categoryService.GetCategories(),
            Authors = bookService.GetBooksByCategoryId(0).Select(book => book.Author).Distinct().ToList()
        };

        #endregion

    }
}