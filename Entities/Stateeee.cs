using Hangfire.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace MyCaseApi.Entities
{
    public class Stateeee
    {
        public long Id { get; set; }
        public long JobId { get; set; }
        public string Name { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Data { get; set; }

    }
}
