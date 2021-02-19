using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Medical.Models
{
    public class Payment
    {
        [Key]
        public int Payment_Type_ID { get; set; }

        [Required(ErrorMessage = "Please Fill the field!!!")]
        public string Payment_Type_Name { get; set; }
    }
}
