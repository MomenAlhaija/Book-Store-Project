using BookStore;
using BookStore.Models;
using BookStore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace BookStoreProj.Controllers
{
    public class UserController : Controller
    {
        UserService userService = new UserService();
        SaleService saleService = new SaleService();
        public ActionResult UserProfile()
        {
            try
            {
                if (Session["UserId"] is null)
                    throw new Exception("Please Login First");
                int? userId = int.Parse(Session["UserId"].ToString());
                if (userId != null)
                {
                    var user = userService.GetUserById(userId.Value);
                    return View(user);
                }
                return RedirectToAction("Login", "LogIn");
            }
            catch (Exception ex) {
                ViewBag.ErrorMessage=ex.Message;
                return View();
            }
        }
        [HttpPost]
        public ActionResult UpdateProfile(UserModel userModel)
        {
            try
            {
                userService.AddOrUpdateUser(new RegisterUserModel
                {
                    UserName = userModel.UserName,
                    Email=userModel.Email,
                    FirstName=userModel.FirstName,
                    LastName =userModel.LastName,
                    Password=userModel.Password,
                    UserId= userModel.UserId,
                    UserType=(int)UserTypeEnum.Customer,
                    Gender=userModel.Gender,
                    BirthDate=userModel.BirthDate,
                    Phone=userModel.Phone
                });
                return Json(new { success = true });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = ex.Message });
            }
        }
        public ActionResult EditProfile()
        {
            try
            {
                if (Session["UserId"] is null)
                    throw new Exception("Please Login First To Can Update Your Profile");
                int? userId = int.Parse(Session["UserId"].ToString());
                var user = userService.GetUserById(userId.Value);
                return View(user);
            }
            catch(Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }
        public ActionResult ViewSaleDetailes(int saleId)
        {
            var saleDetailes = saleService.GetSaleDetailes(saleId);
            return View(saleDetailes);
        }
        public ActionResult GetSalesForUser()
        {
            int? userId = int.Parse(Session["UserId"].ToString());
            var userSales = saleService.GetSales(userId);
            return PartialView("_userSales",userSales);
        }
        [HttpPost]
        public ActionResult UpdatePassword(ResetPasswordModel password)
        {
            try
            {
                if (password.NewPassword == password.ConfirmPassword)
                {
                    if (Session["UserId"] is null)
                        throw new Exception("Please Login fist To Can Update Password");
                    int? userId = int.Parse(Session["UserId"].ToString());
                    if (userId != null)
                    {
                        var user = userService.GetUserById(userId.Value);
                        if (user.Password == password.OldPassword)
                        {
                            userService.ChangePassword(password.NewPassword, userId.Value);
                            return Json(new { success = true });
                        }
                        throw new Exception("The Old Password is Incorrect");
                    }
                    throw new Exception("Not Fond The User");
                }
                throw new Exception("The Password and Confirm Password Must Math");

            }
            catch(Exception ex)
            {
                return Json(new { success = false, errorMessage = ex.Message });
            }
        }

    }
}