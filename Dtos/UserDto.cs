using MyCaseApi.Entities;
using System.Collections.Generic;

namespace MyCaseApi.Dtos
{
    public class UserDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string RoleName { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string ImagePath { get; set; }
        public string ParentId { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool AdminApproval { get; set; }
        public List<Service> Services { get; set; } 
        public List<int> PricePlanIds { get; set; }
    }
}
