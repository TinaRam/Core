using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Workflow.Models
{
    public partial class WorkflowContext : DbContext
    {
        public WorkflowContext()
        {
        }

        public WorkflowContext(DbContextOptions<WorkflowContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AssignedTask> AssignedTask { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<ProjectParticipant> ProjectParticipant { get; set; }
        public virtual DbSet<PTask> Ptask { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL("server=127.0.0.1;port=3306;user=app;password=app2000;database=Workflow");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.2-servicing-10034");

            modelBuilder.Entity<AssignedTask>(entity =>
            {
                entity.HasKey(e => new { e.EmployeeId, e.TaskId });

                entity.HasIndex(e => e.TaskId)
                    .HasName("AssignedTaskPTaskFK");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.AssignedTask)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AssignedTaskEmployeeFK");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.AssignedTask)
                    .HasForeignKey(d => d.TaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AssignedTaskPTaskFK");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasIndex(e => e.UserId)
                    .HasName("EmployeeUserFK");

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.FirstName).IsUnicode(false);

                entity.Property(e => e.LastName).IsUnicode(false);

                entity.Property(e => e.PhoneNumber).IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Employee)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("EmployeeUserFK");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasIndex(e => e.ProjectManager)
                    .HasName("ProjectEmployeeFK");

                entity.Property(e => e.ProjectDescription).IsUnicode(false);

                entity.Property(e => e.ProjectName).IsUnicode(false);

                entity.HasOne(d => d.ProjectManagerNavigation)
                    .WithMany(p => p.Project)
                    .HasForeignKey(d => d.ProjectManager)
                    .HasConstraintName("ProjectEmployeeFK");
            });

            modelBuilder.Entity<ProjectParticipant>(entity =>
            {
                entity.HasKey(e => new { e.ProjectId, e.EmployeeId });

                entity.HasIndex(e => e.EmployeeId)
                    .HasName("ProjectParticipantEmployeeFK");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.ProjectParticipant)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ProjectParticipantEmployeeFK");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectParticipant)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ProjectParticipantProjectFK");
            });

            modelBuilder.Entity<PTask>(entity =>
            {
                entity.HasIndex(e => e.TaskProjectId)
                    .HasName("PTaskProjectFK");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.TaskCreationDate).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.TaskName).IsUnicode(false);

                entity.HasOne(d => d.TaskProject)
                    .WithMany(p => p.PTask)
                    .HasForeignKey(d => d.TaskProjectId)
                    .HasConstraintName("PTaskProjectFK");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.Username).IsUnicode(false);
            });

            #region UserSeed
            modelBuilder.Entity<User>().HasData(
                new User() { UserId = 1, Username = "pernille",Password = "123",IsAdmin = 1 },
                new User() { UserId = 2, Username = "tinahodepina",Password = "paracet",IsAdmin = 1 });
            #endregion
        }
    }
}
