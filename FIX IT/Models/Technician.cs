using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIX_IT.Models
{
    public class Technician
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "يرجى إدخال اسم الفني")]
        [StringLength(150)]
        public string Name { get; set; }

        [StringLength(15)]
        public string PhoneNumber { get; set; }

        public bool IsAvailable { get; set; } = true;

        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }
        public virtual ICollection<Booking>? AssignedIssues { get; set; } = new List<Booking>();
    }
}
