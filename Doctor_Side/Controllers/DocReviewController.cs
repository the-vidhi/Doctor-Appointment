using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore;
using System.Web;
using System.IO;
using Doctor_Side.Models;
using Microsoft.AspNetCore.Http;

namespace Doctor_Side.Controllers
{
    public class DocReviewController : Controller
    {
        private readonly DoctorContext  _context;
        public DocReviewController(DoctorContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // HttpContext.Session.SetInt32("SessionID", user.Doctor_ID);
            // ViewBag.SID = HttpContext.Session.GetInt32("SessionID");
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Login", "DoctorReg");
            }

            var id = ViewBag.SID;
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
                                 Patient_Profile = p == null ?"":p.Patient_Profile,

                                 Patient_Name = p == null ? "" : p.Patient_Name
                             };

            ViewBag.DoctorName = TempData["SessionName"];
            TempData.Keep("SessionName");
            ViewBag.DoctorImg = TempData["SessionImg"];
            TempData.Keep("SessionImg");
            ViewBag.SID = TempData["Sessionid"];
            TempData.Keep("Sessionid");
            return View( reviewList);
        }

        public async Task<IActionResult> View(int? id)
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Login", "DoctorReg");
            }
            if (id == null)
            {
                return NotFound();
            }

            var clinic = (from p in _context.PATIENTTB
                         join s in _context.STATETB on p.State_ID equals s.State_ID
                         join c in _context.CITYTB on p.City_ID equals c.City_ID
                         

                         select new DocReview
                         {
                             Patient_ID = p.Patient_ID,
                             Patient_Name = p.Patient_Name,
                             Patient_Address = p.Patient_Address,
                             Patient_Contact = p.Patient_Contact,
                             State_ID = p.State_ID,
                             City_ID = p.City_ID,
                             Patient_Email = p.Patient_Email,
                             //Patient_IsActive = p.Patient_IsActive,
                             Patient_Profile = p.Patient_Profile,

                             State_Name = s == null ? "" : s.State_Name,
                             City_Name = c == null ? "" : c.City_Name,

                         }).FirstOrDefault(m => m.Patient_ID == id);
            //.FirstOrDefaultAsync(m => m.Patient_ID == id);
            if (clinic == null)
            {
                return NotFound();
            }
            ViewBag.DoctorName = TempData["SessionName"];
            TempData.Keep("SessionName");
            ViewBag.DoctorImg = TempData["SessionImg"];
            TempData.Keep("SessionImg");
            ViewBag.SID = TempData["Sessionid"];
            TempData.Keep("Sessionid");
            return View(clinic);
        }
        //public IActionResult View()
        //{
        //    var patientdetails = from p in _context.PATIENTTB
        //                         join 
        //    return View();

        //}
    }
}
