using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Workflow.Models
{
    public class WorkflowContext : DbContext
    {
        public WorkflowContext()
        {
        }

        public WorkflowContext(DbContextOptions<WorkflowContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AssignedTask> AssignedTask { get; set; }
        public virtual DbSet<EmployeeLeave> EmployeeLeave { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<ProjectParticipant> ProjectParticipant { get; set; }
        public virtual DbSet<Ptask> Ptask { get; set; }
        public virtual DbSet<Report> Report { get; set; }
        public virtual DbSet<TaskList> TaskList { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL("server=waindor.com;port=3306;user=app2000g11;password=Wørkflow11;database=app2000g11");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.2-servicing-10034");

            modelBuilder.Entity<AssignedTask>(entity =>
            {
                entity.HasIndex(e => e.TaskId)
                    .HasName("AssignedTaskPTaskFK");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.AssignedTask)
                    .HasForeignKey(d => d.TaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AssignedTaskPTaskFK");
            });

            modelBuilder.Entity<EmployeeLeave>(entity =>
            {
                entity.HasIndex(e => e.UserId)
                    .HasName("EmployeeLeaveUserFK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.EmployeeLeave)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("EmployeeLeaveUserFK");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasIndex(e => e.ProjectManager)
                    .HasName("ProjectUserFK");

                entity.Property(e => e.ProjectDescription).IsUnicode(false);

                entity.Property(e => e.ProjectName).IsUnicode(false);

                entity.HasOne(d => d.ProjectManagerNavigation)
                    .WithMany(p => p.Project)
                    .HasForeignKey(d => d.ProjectManager)
                    .HasConstraintName("ProjectUserFK");
            });

            modelBuilder.Entity<ProjectParticipant>(entity =>
            {
                entity.HasKey(e => new { e.ProjectId, e.UserId });

                entity.HasIndex(e => e.UserId)
                    .HasName("ProjectParticipantUserFK");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectParticipant)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ProjectParticipantProjectFK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ProjectParticipant)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ProjectParticipantUserFK");
            });

            modelBuilder.Entity<Ptask>(entity =>
            {
                entity.HasIndex(e => e.TaskListId)
                    .HasName("PTaskTaskListFK");

                entity.HasIndex(e => e.TaskProjectId)
                    .HasName("PTaskProjectFK");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.TaskCreationDate).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.TaskName).IsUnicode(false);

                entity.HasOne(d => d.TaskList)
                    .WithMany(p => p.Ptask)
                    .HasForeignKey(d => d.TaskListId)
                    .HasConstraintName("PTaskTaskListFK");

                entity.HasOne(d => d.TaskProject)
                    .WithMany(p => p.Ptask)
                    .HasForeignKey(d => d.TaskProjectId)
                    .HasConstraintName("PTaskProjectFK");
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.HasIndex(e => e.ProjectId)
                    .HasName("ReportProjectFK");

                entity.Property(e => e.Comment).IsUnicode(false);

                entity.Property(e => e.CompletionDate).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Report)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("ReportProjectFK");
            });

            modelBuilder.Entity<TaskList>(entity =>
            {
                entity.HasIndex(e => e.ProjectId)
                    .HasName("TaskListFK");

                entity.Property(e => e.ListName).IsUnicode(false);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.TaskList)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TaskListFK");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.About).IsUnicode(false);

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.FirstName).IsUnicode(false);

                entity.Property(e => e.LastName).IsUnicode(false);

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.PhoneNumber).IsUnicode(false);

                entity.Property(e => e.Username).IsUnicode(false);
            });
        }
    }
}
