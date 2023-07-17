using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCaseApi.Entities
{
    public enum TransactionType
    {
        Credit = 1,
        Debit = 2,
    }
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public TransactionType transactionType { get; set; }
        public string Amount { get; set; }
        public DateTime Date { get; set; }
        public int DetailAccountId { get; set; }
        public string ClosingBalance { get; set; }
        public string AccountType { get; set; }
        [NotMapped]
        public string OpeningBalance { get; set; }
        [NotMapped]
        public string Description { get; set; }
        [NotMapped]
        public PaymentType Paymenttype { get; set; }
    }
}
