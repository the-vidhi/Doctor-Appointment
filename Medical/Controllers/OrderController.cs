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
    public class OrderController : Controller
    {
        private readonly MedContext _context;

        public OrderController(MedContext context)
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

            return View(await PaginatedList<Order>.CreateAsync
            (_context.ORDERTB.AsNoTracking(), pageNumber ?? 1, pageSize));

        }

        public async Task<IActionResult> Details(int? id)
        {
            //if (HttpContext.Session.GetString("SessionID") == null)
            //{
            //    return RedirectToAction("Create", "Login");
            //}
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.ORDERTB
                .FirstOrDefaultAsync(m => m.Order_ID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
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
        public async Task<IActionResult> Create([Bind("Order_ID,Order_Status,Patient_ID,Payment_Type_ID,Order_Amount,Order_Date,Deliver_Date,Is_Paid")] Order order)
        {
            //if (HttpContext.Session.GetString("SessionID") == null)
            //{
            //    return RedirectToAction("Create", "Login");
            //}
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
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

        //    var order = await _context.ORDERTB.FindAsync(id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(order);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Order_ID,Order_Status,Patient_ID,Payment_Type_ID,Order_Amount,Order_Date,Deliver_Date,Is_Paid")] Order order)
        //{
        //    //if (HttpContext.Session.GetString("SessionID") == null)
        //    //{
        //    //    return RedirectToAction("Create", "Login");
        //    //}
        //    if (id != order.Order_ID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            if(order.Order_Status == "Confirm")
        //            {
        //                order.Order_Status = "Shipped";
        //            }
        //            else if(order.Order_Status == "Shipped")
        //            {
        //                order.Order_Status = "Delivered";
        //            }
        //            _context.Update(order);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!OrderExists(order.Order_ID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction("Index","Order");
        //    }
        //    return View(order);
        //}
        
        
        public async Task<IActionResult> OrderShip(int id,Order order)
        {
            var ord = await _context.ORDERTB.FindAsync(id);
            ord.Order_ID = id;

            if (id != ord.Order_ID)
            {
                return NotFound();
            }
            ord.Order_Status = "Shipped";
            _context.ORDERTB.Update(ord);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Index", "Order");
        }
        public async Task<IActionResult> OrderDeliver(int id)
        {
            var ord = await _context.ORDERTB.FindAsync(id);
            ord.Order_ID = id;

            if (id != ord.Order_ID)
            {
                return NotFound();
            }
            ord.Order_Status = "Delivered";
            ord.Is_Paid = true;
            ord.Deliver_Date = Convert.ToString(DateTime.Now);
            _context.ORDERTB.Update(ord);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Index", "Order");
        }
        public async Task<IActionResult> OrderCancle(int id)
        {
            var ord = await _context.ORDERTB.FindAsync(id);
            ord.Order_ID = id;

            if (id != ord.Order_ID)
            {
                return NotFound();
            }
            ord.Order_Status = "Cancled";
            _context.ORDERTB.Update(ord);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Index", "Order");
        }

       

       

        private bool OrderExists(int id)
        {
            return _context.ORDERTB.Any(e => e.Order_ID == id);
        }
    }
}
