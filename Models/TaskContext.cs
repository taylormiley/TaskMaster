using System;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;

namespace TaskMaster.Models
{
    public class TaskContext : IdentityDbContext<TaskUser>
    {
        public TaskContext()
        {
            Database.EnsureCreated();
        }
        public DbSet<Level> Levels { get; set; }
        public DbSet<Quest> Quests { get; set; }

         protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connString = Startup.Configuration["Data:TaskContextConnection"];

            optionsBuilder.UseSqlServer(connString);
            
            base.OnConfiguring(optionsBuilder);
        }
    }
}