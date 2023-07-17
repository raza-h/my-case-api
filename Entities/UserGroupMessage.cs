using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCaseApi.Entities
{
    public class UserGroupMessage
    {
        [Key]
        public int Id { get; set; }
        public int MessageId { get; set; }
        public int GroupId { get; set; }
        public string UserId { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
