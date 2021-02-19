using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Patient_Side.Models;
using Medical.Models;

namespace Patient_Side.Controllers
{
    public class Home2Controller : Controller
    {
        private readonly MedContext _context;

        public Home2Controller(MedContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewModel vm = new ViewModel();
            vm.doctorList = _context.DOCTORTB.Where(x=>x.Doctor_IsActive == true).Take(4).ToList();


            TempData["OrderCount"] = _context.ORDERTB.Count();
            TempData["DoctorCount"] = _context.DOCTORTB.Count();
            TempData["MedicineCount"] = _context.MEDICINETB.Count();
            TempData["PatientCount"] = _context.PATIENTTB.Count();

            ViewBag.OrderCount = TempData["OrderCount"];
            ViewBag.DoctorCount = TempData["DoctorCount"];
            ViewBag.MedicineCount = TempData["MedicineCount"];
            ViewBag.PatientCount = TempData["PatientCount"];

            TempData.Keep("OrderCount");
            TempData.Keep("DoctorCount");
            TempData.Keep("MedicineCount");
            TempData.Keep("PatientCount");


            var rm = from r in _context.REVIEWTB
                     join p in _context.PATIENTTB on r.Patient_ID equals p.Patient_ID
                     select new Review
                     {
                         Review_ID = r.Review_ID,
                         Review_Msg = r.Review_Msg,
                         Patient_ID = r.Patient_ID,
                         Patient_Name = p.Patient_Name,
                         Patient_Profile = p.Patient_Profile
                     };
            vm.reviewList = rm.ToList();
            

            TempData.Keep("SessionID");
            return View(vm);
        }

        

        
    }
}
