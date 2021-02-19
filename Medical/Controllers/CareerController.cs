using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Medical.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server;
using System.IO;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using cloudscribe.Pagination.Models;
//using System.Web.Mvc;



namespace Medical.Controllers
{
    public class CareerController : Controller
    {
        private readonly MedContext _context;
        public CareerController(MedContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? pageNumber)
        {
            int pageSize = 3;
            
            return View(await PaginatedList<Career>.CreateAsync
            (_context.CAREERTB.AsNoTracking(), pageNumber ?? 1, pageSize));
            //var l = _context.CAREERTB.ToList();
            //return View(l);
        }

        public FileResult Download(IFormFile fileobj, string fname, int id)
        {

            var getimg = _context.CAREERTB.Find(id);
            fname = Path.Combine("wwwroot", "Resume",getimg.Candidate_Resume);
            byte[] fileBytes = GetFile(fname);
            FileInfo fi = new FileInfo(fname);
            //return File(fileBytes,"image/jpg","1.jpg");
            return File(fileBytes,System.Net.Mime.MediaTypeNames.Application.Octet,getimg.Candidate_Resume);


        }

        byte[] GetFile(string fname)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(fname);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if(br != fs.Length)
                throw new NotImplementedException();
            return data;
        }
    }
}
