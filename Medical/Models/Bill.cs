using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Medical.Models
{
    public class Bill
    {
        [Key]
        public int Bill_ID { get; set; }
        public int Patient_ID { get; set; }
        [Required(ErrorMessage = "Please Enter Name !!!")]
        public string Patient_Name { get; set; }
        [Required(ErrorMessage = "Please Enter Email !!!")]
        public string Bill_Email { get; set; }

        [Required(ErrorMessage = "Please Enter Contact !!!")]
        public string Bill_Contact { get; set; }

        [Required(ErrorMessage = "Please Enter Address !!!")]
        public string Bill_Address { get; set; }
        public int Order_ID { get; set; }
        public int State_ID { get; set; }
        public int City_ID { get; set; }

        [Required(ErrorMessage = "Please Enter Pincode !!!")]
        public int Bill_Pincode { get; set; }
        [NotMapped]
        public string Order_Status { get; set; }
        [NotMapped]
        public string Order_Date { get; set; }
        [NotMapped]
        public string Order_Amount { get; set; }
        [NotMapped]
        public int Order_Medicine_ID { get; set; }
        [NotMapped]
        public int Price { get; set; }
        [NotMapped]
        public int Order_Qty { get; set; }
       
        [NotMapped]
        public int Medicine_ID { get; set; }
        [NotMapped]
        public string Medicine_Name { get; set; }
        public string Doctor_Prescription { get; set; }

        [NotMapped]
        public string State_Name { get; set; }

        [NotMapped]
        public string City_Name { get; set; }
    }
}
