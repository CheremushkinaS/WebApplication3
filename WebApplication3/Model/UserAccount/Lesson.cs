using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Model.UserAccount
{
    public class Lesson
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Добавьте этот атрибут
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [ForeignKey(nameof(Course))]
        public int CourseId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Course Course { get; set; }
        public ICollection<Material> Materials { get; set; }
        public ICollection<Assignments> Assignments { get; set; }

    }
}
