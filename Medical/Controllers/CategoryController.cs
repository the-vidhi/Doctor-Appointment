using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Medical.Models;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Medical.Controllers
{
    public class CategoryController : Controller
    {
        private readonly MedContext _context;

        public CategoryController(MedContext context)
        {
            _context = context;
        }

        
        // GET: Category
        public async Task<IActionResult> Index(int? pageNumber)
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Create", "Login");
            }
            TempData.Keep();
            int pageSize = 3;
            return View(await PaginatedList<Category>.CreateAsync(_context.CATEGORYTB.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Category/Details/5
        
        // GET: Category/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Create", "Login");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Category_ID,Category_Name,Category_Description,Category_Profile,Category_SubDescription,Category_MainProfile")] Category category, IFormFile fileobj, IFormFile fileobj1)
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Create", "Login");
            }
            var imgext = Path.GetExtension(fileobj.FileName);

            if (imgext == ".jpg" || imgext == ".png")
            {
                var uploadimg = Path.Combine("wwwroot", "Category_Image", fileobj.FileName);
                var stream = new FileStream(uploadimg, FileMode.Create);
                await fileobj.CopyToAsync(stream);
                stream.Close();

                var uploadimg1 = Path.Combine("wwwroot", "Category_Image", fileobj1.FileName);
                var stream1 = new FileStream(uploadimg1, FileMode.Create);
                await fileobj1.CopyToAsync(stream1);
                stream1.Close();


                

                //mi.Medicine_ID = 1;
                category.Category_Profile = fileobj.FileName;
                category.Category_MainProfile = fileobj1.FileName;
                await _context.CATEGORYTB.AddAsync(category);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Create", "Login");
            }
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.CATEGORYTB.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Category_ID,Category_Name,Category_Description,Category_Profile,Category_SubDescription,Category_MainProfile")] Category category, IFormFile fileobj, IFormFile fileobj1, string fname, string fname1, int id)
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Create", "Login");
            }
            if (id != category.Category_ID)
            {
                return NotFound();
            }

            var getimg = await _context.CATEGORYTB.FindAsync(id);
            _context.CATEGORYTB.Remove(getimg);
            fname = Path.Combine("wwwroot", "Category_Image", getimg.Category_Profile);
            FileInfo fi = new FileInfo(fname);
            if (fi.Exists)
            {
                System.IO.File.Delete(fname);
                fi.Delete();
            }
            
            fname1 = Path.Combine("wwwroot", "Category_Image", getimg.Category_MainProfile);
            FileInfo fi1 = new FileInfo(fname1);
            if (fi1.Exists)
            {
                System.IO.File.Delete(fname1);
                fi1.Delete();
            }

            var imgext = Path.GetExtension(fileobj.FileName);
            if (imgext == ".jpg" || imgext == ".png")
            {
                var uploadimg = Path.Combine("wwwroot", "Category_Image", fileobj.FileName);
                var stream = new FileStream(uploadimg, FileMode.Create);
                await fileobj.CopyToAsync(stream);
                stream.Close();

                var uploadimg1 = Path.Combine("wwwroot", "Category_Image", fileobj1.FileName);
                var stream1 = new FileStream(uploadimg1, FileMode.Create);
                await fileobj1.CopyToAsync(stream1);
                stream1.Close();


                //mi.Medicine_ID = 1;
                category.Category_Profile = fileobj.FileName;
                category.Category_MainProfile = fileobj1.FileName;
                _context.Update(category);
                await _context.SaveChangesAsync();
            }

         return RedirectToAction("Index");
    }

        // GET: Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Create", "Login");
            }
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.CATEGORYTB
                .FirstOrDefaultAsync(m => m.Category_ID == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Create", "Login");
            }

            var category = await _context.CATEGORYTB.FindAsync(id);
            _context.CATEGORYTB.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.CATEGORYTB.Any(e => e.Category_ID == id);
        }
    }
}
