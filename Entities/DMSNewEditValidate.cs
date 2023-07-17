﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Entities
{
    public class DMSNewEditValidate
    {
        public int? mainId { get; set; }
        public int? sub1Id { get; set; }
        public int? sub2Id { get; set; }
        public int? sub3Id { get; set; }
        public string mainName { get; set; }
        public string sub1Name { get; set; }
        public string sub2Name { get; set; }
        public string sub3Name { get; set; }

    }
}
