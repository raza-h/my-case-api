using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCaseApi.Entities
{
    public class TimeEntryActivity
    {
        [Key]
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public string UserId { get; set; }
        public int? FirmId { get; set; }

        [NotMapped]
        public List<CustomField> customField { get; set; }
        [NotMapped]
        public List<CFieldValue> cfieldValue { get; set; }

    }
}
