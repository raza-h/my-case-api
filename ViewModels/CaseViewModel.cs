using MyCaseApi.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.ViewModels
{
    public class CaseViewModel
    {
        public int Id { get; set; }
        public string CaseName { get; set; }
        public int? CaseNumber { get; set; }
        public int? PracticeArea { get; set; }
        public Stages? CaseStage { get; set; }
        public DateTime DateAppend { get; set; }
        public string Office { get; set; }
        public string Description { get; set; }
        public DateTime? StatueOfLimitation { get; set; }
        public string ConflictCheckNotes { get; set; }
        public string JobTitle { get; set; }
        public int? CaseRate { get; set; }
        public int? BillingContact { get; set; }
        public int? BillingMethod { get; set; }
        public string LeadAttorney { get; set; }
        public string LeadAttTest { get; set; }
        public string OriginatingLeadAttorney { get; set; }
        public string CaseAddedBy { get; set; }
        [NotMapped]
        public string BillingContactList { get; set; }
        public int? FirmId { get; set; }

        public List<StafJsonViewModel> staflist { get; set; }
        [NotMapped]
        public List<CustomField> customField { get; set; }

        [NotMapped]
        public List<CFieldValue> cfieldValue { get; set; }
    }
}
