using PlannerApplication.Models;
namespace PlannerApplication.Repositories
{
    public class ProjectRepository
    {
        private List<Project> projects;
        private ProjectJson projectjson = new ProjectJson();

        public ProjectRepository()
        {
            projects = projectjson.GetProjects();
        }

        public void AddProject(Project project)
        {
            if (project.Id == Guid.Empty) project.Id = Guid.NewGuid();
            projects.Add(project);
            projectjson.SaveProjects(projects);
        }

        public List<Project> GetProjects()
        {
            return projects;
        }

        public bool UpdateProject(Project project)
        {
            Project? existingProject = projects.FirstOrDefault(t => t.Id == project.Id);
            if (existingProject == null) return false;

            //existingProject.Name = task.Name;
            //existingProject.Description = task.Description;
            //existingProject.TargetStartDate = task.TargetStartDate;
            //existingProject.TargetEndDate = task.TargetEndDate;
            //existingProject.Status = task.Status;
            //projectjson.SaveProjects(tasks);
            return true;
        }

        public bool DeleteProject(Guid id)
        {
            Project? project = projects.FirstOrDefault(t => t.Id == id);
            if (project == null) return false;

            projects.Remove(project);
            projectjson.SaveProjects(projects);
            return true;
        }
    }
}