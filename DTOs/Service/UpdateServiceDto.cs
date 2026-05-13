namespace AppointmentSystem.DTOs.Service
{
    public class UpdateServiceDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? DurationInMinutes { get; set; }
    }
}