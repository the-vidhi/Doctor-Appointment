using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Medical.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Medical.Controllers
{
    public class DashboardController : Controller
    {
        private readonly MedContext _context;

        public DashboardController(MedContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Create", "Login");
            }
            //else
            //{
            ViewBag.doctorcount = _context.DOCTORTB.Count();
            TempData["doctorcount"] = _context.DOCTORTB.Count();

            ViewBag.patientcount = _context.PATIENTTB.Count();
            TempData["patientcount"] = _context.PATIENTTB.Count();

            ViewBag.ordercount = _context.ORDERTB.Count();
            TempData["ordercount"] = _context.ORDERTB.Count();

            ViewBag.medicinecount = _context.MEDICINETB.Count();
            TempData["medicinecount"] = _context.MEDICINETB.Count();

            //ViewBag.Keep();
            //}
            return View();
        }
    }
}
