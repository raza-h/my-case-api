using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCaseApi.Entities
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string MainPhone { get; set; }
        public string FaxNumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int? ZipCode { get; set; }
        public int? CountryId { get; set; }
        public string PrivateNotes { get; set; }
        [NotMapped]
        public string CountryName { get; set; }
        public int? FirmId { get; set; }
        [NotMapped]
        public List<CustomField> customField { get; set; }
        [NotMapped]
        public List<CFieldValue> cfieldValue { get; set; }

    }
}
