namespace AppointmentSystem.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Unpaid"; // Unpaid/Paid/PartiallyPaid
        public DateTime IssuedDate { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public Appointment Appointment { get; set; } = null!;
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}