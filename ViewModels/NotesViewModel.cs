using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.ViewModels
{
    public class NotesViewModel
    {

        public string NotesTag { get; set; }
        public string NotesTittle { get; set; }
        public string NotesType { get; set; }
        [DataType(DataType.MultilineText)]
        public string NotesDescripation { get; set; }
        public string UserId { get; set; }
        public string CaseId { get; set; }
      
        public string CreatedDate { get; set; }
    }
}
