using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Doctor_Side.Models;
using Microsoft.AspNetCore.Http;
using Medical.Models;

namespace Doctor_Side.Controllers
{
    public class Home1Controller : Controller
    {
        private readonly ILogger<Home1Controller> _logger;
        private readonly DoctorContext _context;


        public Home1Controller(ILogger<Home1Controller> logger,DoctorContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Login", "DoctorReg");
            }

            var applist = (from a in _context.APPOINTMENTTB
                           join p in _context.PATIENTTB on a.Patient_ID equals p.Patient_ID
                           join d in _context.DOCTORTB on a.Doctor_ID equals d.Doctor_ID
                           where d.Doctor_ID.Equals(@TempData["SessionID"])



                           select new DocAppointment
                           {
                               Appointment_ID = a.Appointment_ID,
                               Appointment_Date = a.Appointment_Date,
                               Patient_ID = a.Patient_ID,
                               Doctor_ID = a.Doctor_ID,
                               Patient_Profile = p == null ? "" : p.Patient_Profile,
                               Patient_Name = p == null ? "" : p.Patient_Name
                           });
            var ca = applist.Count();
            var reviewList = from r in _context.REVIEWTB
                             join p in _context.PATIENTTB on r.Patient_ID equals p.Patient_ID
                             join d in _context.DOCTORTB on r.Doctor_ID equals d.Doctor_ID
                             where d.Doctor_ID.Equals(@TempData["SessionID"])
                             select new DocReview
                             {
                                 Review_ID = r.Review_ID,
                                 Patient_ID = r.Patient_ID,
                                 Doctor_ID = r.Doctor_ID,
                                 Review_Msg = r.Review_Msg,
                                 Patient_Profile = p == null ? "" : p.Patient_Profile,

                                 Patient_Name = p == null ? "" : p.Patient_Name
                             };


            HttpContext.Session.SetInt32("Sessionca", ca);
            TempData["Sessionca"] = HttpContext.Session.GetInt32("Sessionca");
            ViewBag.CountAppo = HttpContext.Session.GetInt32("Sessionca");
           
            
            ViewBag.CountPat = _context.PATIENTTB.Count();
            ViewBag.CountReview = reviewList.Count();


            ViewBag.CountAppo = TempData["Sessionca"];
            TempData.Keep("Sessionca");
            ViewBag.DoctorName = TempData["SessionName"];
            TempData.Keep("SessionName");
            ViewBag.DoctorImg = TempData["SessionImg"];
            TempData.Keep("SessionImg");
            ViewBag.SID = TempData["Sessionid"];
            TempData.Keep("Sessionid");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new Models.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
