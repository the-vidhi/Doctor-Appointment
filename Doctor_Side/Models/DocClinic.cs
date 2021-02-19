using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Doctor_Side.Models
{
    public class DocClinic
    {
        [Key]
        public int Clinic_ID { get; set; }

        [Required(ErrorMessage = "Please Enter Name !!")]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Enter Only Alphabet")]

        public string Clinic_Name { get; set; }

        [Required(ErrorMessage = "Please Enter Clinic Address !!")]
        [StringLength(100, MinimumLength = 3)]
        public string Clinic_Address { get; set; }

        [Required(ErrorMessage = "Please Enter Pincode !!")]


        public string Clinic_Pincode { get; set; }

        [Required(ErrorMessage = "Please Enter Clinic Contact !!")]
        [DataType(DataType.PhoneNumber)]
        public string Clinic_Contact { get; set; }
        public bool Clinic_IsActive { get; set; }

        [Required(ErrorMessage = "Please Select State !!")]
        public int State_ID { get; set; }

        [Required(ErrorMessage = "Please Select City !!")]
        public int City_ID { get; set; }

        public int Doctor_ID { get; set; }

        [NotMapped]
        public string State_Name { get; set; }

        [NotMapped]
        public string City_Name { get; set; }

        [Required(ErrorMessage = "Please Select Profile !!")]
        public string Clinic_Profile { get; set; }

    }



}
