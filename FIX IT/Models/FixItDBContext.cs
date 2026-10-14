using Microsoft.EntityFrameworkCore;
using FIX_IT.Models;

namespace FIX_IT.Models
{
    public class FixItDBContext : DbContext
    {
        public FixItDBContext(DbContextOptions<FixItDBContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Technician> Technicians { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<Feedback> Feedback { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Reporter)
                .WithMany(u => u.ReportedIssues)
                .HasForeignKey(b => b.ReporterId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
