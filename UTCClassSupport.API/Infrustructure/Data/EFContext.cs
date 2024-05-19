using DocumentFormat.OpenXml.Vml.Office;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;
using UTCClassSupport.API.Models;
using File = UTCClassSupport.API.Models.File;

namespace UTCClassSupport.API.Infrustructure.Data
{
  public class EFContext : IdentityDbContext<User, Role, string>
  {
    public EFContext(DbContextOptions<EFContext> options)
    : base(options)
    { }

    public DbSet<Bulletin> Bulletins { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<MessageFile> MessagesFile { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Timetable> Timetables { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserGroupRole> UserGroupRoles { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      modelBuilder.Entity<Bulletin>(e =>
      {
        e.HasKey(e => e.Id);
        e.HasOne(e => e.Group)
        .WithMany(e => e.Bulletins)
        .HasForeignKey(e => e.GroupId);
      });
      modelBuilder.Entity<Comment>(e =>
      {
        e.HasKey(e => e.Id);
        e.HasOne(e => e.Post)
        .WithMany(e => e.Comments)
        .HasForeignKey(e => e.PostId);
      });
      modelBuilder.Entity<Group>(e =>
      {
        e.HasKey(e => e.Id);
      });
      modelBuilder.Entity<Message>(e =>
      {
        e.HasKey(e => e.Id);
        e.HasOne(e => e.Chat)
        .WithMany(e => e.Messages)
        .HasForeignKey(e => e.ChatId);
      });
      modelBuilder.Entity<Chat>(e =>
      {
        e.HasKey(e => e.Id);
        e.HasMany(e => e.Joinners)
        .WithMany(e => e.Chats);
      });
      modelBuilder.Entity<MessageFile>(e =>
      {
        e.HasKey(e => e.Id);
      });
      modelBuilder.Entity<Event>(e =>
      {
        e.HasKey(e => e.Id);
        e.HasOne(e => e.Timetable)
        .WithMany(e => e.Shifts)
        .HasForeignKey(e => e.TimetableId);
        e.Property(e => e.Category).HasConversion<string>();
      });
      modelBuilder.Entity<Timetable>(e =>
      {
        e.HasKey(e => e.Id);
      });
      modelBuilder.Entity<Schedule>(e =>
      {
        e.HasKey(e => e.Id);
        e.HasOne(e => e.Shift)
        .WithOne(e => e.Schedule)
        .HasForeignKey<Schedule>(e => e.ShiftId)
        .IsRequired();
      });
      modelBuilder.Entity<UserGroupRole>(e =>
      {
        e.HasKey(e => new { e.UserId, e.GroupId, e.RoleId });
      });
      modelBuilder.Entity<Notification>(e =>
      {
        e.HasKey(e => e.Id);
        e.HasOne(e => e.Group)
        .WithMany(e => e.Notifications)
        .HasForeignKey(e => e.GroupId);
        e.HasOne(e => e.User)
        .WithMany(e => e.Notifications)
        .HasForeignKey(e => e.UserId);
        e.Property(e => e.Action).HasConversion<string>();
        e.Property(e => e.Range).HasConversion<string>();
      });
      base.OnModelCreating(modelBuilder);
      foreach (var entityType in modelBuilder.Model.GetEntityTypes())
      {
        var tableName = entityType.GetTableName();
        if (tableName.StartsWith("AspNet"))
        {
          entityType.SetTableName(tableName.Substring(6));
        }
      }
    }
  }
}
