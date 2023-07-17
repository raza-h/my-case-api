using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyCaseApi.Entities
{
    public class DocumentFolder
    {
        [Key]
        [Display(Name = "Folder Id")]
        public int DocumentFolderId { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public ICollection<DocSub1Folder> DocSub1Folders { get; set; }

        public int? FirmId { get; set; }

    }
}