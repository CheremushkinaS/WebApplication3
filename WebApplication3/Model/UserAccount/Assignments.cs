using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Model.UserAccount
{
    public class Assignments
    {
        [Key] // Добавляем атрибут первичного ключа
        public int AssignmentID { get; set; }

        [Required]
        public string Title { get; set; }

        [DataType(DataType.Html)]
        public string Description { get; set; }

        public int LessonID { get; set; }
   


        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [DataType(DataType.DateTime)]
        public DateTime Deadline { get; set; }

        [Range(1, 1000)]
        public int MaxScore { get; set; } = 100;

        public bool IsPublished { get; set; }

        public Lesson Lesson { get; set; }
    }
}

