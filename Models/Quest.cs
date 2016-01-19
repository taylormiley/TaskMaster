using System;
using System.Collections.Generic;

namespace TaskMaster.Models
{
    public class Quest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DueBy { get; set; }
        public string Reward { get; set; }
        public string Status { get; set; }
        public string UserName { get; internal set; }
    }
}