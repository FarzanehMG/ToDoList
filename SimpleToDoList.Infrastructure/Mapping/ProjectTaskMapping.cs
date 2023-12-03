using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleToDoList.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleToDoList.Infrastructure.Mapping
{
    public class ProjectTaskMapping : IEntityTypeConfiguration<ProjectTask>
    {
        public void Configure(EntityTypeBuilder<ProjectTask> builder)
        {
            builder.ToTable("Task");

            builder.Property(x => x.Name).HasMaxLength(500);


            builder.HasMany(x => x.Projects).WithMany(x => x.ProjectTasks);
            builder.HasMany(x => x.Employees).WithMany(x => x.ProjectTasks);

            builder.HasMany(x => x.ToDoes).WithOne(x => x.ProjectTask).HasForeignKey(x => x.TaskId);

        }
    }
}
