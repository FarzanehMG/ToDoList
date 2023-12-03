using Microsoft.EntityFrameworkCore;
using SimpleToDoList.Domain;
using SimpleToDoList.Domain.ToDo;
using SimpleToDoList.Infrastructure.Mapping;


namespace SimpleToDoList.Infrastructure
{
    public class ToDoContext : DbContext
    {
        public DbSet<ToDo> ToDos { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Employee>  Employees { get; set; }

        public ToDoContext(DbContextOptions<ToDoContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly = typeof(ToDoMapping).Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
            base.OnModelCreating(modelBuilder);
        }
    }


}
