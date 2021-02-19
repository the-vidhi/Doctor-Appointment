using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Medical.Models;

namespace Medical.Controllers
{
    public class Citie1Controller : Controller
    {
        private readonly MedContext _context;

        public Citie1Controller(MedContext context)
        {
            _context = context;
        }

        // GET: Citie1
        public async Task<IActionResult> Index()
        {
            return View(await _context.CITYTB.ToListAsync());
        }

        // GET: Citie1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
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

        // GET: Citie1/Create
        public IActionResult Create()
        {
            //PopulateDropDownList(_context);
            return View();
        }

        // POST: Citie1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("City_ID,State_ID,City_Name")] City city)
        {
            if (ModelState.IsValid)
            {
                _context.Add(city);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateDropDownList(city,city.State_ID);
            return View(city);
        }

        // GET: Citie1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
           
            var city = await _context.CITYTB.AsNoTracking().FirstOrDefaultAsync(m=>m.City_ID == id);
            if (city == null)
            {
                return NotFound();
            }
            PopulateDropDownList(city, city.State_ID);
            return View(city);
        }

        // POST: Citie1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("City_ID,State_ID,City_Name")] City city)
        {
            if (id != city.City_ID)
            {
                return NotFound();
            }
            var citytoupdate = await _context.CITYTB.AsNoTracking().FirstOrDefaultAsync(m => m.City_ID == id);
            if (await TryUpdateModelAsync<City>(citytoupdate,
                   "",
                   c => c.City_Name, c => c.State_ID))
            {
               
                    try
                    {
                    _context.Update(city);
                    await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateException /* ex */)
                    {
                        //Log the error (uncomment ex variable name and write a log.)
                        ModelState.AddModelError("", "Unable to save changes. " +
                            "Try again, and if the problem persists, " +
                            "see your system administrator.");
                    }
                    return RedirectToAction(nameof(Index));
                
            }
                PopulateDropDownList(city, citytoupdate.State_ID);
                return View(citytoupdate);
                //    try
                //    {
                //        _context.Update(city);
                //        await _context.SaveChangesAsync();
                //    }
                //    catch (DbUpdateConcurrencyException)
                //    {
                //        if (!CityExists(city.City_ID))
                //        {
                //            return NotFound();
                //        }
                //        else
                //        {
                //            throw;
                //        }
                //    }
                //    return RedirectToAction(nameof(Index));
                //}
                //return View(city);
            
            }

        // GET: Citie1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
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

        // POST: Citie1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var city = await _context.CITYTB.FindAsync(id);
            _context.CITYTB.Remove(city);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CityExists(int id)
        {
            return _context.CITYTB.Any(e => e.City_ID == id);
        }
        private void PopulateDropDownList(City city, object selectedDepartment = null)
        {
            var Query = from d in _context.STATETB
                                   orderby d.State_Name
                                   select d;
            ViewBag.stateID = new SelectList(Query.AsNoTracking(), "State_ID", "State_Name", selectedDepartment);
        }
    }
   
}
