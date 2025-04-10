using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace WebApplication3.Model.UserAccount
{
    public class Material
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        [Required]
        public int LessonId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(LessonId))]
        public Lesson Lesson { get; set; }

        // Исправляем на единственное число
    }
}
