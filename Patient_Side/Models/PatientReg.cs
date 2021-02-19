using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Patient_Side.Models
{
    public class PatientReg
    {
        [Key]
        public int Patient_ID { get; set; }

        public string Patient_Name { get; set; }
        public string Patient_Address { get; set; }
        public string Patient_Contact { get; set; }
        public int State_ID { get; set; }
        public int City_ID { get; set; }
        public string Patient_Email { get; set; }
        public string Patient_Password { get; set; }
        public bool Patient_IsActive { get; set; }
        public string Patient_Profile { get; set; }

        [NotMapped]
        public string State_Name { get; set; }
        [NotMapped]
        public string City_Name { get; set; }
    }
}
