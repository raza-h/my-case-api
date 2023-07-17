using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCaseApi.Entities
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public string MessageText { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string ImagePath { get; set; }
        [NotMapped]
        public byte[] Image { get; set; }
        [NotMapped]
        public string Contact { get; set; }
        [NotMapped]
        public string UserImagePath { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsRead { get; set; }
        public bool Status { get; set; }
        public bool IsGroupMessage { get; set; }
        [NotMapped]
        public int? GroupId { get; set; }
        [NotMapped]
        public int UnreadCount { get; set; }
        [NotMapped]
        public bool IsArchived { get; set; }
    }
}
