using Microsoft.AspNetCore.Http;
using MyCaseApi.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Dtos
{
    public class SignupDto
    {
        public UserSignupDto UserSignupDto { get; set; }
        public PaymentDto PaymentInfoDto { get; set; }
        public int PricePlanId { get; set; }
        public PricePlan PricePlan { get; set; }
        public byte[] File { get; set; }
    }

    public class UserSignupDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string PhoneNumber { get; set; }
        public string RoleName { get; set; }
        public string Rate { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public int UserTitleId { get; set; }
        public string ParentId { get; set; }
        public byte[] file { get; set; }
        public IFormFile profile_img { get; set; }
    }

    public class PaymentDto
    {
        public BankInfoDto Bank { get; set; }

        public CreditCardDto CreditCard { get; set; }

        public PaymentType PaymentType { get; set; }
    }

    public class BankInfoDto
    {
        public string FilePath { get; set; }
    }

    public class CreditCardDto
    {
        public string FullName { get; set; }
        public string CardNumber { get; set; }
        public int ExpirationMonth { get; set; }
        public int ExpirationYear { get; set; }
        public int CVV { get; set; }
    }

}
