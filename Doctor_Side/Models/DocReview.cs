using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Doctor_Side.Models
{
    public class DocReview
    {
        [Key]
        public int Review_ID { get; set; }
        public int Patient_ID { get; set; }
        public int Doctor_ID { get; set; }
        public string Review_Msg { get; set; }

        [NotMapped]
        public string Patient_Name { get; set; }

        [NotMapped]
        public string Doctor_Name { get; set; }

        [NotMapped]
        public string Patient_Profile { get; set; }
        [NotMapped]
        public string Patient_Address { get; set; }
        [NotMapped]
        public string Patient_Contact { get; set; }
        [NotMapped]
        public int State_ID { get; set; }
        [NotMapped]
        public int City_ID { get; set; }
        [NotMapped]
        public string Patient_Email { get; set; }

        [NotMapped]
        public string State_Name { get; set; }
        [NotMapped]
        public string City_Name { get; set; }
    }
}
