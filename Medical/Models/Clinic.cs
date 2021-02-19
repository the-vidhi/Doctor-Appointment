using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Medical.Models
{
    public class Clinic
    {
        [Key]
        public int Clinic_ID { get; set; }
        public string Clinic_Name { get; set; }
        public string Clinic_Address { get; set; }
        public string Clinic_Pincode { get; set; }
        public string Clinic_Contact { get; set; }
        public Int32 State_ID { get; set; }
        public Int32 City_ID { get; set; }
        [NotMapped]
        public string State_Name { get; set; }
        [NotMapped]
        public string City_Name { get; set; }
        public string Clinic_Profile { get; set; }
        public string Clinic_Time { get; set; }
        public int Doctor_ID { get; set; }
        public bool Clinic_IsActive { get; set; }

        //[NotMapped]
        //public string  Doctor_Name { get; set; }

    }
}
