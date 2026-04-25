namespace AppointmentSystem.Models
{
    public class DoctorService
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int ServiceId { get; set; }
        public int DurationInMinutes { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public Doctor Doctor { get; set; } = null!;
        public Service Service { get; set; } = null!;
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}