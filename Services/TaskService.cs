namespace TaskManagerApp.Services
{
    public class TaskService
    {
        private List<TaskManagerApp.Models.Task> _tasks = new List<TaskManagerApp.Models.Task>();
        private int _nextId = 1;

        public List<TaskManagerApp.Models.Task> GetTasks()
        {
            return _tasks;
        }

        public void AddTask(TaskManagerApp.Models.Task task)
        {
            task.Id = _nextId++;
            _tasks.Add(task);
        }

        public void DeleteTask(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                _tasks.Remove(task);
            }
        }

        public void CompleteTask(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                task.IsCompleted = true;
            }
        }
    }
}