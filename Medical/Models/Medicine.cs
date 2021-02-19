using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Medical.Models
{
    public class Medicine
    {
        [Key]
        public int Medicine_ID { get; set; }


        [Required(ErrorMessage = "Required")]
        public string Medicine_Name { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Medicine_Price { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Medicine_Brand { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Medicine_Strip_Tablets { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Medicine_Image { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Medicine_Content { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Medicine_Description { get; set; }

        // public HttpPostedFileBase ImageFile { get; set; }
    }
}
