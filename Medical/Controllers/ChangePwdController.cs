using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Medical.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
//using Microsoft.AspNet.Identity;

namespace Medical.Controllers
{
    public class ChangePwdController : Controller
    {
        //private readonly MedContext _context;
        //private readonly UserManager<> _userManager;
        //public ChangePwdController(MedContext context,UserManager<> userManager)
        //{
        //    _context = context;
        //    _userManager = userManager;
        //}

        [HttpGet]
        public IActionResult ChangePwd()
        {
            return View();
        }

             
        
    }
}
