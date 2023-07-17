using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Entities
{
    public enum TimeLineType
    {
        Comment = 1,
        Document = 2,
        Message = 3,
        Zoom=4
    }
    public class TimeLine
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int? CaseId { get; set; }
        public string Comment { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }
        public string FilePath { get; set; }
        [NotMapped]
        public string UserImagePath { get; set; }
        [NotMapped]
        public string UserName { get; set; }
        [NotMapped]
        public string CaseName { get; set; }
        public DateTime? DueDate { get; set; }
        public int? ClientId { get; set; }
        public int? FirmId { get; set; }
        public TimeLineType TimeLineType { get; set; }
        public string HostLink { get; set; }
        public string JoinLink { get; set; }
        public DateTime? MeetingTime { get; set; }
        [NotMapped]
        public string Matching { get; set; }
        public bool IsReminder { get; set; }
        public Byte[] FilePathbyte { get; set; }
        public Byte[] VideoFilePathbyte { get; set; }
        public string VideoFilePath { get; set; }
        public string DocFilePath { get; set; }
        public Byte[] DocFilePathbyte { get; set; }
        [NotMapped]
        public string FileType { get; set; }
    }
}
