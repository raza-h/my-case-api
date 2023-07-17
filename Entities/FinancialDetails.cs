using System.ComponentModel.DataAnnotations;

namespace MyCaseApi.Entities
{
    public enum Usertype
    {
        Customer = 1,
        Staff = 2,
        Client = 3,
        Attorney = 4,
        Expense = 5,
        CashInHand = 6,
        Sales = 7

    }
    public class FinancialDetails
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public Usertype Type { get; set; }
        public string AccountNumber {get;set;}
        public string UserId { get; set; }
        public string ParentId { get; set; }
    }
}
