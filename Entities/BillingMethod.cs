using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Entities
{
    public class BillingMethod
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Billing Name is Required")]
        public string Name { get; set; }
        public int? FirmId { get; set; }
        public string UserId { get; set; }
    }
}
