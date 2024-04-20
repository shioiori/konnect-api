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
    public DbSet<Group> Groups { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<MessageFile> MessagesFile { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<ShareFile> ShareFiles { get; set; }
    public DbSet<ShareFolder> ShareFolders { get; set; }
    public DbSet<Shift> Shifts { get; set; }
    public DbSet<Timetable> Timetables { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserGroupRole> UserGroupRoles { get; set; }
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
      modelBuilder.Entity<Group>(e =>
      {
        e.HasKey(e => e.Id);
      });
      modelBuilder.Entity<Message>(e =>
      {
        e.HasKey(e => e.Id);
      });
      modelBuilder.Entity<MessageFile>(e =>
      {
        e.HasKey(e => e.Id);
      });
      modelBuilder.Entity<ShareFile>(e =>
      {
        e.HasKey(e => e.Id);
        e.HasOne(e => e.Folder)
        .WithMany(e => e.Files)
        .HasForeignKey(e => e.FolderId);
      });
      modelBuilder.Entity<ShareFolder>(e =>
      {
        e.HasKey(e => e.Id);
        e.HasOne(e => e.Group)
        .WithMany(e => e.Folders)
        .HasForeignKey(e => e.GroupId);
      });
      modelBuilder.Entity<Shift>(e =>
      {
        e.HasKey(e => e.Id);
        e.HasOne(e => e.Timetable)
        .WithMany(e => e.Shifts)
        .HasForeignKey(e => e.TimetableId);
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
