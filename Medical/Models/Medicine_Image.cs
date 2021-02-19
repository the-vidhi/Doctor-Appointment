using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Medical.Models
{
    public class Medicine_Image
    {
        [Key]
        public int Medicine_Image_ID { get; set; }
        public int Medicine_ID { get; set; }
        public string Medicine_Images { get; set; }

        [NotMapped]
        public string Medicine_Name { get; set; }
    }
}
