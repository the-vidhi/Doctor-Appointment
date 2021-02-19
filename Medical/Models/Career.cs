using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Medical.Models
{
    public class Career
    {
        [Key]
        public int Career_ID { get; set; }
        public int Patient_ID { get; set; }
        public string Candidate_Name { get; set; }
        public string Candidate_Contact { get; set; }
        public string Candidate_Resume { get; set; }
        public string Candidate_Email { get; set; }

        [NotMapped]
        public string Patient_Name { get; set; }
    }
}
