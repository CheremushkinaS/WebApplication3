using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Model.UserAccount
{
    public class Application
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public DateTime ApplicationDate { get; set; }

        public int? GroupId { get; set; } // Nullable
                                          // Навигационные свойства
        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("CourseId")]
        public Course Course { get; set; }

        [ForeignKey("GroupId")]
        public Group Group { get; set; }

        public ICollection<Payment> Payments { get; set; }
    }
}
