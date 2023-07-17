using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCaseApi.Entities
{
    public class PackageService
    {
        [Key]
        public int Id { get; set; }
        public int? ServiceId { get; set; }
        public int? PricePlanId { get; set; }
        [NotMapped]
        public List<Service> services { get; set; }
        [NotMapped]
        public PricePlan pricePlan { get; set; }
        [NotMapped]
        public int[] ServicesIds { get; set; }
        public bool? IsSeleced { get; set; }
    }
}
