using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Medical.Models
{
    public class OrderMedical
    {
        [Key]
        public int Order_Medicine_ID { get; set; }
        public int Order_ID { get; set; }
        public int Medicine_ID { get; set; }

        public string Price { get; set; }
        [NotMapped]
        public string Medicine_Price { get; set; }
        [NotMapped]
        public int Bill_ID { get; set; }
        [NotMapped]
        public string Order_Amount { get; set; }
        [NotMapped]
        public string Medicine_Name { get; set; }

        [NotMapped]
        public string Medicine_Image { get; set; }
        public int Order_Qty { get; set; }

        [NotMapped]
        public string Order_Date { get; set; }

        [NotMapped]
        public string Order_Status { get; set; }

    }
}
