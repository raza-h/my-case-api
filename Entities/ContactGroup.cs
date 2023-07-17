using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Entities
{
    public class ContactGroup
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Group Name Required")]
        public string ContactGroupName { get; set; }
        public int? FirmId { get; set; }
        public string UserId { get; set; }
    }
}
