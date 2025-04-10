namespace WebApplication3.Model.UserAccount
{
    public class Group_Student
    {
        public int id_student { get; set; }
        public int id_Group { get; set; }


        public virtual Group Group { get; set; } = null!;
        public virtual User Student { get; set; } = null!;
    }
}
