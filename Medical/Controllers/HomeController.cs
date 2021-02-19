using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Medical.Models;
using Microsoft.AspNetCore.Http;

namespace Medical.Controllers
{
    public class HomeController : Controller
    {
        private readonly MedContext _Db;
        public HomeController(MedContext Db)
        {
            _Db = Db;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Create", "Login");
            }

            // var dl = _Db.DOCTORTB.Count();

            ViewBag.doctorcount = _Db.DOCTORTB.Count();
            TempData["doctorcount"] = _Db.DOCTORTB.Count();

            ViewBag.patientcount = _Db.PATIENTTB.Count();
            TempData["patientcount"] = _Db.PATIENTTB.Count();

            ViewBag.ordercount = _Db.ORDERTB.Count();
            TempData["ordercount"] = _Db.ORDERTB.Count();

            ViewBag.medicinecount = _Db.MEDICINETB.Count();
            TempData["medicinecount"] = _Db.MEDICINETB.Count();

            return View();

            
           

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
