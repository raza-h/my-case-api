using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyCaseApi.Entities
{
    public class DocSub1Folder
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual int DocFolderId { get; set; }
        [ForeignKey("DocFolderId")]
        public  DocumentFolder DocumentFolder { get; set; }
        public int? FirmId { get; set; }
        [NotMapped]
        public string DirectoryType { get; set; }
    }
}