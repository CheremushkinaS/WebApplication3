namespace WebApplication3.Model.UserAccount
{
    public class Group
    {
        public int id { get; set; }
        public int Courseid { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }

        public virtual Course Course { get; set; } = null!;
        public virtual ICollection<Group_Student> GroupStudents { get; set; } = new List<Group_Student>();
    }
}
