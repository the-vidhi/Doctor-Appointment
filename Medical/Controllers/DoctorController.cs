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
    public class DoctorController : Controller
    {
        private readonly MedContext _context;

        public DoctorController(MedContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Create", "Login");
            }

            var docList = from d in _context.DOCTORTB
                          join ct in _context.CATEGORYTB on d.Category_ID equals ct.Category_ID
                          where d.Doctor_IsActive == true

                          select new Doctor
                          {
                              Doctor_ID = d.Doctor_ID,
                              Doctor_Name = d.Doctor_Name,
                              Doctor_Email = d.Doctor_Email,
                              Doctor_Gender = d.Doctor_Gender,
                              Doctor_Contact = d.Doctor_Contact,
                              Doctor_IsActive = d.Doctor_IsActive,
                              Category_ID = d.Category_ID,
                              Doctor_Degree = d.Doctor_Degree,
                              Doctor_Profile = d.Doctor_Profile,

                              Category_Name = ct == null ? "" : ct.Category_Name,
                              
                          };
            int pageSize = 3;
            return View(await PaginatedList<Doctor>.CreateAsync(docList.AsNoTracking(), pageNumber ?? 1, pageSize));
            //return View(docList);
        }
        public IActionResult DoctorList()
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Create", "Login");
            }

            var docList = from d in _context.DOCTORTB
                          join ct in _context.CATEGORYTB on d.Category_ID equals ct.Category_ID
                          where d.Doctor_IsActive == true

                          select new Doctor
                          {
                              Doctor_ID = d.Doctor_ID,
                              Doctor_Name = d.Doctor_Name,
                              Doctor_Email = d.Doctor_Email,
                              Doctor_Gender = d.Doctor_Gender,
                              Doctor_Contact = d.Doctor_Contact,
                              Doctor_IsActive = d.Doctor_IsActive,
                              Category_ID = d.Category_ID,
                              Doctor_Degree = d.Doctor_Degree,
                              Doctor_Profile = d.Doctor_Profile,

                              Category_Name = ct == null ? "" : ct.Category_Name,
                              
                          };

            return View(docList);
        }

        public async Task<IActionResult> BlockedDoctor(int? pageNumber)
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Create", "Login");
            }
            var docList = from d in _context.DOCTORTB
                          join ct in _context.CATEGORYTB on d.Category_ID equals ct.Category_ID
                          
                          where d.Doctor_IsActive == false
                          select new Doctor
                          {
                              Doctor_ID = d.Doctor_ID,
                              Doctor_Name = d.Doctor_Name,
                              Doctor_Email = d.Doctor_Email,
                              Doctor_Gender = d.Doctor_Gender,
                              Doctor_Contact = d.Doctor_Contact,
                              Doctor_IsActive = d.Doctor_IsActive,
                              Category_ID = d.Category_ID,
                              Doctor_Degree = d.Doctor_Degree,
                              Doctor_Profile = d.Doctor_Profile,

                              Category_Name = ct == null ? "" : ct.Category_Name,
                              
                          };
            int pageSize = 3;
            return View(await PaginatedList<Doctor>.CreateAsync(docList.AsNoTracking(), pageNumber ?? 1, pageSize));
          //  return View(docList);
        }

        // GET: Doctor/Details/5
        public IActionResult Details(int? id)
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Create", "Login");
            }
            if (id == null)
            {
                return NotFound();
            }

            //var medlist = (from c in _context.CLINICTB
            //               join d in _context.DOCTORTB on c.Doctor_ID equals d.Doctor_ID
            //               join ct in _context.CATEGORYTB on d.Category_ID equals ct.Category_ID
            //               join s in _context.STATETB on c.State_ID equals s.State_ID
            //               join cty in _context.CITYTB on c.City_ID equals cty.City_ID


            //               where d.Doctor_IsActive == true
            //               select new Doctor
            //               {
            //                   Doctor_ID = d.Doctor_ID,
            //                   Doctor_Name = d.Doctor_Name,
            //                   Doctor_Email = d.Doctor_Email,
            //                   Doctor_Gender = d.Doctor_Gender,
            //                   Doctor_Contact = d.Doctor_Contact,
            //                   Doctor_IsActive = d.Doctor_IsActive,
            //                   Doctor_Degree = d.Doctor_Degree,
            //                   Doctor_Profile = d.Doctor_Profile,
            //                   Doctor_Education = d.Doctor_Education,
            //                   Doctor_Experience = d.Doctor_Experience,
            //                   Doctor_Fees = d.Doctor_Fees,                               

            //                   Category_ID = d.Category_ID,
            //                   Category_Name = ct == null ? "" : ct.Category_Name,

            //               })
            //                .FirstOrDefault(m => m.Doctor_ID == id);
            //ViewModel vm = new ViewModel();            
            //vm.doctor = medlist;
            //if (medlist == null)
            //{
            //    return NotFound();
            //}
            //return View(vm);

            ViewModel vm = new ViewModel();
            vm.doctor =(from d in _context.DOCTORTB
                        join ct in _context.CATEGORYTB on d.Category_ID equals ct.Category_ID

                        select new Doctor
                        {
                            Doctor_ID = d.Doctor_ID,
                            Doctor_Name = d.Doctor_Name,
                            Doctor_Email = d.Doctor_Email,
                            Doctor_Gender = d.Doctor_Gender,
                            Doctor_Contact = d.Doctor_Contact,
                            Doctor_IsActive = d.Doctor_IsActive,
                            Doctor_Degree = d.Doctor_Degree,
                            Doctor_Profile = d.Doctor_Profile,
                            Doctor_Education = d.Doctor_Education,
                            Doctor_Experience = d.Doctor_Experience,
                            Doctor_Fees = d.Doctor_Fees,

                            Category_ID = d.Category_ID,
                            Category_Name = ct == null ? "" : ct.Category_Name,
                        })
                        .FirstOrDefault(m => m.Doctor_ID == id);

            vm.reviewList = (from r in _context.REVIEWTB
                             join p in _context.PATIENTTB on r.Patient_ID equals p.Patient_ID
                             join d in _context.DOCTORTB on r.Doctor_ID equals d.Doctor_ID
                             where r.Doctor_ID == id

                             select new Review
                             {
                                 Review_ID = r.Review_ID,
                                 Review_Msg = r.Review_Msg,
                                 Patient_ID = r.Patient_ID,
                                 Patient_Name = p.Patient_Name,
                                 Doctor_ID = r.Doctor_ID,
                                 Doctor_Name = d.Doctor_Name,
                                 Patient_Profile = p.Patient_Profile
                             }).ToList();

            vm.clinic = (from c in _context.CLINICTB
                         join d in _context.DOCTORTB on c.Doctor_ID equals d.Doctor_ID
                         // where c.Doctor_ID == id
                         select new Clinic
                         {
                             Clinic_ID = c.Clinic_ID,
                             Clinic_Address = c.Clinic_Address,
                             Clinic_Contact = c.Clinic_Contact,
                             Clinic_IsActive = c.Clinic_IsActive

                         }).FirstOrDefault(m => m.Doctor_ID == id);

            //vm.clinic = _context.CLINICTB.FirstOrDefault(m => m.Doctor_ID == id);
            //vm.clinic = _context.CLINICTB.ToList();

            return View(vm);
        }

        
        public async Task<IActionResult> BlockDoctor(int id)
        {
            var ord = await _context.DOCTORTB.FindAsync(id);
            ord.Doctor_ID = id;

            if (id != ord.Doctor_ID)
            {
                return NotFound();
            }
            ord.Doctor_IsActive = false;
            _context.DOCTORTB.Update(ord);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Doctor");
        }
        public async Task<IActionResult> UnblockDoctor(int id)
        {
            var ord = await _context.DOCTORTB.FindAsync(id);
            ord.Doctor_ID = id;

            if (id != ord.Doctor_ID)
            {
                return NotFound();
            }
            ord.Doctor_IsActive = true;
            _context.DOCTORTB.Update(ord);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Doctor");
        }

        private bool DoctorExists(int id)
        {
            return _context.DOCTORTB.Any(e => e.Doctor_ID == id);
        }
    }
}
