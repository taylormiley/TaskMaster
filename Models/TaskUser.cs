using System;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TaskMaster.Models
{
    public class TaskUser : IdentityUser
    {
        public DateTime FirstQuest { get; set; }
    }
}