namespace PlannerApplication.Models
{
    public class ToDoTask
    {
        public int Id { get; set; }

        public Guid UserId { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskState Status { get; set; } = TaskState.NotStarted;
        public DateOnly TargetStartDate { get; set; }
        public DateOnly TargetEndDate { get; set; }

        public ToDoTask()
        {
        }

        public ToDoTask(string name, string desc, DateOnly startdate, DateOnly enddate, Guid userID)
        {
            this.Name = name;
            this.Description = desc;
            this.TargetStartDate = startdate;
            this.TargetEndDate = enddate;
            this.UserId = userID;
        }
    }
}