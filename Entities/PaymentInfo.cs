using System;
using System.ComponentModel.DataAnnotations;

namespace MyCaseApi.Entities
{
    public class PaymentInfo
    {
        [Key]
        public int Id { get; set; }
        public string InvoiceNo { get; set; }
        public string Price { get; set; }
        public string PlanName { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public PaymentType PaymentType { get; set; }
        public int? SubsciptionId { get; set; }
        public string FromScreen { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
    }
}
