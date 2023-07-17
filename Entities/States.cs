using System.ComponentModel.DataAnnotations;

namespace MyCaseApi.Entities
{
    public class States
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
