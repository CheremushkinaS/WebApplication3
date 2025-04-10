using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Model.UserAccount
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Courses")]
    public class Course
    {
        public int id { get; set; }

        [Required]
        [StringLength(200)]
        public string title { get; set; } = null!;

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string desceiption { get; set; } = null!;

        [Column(TypeName = "decimal(10, 2)")]
        public decimal price { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal discount { get; set; }

        [Required]

        public int TeacherId { get; set; }

        // Навигационные свойства
        public virtual User Teacher { get; set; } = null!;
        public virtual ICollection<Group> Groups { get; set; } = new HashSet<Group>();
        public virtual ICollection<Course_Categories> CourseCategories { get; set; } = new HashSet<Course_Categories>();
        public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
    }
}
