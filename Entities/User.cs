using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCaseApi.Entities
{
    public enum VerificationStatus
    {
        Pending = 1,
        Approved = 2,
        Rejected = 3,
    }
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string RoleName { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string ImagePath { get; set; }
        [NotMapped]
        public byte [] File { get; set; }
        public bool Status { get; set; }
        public VerificationStatus VerificationStatus { get; set; }
        public string ParentId { get; set; }
        public string CaseRate { get; set; }
      
        public int? UserTitleId { get; set; }
        [NotMapped]
        public string newPassword { get; set; }
    }
}
