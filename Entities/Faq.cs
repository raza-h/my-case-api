using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Entities
{
    public class Faq
    {
        [Key]
        public int Id { get; set; }
   
        public string Question { get; set; }
        public string Answer { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
