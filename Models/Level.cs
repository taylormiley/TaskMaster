using System;
using System.Collections.Generic;

namespace TaskMaster.Models
{
    public class Level
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public string UserName { get; set; }
        public int CurrentLevel { get; set; }
    }
}
