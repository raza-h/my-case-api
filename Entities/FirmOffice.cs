using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Entities
{
    public class FirmOffice
    {
        [Key]
        public int Id { get; set; }
        public int FirmId { get; set; }
        public string OfficeName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsPrimary { get; set; }
        [NotMapped]
        public List<FirmOfficeAddress> FirmOfficeAddresses { get; set; }
    }
}
