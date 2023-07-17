using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Entities
{
    public class UserVerification
    {
        [Key]
        public int Id { get; set; }
        public bool AutoApproval { get; set; }
        public bool AdminApproval { get; set; }
        public string UserId { get; set; }
        public int? PaymentId { get; set; }
        public PaymentType PaymentType { get; set; }
        public string ImagePath { get; set; }
    }
}
