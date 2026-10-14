using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace FIX_IT.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; }

        [Required]
        [StringLength(15)]
        public string PhoneNumber { get; set; }

        public bool IsAdmin { get; set; } = false;

        public virtual ICollection<Booking>? ReportedIssues { get; set; } = new List<Booking>();
    }
}
