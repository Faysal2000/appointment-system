namespace AppointmentSystem.DTOs.Doctor
{
    public class UpdateDoctorDto
    {
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public int? ExperienceYears { get; set; }
    }
}