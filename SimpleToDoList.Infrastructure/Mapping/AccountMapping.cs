using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleToDoList.Domain;


namespace SimpleToDoList.Infrastructure.Mapping
{
    public class AccountMapping : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Account");
            builder.Property(x => x.Fullname).HasMaxLength(500).IsRequired();
            builder.Property(x => x.Username).HasMaxLength(500).IsRequired();
            builder.Property(x => x.Password).HasMaxLength(500).IsRequired();
            builder.Property(x => x.Mobile).HasMaxLength(20).IsRequired();

            builder.HasMany(x => x.ToDos)
                .WithOne(x => x.Account)
                .HasForeignKey(x => x.AccountId);



        }
    }
}
