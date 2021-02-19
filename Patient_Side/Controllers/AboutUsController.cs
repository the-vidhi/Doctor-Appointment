using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Medical.Models;

namespace Patient_Side.Controllers
{
    public class AboutUsController : Controller
    {
        private readonly MedContext _context;
        public AboutUsController(MedContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewModel vm = new ViewModel();
            var rm = from r in _context.PRODUCTREVIEWTB
                     join p in _context.PATIENTTB on r.Patient_ID equals p.Patient_ID
                     select new ProductReview
                     {
                         Product_Review_ID = r.Product_Review_ID,
                         Product_Review_Msg = r.Product_Review_Msg,
                         Patient_ID = r.Patient_ID,
                         Patient_Name = p.Patient_Name,
                         Patient_Profile = p.Patient_Profile
                     };
            vm.ProductReviewList = rm.ToList();
            TempData.Keep("SessionID");
            return View(vm);
        }
    }
}
