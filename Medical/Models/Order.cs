using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Medical.Models
{
    public class Order
    {
        [Key]
        public int Order_ID { get; set; }

        public string Order_Status{ get; set; }

        public int Patient_ID { get; set; }

        public int Payment_Type_ID { get; set; }

        public string Order_Amount { get; set; }

        public string Order_Date { get; set; }

        public string Deliver_Date { get; set; }

        public int Cart_ID { get; set; }

        public bool Is_Paid { get; set; }

        [NotMapped]
        public int Order_Qty { get; set; }

        [NotMapped]
        public string Patient_Name { get; set; }

        [NotMapped]
        public string Patient_Profile { get; set; }

        [NotMapped]
        public string Payment_Type_Name { get; set; }

        [NotMapped]
        public string Medicine_Name { get; set; }

        [NotMapped]
        public int Order_Medicine_ID { get; set; }
        [NotMapped]
        public int Medicine_ID { get; set; }
        [NotMapped]
        public int Price { get; set; }
    }
}
