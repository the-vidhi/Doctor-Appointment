using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Doctor_Side.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Medical.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;


namespace Doctor_Side.Controllers
{
    public class DocAppointmentController : Controller
    {
        private readonly DoctorContext _context;
        private readonly IWebHostEnvironment _env;

        public DocAppointmentController(DoctorContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                ViewBag.SID = HttpContext.Session.GetInt32("SessionID");
                return RedirectToAction("Create", "Login");
            }

            var id = Convert.ToInt32(ViewBag.SID);
            var i = ViewBag.SID;
            var idd = TempData["SessionID"];
            var applist = (from a in _context.APPOINTMENTTB
                           join p in _context.PATIENTTB on a.Patient_ID equals p.Patient_ID
                           join d in _context.DOCTORTB on a.Doctor_ID equals d.Doctor_ID
                           where d.Doctor_ID.Equals(@TempData["SessionID"])
                          


                           select new DocAppointment
                           {
                               Appointment_ID = a.Appointment_ID,
                               Appointment_Date = a.Appointment_Date,
                               Appointment_Status = a.Appointment_Status,
                               Appointment_Msg = a.Appointment_Msg,
                               Appointment_Time = a.Appointment_Time,
                               Patient_ID = a.Patient_ID,
                               Doctor_ID = a.Doctor_ID,
                               Patient_Profile = p == null ?"":p.Patient_Profile,
                               Patient_Name = p == null ? "" : p.Patient_Name
                           });
            //.SingleOrDefault(m => m.Doctor_ID == i);

            ViewBag.DoctorName = TempData["SessionName"];
            TempData.Keep("SessionName");
            ViewBag.DoctorImg = TempData["SessionImg"];
            TempData.Keep("SessionImg");
            ViewBag.SID = TempData["Sessionid"];
            TempData.Keep("Sessionid");
            return View(applist);
        }
        public async Task<IActionResult> AppConfirm(int id, Appointment appointment)
        {
            var ord = await _context.APPOINTMENTTB.FindAsync(id);
            ord.Appointment_ID = id;

            if (id != ord.Appointment_ID)
            {
                return NotFound();
            }
            ord.Appointment_Status = "Confirm"; 
            _context.APPOINTMENTTB.Update(ord);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "DocAppointment");
        }
        public async Task<IActionResult> AppWait(int id, Appointment appointment)
        {
            var ord = await _context.APPOINTMENTTB.FindAsync(id);
            ord.Appointment_ID = id;

            if (id != ord.Appointment_ID)
            {
                return NotFound();
            }
            ord.Appointment_Status = "Waiting"; 
            _context.APPOINTMENTTB.Update(ord);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "DocAppointment");
        } 
        public async Task<IActionResult> AppRefuse(int id, Appointment appointment)
        {
            var ord = await _context.APPOINTMENTTB.FindAsync(id);
            ord.Appointment_ID = id;

            if (id != ord.Appointment_ID)
            {
                return NotFound();
            }
            ord.Appointment_Status = "Cancle"; 
            _context.APPOINTMENTTB.Update(ord);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "DocAppointment");
        }
    }
}
