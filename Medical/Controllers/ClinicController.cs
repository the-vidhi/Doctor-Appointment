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
    public class ClinicController : Controller
    {
        private readonly MedContext _context;
       
        public ClinicController(MedContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? pageNumber)
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Create", "Login");
            }
            int pageSize = 3;
            
            var lists = _context.CLINICTB.Where(c => c.Clinic_IsActive == true);
            return View(await PaginatedList<Clinic>.CreateAsync(lists, pageNumber ?? 1, pageSize));

            
           // return View(ClinicList);
        }
        public async Task<IActionResult> BlockedClinic(int? pageNumber)
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Create", "Login");
            }
            int pageSize = 3;
            var c = _context.CLINICTB.Where(c => c.Clinic_IsActive == false);
            return View(await PaginatedList<Clinic>.CreateAsync(c, pageNumber ?? 1, pageSize));

        }

       

        
        public async Task<IActionResult> BlockClinic(int id)
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Create", "Login");
            }
            var ord = await _context.CLINICTB.FindAsync(id);
            ord.Clinic_ID = id;

            if (id != ord.Clinic_ID)
            {
                return NotFound();
            }
            ord.Clinic_IsActive = false;
            _context.CLINICTB.Update(ord);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Clinic");
        }
        public async Task<IActionResult> UnBlockClinic(int id)
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Create", "Login");
            }
            var ord = await _context.CLINICTB.FindAsync(id);
            ord.Clinic_ID = id;

            if (id != ord.Clinic_ID)
            {
                return NotFound();
            }
            ord.Clinic_IsActive = true;
            _context.CLINICTB.Update(ord);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Clinic");
        }
        private bool ClinicExists(int id)
        {
            return _context.CLINICTB.Any(e => e.Clinic_ID == id);
        }
        
    }
}
