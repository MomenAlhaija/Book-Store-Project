using BookStore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStoreProj.Controllers
{
    public class AdminController : Controller
    {
        SataticsService sataticsService=new SataticsService();
        public ActionResult Index()
        {
            var statics=sataticsService.GetSatics();
            return View(statics);
        }
    }
}