using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Medical.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Patient_Side.Controllers
{
    public class PatCareerController : Controller
    {
        private readonly MedContext _context;

        public PatCareerController(MedContext context)
        {
            _context = context;
        }

        // GET: PatCareer
       
        // GET: PatCareer/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Login", "PatientReg");
            }
            TempData["SessionID"] = HttpContext.Session.GetInt32("SessionID");
            ViewBag.SID = TempData["Sessionid"];
            TempData.Keep("SessionID");
            return View();
        }

        // POST: PatCareer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile fileobj, [Bind("Career_ID,Patient_ID,Candidate_Name,Candidate_Contact,Candidate_Resume,Candidate_Email")] Career career)
        {
           
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Login", "PatientReg");
            }
            var imgext = Path.GetExtension(fileobj.FileName);

            if (imgext == ".txt" || imgext == ".pdf")
            {
                var uploadimg = Path.Combine("../Medical/wwwroot", "Resume", fileobj.FileName);
                var stream = new FileStream(uploadimg, FileMode.Create);

                await fileobj.CopyToAsync(stream);
                stream.Close();
                TempData["SessionID"] = HttpContext.Session.GetInt32("SessionID");
                //TempData["SessionID"] = HttpContext.Session.GetString("SessionID");
                //mi.Medicine_ID = 1;
                career.Patient_ID = (int)TempData["SessionID"];
                career.Candidate_Resume = fileobj.FileName;
                await _context.CAREERTB.AddAsync(career);
                await _context.SaveChangesAsync();
            }
            ViewBag.SID = TempData["Sessionid"];
            TempData.Keep("SessionID");
            return RedirectToAction("Index","Home2");
        }

        

        

        private bool CareerExists(int id)
        {
            return _context.CAREERTB.Any(e => e.Career_ID == id);
        }
    }
}
