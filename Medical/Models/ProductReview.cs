using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Medical.Models
{
    public class ProductReview
    {
        [Key]
        public int Product_Review_ID { get; set; }
        public string Product_Review_Msg { get; set; }
        public int Medicine_ID { get; set; }
        public int Patient_ID { get; set; }

        [NotMapped]
        public string Patient_Name { get; set; }
        [NotMapped]
        public string Patient_Profile { get; set; }

        [NotMapped]
        public string Medicine_Name { get; set; }
        [NotMapped]
        public string Patient_Email { get; set; }


    }
}
