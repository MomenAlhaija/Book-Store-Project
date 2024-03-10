using BookStore.Models;
using BookStore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace BookStoreProj.Controllers
{
    public class SaleController : Controller
    {
        SaleService saleService=new SaleService();
        public ActionResult Index(List<SalesModel> sales)
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetSales( FilterSaleDateModel filterModel)
        {

            var sales = saleService.GetSales();
            if (filterModel.FromDate != null)
            {
                sales = sales.Where(saleItem => (saleItem.SalesDate.Value.ToShortDateString()).AsDateTime() >=
                                    filterModel.FromDate.Value.ToShortDateString().AsDateTime()).ToList();
            }

            if (filterModel.ToDate != null)
            {
                sales = sales.Where(saleItem => saleItem.SalesDate.Value.ToShortDateString().AsDateTime()
                <= filterModel.ToDate.Value.ToShortDateString().AsDateTime()).ToList();
            }
            return PartialView("_sales",sales);
        }
        public ActionResult SaleDetails(int saleId) {
        
             var saleDetailes=saleService.GetSaleDetailes(saleId);  
             return View(saleDetailes);
        
        }

    }
}