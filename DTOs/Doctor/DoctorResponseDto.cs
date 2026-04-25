// DTOs/Doctor/DoctorResponseDto.cs
namespace AppointmentSystem.DTOs.Doctor
{
    public class DoctorResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public int ExperienceYears { get; set; }
        public List<string> Specialties { get; set; } = new();
        public List<DoctorServiceDto> Services { get; set; } = new();
    }

    public class DoctorServiceDto
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public int DurationInMinutes { get; set; }
        public decimal Price { get; set; }
    }
}