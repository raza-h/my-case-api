using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Dtos
{
    public class UserDropDown
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserTitle { get; set; }
        public string ImagePath { get; set; }
        public int UserTitleId { get; set; }
    }
}
