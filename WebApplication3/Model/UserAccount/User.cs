using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Model.UserAccount
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; } = null!;

        public virtual ICollection<Group_Student> GroupStudents { get; set; } = new List<Group_Student>();
    }
}
