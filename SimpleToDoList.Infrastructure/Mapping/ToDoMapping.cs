using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleToDoList.Domain;
using SimpleToDoList.Domain.ToDo;



namespace SimpleToDoList.Infrastructure.Mapping
{
    public class ToDoMapping : IEntityTypeConfiguration<ToDo>
    {
        public void Configure(EntityTypeBuilder<ToDo> builder)
        {
            builder.ToTable("ToDo");

            builder.Property(x => x.Name).HasMaxLength(500);
            //builder.HasOne<Account>(x => x.Account).WithMany(x => x.ToDoes).HasForeignKey(x => x.AccountId);
            builder.HasOne(x => x.Account).WithMany(x => x.ToDos).HasForeignKey(x => x.AccountId).OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.ProjectTask).WithMany(x => x.ToDoes).HasForeignKey(x => x.TaskId).IsRequired(false);


        }
    }
}
