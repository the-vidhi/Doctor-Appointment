using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Medical.Models
{
    public class Doctor
    {
        [Key]
        public int Doctor_ID { get; set; }
        public string Doctor_Name { get; set; }
        public string Doctor_Email { get; set; }
        public string Doctor_Gender { get; set; }
        public string Doctor_Password { get; set; }
        public string Doctor_Contact { get; set; }
        
        public bool Doctor_IsActive { get; set; }
        public int Category_ID { get; set; }
        public string Doctor_Degree { get; set; }
        public string Doctor_Education { get; set; }
        public string Doctor_Experience { get; set; }
       
        public string Doctor_Profile { get; set; }
        public string Doctor_Fees { get; set; }

        [NotMapped]
        public string Category_Name { get; set; }

        //public int Clinic_ID { get; set; }
        //[NotMapped]
        //public string Clinic_Name { get; set; }

        //[NotMapped]
        //public string Clinic_Address { get; set; }

        //[NotMapped]
        //public int Clinic_Pincode { get; set; }

        //[NotMapped]
        //public Int64 Clinic_Contact { get; set; }

        //[NotMapped]
        //public Int32 State_ID { get; set; }

        //[NotMapped]
        //public Int32 City_ID { get; set; }

        //[NotMapped]
        //public string State_Name { get; set; }

        //[NotMapped]
        //public string City_Name { get; set; }

        //[NotMapped]
        //public string Clinic_Profile { get; set; }

        //[NotMapped]
        //public bool Clinic_IsActive { get; set; }

        //public  Clinic Clinic { get; set; }



    }
}
