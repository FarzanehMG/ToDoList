using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleToDoList.Domain;


namespace SimpleToDoList.Infrastructure.Mapping
{
    public class ProjectMapping : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable("Projects");
            builder.Property(x => x.Name).HasMaxLength(500).IsRequired();


            builder.HasMany(x => x.ProjectTasks)
                .WithMany(x => x.Projects);


            builder.HasMany(x => x.Employees)
                .WithMany(x => x.Projects);


            /*builder.HasMany(x => x.Employees)
            .WithMany(x => x.Projects)
            .UsingEntity<EmployeeProject>(
                j => j.HasOne(ep => ep.Employee).WithMany(e => e.EmployeeProjects)
                    .HasForeignKey(ep => ep.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade), // Adjust this based on your requirements
                j => j.HasOne(ep => ep.Project).WithMany(p => p.EmployeeProjects)
                    .HasForeignKey(ep => ep.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade)); // Adjust this based on your requirements*/
        }
    }
}
