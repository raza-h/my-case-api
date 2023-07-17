using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Entities
{
    public class UserTitle
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "User title Required")]
        public string UserTitleName { get; set; }
    }
}
