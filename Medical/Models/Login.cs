using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Medical.Models
{
    public class Login
    {
        [Key]
        public int Admin_ID { get; set; }
        public string Admin_Name { get; set; }

        [Required(ErrorMessage = "Please Fill The Field!!")]
        public string Admin_Password { get; set; }

        [Required(ErrorMessage = "Please Fill The Field!!")]
        public string Admin_Email { get; set; }    
    }
}
