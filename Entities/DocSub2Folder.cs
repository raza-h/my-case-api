using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyCaseApi.Entities
{
    public class DocSub2Folder
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonProperty(PropertyName = "DocFolderId")]

        public virtual int DocSub1FolderId { get; set; }
        [ForeignKey("DocSub1FolderId")]
        public  DocSub1Folder DocSub1Folder { get; set; }
        [JsonProperty(PropertyName = "DocSubFolders")]
        public ICollection<DocSub3Folder> DocSub3Folders { get; set; }

        public ICollection<DocumentCategory> DocumentCategory { get; set; }
        public int? FirmId { get; set; }
    }
}