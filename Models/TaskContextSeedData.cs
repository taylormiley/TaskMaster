using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TaskMaster.Models
{
    public class TaskContextSeedData
    {
        private TaskContext _context;
        private UserManager<TaskUser> _userManager;

        public TaskContextSeedData(TaskContext context, UserManager<TaskUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task EnsureSeedDataAsync()
        {
            if (await _userManager.FindByEmailAsync("taylor.miley@taskmaster.com") == null)
            {
                // Add the User.
                var user = new TaskUser()
                {
                    UserName = "taylormiley",
                    Email = "taylor.miley@taskmaster.com",
                    FirstQuest = DateTime.UtcNow
                };

                await _userManager.CreateAsync(user, "Metal242$");
            }

            if (!_context.Quests.Any())
            {
                // Add new Data
                var beerQuest = new Quest()
                {
                    Name = "Beer Run",
                    DueBy = DateTime.UtcNow,
                    Reward = "Lost of Beer!",
                    Status = "Incomplete",
                    UserName = "taylormiley"
                };

                _context.Quests.Add(beerQuest);

                var laundryQuest = new Quest()
                {
                    Name = "Do Laundry",
                    DueBy = DateTime.UtcNow,
                    Reward = "Game Time!",
                    Status = "Complete",
                    UserName = "taylormiley"
                };

                _context.Quests.Add(laundryQuest);

                _context.SaveChanges();
            }
        }
    }
}