using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System; 

namespace FIX_IT.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [Display(Name = "Service Title")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Creation Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Status")]
        public bool IsClosed { get; set; } = false;

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        [Display(Name = "Rating")]
        public int? Rating { get; set; }

        [Display(Name = "Client Feedback")]
        public string? Feedback { get; set; }

        [Display(Name = "Client")]
        [ForeignKey("Reporter")]
        public int ReporterId { get; set; }
        public User? Reporter { get; set; }

        [Display(Name = "Service Type")]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        [Display(Name = "Assigned Technician")]
        [ForeignKey("AssignedTechnician")]
        public int? TechnicianId { get; set; }
        public Technician? AssignedTechnician { get; set; }
    }
}
