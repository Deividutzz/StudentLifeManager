using StudentLifeManager.classes.Models;

namespace StudentLifeManager.classes.Services;

class TaskService
{
    public void AddTask(Subject subject, string title, DateTime deadline)
    {
        subject.Tasks.Add(new TaskItem
        {
            Title = title,
            Deadline = deadline,
            IsCompleted = false
        });
    }
}