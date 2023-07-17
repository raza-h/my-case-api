using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCaseApi.Entities
{
    public class ChatGroup
    {
        [Key]
        public int Id { get; set; }
        public string GroupName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string AdminId { get; set; }
        [NotMapped]
        public List<string> UserIds { get; set; }
        public string ImagePath { get; set; }
        [NotMapped]
        public byte[] Image { get; set; }
        [NotMapped]
        public Message message { get; set; }
    }
}
