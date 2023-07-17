using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCaseApi.Entities
{
    public enum Priority
    {
        NoPriority = 1,
        Low = 2,
        Medium = 3,
        High = 4
    }
    public enum Status
    {
        Active = 1,
        InProgress = 2,
        Completed = 3,
        OnHold = 4,
        Dismissed = 5,
        Closed = 6
    }
    public class Tasks
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? DueDate { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }
        public string Description { get; set; }
        public string ClientId { get; set; }
        public string StaffId { get; set; }
        public int? CaseId { get; set; }
        [NotMapped]
        public string CaseName { get; set; }
        public int? FirmId { get; set; }
        public int? WorkflowId { get; set; }
    }
}
