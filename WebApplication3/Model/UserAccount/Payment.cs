using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Model.UserAccount
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ApplicationId { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Amount { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime PaymentDate { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; }

        [StringLength(100)]
        public string TransactionId { get; set; }

        [Required]
        public bool IsConfirmed { get; set; } = false;

        // Навигационное свойство
        [ForeignKey("ApplicationId")]
        public Application Application { get; set; }
    }
}
