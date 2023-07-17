using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Entities
{
    public class GroupMessage
    {
        [Key]
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int MessageId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
