﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskMaster.ViewModels
{
    public class QuestViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 5)]
        public string Name { get; set; }
        public DateTime DueBy { get; set; }
        public string Reward { get; set; }
        public string Status { get; set; }
    }
}
