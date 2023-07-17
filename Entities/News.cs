using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Entities
{
    public class News
    {
        [Key]
        public int Id { get; set; }
        public string NewsTittle { get; set; }
        public string NewsDescription { get; set; }
        public string NewsType { get; set; }
        public string SendTo { get; set; }
        public DateTime? Time { get; set; }
        public bool status { get; set; }


        public DateTime? ExpireDate { get; set; }
        public DateTime? PublishDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public int? FirmId { get; set; }


    }
}
