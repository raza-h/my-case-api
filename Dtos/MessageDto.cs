using System;

namespace MyCaseApi.Dtos
{
    public class MessageDto
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string MessageText { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string ImagePath { get; set; }
        public byte[] Image { get; set; }
        public string Contact { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsRead { get; set; }
        public bool Status { get; set; }
    }
}
