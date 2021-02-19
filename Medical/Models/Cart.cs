using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Medical.Models
{
    public class Cart
    {
        [Key]
        public int Cart_ID { get; set; }
        public int Patient_ID { get; set; }
        public int Medicine_ID { get; set; }

        [NotMapped]
        public string Medicine_Price { get; set; }

        [NotMapped]
        public string Medicine_Name { get; set; }

        [NotMapped]
        public string Medicine_Image { get; set; }
        public int Cart_Qty { get; set; }

        //[NotMapped]
        //public int Medicine_Qty { get; set; }
       
    }
}
