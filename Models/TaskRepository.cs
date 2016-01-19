using Microsoft.Data.Entity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskMaster.Models
{
    public class TaskRepository : ITaskRepository
    {
        private TaskContext _context;
        private ILogger<TaskRepository> _logger;

        public TaskRepository(TaskContext context, ILogger<TaskRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Quest> GetAllQuests()
        {
            try
            {
                return _context.Quests.OrderBy(t => t.Name).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get quests from database", ex);
                return null;
            }
        }

        public IEnumerable<Quest> GetAllIncompleteQuests()
        {
            try
            {
                return _context.Quests
                    .OrderBy(t => t.Name)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get incomplete quests from database", ex);
                return null;
            }
        }

        public void AddQuest(Quest newQuest)
        {
            _context.Add(newQuest);
        }

        public void DeleteQuest(Quest questToDelete)
        {
            _context.Remove(questToDelete);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public IEnumerable<Quest> GetAllIncompleteUserQuests(string name)
        {
            try
            {
                return _context.Quests
                    .OrderBy(t => t.Name)
                    .Where(t => t.UserName == name)
                    .Where(t => t.Status == "Incomplete")
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get incomplete quests from database", ex);
                return null;
            }
        }
    }
}
