using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Entities
{
    public class PricePlan
    {
        [Key]
        public int PlanID { get; set; }
        public string PlanName { get; set; }
        public string PriceRange { get; set; }
        public string TimeRange { get; set; }
        [NotMapped]
        public int[] ServicesIds { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
