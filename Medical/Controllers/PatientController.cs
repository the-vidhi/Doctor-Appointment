using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Medical.Models;
using Microsoft.AspNetCore.Http;

namespace Medical.Controllers
{
    public class PatientController : Controller
    {
        private readonly MedContext _context;

        public PatientController(MedContext context)
        {
            _context = context;
        }

        // GET: Patient
        public async Task<IActionResult> Index(int? pageNumber)
        {
            //if (HttpContext.Session.GetString("SessionID") == null)
            //{
            //    return RedirectToAction("Create", "Login");
            //}
            var patList = from p in _context.PATIENTTB
                          join s in _context.STATETB on p.State_ID equals s.State_ID
                          join c in _context.CITYTB on p.City_ID equals c.City_ID
                          where p.Patient_IsActive == true

                          select new Patient
                          {
                              Patient_ID = p.Patient_ID,
                              Patient_Name = p.Patient_Name,
                              Patient_Address = p.Patient_Address,
                              Patient_Contact = p.Patient_Contact,
                              State_ID = p.State_ID,
                              City_ID = p.City_ID,
                              Patient_Email = p.Patient_Email,
                              Patient_IsActive = p.Patient_IsActive,
                              Patient_Profile = p.Patient_Profile,

                              State_Name = s == null ? "" : s.State_Name,
                              City_Name = c == null ? "" : c.City_Name,

                          };
            int pageSize = 3;

            return View(await PaginatedList<Patient>.CreateAsync
            (patList.AsNoTracking(), pageNumber ?? 1, pageSize));

            //return View(patList);
        }

        public async Task<IActionResult> BlockedPatient(int? pageNumber)
        {
            //if (HttpContext.Session.GetString("SessionID") == null)
            //{
            //    return RedirectToAction("Create", "Login");
            //}
            var patList = from p in _context.PATIENTTB
                          join s in _context.STATETB on p.State_ID equals s.State_ID
                          join c in _context.CITYTB on p.City_ID equals c.City_ID
                          where p.Patient_IsActive == false

                          select new Patient
                          {
                              Patient_ID = p.Patient_ID,
                              Patient_Name = p.Patient_Name,
                              Patient_Address = p.Patient_Address,
                              Patient_Contact = p.Patient_Contact,
                              State_ID = p.State_ID,
                              City_ID = p.City_ID,
                              Patient_Email = p.Patient_Email,
                              Patient_IsActive = p.Patient_IsActive,
                              Patient_Profile = p.Patient_Profile,

                              State_Name = s == null ? "" : s.State_Name,
                              City_Name = c == null ? "" : c.City_Name,

                          };
            int pageSize = 3;

            return View(await PaginatedList<Patient>.CreateAsync
            (patList.AsNoTracking(), pageNumber ?? 1, pageSize));

            //return View(patList);
        }

        public async Task<IActionResult> Details(int? id)
        {
            //if (HttpContext.Session.GetString("SessionID") == null)
            //{
            //    return RedirectToAction("Create", "Login");
            //}
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.PATIENTTB
                .FirstOrDefaultAsync(m => m.Patient_ID == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        
        
        public async Task<IActionResult> BlockPatient(int id)
        {
            var ord = await _context.PATIENTTB.FindAsync(id);
            ord.Patient_ID = id;

            if (id != ord.Patient_ID)
            {
                return NotFound();
            }
            ord.Patient_IsActive = false;
            _context.PATIENTTB.Update(ord);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Patient");
        }
        public async Task<IActionResult> UnBlockPatient(int id)
        {
            var ord = await _context.PATIENTTB.FindAsync(id);
            ord.Patient_ID = id;

            if (id != ord.Patient_ID)
            {
                return NotFound();
            }
            ord.Patient_IsActive = true;
            _context.PATIENTTB.Update(ord);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Patient");
        }
        private bool PatientExists(int id)
        {
            return _context.PATIENTTB.Any(e => e.Patient_ID == id);
        }


    }
}
