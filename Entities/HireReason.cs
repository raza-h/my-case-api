using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Entities
{
    public class HireReason
    {
        [Key]
        public int ReasonId { get; set; }
   
        public string ReasonName { get; set; }
        public int? FirmId { get; set; }
        public string UserId { get; set; }
    }
}
