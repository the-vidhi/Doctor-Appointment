using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Medical.Models
{
    public class State
    {
        [Key]
        public int State_ID { get; set; }

        [Required(ErrorMessage ="Please Fill the field!!!")]
        public string State_Name { get; set; }

        [NotMapped]
        public int City_ID { get; set; }
    }
}
