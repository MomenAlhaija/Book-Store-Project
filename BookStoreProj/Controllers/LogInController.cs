using BookStore;
using BookStore.Models;
using BookStore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStoreProj.Controllers
{
    public class LogInController : Controller
    {
        UserService userService=new UserService();
    
        public ActionResult Login()
        {
            var model=new LoginModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult Login(LoginModel login)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = userService.GetUserByEmail(login.Email);
                    if (user != null && user.Password == login.Password)
                    {
                        SaveUserIdInSession(user.UserId.Value);
                        switch (user.UserType)
                        {
                            case (int)(UserTypeEnum.Admin):
                                return RedirectToAction("Index", "Admin");
                            default:
                                return RedirectToAction("BookFilter", "Book");
                        }
                    }
                    throw new Exception("The Email or Password is Incorrect");
                }
                return View(login);
            }
            catch(Exception ex) 
            { 
                ViewBag.ErrorMessage=ex.Message;
                return View(login); 
            }
        }
        public ActionResult Register()
        {
            var model=new RegisterUserModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult Register(RegisterUserModel user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (userService.GetUserByEmail(user.Email) is null)
                    {
                        int? userId = userService.AddOrUpdateUser(user);
                        SaveUserIdInSession(userId.Value);
                        return RedirectToAction("Index", "Home");
                    }
                    throw new Exception($"The Email {user.Email} Is Already Register");
                }
                return View(user);
            }
            catch(Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message; 
                return View(user);
            }
        }
        [HttpPost]
        public ActionResult Logout()
        {
            DeleteUserIdFromSession();
            return RedirectToAction("Index", "Home");
        }
        #region Private Method
        private void SaveUserIdInSession(int userId)
        {
            Session["UserId"] = userId;
        }

        private void DeleteUserIdFromSession()
        {
            if (Session["UserId"] != null)
            {
                Session.Remove("UserId");
            }
        }
        #endregion
    }
}