using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCaseApi.Entities
{
    public class CustomField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FieldId { get; set; }
        public string CustomFieldName { get; set; }
        public string CustomFieldType { get; set; }
        public int? FirmId { get; set; }
        public string UserId { get; set; }
        public string Type { get; set; }
        public bool FullPractice { get; set; }
        public bool PartialPractice { get; set; }

        [NotMapped]
        public List<CFieldValue> CFieldValue { get; set; }
        [NotMapped]
        public string CustomFieldNametemp { get; set; }

    }
}
