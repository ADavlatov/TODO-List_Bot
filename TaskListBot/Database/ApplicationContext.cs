using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TaskListBot.Database;

public sealed class ApplicationContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<TaskObject> UserTasks => Set<TaskObject>();

    public ApplicationContext()
    {
        Database.EnsureCreated();
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=Database/Users.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new TaskObjectConfiguration());
    }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
    }
}

public class TaskObjectConfiguration : IEntityTypeConfiguration<TaskObject>
{
    public void Configure(EntityTypeBuilder<TaskObject> builder)
    {
        // builder.HasKey(x => new { x.Id, x.UserId});
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.User).WithMany(x => x.UserTasks).HasForeignKey(x => x.UserId);
    }
}