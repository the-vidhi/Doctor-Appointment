using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Medical.Models
{
    public class Category
    {
        [Key]
        public int Category_ID { get; set; }

        [Required(ErrorMessage = "Please Fill the field!!!")]
        public string Category_Name { get; set; }
        
        [Required(ErrorMessage = "Please Fill the field!!!")]
        public string Category_Description { get; set; }
        
        [Required(ErrorMessage = "Please Fill the field!!!")]
        public string Category_SubDescription { get; set; }
        
        [Required(ErrorMessage = "Please Fill the field!!!")]
        public string Category_Profile { get; set; }
        [Required(ErrorMessage = "Please Fill the field!!!")]
        public string Category_MainProfile { get; set; }
    }
}
