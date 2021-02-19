using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Doctor_Side.Models
{
    public class DoctorReg
    {
        [Key]
        public int Doctor_ID { get; set; }

        [Required(ErrorMessage = "Please Enter Name !!")]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Enter Only Alphabet")]
        public string Doctor_Name { get; set; }


        [Required(ErrorMessage = "Please Enter Valid Email ID !!")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                            ErrorMessage = "Please enter a valid email address")]

        public string Doctor_Email { get; set; }

        [Required(ErrorMessage = "Select Gender ...")]
        public string Doctor_Gender { get; set; }

        [Required(ErrorMessage = "Please Enter Password !!")]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[@])(?=.{6,})$", ErrorMessage = "The string must contain at least 1 lowercase,1 uppercase, 1 numeric,one special character alphabetical character and six characters or longer")]
        //[Compare("ConfirmPassword",ErrorMessage ="Password Does Not Match !!")]
        [DataType(DataType.Password)]
        public string Doctor_Password { get; set; }

        [Required(ErrorMessage = "Please Enter Contact !!")]
        [DataType(DataType.PhoneNumber)]
        public string Doctor_Contact { get; set; }


        public bool Doctor_IsActive { get; set; }

        [Required(ErrorMessage = "Please Select Category")]
        public int Category_ID { get; set; }

        [Required(ErrorMessage = "Please Enter Degree !!")]

        public string Doctor_Degree { get; set; }



        [Required]
        public string Doctor_Profile { get; set; }

        [Required(ErrorMessage = "Please Enter Education Qulification !!")]
        public string Doctor_Education { get; set; }

        [Required(ErrorMessage = "Please Enter Your Experience !!")]
        public string Doctor_Experience { get; set; }

        [NotMapped]
        public string Category_Name { get; set; }
        [NotMapped]
        public string Clinic_Name { get; set; }


    }
}
