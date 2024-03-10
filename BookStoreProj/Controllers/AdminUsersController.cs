using BookStore.Models;
using BookStore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStoreProj.Controllers
{
    public class AdminUsersController : Controller
    {
        UserService userService=new UserService();  
        
        public ActionResult Index()
        {
            try
            {
                var users = userService.GetUsers();
                return View(users);
            }
            catch(Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            try
            {
                if (model.NewPassword == model.ConfirmPassword)
                {  
                    userService.ChangePassword(model.NewPassword, model.UserId.Value);
                    return Json(new { success = true });
                }
                throw new Exception("The Password and Confirm Password Must Match");
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = ex.Message });
            }
        }
        public ActionResult Detailes(int userId)
        {
            var user=userService.GetUserById(userId);   
            return View(user);
        }
        public ActionResult AddUser()
        {
            try
            {
                var model = new RegisterUserModel();
                return View(model);
            }
            catch(Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }
        [HttpPost]
        public ActionResult AddUser(RegisterUserModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (userService.GetUserByEmail(model.Email)!=null)
                        throw new Exception($"The Email:{model.Email} IS Already Exist");
                    userService.AddOrUpdateUser(model);
                    return RedirectToAction(nameof(Index));
                }
                return View(model);
            }
            catch(Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(model);
            }
        }    
        public ActionResult EditUser(int userId)
        {
            try
            {

                var user = userService.GetUserById(userId);
                return View(user);
            }
            catch(Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        public ActionResult EditUser(RegisterUserModel model)
        {
            try
            {   
                if (ModelState.IsValid)
                {
                    var user = userService.GetUserByEmail(model.Email);
                    if (user!=null && user.UserId!=model.UserId)
                        throw new Exception($"The Email:{model.Email} IS Already Exist");
                    userService.AddOrUpdateUser(model);
                    return RedirectToAction(nameof(Index));
                }
                return View(model);
            }
            catch(Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(model);
            }
        }
        [HttpPost]
        public void Delete(int id)
        {
            userService.DeleteUser(id);
        }
    }
}