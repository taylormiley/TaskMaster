using System.Collections.Generic;

namespace TaskMaster.Models
{
    public interface ITaskRepository
    {
        IEnumerable<Quest> GetAllIncompleteQuests();
        IEnumerable<Quest> GetAllQuests();
        void AddQuest(Quest newQuest);
        void DeleteQuest(Quest questToDelete);
        bool SaveAll();
        IEnumerable<Quest> GetAllIncompleteUserQuests(string name);
    }
}