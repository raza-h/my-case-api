using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Entities
{
    public class Firm
    {
        [Key]
        public int Id { get; set; }
        public string FirmName { get; set; }
        public string FirmNumber { get; set; }
        public string FirmEmail { get; set; }
        public string RegistrationNumber { get; set; }
        public string OwnerName { get; set; }
        public int NumberofEmployees { get; set; }
        public string FirmWebsite { get; set; }
        public string UserId { get; set; }
        [NotMapped]
        public int[] DeletedOffice { get; set; }
        [NotMapped]
        public int[] DeletedAddresses { get; set; }
        [NotMapped]
        public List<FirmOffice> FirmOffices { get; set; }
    }
}
