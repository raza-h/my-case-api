using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Entities
{
    public class GroupUser
    {
        [Key]
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string UserId { get; set; }
        [NotMapped]
        public string UserImagePath { get; set; }
        public bool IsArchive { get; set; }
    }
}
