namespace AppointmentSystem.DTOs.Doctor
{
    public class CreateDoctorDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public int ExperienceYears { get; set; }
        public List<int> SpecialtyIds { get; set; } = new();
    }
}