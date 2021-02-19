using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Doctor_Side.Models;
using System.IO;
using Microsoft.AspNetCore.Http;
using Medical.Models;
//using X.PagedList;

namespace Doctor_Side.Controllers
{
    public class DocClinicController : Controller
    {
        private readonly DoctorContext _context;

        public DocClinicController(DoctorContext context)
        {
            _context = context;
        }

        // GET: Clinic
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Login", "DoctorReg");
            }

            var cl = (from c in _context.CLINICTB
                      join s in _context.STATETB on c.State_ID equals s.State_ID
                      join ct in _context.CITYTB on c.City_ID equals ct.City_ID
                      where c.Doctor_ID.Equals(@TempData["SessionID"])
                      //ViewBag.SID

                      select new DocClinic
                      {
                          Clinic_ID = c.Clinic_ID,
                          Clinic_Name = c.Clinic_Name,
                          Clinic_Address = c.Clinic_Address,
                          Clinic_Pincode = c.Clinic_Pincode,
                          Clinic_Contact = c.Clinic_Contact,
                          Clinic_IsActive = c.Clinic_IsActive,
                          Clinic_Profile = c.Clinic_Profile,


                          State_Name = s == null ? "" : s.State_Name,
                          City_Name = ct == null ? "" : ct.City_Name
                      });

            ViewBag.DoctorName = TempData["SessionName"];
            TempData.Keep("SessionName");
            ViewBag.DoctorImg = TempData["SessionImg"];
            TempData.Keep("SessionImg");
            ViewBag.SID = TempData["Sessionid"];
            TempData.Keep("Sessionid");
            return View(cl);
            //.FirstOrDefaultAsync(m => m.Clinic_ID == cl);


        }

        // GET: Clinic/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Login", "DoctorReg");
            }
            if (id == null)
            {
                return NotFound();
            }

            var clinic = await _context.CLINICTB
                .FirstOrDefaultAsync(m => m.Clinic_ID == id);
            if (clinic == null)
            {
                return NotFound();
            }

            return View(clinic);
        }

        // GET: Clinic/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Login", "DoctorReg");
            }

            List<State> statelist = new List<State>();

            // ------- Getting Data from Database Using EntityFrameworkCore -------
            statelist = (from statetb in _context.STATETB
                         select statetb).ToList();

            // ------- Inserting Select Item in List -------
            statelist.Insert(0, new State { State_ID = 0, State_Name = "---Select State---" });

            // ------- Assigning categorylist to ViewBag.ListofCategory -------
            ViewBag.ListofState = statelist;

            ViewBag.DoctorName = TempData["SessionName"];
            TempData.Keep("SessionName");
            ViewBag.DoctorImg = TempData["SessionImg"];
            TempData.Keep("SessionImg");
            ViewBag.SID = TempData["Sessionid"];
            TempData.Keep("Sessionid");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile fileobj,[Bind("Clinic_ID,Clinic_Name,Clinic_Address,Clinic_Pincode,Clinic_Contact,Clinic_IsActive,Clinic_Profile,State_ID,City_ID,Doctor_ID")] DocClinic clinic)
        {
            if (clinic.State_ID == 0)
            {
                ModelState.AddModelError("", "---Select State---");
            }
            else if (clinic.City_ID == 0)
            {
                ModelState.AddModelError("", "---Select City---");
            }


            // ------- Getting selected Value ------- //
            var SubCategoryID = HttpContext.Request.Form["City_ID"].ToString();
           

            // ------- Setting Data back to ViewBag after Posting Form ------- //
            List<State> statelist = new List<State>();
            statelist = (from state in _context.STATETB
                         select state).ToList();
            statelist.Insert(0, new State { State_ID = 0, State_Name = "---Select State---" });
            //-------Assigning categorylist to ViewBag.ListofCategory------//
          ViewBag.ListofCategory = statelist;
            //return View(doctorReg);

            var imgext = Path.GetExtension(fileobj.FileName);
            if (imgext == ".jpg" || imgext == ".png")
            {
                var uploadimg = Path.Combine("../Medical/wwwroot", "Clinic_Images", fileobj.FileName);
                var stream = new FileStream(uploadimg, FileMode.Create);

                await fileobj.CopyToAsync(stream);
                stream.Close();
              var d =  HttpContext.Session.GetInt32("SessionID");
                clinic.Clinic_Profile = fileobj.FileName;
                clinic.Clinic_IsActive = false;
                clinic.Doctor_ID = (int)d;
                await _context.CLINICTB.AddAsync(clinic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //if (ModelState.IsValid)
            //{
            //    _context.Add(clinic);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}


            ViewBag.DoctorName = TempData["SessionName"];
            TempData.Keep("SessionName");
            ViewBag.DoctorImg = TempData["SessionImg"];
            TempData.Keep("SessionImg");
            ViewBag.SID = TempData["Sessionid"];
            TempData.Keep("Sessionid");
            return View(clinic);
        }
        public JsonResult GetSubCategory(int State_ID)
        {
            List<City> citylist = new List<City>();

            // ------- Getting Data from Database Using EntityFrameworkCore -------
            citylist = (from city in _context.CITYTB
                        where city.State_ID == State_ID
                        select city).ToList();

            // ------- Inserting Select Item in List -------
            citylist.Insert(0, new City { City_ID = 0, City_Name = "---Select City---" });


            return Json(new SelectList(citylist, "City_ID", "City_Name"));
        }
        // GET: Clinic/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Login", "DoctorReg");
            }
            if (id == null)
            {
                return NotFound();
            }

            var clinic = await _context.CLINICTB.FindAsync(id);
            if (clinic == null)
            {
                return NotFound();
            }
            List<State> statelist = new List<State>();

            // ------- Getting Data from Database Using EntityFrameworkCore -------
            statelist = (from statetb in _context.STATETB
                         select statetb).ToList();

            // ------- Inserting Select Item in List -------
            statelist.Insert(0, new State { State_ID = 0, State_Name = "---Select State---" });

            // ------- Assigning categorylist to ViewBag.ListofCategory -------
            ViewBag.ListofState = statelist;

            ViewBag.DoctorName = TempData["SessionName"];
            TempData.Keep("SessionName");
            ViewBag.DoctorImg = TempData["SessionImg"];
            TempData.Keep("SessionImg");
            ViewBag.SID = TempData["Sessionid"];
            TempData.Keep("Sessionid");
            return View(clinic);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormFile fileobj, [Bind("Clinic_ID,Clinic_Name,Clinic_Address,Clinic_Pincode,Clinic_Contact,Clinic_IsActive,Clinic_Profile,State_ID,City_ID,Doctor_ID")] DocClinic clinic,string fname,int id)
        {
            if (clinic.State_ID == 0)
            {
                ModelState.AddModelError("", "---Select State---");
            }
            else if (clinic.City_ID == 0)
            {
                ModelState.AddModelError("", "---Select City---");
            }


            // ------- Getting selected Value ------- //
            var SubCategoryID = HttpContext.Request.Form["City_ID"].ToString();


            // ------- Setting Data back to ViewBag after Posting Form ------- //
            List<State> statelist = new List<State>();
            statelist = (from state in _context.STATETB
                         select state).ToList();
            statelist.Insert(0, new State { State_ID = 0, State_Name = "---Select State---" });
            //-------Assigning categorylist to ViewBag.ListofCategory------//
            ViewBag.ListofCategory = statelist;


            if (ModelState.IsValid)
            {
                var getimg = await _context.CLINICTB.FindAsync(id);
                _context.CLINICTB.Remove(getimg);
                fname = Path.Combine("../Medical/wwwroot", "Clinic_Images", getimg.Clinic_Profile);
                FileInfo fi = new FileInfo(fname);
                if (fi.Exists)
                {
                    System.IO.File.Delete(fname);
                    fi.Delete();
                }
                var imgext = Path.GetExtension(fileobj.FileName);
                if (imgext == ".jpg" || imgext == ".png")
                {
                    var uploadimg = Path.Combine("../Medical/wwwroot", "Clinic_Images", fileobj.FileName);
                    var stream = new FileStream(uploadimg, FileMode.Create);

                    await fileobj.CopyToAsync(stream);
                    stream.Close();

                    var d = HttpContext.Session.GetInt32("SessionID");
                   
                    //mi.Medicine_ID = 1;
                    clinic.Clinic_Profile = fileobj.FileName;
                    clinic.Clinic_IsActive = false;
                    clinic.Doctor_ID = (int)d;
                 
                    _context.Update(clinic);
                    await _context.SaveChangesAsync();
                }
            }

            ViewBag.DoctorName = TempData["SessionName"];
            TempData.Keep("SessionName");
            ViewBag.DoctorImg = TempData["SessionImg"];
            TempData.Keep("SessionImg");
            ViewBag.SID = TempData["Sessionid"];
            TempData.Keep("Sessionid");
            return RedirectToAction("Index");            
        }

        // GET: Clinic/Delete/5
        public IActionResult Delete(int? id)
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Login", "DoctorReg");
            }
            if (id == null)
            {
                return NotFound();
            }
            //var clinic = (await from c in  _context.CLINICTB
            //                    join s in 

            var clinic = ( from c in _context.CLINICTB
                          join s in _context.STATETB on c.State_ID equals s.State_ID
                          join ct in _context.CITYTB on c.City_ID equals ct.City_ID


                          select new DocClinic
                          {
                              Clinic_ID = c.Clinic_ID,
                              Clinic_Name = c.Clinic_Name,
                              Clinic_Address = c.Clinic_Address,
                              Clinic_Pincode = c.Clinic_Pincode,
                              Clinic_Contact = c.Clinic_Contact,
                              // Clinic_IsActive = c.Clinic_IsActive,
                              Clinic_Profile = c.Clinic_Profile,


                              State_Name = s == null ? "" : s.State_Name,
                              City_Name = ct == null ? "" : ct.City_Name
                          }).FirstOrDefault(m => m.Clinic_ID == id);

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

        // POST: Clinic/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clinic = await _context.CLINICTB.FindAsync(id);
            _context.CLINICTB.Remove(clinic);
            await _context.SaveChangesAsync();

            ViewBag.DoctorName = TempData["SessionName"];
            TempData.Keep("SessionName");
            ViewBag.DoctorImg = TempData["SessionImg"];
            TempData.Keep("SessionImg");
            ViewBag.SID = TempData["Sessionid"];
            TempData.Keep("Sessionid");
            return RedirectToAction(nameof(Index));
        }

        private bool ClinicExists(int id)
        {
            return _context.CLINICTB.Any(e => e.Clinic_ID == id);
        }
    }
}
