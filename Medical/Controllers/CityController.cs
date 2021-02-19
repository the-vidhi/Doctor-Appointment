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
    public class CityController : Controller
    {
        private readonly MedContext _context;

        public CityController(MedContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Create", "Login");
            }

            var Citylist = from c in _context.CITYTB
                          join s in _context.STATETB on c.State_ID equals s.State_ID
                          
                          select new City
                          {
                             
                              State_ID = c.State_ID,
                              City_ID = c.City_ID,

                              State_Name = s == null ? "" : s.State_Name,
                              City_Name = c == null ? "" : c.City_Name
                          };
            int pageSize = 3;
            return View(await PaginatedList<City>.CreateAsync(Citylist.AsNoTracking(), pageNumber ?? 1, pageSize));

            //return View(Citylist);
            //return View(await _context.CITYTB.ToListAsync());
        }

        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Create", "Login");
            }
            loadDDL();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("City_ID,State_ID,City_Name")] City city)
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Create", "Login");
            }
            if (ModelState.IsValid)
            {
                _context.Add(city);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(city);
        }
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

            var city = await _context.CITYTB.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }
            loadDDL();
            return View(city);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("City_ID,State_ID,City_Name")] City city)
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Create", "Login");
            }
            if (id != city.City_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                loadDDL();
                try
                {
                    _context.Update(city);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CityExists(city.City_ID))
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
            return View(city);
        }
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

            var city = await _context.CITYTB
                .FirstOrDefaultAsync(m => m.City_ID == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Create", "Login");
            }
            var city = await _context.CITYTB.FindAsync(id);
            _context.CITYTB.Remove(city);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CityExists(int id)
        {
            return _context.CITYTB.Any(e => e.City_ID == id);
        }

        private void loadDDL()
        {
            try
            {
                List<State> stList = new List<State>();
                stList = _context.STATETB.ToList();

                stList.Insert(0, new State { State_ID = 0, State_Name = "Select State" });
                ViewBag.StList = stList;
            }
            catch (Exception ex)
            {

            }
        }
    }
}
