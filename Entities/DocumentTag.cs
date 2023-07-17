using System.ComponentModel.DataAnnotations;

namespace MyCaseApi.Entities
{
    public class DocumentTag
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Document Name Requird")]
        public string DocumentTagName { get; set; }
        public int? FirmId { get; set; }
        public string UserId { get; set; }

    }
}
