using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Medical.Models;
using System.Web;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Medical.Controllers
{
    public class MedicineController : Controller
    {

        private readonly MedContext _context;
        private readonly IWebHostEnvironment _env;

        public MedicineController(MedContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index(int? pageNumber)
        {
            int pageSize = 3;
           
            return View(await PaginatedList<Medicine>.CreateAsync
            (_context.MEDICINETB.AsNoTracking(), pageNumber ?? 1, pageSize));
            //var items = _context.MEDICINETB.ToList();
            //return View(items);
        }

        public IActionResult Create()
        {
            
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(IFormFile fileobj, [Bind("Medicine_ID,Medicine_Name,Medicine_Price,Medicine_Brand,Medicine_Strip_Tablets,Medicine_Image,Medicine_Content,Medicine_Description")] Medicine mi)
        {
            var imgext = Path.GetExtension(fileobj.FileName);

            if (imgext == ".jpg" || imgext == ".png")
            {
                var uploadimg = Path.Combine("wwwroot", "Medicine_Main_Image", fileobj.FileName);
                var stream = new FileStream(uploadimg, FileMode.Create);

                await fileobj.CopyToAsync(stream);
                stream.Close();

                //mi.Medicine_ID = 1;
                mi.Medicine_Image = fileobj.FileName;
                await _context.MEDICINETB.AddAsync(mi);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            
            if (id == null)
            {
                return RedirectToAction("index");
            }
            var disp = await _context.MEDICINETB.FindAsync(id);
            return View(disp);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([Bind("Medicine_ID,Medicine_Name,Medicine_Price,Medicine_Brand,Medicine_Strip_Tablets,Medicine_Image,Medicine_Content,Medicine_Description")] Medicine mi, IFormFile fileobj, string fname, int id)
        {
            if (id != mi.Medicine_ID)
            {
                return NotFound();
            }
            else
            {
                var getimg = await _context.MEDICINETB.FindAsync(id);
                _context.MEDICINETB.Remove(getimg);
                fname = Path.Combine("wwwroot", "Medicine_Main_Image", getimg.Medicine_Image);
                FileInfo fi = new FileInfo(fname);
                if (fi.Exists)
                {
                    System.IO.File.Delete(fname);
                    fi.Delete();
                }
                var imgext = Path.GetExtension(fileobj.FileName);
                if (imgext == ".jpg" || imgext == ".png")
                {
                    var uploadimg = Path.Combine("wwwroot", "Medicine_Main_Image", fileobj.FileName);
                    var stream = new FileStream(uploadimg, FileMode.Create);

                    await fileobj.CopyToAsync(stream);
                    stream.Close();

                    //mi.Medicine_ID = 1;
                    mi.Medicine_Image = fileobj.FileName;
                    _context.Update(mi);
                    await _context.SaveChangesAsync();
                }

            }




            //return View();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("index");
            }
            var disp = await _context.MEDICINETB.FindAsync(id);
            return View(disp);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([Bind("Medicine_ID,Medicine_Name,Medicine_Price,Medicine_Brand,Medicine_Strip_Tablets,Medicine_Image,Medicine_Content,Medicine_Description")] Medicine mi, string fname, int id)
        {
            var getimg = await _context.MEDICINETB.FindAsync(id);
            _context.MEDICINETB.Remove(getimg);
            fname = Path.Combine("wwwroot", "Medicine_Main_Image", getimg.Medicine_Image);
            FileInfo fi = new FileInfo(fname);
            if (fi.Exists)
            {
                System.IO.File.Delete(fname);
                fi.Delete();
            }

            await _context.SaveChangesAsync();
            return RedirectToActionPermanent("Index");

        }
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    //if (HttpContext.Session.GetString("SessionID") == null)
        //    //{
        //    //    return RedirectToAction("Create", "Login");
        //    //}
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var medicine = await _context.MEDICINETB.FindAsync(id);
        //    if (medicine == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(medicine);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Medicine_ID,Medicine_Name,Medicine_Price,Medicine_Power,Medicine_Brand,Medicine_Strip_Tablets,Medicine_Images")] Medicine medicine)
        //{
        //    //if (HttpContext.Session.GetString("SessionID") == null)
        //    //{
        //    //    return RedirectToAction("Create", "Login");
        //    //}
        //    if (id != medicine.Medicine_ID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(medicine);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!MedicineExists(medicine.Medicine_ID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(medicine);
        //}

        //public async Task<IActionResult> Delete(int? id)
        //{
        //    //if (HttpContext.Session.GetString("SessionID") == null)
        //    //{
        //    //    return RedirectToAction("Create", "Login");
        //    //}
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var medicine = await _context.MEDICINETB
        //        .FirstOrDefaultAsync(m => m.Medicine_ID == id);
        //    if (medicine == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(medicine);
        //}

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    //if (HttpContext.Session.GetString("SessionID") == null)
        //    //{
        //    //    return RedirectToAction("Create", "Login");
        //    //}
        //    var medicine = await _context.MEDICINETB.FindAsync(id);
        //    _context.MEDICINETB.Remove(medicine);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool MedicineExists(int id)
        //{
        //    return _context.MEDICINETB.Any(e => e.Medicine_ID == id);
        //}
    }
}
