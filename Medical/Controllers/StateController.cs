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
    public class StateController : Controller
    {
        private readonly MedContext _context;

        public StateController(MedContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            //if (HttpContext.Session.GetString("SessionID") == null)
            //{
            //    return RedirectToAction("Create", "Login");
            //}
            int pageSize = 3;

            return View(await PaginatedList<State>.CreateAsync
            (_context.STATETB.AsNoTracking(), pageNumber ?? 1, pageSize));

        }

        public IActionResult Create()
        {
            //if (HttpContext.Session.GetString("SessionID") == null)
            //{
            //    return RedirectToAction("Create", "Login");
            //}
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("State_ID,State_Name")] State state)
        {
            //if (HttpContext.Session.GetString("SessionID") == null)
            //{
            //    return RedirectToAction("Create", "Login");
            //}
            if (ModelState.IsValid)
            {
                _context.Add(state);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(state);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            //if (HttpContext.Session.GetString("SessionID") == null)
            //{
            //    return RedirectToAction("Create", "Login");
            //}
            if (id == null)
            {
                return NotFound();
            }

            var state = await _context.STATETB.FindAsync(id);
            if (state == null)
            {
                return NotFound();
            }
            return View(state);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("State_ID,State_Name")] State state)
        {
            //if (HttpContext.Session.GetString("SessionID") == null)
            //{
            //    return RedirectToAction("Create", "Login");
            //}
            if (id != state.State_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(state);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StateExists(state.State_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(state);
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

            var state = await _context.STATETB
                .FirstOrDefaultAsync(m => m.State_ID == id);
            if (state == null)
            {
                return NotFound();
            }

            return View(state);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //if (HttpContext.Session.GetString("SessionID") == null)
            //{
            //    return RedirectToAction("Create", "Login");
            //}
            var state = await _context.STATETB.FindAsync(id);
            _context.STATETB.Remove(state);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StateExists(int id)
        {
            return _context.STATETB.Any(e => e.State_ID == id);
        }
    }
}
