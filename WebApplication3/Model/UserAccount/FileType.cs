using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace WebApplication3.Model.UserAccount
{
    public class FileType
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Type { get; set; }  // Пример: "Video", "PDF", "Image"

      
 
    }
}
