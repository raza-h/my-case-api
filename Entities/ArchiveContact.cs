using System.ComponentModel.DataAnnotations;

namespace MyCaseApi.Entities
{
    public class ArchiveContact
    {
        [Key]
        public int Id { get; set; }
        // for current logged in user
        public string ContactOne { get; set; }
        // for other user we have chat with
        public string ContactTwo { get; set; }
        public int? GroupId { get; set; }
    }
}
