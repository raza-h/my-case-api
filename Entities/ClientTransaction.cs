using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCaseApi.Entities
{
    public enum ClientPaymentType
    {
        Cash = 1,
        Bank = 2
    }
    public class ClientTransaction
    {
        [Key]
        public int Id { get; set; }
        public string Amount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string InvoiceNo { get; set; }
        public ClientPaymentType PaymentType { get; set; }
        public string Note { get; set; }
        public int? ContactId { get; set; }
        public bool IsCash { get; set; }
        public bool IsCredit { get; set; }
        public string CheckNumber { get; set; }
        public string CheckTitle { get; set; }
        public DateTime? CheckDate { get; set; }
        public string CheckAmount { get; set; }
        public string CheckImagePath { get; set; }
        [NotMapped]
        public byte[] File { get; set; } 
        public string UserId { get; set; }
        public string ParentId { get; set; }
        [NotMapped]
        public string ClientName { get; set; }
        [NotMapped]
        public string CaseName { get; set; }
        [NotMapped]
        public string CreatedDate { get; set; }
        [NotMapped]
        public Contact Contact { get; set; }
        [NotMapped]
        public User User { get; set; }
    }
}
