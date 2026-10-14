using FIX_IT.Models;
using System.ComponentModel.DataAnnotations;

namespace FIX_IT.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public int? BookingId { get; set; }
        public Booking Booking { get; set; }
        [Required] public string CustomerName { get; set; }
        [Required] public string Comment { get; set; }
        public int Rating { get; set; } // 1-5
    }
}