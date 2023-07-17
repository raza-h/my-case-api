using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Entities
{
    public class ContactUs
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "You must provide a Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You must provide a Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "You must provide a phone number")]
       
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "You must provide a Address")]
        public string Address { get; set; }

        public string  Comment { get; set; }



    }
}
