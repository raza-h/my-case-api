using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Entities
{
    public class PracticeArea
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Practice Name Required")]
        public string PracticeAreaName { get; set; }
        [NotMapped]
        public int ActiveCases { get; set; }
        public int? FirmId { get; set; }
        public string UserId { get; set; }
    }
}
