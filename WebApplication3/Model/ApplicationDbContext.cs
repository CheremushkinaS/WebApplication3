using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using WebApplication3.Model.UserAccount;

namespace WebApplication3.Model.UserAccount

{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> UserRoles { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Group> Groups { get; set; } = null!;
        public DbSet<Group_Student> Group_Student { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!; // Добавлено
        public DbSet<Course_Categories> Course_Categories { get; set; } = null!; // Добавлено
        public DbSet<Application> Applications { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<FileType> FileTypes { get; set; } = null!;
        public DbSet<Lesson> Lessons { get; set; } = null!;
        public DbSet<Assignments> Assignments { get; set; } // Множественное число для коллекции


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка связи многие-ко-многим для Group_Student
            modelBuilder.Entity<Group_Student>()
                .HasKey(gs => new { gs.id_Group, gs.id_student });

            modelBuilder.Entity<Group_Student>()
                .HasOne(gs => gs.Group)
                .WithMany(g => g.GroupStudents)
                .HasForeignKey(gs => gs.id_Group);

            modelBuilder.Entity<Group_Student>()
                .HasOne(gs => gs.Student)
                .WithMany()
                .HasForeignKey(gs => gs.id_student);

            // Настройка связи многие-ко-многим для Course_Categories
            modelBuilder.Entity<Course_Categories>()
                .HasKey(cc => new { cc.course_id, cc.category_id }); // Составной ключ

            modelBuilder.Entity<Course_Categories>()
                .HasOne(cc => cc.Course)
                .WithMany(c => c.CourseCategories)
                .HasForeignKey(cc => cc.course_id);

            modelBuilder.Entity<Course_Categories>()
                .HasOne(cc => cc.Category)
                .WithMany(c => c.CourseCategories)
                .HasForeignKey(cc => cc.category_id);

            // Исправление опечатки для Courses
            modelBuilder.Entity<Course>()
                .Property(c => c.desceiption)
                .HasColumnName("desceiption");

            // Ограничения для Application


            // Ограничения для Payment
            modelBuilder.Entity<Payment>()
                .Property(p => p.PaymentMethod)
                .HasConversion<string>();


            // Уникальность TransactionId
            modelBuilder.Entity<Payment>()
                .HasIndex(p => p.TransactionId)
                .IsUnique();
            modelBuilder.Entity<Material>()
               .Property(m => m.CreatedAt)
               .HasDefaultValueSql("GETDATE()"); // Для SQL Server

            modelBuilder.Entity<Lesson>(entity =>
            {
                entity.HasOne(l => l.Course)
                      .WithMany(c => c.Lessons)
                      .HasForeignKey(l => l.CourseId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Lesson>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()"); // Для SQL Server

                entity.HasIndex(e => e.CourseId)
                    .HasDatabaseName("IX_Lessons_CourseId");
            });

            // Конфигурация для Material
            modelBuilder.Entity<Material>(entity =>
            {
                entity.HasOne(m => m.Lesson)
                      .WithMany(l => l.Materials)
                      .HasForeignKey(m => m.LessonId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Конфигурация для FileType
            modelBuilder.Entity<FileType>()
                .HasIndex(ft => ft.Type)
                .IsUnique(); // Уникальность типа файла

            modelBuilder.Entity<Assignments>(entity =>
            {
                entity.HasOne(m => m.Lesson)
                      .WithMany(l => l.Assignments)
                      .HasForeignKey(m => m.LessonID)
                      .OnDelete(DeleteBehavior.Cascade);
            });





        }
    }
    }




