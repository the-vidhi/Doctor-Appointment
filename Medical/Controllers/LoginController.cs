using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Medical.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;

namespace Medical.Controllers
{
    public class LoginController : Controller
    {
        private readonly MedContext _context;

        public LoginController(MedContext context)
        {
            _context = context;
        }

        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Create", "Login");
            }
            HttpContext.Session.Clear();
            return RedirectToAction("Create", "Login");
        }

        public IActionResult LoginFail()
        {
            return View();
        }
        public IActionResult LoginFails()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View(new Login());
        }

        public IActionResult PasswordChangeMsg()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Login login)
        {

            try
            {

                //var user = _context.ADMINTB.SingleOrDefault(
                //    u => u.Admin_Email.Equals(login.Admin_Email)
                //    && u.Admin_Password.Equals(login.Admin_Password)
                //    );
                var user = _context.ADMINTB.Where(u => u.Admin_Email == login.Admin_Email && u.Admin_Password == login.Admin_Password).FirstOrDefault();

                if (user != null)
                {
                    //var SessionID = "_Name";

                    HttpContext.Session.SetString("SessionName", user.Admin_Name);                   
                    ViewBag.AdminName = HttpContext.Session.GetString("SessionName");
                    TempData["AdminName"] = HttpContext.Session.GetString("SessionName");

                    HttpContext.Session.SetString("SessionID", user.Admin_Email);                   
                    ViewBag.AdminID = HttpContext.Session.GetString("SessionID");
                    TempData["AdminID"] = HttpContext.Session.GetString("SessionID");
                    //ViewBag.Keep();
                    TempData.Keep();

                    //return View("../Dashboard/Index");
                    return RedirectToAction("Index", "Home");
                }

                else
                {
                    return RedirectToAction("LoginFail");
                }
                // return View();
            }
            catch (Exception e)
            {
                return View(e);
            }




            /*if (ModelState.IsValid)
            {
                _context.Add(login);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(login);
            */
        }
        public IActionResult Check()
        {
            return View(new Login());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Check(Login login)
        {

            try
            {
                var user = _context.ADMINTB.Where(u => u.Admin_Email == login.Admin_Email && u.Admin_Password == login.Admin_Password).FirstOrDefault();

                if (user != null)
                {
                    HttpContext.Session.SetString("SessionName", user.Admin_Name);                   
                    ViewBag.AdminName = HttpContext.Session.GetString("SessionName");
                    TempData["AdminName"] = HttpContext.Session.GetString("SessionName");

                    HttpContext.Session.SetString("SessionID", user.Admin_Email);                   
                    ViewBag.AdminID = HttpContext.Session.GetString("SessionID");
                    TempData["AdminID"] = HttpContext.Session.GetString("SessionID");
                    //ViewBag.Keep();
                    TempData.Keep();

                    //return View("../Dashboard/Index");
                    return RedirectToAction("ChangePassword", "Login");
                }

                else
                {
                    return RedirectToAction("LoginFails");
                }
            }
            catch (Exception e)
            {
                return View(e);
            }
        }

        public ActionResult ChangePassword(int? id)
        {
            //var item = _context.ADMINTB.Where(x => x.Admin_ID == id).First();

            //ViewBag.AdminName = TempData["SessionName"];
            //TempData.Keep("SessionName");
            //ViewBag.AdminImg = TempData["SessionImg"];
            //TempData.Keep("SessionImg");
            //ViewBag.SID = TempData["Sessionid"];
            //TempData.Keep("Sessionid");
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(int id, Login model)
        {
            var item = _context.ADMINTB.Where(x => x.Admin_ID == id).First();
            item.Admin_Password = model.Admin_Password;
            _context.Update(item);
            _context.SaveChanges();

            ViewBag.AdminName = TempData["SessionName"];
            TempData.Keep("SessionName");
            ViewBag.AdminImg = TempData["SessionImg"];
            TempData.Keep("SessionImg");
            ViewBag.SID = TempData["Sessionid"];
            TempData.Keep("Sessionid");
            return RedirectToAction("PasswordChangeMsg");
        }
        private bool LoginExists(int id)
        {
            return _context.ADMINTB.Any(e => e.Admin_ID == id);
        }
    }
}
