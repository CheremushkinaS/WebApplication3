namespace WebApplication3.Model.UserAccount
{
    public class Category
    {
        public int id { get; set; }
        public string Name { get; set; } = null!;
        public virtual ICollection<Course_Categories> CourseCategories { get; set; } = new List<Course_Categories>();

    }
}
