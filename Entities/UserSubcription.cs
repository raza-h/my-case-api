using System;
using System.ComponentModel.DataAnnotations;

namespace MyCaseApi.Entities
{
    public enum PaymentType
    {
        Paypal = 1,
        CreditCard = 2,
        BankAccount = 3,
        None = 4
    }
    public enum PaymentStatus
    {
        Paid = 1,
        UnPaid = 2,
        Trial = 3,
        Cancelled = 4
    }
    public class UserSubcription
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int PricePlanId { get; set; }
        public string Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Period { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public PaymentType Paymenttype { get; set; }
        public TimeSpan CreationTime { get; set; }

    }
}
