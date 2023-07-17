using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Entities
{
    public class NotesTag
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Notes Name Required")]
        public string NotesTagName { get; set; }
        public int? FirmId { get; set; }

        public string UserId { get; set; }

    }
}
