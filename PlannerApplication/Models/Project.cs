namespace PlannerApplication.Models
{
    public class Project
    {
        public Project()
        {
        }

        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid[]? UserIds { get; set; }

    }
}
