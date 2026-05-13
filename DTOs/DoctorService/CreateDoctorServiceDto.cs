namespace AppointmentSystem.DTOs.DoctorService
{
    public class CreateDoctorServiceDto
    {
        public int DoctorId { get; set; }
        public int ServiceId { get; set; }
        public int DurationInMinutes { get; set; }
        public decimal Price { get; set; }
    }
}
