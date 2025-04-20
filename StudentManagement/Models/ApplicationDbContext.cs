using Microsoft.EntityFrameworkCore;

namespace StudentManagement.Models;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<ClassInfo> ClassInfos { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<Semester> Semesters { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Student>()
            .HasOne(s => s.ClassInfo)
            .WithMany(c => c.Students)
            .HasForeignKey(s => s.ClassId);
        builder.Entity<Grade>()
            .HasOne(g => g.Student)
            .WithMany(s => s.Grades)
            .HasForeignKey(g => g.StudentId);
        builder.Entity<User>()
            .HasIndex(u=>u.UserName)
            .IsUnique();
    }
    
}