namespace PlannerApplication.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Password { get; set; }

        public User(string name, string password)
        {
            this.Name = name;
            this.Password = password;
        }
    }
}
