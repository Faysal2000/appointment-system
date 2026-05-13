namespace AppointmentSystem.DTOs.Service
{
    public class ServiceResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int DurationInMinutes { get; set; }
    }
}