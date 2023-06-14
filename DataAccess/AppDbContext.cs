using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class AppDbContext : DbContext
    {
        public DbSet<Position> Positions { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectEmployee> ProjectsEmployees { get; set; }

        public string DbPath { get; }

        public AppDbContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "ProjectsWorkflowDatabase.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                        .HasMany(e => e.ManagedProjects)
                        .WithOne(p => p.ProjectManager)
                        .HasForeignKey(p => p.ProjectManagerId)
                        .IsRequired(false)
                        .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Position>()
                .HasMany(p => p.Employees)
                .WithOne(e => e.Position)
                .HasForeignKey(e => e.PositionId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Project>()
                        .HasIndex(p => p.Name)
                        .IsUnique();

            modelBuilder.Entity<Project>().HasMany(x => x.Employees)
                .WithMany(x => x.AssignedProjects)
                .UsingEntity<ProjectEmployee>(
                    x => x.HasOne(x => x.Employee)
                    .WithMany().HasForeignKey(x => x.EmployeeId),
                    x => x.HasOne(x => x.Project)
                   .WithMany().HasForeignKey(x => x.ProjectId));
        }


        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={DbPath}");
    }

}
