using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Medical.Models
{
    public class Appointment
    {
        [Key]
        public int Appointment_ID { get; set; }
        public string Appointment_Date { get; set; }
        public int Doctor_ID { get; set; }
        public int Patient_ID { get; set; }
        public int Category_ID { get; set; }
        public string Appointment_Time { get; set; }
        public string Appointment_Status { get; set; }
        public string Appointment_Msg { get; set; }

        [NotMapped]
        public string Patient_Name { get; set; }
        
        [NotMapped]
        public string Doctor_Name { get; set; }

        [NotMapped]
        public string Patient_Profile { get; set; }
    }
}
