using FIX_IT.Models;
using System.ComponentModel.DataAnnotations;

namespace FIX_IT.Models
{
    public class Payment
    {
        public int Id { get; set; }
        [Required] public int BookingId { get; set; }
        public Booking Booking { get; set; }
        [Required] public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } 
        public string TransactionId { get; set; }
    }
}