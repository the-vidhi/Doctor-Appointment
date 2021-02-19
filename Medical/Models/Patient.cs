using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Medical.Models
{
    public class Patient
    {
        [Key]
        public int Patient_ID { get; set; }

        [Required(ErrorMessage = "Please Enter Your Name !!!")]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Enter Only Alphabet")]
        public string Patient_Name { get; set; }

        [Required(ErrorMessage = "Please Enter Address !!!")]
        [StringLength(100)]
        public string Patient_Address { get; set; }

        [Required(ErrorMessage = "Please Enter Contact !!!")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Contact number")]
        public string Patient_Contact { get; set; }

        [Required(ErrorMessage = "Please Enter Password !!!")]
        [DataType(DataType.Password)]
        public string Patient_Password { get; set; }

        [Required(ErrorMessage = "Please Select State !!!")]
        public int State_ID { get; set; }

        [Required(ErrorMessage = "Please Select City !!!")]
        public int City_ID { get; set; }

        [Required(ErrorMessage = "Please Enter Email !!!")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                            ErrorMessage = "Please enter a valid email address")]

        public string Patient_Email { get; set; }
        public bool Patient_IsActive { get; set; }

        [Required(ErrorMessage = "Please Select Profile !!!")]
        public string Patient_Profile { get; set; }

        [NotMapped]
        public string State_Name { get; set; }
        [NotMapped]
        public string City_Name { get; set; }

    }
}
