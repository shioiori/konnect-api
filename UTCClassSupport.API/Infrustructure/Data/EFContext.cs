using Microsoft.EntityFrameworkCore;
using System.Data;
using UTCClassSupport.API.Models;

namespace UTCClassSupport.API.Infrustructure.Data
{
  public class EFContext : DbContext
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
    public DbSet<Shift> Shifts { get; set; }
    public DbSet<Timetable> Timetables { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
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
      modelBuilder.Entity<Schedule>(e =>
      {
        e.HasKey(e => e.Id);
      });
      modelBuilder.Entity<ShareFile>(e =>
      {
        e.HasKey(e => e.Id);
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
    }
  }
}
