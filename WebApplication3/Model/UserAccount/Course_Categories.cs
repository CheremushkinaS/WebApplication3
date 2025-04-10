namespace WebApplication3.Model.UserAccount
{
    public class Course_Categories
    {
        public int course_id { get; set; }
        public int category_id { get; set; }


        public virtual Course Course { get; set; } = null!;
        public virtual Category Category { get; set; } = null!;
    }
}
