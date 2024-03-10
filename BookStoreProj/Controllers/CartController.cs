using BookStore.Models;
using BookStore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BookStoreProj.Controllers
{
    public class CartController : Controller
    {
        CartService cartService=new CartService();
        CheckoutService checkoutService=new CheckoutService();
        public ActionResult CartItem()
        {
            try
            {
                if (Session["UserId"] is null)
                    throw new Exception("Please Login First");
                int? userId = int.Parse(Session["UserId"].ToString());
                var cartItems = cartService.GetCartItems(userId.Value);
                return View(cartItems);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage=ex.Message; 
                return View();
            }
        }
        [HttpGet]
        public ActionResult GetTotalPrice()
        {
            try
            {
                if (Session["UserId"] is null)
                    throw new Exception("Please Login First");
                int? userId = int.Parse(Session["UserId"].ToString());
                decimal? totalPrice = cartService.GetTotalPrice(userId.Value);
                return Json(new { totalPrice }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { success = false, errorMessage = ex.Message });
            }
        }
        [HttpGet]
        public ActionResult CartItemsCount()
        {
            try
            {
                if (Session["UserId"] is null)
                        throw new Exception("Please Login First");
                int? userId = int.Parse(Session["UserId"].ToString());
                if (userId != null) {
                    var cartItems = cartService.GetCartItems(userId.Value);
                    int itemCount = cartItems != null ? cartItems.Count() : 0;
                    return Json(new { count = itemCount }, JsonRequestBehavior.AllowGet);
                }
                return RedirectToAction("Login", "LogIn");
            }
            catch(Exception ex)
            {
                return Json(new { success = false, errorMessage = ex.Message },JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost]
        public ActionResult AddToCart(AddToCartModel cart)
        {
            try
            {
                if (Session["UserId"] is null)
                    throw new Exception("Please Lgoin To Can Add To Your Cart");
                int? userId = int.Parse(Session["UserId"].ToString());
                if (userId is null)
                    throw new Exception("Please Login First");
                cart.UserId = userId.Value;
                cart.Quantiity = cart.Quantiity != null && cart.Quantiity != 0 ? cart.Quantiity : 1;
                cartService.AddToCart(cart);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = ex.Message });
            }
        }

        public ActionResult CheckOut() 
        {
            int? userId= int.Parse(Session["UserId"].ToString());
            if (userId != null)
            {
                var cartItem = cartService.GetCartItems(userId.Value);

                return View(cartItem);
            }
            return RedirectToAction("Login", "LogIn");
        }
        [HttpPost]
        public ActionResult Checkout()
        {
            try
            {
                int? userId = int.Parse(Session["UserId"].ToString());
                if (userId != null)
                {
                    checkoutService.Checkout(userId.Value);
                    return Json(new { message = "OK" }, JsonRequestBehavior.AllowGet);
                }
                return RedirectToAction("Login", "LogIn");
            }
            catch(Exception ex)
            {
                return Json(new { success = false, errorMessage=ex.Message });
            }
        }
        [HttpPost]
        public void RemoveItem(int itemId)
        {
            cartService.RemoveFromCart(itemId);
        }
        [HttpPost]
        public ActionResult UpdateQuantity(int itemId, int quantity)
        {
            try
            {
                cartService.UpdateQuantity(itemId, quantity);
                decimal updatedPrice = cartService.GetTotalPriceForItem(itemId);
                return Json(updatedPrice, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) {

                return Json(new { success = false, errorMessage =ex.Message }, JsonRequestBehavior.AllowGet);

            }

        }

    }
}