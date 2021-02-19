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
    public class ReviewController : Controller
    {
        private readonly MedContext _context;

        public ReviewController(MedContext context)
        {
            _context = context;
        }

        // GET: Review
        public async Task<IActionResult> Index(int? pageNumber)
        {
            //if (HttpContext.Session.GetString("SessionID") == null)
            //{
            //    return RedirectToAction("Create", "Login");
            //}
            var rList = from r in _context.REVIEWTB
                        join p in _context.PATIENTTB on r.Patient_ID equals p.Patient_ID
                        join d in _context.DOCTORTB on r.Doctor_ID equals d.Doctor_ID

                        select new Review
                        {
                            Review_ID = r.Review_ID,
                            Review_Msg = r.Review_Msg,
                            Doctor_ID = r.Doctor_ID,
                            Patient_ID = r.Patient_ID,
                            Doctor_Name = d.Doctor_Name,
                            Patient_Name = p.Patient_Name
                        };
            int pageSize = 3;

            return View(await PaginatedList<Review>.CreateAsync
            (rList.AsNoTracking(), pageNumber ?? 1, pageSize));

            //return View(rList);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            //if (HttpContext.Session.GetString("SessionID") == null)
            //{
            //    return RedirectToAction("Create", "Login");
            //}
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.REVIEWTB
                .FirstOrDefaultAsync(m => m.Review_ID == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //if (HttpContext.Session.GetString("SessionID") == null)
            //{
            //    return RedirectToAction("Create", "Login");
            //}
            var review = await _context.REVIEWTB.FindAsync(id);
            _context.REVIEWTB.Remove(review);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReviewExists(int id)
        {
            return _context.REVIEWTB.Any(e => e.Review_ID == id);
        }
    }
}
