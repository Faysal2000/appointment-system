namespace AppointmentSystem.DTOs.Service
{
    public class CreateServiceDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int DurationInMinutes { get; set; }
    }
}