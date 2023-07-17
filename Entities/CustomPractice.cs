using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCaseApi.Entities
{
    public class CustomPractice
    {
        [Key]
        public int PracticeFieldID { get; set; }
        public int PracticeAreaID { get; set; }
        public int FieldID { get; set; }
        [NotMapped]
        public CFieldValue cFieldValue { get; set; }
        [NotMapped]
        public PracticeArea practiceArea { get; set; }
    }
}
