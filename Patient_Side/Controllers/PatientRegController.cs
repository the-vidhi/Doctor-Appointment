using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Patient_Side.Models;
using Medical.Models;
using System.IO;
using Microsoft.AspNetCore.Session;
using System.Web;

namespace Patient_Side.Controllers
{
    public class PatientRegController : Controller
    {
        private readonly MedContext _context;

        public PatientRegController(MedContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {

            return View();
        }
        public IActionResult Logout()
        {
            //if (HttpContext.Session.GetString("SessionID") == null)
            //{
            //    return RedirectToAction("Create", "Login");
            //}
            HttpContext.Session.Clear();
                return RedirectToAction("Login", "PatientReg");
        }


        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(Patient patient)
        {
            try
            {
                var user = _context.PATIENTTB.SingleOrDefault(
                    u => u.Patient_Email.Equals(patient.Patient_Email)
                    && u.Patient_Password.Equals(patient.Patient_Password)
                    );
                if (user != null)
                {
                    var u1 = _context.PATIENTTB.FirstOrDefault(x => x.Patient_Email == user.Patient_Email);
                    if(u1.Patient_IsActive == true)
                    {
                        HttpContext.Session.SetInt32("SessionID", user.Patient_ID);
                        ViewBag.SID = HttpContext.Session.GetInt32("SessionID");
                        TempData["SessionID"] = HttpContext.Session.GetInt32("SessionID");

                        ViewBag.SID = TempData["SessionID"];
                        TempData.Keep("SessionID");
                        return RedirectToAction("Index", "Home2");
                    }
                    else
                    {
                        return RedirectToAction("BlockLogin");
                    }
                }
                else
                {
                    return RedirectToAction("LoginFail");
                }
            }
            catch (Exception e)
            {
                return View(e);
            }
        }

        public IActionResult BlockLogin()
        {
            return View();
        }
        public IActionResult LoginFail()
        {
            return View();
        }
        public IActionResult RegFail()
        {
            return View();
        }

        public IActionResult Reg()
        {
            List<State> statelist = new List<State>();
            statelist = (from statetb in _context.STATETB
                         select statetb).ToList();
            statelist.Insert(0, new State { State_ID = 0, State_Name = "Select State" });
            ViewBag.ListofState = statelist;

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reg([Bind("Patient_ID,Patient_Name,Patient_Address,Patient_Contact,State_ID,City_ID,Patient_Email,Patient_Password,Patient_IsActive,Patient_Profile")] Patient patientReg, IFormCollection formCollection, IFormFile fileobj)
        {
            if (patientReg.State_ID == 0)
            {
                ModelState.AddModelError("", "Select State");
            }
            else if (patientReg.City_ID == 0)
            {
                ModelState.AddModelError("", "Select City");
            }
            var SubCategoryID = HttpContext.Request.Form["City_ID"].ToString();
            List<State> statelist = new List<State>();
            statelist = (from state in _context.STATETB
                         select state).ToList();
            statelist.Insert(0, new State { State_ID = 0, State_Name = "Select State" });
            ViewBag.ListofCategory = statelist;
            var imgext = Path.GetExtension(fileobj.FileName);

            if (imgext == ".jpg" || imgext == ".png")
            {
                var uploadimg = Path.Combine("../Medical/wwwroot", "Patient_Image", fileobj.FileName);
                var stream = new FileStream(uploadimg, FileMode.Create);

                await fileobj.CopyToAsync(stream);
                stream.Close();

                //mi.Medicine_ID = 1;
                patientReg.Patient_Profile = fileobj.FileName;
                patientReg.Patient_IsActive = true;

                var user = _context.PATIENTTB.SingleOrDefault(
                    u => u.Patient_Email.Equals(patientReg.Patient_Email)
                    );
                if(user == null)
                {
                    await _context.PATIENTTB.AddAsync(patientReg);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Login", "PatientReg");
                }
                else
                {
                    return RedirectToAction("RegFail");
                }

                
            }
            return View(patientReg);
        }

        public JsonResult GetSubCategory(int State_ID)
        {
            List<City> citylist = new List<City>();
            citylist = (from city in _context.CITYTB
                        where city.State_ID == State_ID
                        select city).ToList();
            citylist.Insert(0, new City { City_ID = 0, City_Name = "Select City" });
            TempData.Keep("SessionID");


            return Json(new SelectList(citylist, "City_ID", "City_Name"));
        }
        
        public async Task<IActionResult> Profile(int? id)
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Login", "PatientReg");
            }
            if (id == null)
            {
                TempData.Keep("SessionID");
                return NotFound();
            }
            List<State> statelist = new List<State>();
            statelist = (from statetb in _context.STATETB
                         select statetb).ToList();
            statelist.Insert(0, new State { State_ID = 0, State_Name = "Select State" });
            ViewBag.ListofState = statelist;

            var patientReg = await _context.PATIENTTB.FindAsync(id);
            if (patientReg == null)
            {
                TempData.Keep("SessionID");
                return NotFound();
            }
            var vm = new ViewModel();
            vm.patient = patientReg;

            vm.orderList = _context.ORDERTB.Where(x => x.Patient_ID == id).ToList();

            var aList = from a in _context.APPOINTMENTTB
                        join d in _context.DOCTORTB on a.Doctor_ID equals d.Doctor_ID
                        where a.Patient_ID == id
                        select new Appointment
                        {
                            Appointment_ID = a.Appointment_ID,
                            Appointment_Date = a.Appointment_Date,
                            Appointment_Msg = a.Appointment_Msg,
                            Appointment_Status = a.Appointment_Status,
                            Appointment_Time = a.Appointment_Time,
                            Doctor_ID = a.Doctor_ID,
                            Doctor_Name = d.Doctor_Name
                        };
            vm.appointmentList = aList.ToList();






            TempData.Keep("SessionID");
            return View(vm);
        }

        ////[Route("PatientReg/Profile/")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProfileEdit(int id,IFormFile fileobj,string fname ,[Bind("Patient_ID,Patient_Name,Patient_Address,Patient_Contact,State_ID,City_ID,Patient_Email,Patient_Password,Patient_IsActive,Patient_Profile")] Patient patientReg)
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Login", "PatientReg");
            }
            var sessionid = (int)TempData["SessionID"];
            if (id != patientReg.Patient_ID)
            {
                TempData.Keep("SessionID");
                return NotFound();
            }

            int SID = Convert.ToInt32(Request.Form["State_ID"]);
            int CID = Convert.ToInt32(Request.Form["City_ID"]);
            var pname = Convert.ToString(Request.Form["pname"]);
            var paddress = Convert.ToString(Request.Form["paddress"]);
            var pcontact = Convert.ToString(Request.Form["pcontact"]);
            var pemail = Convert.ToString(Request.Form["pemail"]);

            patientReg.State_ID = SID;
            patientReg.City_ID = CID;
            patientReg.Patient_Name = pname;
            patientReg.Patient_Address = paddress;
            patientReg.Patient_Contact = pcontact;
            patientReg.Patient_Email = pemail;
            if (patientReg.State_ID == 0)
            {
                ModelState.AddModelError("", "---Select State---");
            }
            else if (patientReg.City_ID == 0)
            {
                ModelState.AddModelError("", "---Select City---");
            }
            var SubCategoryID = HttpContext.Request.Form["City_ID"].ToString();
            List<State> statelist = new List<State>();
            statelist = (from state in _context.STATETB
                         select state).ToList();
            statelist.Insert(0, new State { State_ID = 0, State_Name = "---Select State---" });
            ViewBag.ListofCategory = statelist;


            if (ModelState.IsValid)
            {
                var getimg = await _context.PATIENTTB.FindAsync(sessionid);

                _context.PATIENTTB.Remove(getimg);

                patientReg.Patient_Password = getimg.Patient_Password;
                patientReg.Patient_IsActive = getimg.Patient_IsActive;
                fname = Path.Combine("../Medical/wwwroot", "Patient_Image", getimg.Patient_Profile);
                FileInfo fi = new FileInfo(fname);
                if (fi.Exists)
                {
                    System.IO.File.Delete(fname);
                    fi.Delete();
                }
                var imgext = Path.GetExtension(fileobj.FileName);
                if (imgext == ".jpg" || imgext == ".png")
                {
                    var uploadimg = Path.Combine("../Medical/wwwroot", "Patient_Image", fileobj.FileName);
                    var stream = new FileStream(uploadimg, FileMode.Create);

                    await fileobj.CopyToAsync(stream);
                    stream.Close();

                    var d = HttpContext.Session.GetInt32("SessionID");

                    patientReg.Patient_Profile = fileobj.FileName;
                    patientReg.Patient_ID = sessionid;
                    
                    _context.Update(patientReg);
                    await _context.SaveChangesAsync();
                }
            }
            TempData.Keep("SessionID");
            return RedirectToAction("Profile","PatientReg", new { id = TempData["SessionID"] });
            //return RedirectToAction("Index","Home2");
        }

        public IActionResult Check()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Check(Patient patient)
        {
            try
            {
                var user = _context.PATIENTTB.SingleOrDefault(
                    u => u.Patient_Email.Equals(patient.Patient_Email)
                    && u.Patient_Password.Equals(patient.Patient_Password)
                    );
                if (user != null)
                {
                    HttpContext.Session.SetInt32("SessionID", user.Patient_ID);
                    ViewBag.SID = HttpContext.Session.GetInt32("SessionID");
                    TempData["SessionID"] = HttpContext.Session.GetInt32("SessionID");

                    ViewBag.SID = TempData["SessionID"];
                    TempData.Keep("SessionID");
                    //return View("../Dashboard/Index");
                    return RedirectToAction("ChangePassword", "PatientReg",new { id = TempData["SessionID"] });
                }
                else
                {
                    return RedirectToAction("LoginFail");
                }
            }
            catch (Exception e)
            {
                return View(e);
            }
           
        }
        public ActionResult ChangePassword(int? id)
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Login", "PatientReg");
            }
            var item = _context.PATIENTTB.Where(x => x.Patient_ID == id).First();
            TempData.Keep("SessionID");
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(int id, Patient model)
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Login", "PatientReg");
            }
            var item = _context.PATIENTTB.Find(id);
            item.Patient_Password = model.Patient_Password;
            _context.Update(item);
            _context.SaveChanges();

            TempData.Keep("SessionID");
            return RedirectToAction("SuccessPassChange");
        }
        public IActionResult SuccessPassChange()
        {
            TempData.Keep("SessionID");
            return View();
        }


        [HttpGet]
        public IActionResult TakeAppointment()
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Login", "PatientReg");
            }
            List<Category> statelist = new List<Category>();
            statelist = (from statetb in _context.CATEGORYTB
                         select statetb).ToList();
            statelist.Insert(0, new Category { Category_ID = 0, Category_Name = "Select Category" });
            ViewBag.ListofState = statelist;
            TempData.Keep("SessionID");
            return View();
        }
        [HttpPost]
        public IActionResult TakeAppointment(Appointment app)
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Login", "PatientReg");
            }
            if (app.Category_ID == 0)
            {
                ModelState.AddModelError("", "Select Category");
            }
            else if (app.Doctor_ID == 0)
            {
                ModelState.AddModelError("", "Select Doctor");
            }
            var SubCategoryID = HttpContext.Request.Form["Doctor_ID"].ToString();
            List<Category> statelist = new List<Category>();
            statelist = (from state in _context.CATEGORYTB
                         select state).ToList();
            statelist.Insert(0, new Category { Category_ID = 0, Category_Name = "Select Category" });
            ViewBag.ListofCategory = statelist;

            app.Patient_ID = (int)TempData["SessionID"];
            app.Appointment_Status = "Requested";
            app.Appointment_Time = Request.Form["time"].ToString();
            _context.APPOINTMENTTB.Add(app);
            _context.SaveChanges();

            TempData.Keep("SessionID");
            return RedirectToAction("Shop","PatMedicine");
        }
        public JsonResult GetSubCategory1(int Category_ID)
        {
            List<Doctor> citylist = new List<Doctor>();
            citylist = (from city in _context.DOCTORTB
                        where city.Category_ID == Category_ID
                        select city).ToList();
            citylist.Insert(0, new Doctor { Doctor_ID = 0, Doctor_Name = "Select Doctor" });

            TempData.Keep("SessionID");
            return Json(new SelectList(citylist, "Doctor_ID", "Doctor_Name"));
        }


        private bool PatientRegExists(int id)
        {

            return _context.PATIENTTB.Any(e => e.Patient_ID == id);
        }
    }
}
