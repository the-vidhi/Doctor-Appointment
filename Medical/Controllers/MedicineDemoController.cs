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
    public class MedicineDemoController : Controller
    {
        private readonly MedContext _context;

        public MedicineDemoController(MedContext context)
        {
            _context = context;
        }

        // GET: MedicineDemo
        public async Task<IActionResult> Index()
        {
            return View(await _context.MEDICINETB.ToListAsync());
        }

        // GET: MedicineDemo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicine = await _context.MEDICINETB
                .FirstOrDefaultAsync(m => m.Medicine_ID == id);
            if (medicine == null)
            {
                return NotFound();
            }

            return View(medicine);
        }

        // GET: MedicineDemo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MedicineDemo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Medicine_ID,Medicine_Name,Medicine_Price,Medicine_Brand,Medicine_Strip_Tablets,Medicine_Image,Medicine_Content,Medicine_Description")] Medicine medicine)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medicine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(medicine);
        }

        // GET: MedicineDemo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicine = await _context.MEDICINETB.FindAsync(id);
            if (medicine == null)
            {
                return NotFound();
            }
            return View(medicine);
        }

        // POST: MedicineDemo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Medicine_ID,Medicine_Name,Medicine_Price,Medicine_Brand,Medicine_Strip_Tablets,Medicine_Image,Medicine_Content,Medicine_Description")] Medicine medicine)
        {
            if (id != medicine.Medicine_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicineExists(medicine.Medicine_ID))
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
            return View(medicine);
        }

        // GET: MedicineDemo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicine = await _context.MEDICINETB
                .FirstOrDefaultAsync(m => m.Medicine_ID == id);
            if (medicine == null)
            {
                return NotFound();
            }

            return View(medicine);
        }

        // POST: MedicineDemo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicine = await _context.MEDICINETB.FindAsync(id);
            _context.MEDICINETB.Remove(medicine);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicineExists(int id)
        {
            return _context.MEDICINETB.Any(e => e.Medicine_ID == id);
        }
    }
}
