using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleToDoList.Domain;


namespace SimpleToDoList.Infrastructure.Mapping
{
    public class EmployeeMapping : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees");
            builder.Property(x => x.Salary).HasMaxLength(500).IsRequired();
            builder.Property(x => x.Job).HasMaxLength(500).IsRequired();
            builder.Property(x => x.Department).HasMaxLength(500).IsRequired();

            builder.HasMany(x => x.ProjectTasks)
                .WithMany(x => x.Employees);

            builder.HasMany(x => x.Projects)
                .WithMany(x => x.Employees);

        }
    }
}
