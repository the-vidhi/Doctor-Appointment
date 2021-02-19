using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Medical.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
//using Grpc.Core;


namespace Medical.Controllers
{
    public class Medicine_ImageController : Controller
    {
        private readonly MedContext _context;
        private readonly IWebHostEnvironment _env;

        public Medicine_ImageController(MedContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            var items = _context.MEDICINE_IMAGETB.ToList();   
            return View(items);
        }

        public IActionResult Create()
        {
            loadMedicine();
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(IFormFile[] fileobj, [Bind("Medicine_Image_ID,Medicine_ID,Medicine_Images")] Medicine_Image mi)
        {
            foreach (var file in fileobj)
            {


                var imgext = Path.GetExtension(file.FileName);

                if (imgext == ".jpg" || imgext == ".png")
                {
                    var uploadimg = Path.Combine("wwwroot", "Medicine_Images", file.FileName);
                    var stream = new FileStream(uploadimg, FileMode.Create);

                    await file.CopyToAsync(stream);
                    stream.Close();

                    //mi.Medicine_ID = 1;
                    mi.Medicine_Images = file.FileName;
                    await _context.MEDICINE_IMAGETB.AddAsync(mi);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index");
        }
        private void loadMedicine()
        {
            try
            {
                List<Medicine> medList = new List<Medicine>();
                medList = _context.MEDICINETB.ToList();

                medList.Insert(0, new Medicine { Medicine_ID = 0, Medicine_Name = "Select Medicine" });
                ViewBag.MedList = medList;
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<IActionResult> Edit(int ?id)
        {
            loadMedicine();
            if(id==null)
            {
                return RedirectToAction("index");
            }
            var disp = await _context.MEDICINE_IMAGETB.FindAsync(id);
            return View(disp);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(IFormFile fileobj, [Bind("Medicine_Image_ID,Medicine_ID,Medicine_Images")] Medicine_Image mi,string fname,int id)
        {
            if(ModelState.IsValid)
            {
                var getimg = await _context.MEDICINE_IMAGETB.FindAsync(id);
                _context.MEDICINE_IMAGETB.Remove(getimg);
                fname = Path.Combine("wwwroot", "Medicine_Images", getimg.Medicine_Images);
                FileInfo fi = new FileInfo(fname);
                if(fi.Exists)
                {
                    System.IO.File.Delete(fname);
                    fi.Delete();
                }
                var imgext = Path.GetExtension(fileobj.FileName);
                if (imgext == ".jpg" || imgext == ".png")
                {
                    var uploadimg = Path.Combine("wwwroot", "Medicine_Images", fileobj.FileName);
                    var stream = new FileStream(uploadimg, FileMode.Create);

                    await fileobj.CopyToAsync(stream);
                    stream.Close();

                    //mi.Medicine_ID = 1;
                    mi.Medicine_Images = fileobj.FileName;
                    _context.Update(mi);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("index");
            }
            var disp = await _context.MEDICINE_IMAGETB.FindAsync(id);
            return View(disp);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([Bind("Medicine_Image_ID,Medicine_ID,Medicine_Images")] Medicine_Image mi, string fname, int id)
        {
            var getimg = await _context.MEDICINE_IMAGETB.FindAsync(id);
            _context.MEDICINE_IMAGETB.Remove(getimg);
            fname = Path.Combine("wwwroot", "Medicine_Images", getimg.Medicine_Images);
            FileInfo fi = new FileInfo(fname);
            if (fi.Exists)
            {
                System.IO.File.Delete(fname);
                fi.Delete();
            }

            await _context.SaveChangesAsync();
            return RedirectToActionPermanent("Index");

        }
    }
}
