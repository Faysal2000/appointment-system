namespace AppointmentSystem.DTOs.Patient
{
    public class CreatePatientDto
    {
        public string FullName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty; 
        public string? Phone { get; set; }
    }
}